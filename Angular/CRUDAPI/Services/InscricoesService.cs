using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Services
{
    public class InscricaoService
    {
        private readonly Contexto _contexto;
        private readonly EmailService _emailService;
        public InscricaoService(Contexto contexto, EmailService emailService)
        {
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

        public async Task<bool> EnviarInscricoesParaOrganizador(Inscricao[] inscricoes, string organizadorEmail)
        {
            // Link de confirmação (substitua pelo domínio correto)
            string confirmationLink = $"http://localhost:4200/inscricoes-confirmadas";

            // Obtém as informações das inscrições corretamente
            var inscricoesInfoArray = await Task.WhenAll(inscricoes.Select(async i =>
            {
                var competidor = await _contexto.Competidores.FindAsync(i.CompetidorId);
                var usuario = competidor != null ? await _contexto.Usuarios.FindAsync(competidor.CriadorId) : null;

                return $"Competidor: {competidor?.Nome ?? "Desconhecido"}, Categoria ID: {i.CategoriaId}, Usuário Email: {usuario?.Email ?? "Não encontrado"}";
            }));

            string inscricoesInfo = string.Join("<br>", inscricoesInfoArray);

            // Enviar e-mail com os dados das inscrições
            await _emailService.SendEmailAsync(organizadorEmail, "Dados das Inscrições",
                $"Aqui estão os dados das inscrições:<br>{inscricoesInfo}<br><br>Acesse o sistema para aprovar ou recusar as inscrições.");

            return true;
        }

    }
}
