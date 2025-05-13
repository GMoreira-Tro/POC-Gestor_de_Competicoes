using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Services
{
    public class InscricaoService
    {
        private readonly Contexto _contexto;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        public InscricaoService(Contexto contexto, EmailService emailService, IConfiguration configuration)
        {
            _configuration = configuration;
            _contexto = contexto;
            _emailService = emailService;
        }

        public async Task<Inscricao> ValidarInscricao(Inscricao inscricao)
        {
            // Verifica se já existe uma inscrição com o mesmo TimeID e CategoriaID
            var inscricaoExistente = await _contexto.Inscricoes
                .AnyAsync(i => i.CompetidorId == inscricao.CompetidorId && i.CategoriaId == inscricao.CategoriaId && i.Id != inscricao.Id);

            if (inscricaoExistente)
            {
                throw new InvalidOperationException($"O Competidor com ID {inscricao.CompetidorId} já possui uma inscrição na Categoria com ID {inscricao.CategoriaId}.");
            }

            if (inscricao.Status != InscricaoStatus.Paga && inscricao.PagamentoId != null)
            {
                throw new InvalidOperationException("Uma inscrição com o Status diferente de Paga não pode possuir um Pagamento.");
            }
            if (inscricao.Status == InscricaoStatus.Paga && inscricao.PagamentoId == null)
            {
                throw new InvalidOperationException("Uma inscrição com o Status de Paga deve possuir um Pagamento.");
            }

            return inscricao;
        }


        public bool InscricaoExists(long id)
        {
            return _contexto.Inscricoes.Any(e => e.Id == id);
        }

        public async Task<bool> EnviarInscricoesParaOrganizador(long[] inscricoesIds, string organizadorEmail, long IdOrganizador)
        {
            string confirmationLink = "";

            // Obtém as informações das inscrições corretamente
            var inscricoesList = await _contexto.Inscricoes
            .Where(i => inscricoesIds.Contains(i.Id))
            .ToListAsync();

            if (!inscricoesList.Any())
            {
                throw new InvalidOperationException("Nenhuma inscrição encontrada com os IDs fornecidos.");
            }

            var inscricoesInfoArray = new List<string>();
            var notificacoesArray = new List<Notificacao>();
            var competicao = new Competicao();
            foreach (var inscricao in inscricoesList)
            {
                var competidor = await _contexto.Competidores.FindAsync(inscricao.CompetidorId);
                var categoria = await _contexto.Categorias.FindAsync(inscricao.CategoriaId);
                competicao = categoria != null ? await _contexto.Competicoes.FindAsync(categoria.CompeticaoId) : null;
                var usuario = competidor != null ? await _contexto.Usuarios.FindAsync(competidor.CriadorId) : null;

                inscricoesInfoArray.Add($"Competidor: {competidor?.Nome}, Categoria: {categoria?.Nome}, Email do solicitante: {usuario?.Email ?? "Não encontrado"}");

                var notificacao = new Notificacao
                {
                    NotificadoId = IdOrganizador,
                    Titulo = $"Inscrição em {competicao?.Titulo} solicitada.",
                    Descricao = $"Uma nova inscrição foi recebida para a competição {competicao?.Titulo}.",
                    DataPublicacao = DateTime.UtcNow,
                    Link = $"{_configuration["Configuration:BaseUrl"]}/aprovar-inscricao/{competicao?.Id}",
                    TipoAnuncio = "Inscrição"
                };

                notificacoesArray.Add(notificacao);
            }

            // Adiciona as notificações ao contexto e salva as alterações
            _contexto.Notificacoes.AddRange(notificacoesArray);
            await _contexto.SaveChangesAsync();

            string inscricoesInfo = string.Join("<br>", inscricoesInfoArray);

            confirmationLink = $"{_configuration["Configuration:BaseUrl"]}/aprovar-inscricao/{competicao?.Id}";
            // Enviar e-mail com os dados das inscrições
            await _emailService.SendEmailAsync(organizadorEmail, $"Dados das Inscrições na Competição: {competicao?.Titulo}",
            $"Aqui estão os dados das inscrições:<br>{inscricoesInfo}<br><br>Acesse o sistema para aprovar ou recusar as inscrições.<br><br><a href='{confirmationLink}'>Confirmar Inscrições</a>");

            return true;
        }

    }
}
