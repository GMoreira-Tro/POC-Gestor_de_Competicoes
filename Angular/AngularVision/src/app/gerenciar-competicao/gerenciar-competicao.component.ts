import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriaService } from '../services/categoria.service';
import { InscricaoService } from '../services/inscricao.service';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { CompetidorService } from '../services/competidor.service';
import { Confronto } from '../interfaces/Confronto';
import { Competidor } from '../interfaces/Competidor';
import { ConfrontoInscricao } from '../interfaces/ConfrontoInscricao';

@Component({
  selector: 'app-gerenciar-competicao',
  templateUrl: './gerenciar-competicao.component.html',
  styleUrls: ['./gerenciar-competicao.component.css']
})
export class GerenciarCompeticaoComponent implements OnInit {
  competicaoId!: number;
  categorias: any[] = [];
  categoriaSelecionada: any;
  inscricoes: any[] = [];
  carregouInscricao: boolean = false;
  chaveamentoSelecionado: any = null;

  chaveamentos: any[] = [];
  chaveamentosPorCategoria: { [categoriaId: number]: any[] } = {};


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private competicaoService: CompeticaoService,
    private userService: UserService,
    private competidorService: CompetidorService,
    private categoriaService: CategoriaService,
    private inscricaoService: InscricaoService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.competicaoId = +id;

    this.competicaoService.getCompeticao(this.competicaoId).subscribe(
      competicao => {
        this.userService.getUsuarioLogado().subscribe(usuario => {
          const userId = usuario.id;
          if (competicao.criadorUsuarioId !== userId) {
            console.log("Usuário tentou editar uma competição que não lhe pertence.");
            this.router.navigate(['/']);
            return;
          }
        });
      },
      error => console.log('Erro ao carregar competição:', error)
    );
    this.carregarCategorias();
  }

  carregarCategorias(): void {
    this.categoriaService.getCategoriasPorCompeticao(this.competicaoId).subscribe(
      res => this.categorias = res,
      err => console.error('Erro ao carregar categorias', err)
    );
  }

  onCategoriaSelecionada(): void {
    if (!this.categoriaSelecionada) return;

    this.inscricoes = [];
    this.carregouInscricao = false;

    // 🧠 Troca de chaveamentos conforme a categoria
    if (this.chaveamentosPorCategoria[this.categoriaSelecionada]) {
      this.chaveamentos = this.chaveamentosPorCategoria[this.categoriaSelecionada];
    } else {
      this.chaveamentos = [];
      this.chaveamentosPorCategoria[this.categoriaSelecionada] = this.chaveamentos;
    }

    this.chaveamentoSelecionado = null;

    this.inscricaoService.getInscricoesPorCategoria(this.categoriaSelecionada).subscribe(
      res => {
        this.inscricoes = res.filter(i => i.status === 1);
        const inscricoesQuant = this.inscricoes.length;
        let inscricoesCarregadas = 0;

        this.inscricoes.forEach(inscricao => {
          this.inscricaoService.getInfoInscricao(inscricao.id).subscribe(inscricaoInfo => {
            inscricao.inscricaoInfo = inscricaoInfo;
            this.competidorService.obterCompetidor(inscricao.competidorId).subscribe(competidor => {
              inscricao.inscricaoInfo.competidor = competidor;
            });
            inscricoesCarregadas++;
            if (inscricoesCarregadas === inscricoesQuant) {
              this.carregouInscricao = true;
            }
          });
        });
      },
      err => console.error('Erro ao carregar inscrições', err)
    );
  }

  criarChaveamento(): void {
    const novo = {
      nome: `Chaveamento ${this.chaveamentos.length + 1}`,
      rounds: [
        {
          numero: 1,
          confrontos: []
        }
      ],
      editandoNome: true
    };
    this.chaveamentos.push(novo);
    this.chaveamentoSelecionado = novo;
    this.chaveamentosPorCategoria[this.categoriaSelecionada] = this.chaveamentos;
  }

  confirmarNome(chaveamento: any): void {
    chaveamento.editandoNome = false;
    // Se desejar, você pode validar nome vazio aqui
  }
  selecionarChaveamento(chave: any): void {
    this.chaveamentoSelecionado = chave;
  }


  adicionarConfronto(chaveamento: any, roundNumero: number): void {
    const round = chaveamento.rounds.find((r: any) => r.numero === roundNumero);
    if (round) {
      round.confrontos.push({
        competidorA: null,
        competidorB: null,
        vencedor: null
      });
    }
  }

  getOpcoes(confronto: Confronto, lado: 'A' | 'B', roundNumero: number): Competidor[] {
    let opcoes: Competidor[] = [];

    // Primeiro round → todos os competidores inscritos
    if (roundNumero === 1) {
      opcoes = this.inscricoes.map(i => i.inscricaoInfo.competidor);
    } else {
      // Rodadas seguintes → apenas vencedores da rodada anterior
      const anterior = this.chaveamentoSelecionado.rounds.find((r: any) => r.numero === roundNumero - 1);
      if (!anterior) return [];

      opcoes = anterior.confrontos
        .map((c: any) => c.vencedor)
        .filter((v: any) => !!v); // remove undefined
    }

    // Remove quem já foi escolhido em outros confrontos desse round
    const usados = this.getCompetidoresUsadosNoRound(roundNumero, confronto);
    opcoes = opcoes.filter(c => !usados.some(u => u.id === c.id));

    // Remove o competidor escolhido no outro lado do mesmo confronto
    const outroLado = lado === 'A' ? confronto.competidorB : confronto.competidorA;
    if (outroLado) {
      opcoes = opcoes.filter(c => c.id !== outroLado.id);
    }

    return opcoes;
  }


  getCompetidoresUsadosNoRound(roundNumero: number, confrontoAtual: any): Competidor[] {
    const round = this.chaveamentoSelecionado.rounds.find((r: any) => r.numero === roundNumero);
    if (!round) return [];

    const usados: Competidor[] = [];

    for (const confronto of round.confrontos) {
      if (confronto === confrontoAtual) continue; // Evita bloquear seleção no próprio confronto

      if (confronto.competidorA) usados.push(confronto.competidorA);
      if (confronto.competidorB) usados.push(confronto.competidorB);
    }

    return usados;
  }

  buscarConfrontoPorId(id: number | string): Confronto | undefined {
    if (!this.chaveamentoSelecionado) return undefined;

    for (const round of this.chaveamentoSelecionado.rounds) {
      for (const confronto of round.confrontos) {
        if (confronto.id === id || confronto.tempId === id) {
          return confronto;
        }
      }
    }

    return undefined;
  }

  trackPorId(index: number, item: any): number {
    return item.id;
  }

  compararCompetidores = (c1: any, c2: any) => {
    return c1 && c2 ? c1.id === c2.id : c1 === c2;
  };

  vincularVencedor(chaveamento: any, roundNumero: number, confrontoAtual: any): void {
    const proximoRoundNumero = roundNumero + 1;
    const rounds = chaveamento.rounds;
  
    // Cria o próximo round se não existir
    let proximoRound = rounds.find((r: any) => r.numero === proximoRoundNumero);
    if (!proximoRound) {
      proximoRound = {
        numero: proximoRoundNumero,
        confrontos: []
      };
      rounds.push(proximoRound);
      rounds.sort((a: any, b: any) => a.numero - b.numero);
    }
  
    const novoVencedor = confrontoAtual.vencedor;
    const antigoVencedor = confrontoAtual._ultimoVencedor;
  
    // 🔁 Atualiza todos os rounds seguintes se o vencedor mudou
    if (antigoVencedor && antigoVencedor.id !== novoVencedor?.id) {
      for (let i = 0; i < rounds.length; i++) {
        const r = rounds[i];
        if (r.numero <= roundNumero) continue;
  
        for (const c of r.confrontos) {
          if (c.competidorA?.id === antigoVencedor.id) c.competidorA = novoVencedor;
          if (c.competidorB?.id === antigoVencedor.id) c.competidorB = novoVencedor;
        }
      }
    }
  
    // ⚠️ Impede duplicatas no próximo round
    if (novoVencedor) {
      const jaExiste = proximoRound.confrontos.some((c : any) =>
        c.competidorA?.id === novoVencedor.id || c.competidorB?.id === novoVencedor.id
      );
      if (jaExiste) return; // Já está alocado nesse round
  
      // Encontra slot vazio
      let destino = proximoRound.confrontos.find((c : any) => !c.competidorA || !c.competidorB);
      if (!destino) {
        destino = {
          competidorA: null,
          competidorB: null,
          vencedor: null
        };
        proximoRound.confrontos.push(destino);
      }
  
      if (!destino.competidorA) {
        destino.competidorA = novoVencedor;
      } else if (!destino.competidorB) {
        destino.competidorB = novoVencedor;
      }
    }
  
    // Atualiza o "último vencedor" do confronto
    confrontoAtual._ultimoVencedor = novoVencedor;
  }
  
}
