import { Component, ElementRef, OnInit, ViewChild, AfterViewInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriaService } from '../services/categoria.service';
import { InscricaoService } from '../services/inscricao.service';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { CompetidorService } from '../services/competidor.service';
import * as go from 'gojs';
import { VisualizacaoChaveamentoBotaoComponent } from '../visualizacao-chaveamento-botao/visualizacao-chaveamento-botao.component';
import { ChaveamentoService } from '../services/chaveamento.service';
import { Chaveamento } from '../interfaces/Chaveamento';

@Component({
  selector: 'app-gerenciar-competicao',
  templateUrl: './gerenciar-competicao.component.html',
  styleUrls: ['./gerenciar-competicao.component.css']
})
export class GerenciarCompeticaoComponent implements OnInit, AfterViewInit {
  @ViewChild('diagramaRef', { static: false }) diagramaRef!: ElementRef;
  @ViewChildren(VisualizacaoChaveamentoBotaoComponent) diagramas!: QueryList<VisualizacaoChaveamentoBotaoComponent>;

  competicaoId!: number;
  categorias: any[] = [];
  categoriaSelecionada: any;
  categoriaSelecionadaAtual: any;
  inscricoes: any[] = [];
  participantesNomes: string[] = [];
  carregouInscricao: boolean = false;
  chaveamentoSelecionado: any = null;
  chaveamentos: any[] = [];
  chaveamentosPorCategoria: { [categoriaId: number]: any[] } = {};
  diagram: go.Diagram | undefined;
  nosChaveamento: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private competicaoService: CompeticaoService,
    private userService: UserService,
    private competidorService: CompetidorService,
    private categoriaService: CategoriaService,
    private chaveamentoService: ChaveamentoService,
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

  ngAfterViewInit() {

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

    this.salvarChaveamentosDaCategoriaAtual();
    this.categoriaSelecionadaAtual = this.categoriaSelecionada;

    if (this.chaveamentosPorCategoria[this.categoriaSelecionada]) {
      this.chaveamentos = this.chaveamentosPorCategoria[this.categoriaSelecionada];
      this.restaurarChaveamentosDaCategoriaSelecionada();
    } else {
      console.log('Nenhum chaveamento encontrado para a categoria selecionada:', this.categoriaSelecionada);
      // Busca do backend os chaveamentos da categoria
      this.chaveamentoService.getChaveamentosPorCategoria(this.categoriaSelecionada)
        .subscribe({
          next: (res) => {
            this.chaveamentos = res.map((chaveamento: Chaveamento) => ({
              id: chaveamento.id,
              tempId: 0,
              categoriaId: chaveamento.categoriaId,
              nome: chaveamento.nome,
              arvoreConfrontos: chaveamento.arvoreConfrontos,
              participantes: [] // reconstruído pelo filho com base na árvore
            }));

            this.chaveamentosPorCategoria[this.categoriaSelecionada] = this.chaveamentos;
            this.restaurarChaveamentosDaCategoriaSelecionada();
          },
          error: (err) => {
            if (err.status === 404) {
              // Nenhum chaveamento encontrado para a categoria
              this.chaveamentos = [];
              this.chaveamentosPorCategoria[this.categoriaSelecionada] = [];
            } else {
              console.error('Erro ao buscar chaveamentos:', err);
            }
          }
        });
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
              this.participantesNomes = this.inscricoes.map(i => i.inscricaoInfo.nomeCompetidor);
              this.carregouInscricao = true;
              this.nosChaveamento = this.montarNosChaveamento(); // <- gera e guarda
            }
          });
        });
      },
      err => console.error('Erro ao carregar inscrições', err)
    );
  }
  montarNosChaveamento(): any[] {
    const competidores = this.inscricoes.map(i => i.inscricaoInfo.nomeCompetidor);
    let id = 1;
    const todosOsNos: any[] = [];

    type NoChaveamento = { key: number; name: string; parent: number | undefined };

    function construirArvore(participantes: string[]): NoChaveamento {
      if (participantes.length === 1) {
        const folha: NoChaveamento = { key: id++, name: participantes[0], parent: undefined };
        todosOsNos.push(folha);
        return folha;
      }

      if (participantes.length === 2) {
        const folha1: NoChaveamento = { key: id++, name: participantes[0], parent: undefined };
        const folha2: NoChaveamento = { key: id++, name: participantes[1], parent: undefined };
        const pai: NoChaveamento = { key: id++, name: '', parent: undefined };

        folha1.parent = pai.key;
        folha2.parent = pai.key;

        todosOsNos.push(folha1, folha2, pai);
        return pai;
      }

      const meio = Math.ceil(participantes.length / 2);
      const esquerda = construirArvore(participantes.slice(0, meio));
      const direita = construirArvore(participantes.slice(meio));

      const pai = { key: id++, name: '', parent: undefined };

      esquerda.parent = pai.key;
      direita.parent = pai.key;

      todosOsNos.push(pai);
      return pai;
    }

    const raiz = construirArvore(competidores);

    // Aqui garantimos que raiz.parent seja undefined (raiz única)
    raiz.parent = undefined;

    // Ajustar os outros para que ninguém mais tenha parent undefined (não raiz)
    todosOsNos.forEach(no => {
      if (no !== raiz && (no.parent === undefined || no.parent === null)) {
        no.parent = raiz.key; // pendura no nó raiz, evita erro no GoJS
      }
    });

    return todosOsNos;
  }

  chaveamentoIdCounter = 1;

  adicionarChaveamento() {
    const novoChaveamento: any = {
      tempId: this.chaveamentoIdCounter++,
      id: 0, // Será atualizado após salvar no backend
      categoriaId: this.categoriaSelecionada!,
      nome: 'Novo Chaveamento',
      participantes: [],
      arvoreConfrontos: '' // Gera a árvore de confrontos inicial
    };

    this.chaveamentos.push(novoChaveamento);
    this.chaveamentosPorCategoria[this.categoriaSelecionada!] = this.chaveamentos;
  }

  removerChaveamento(id: number, tempId?: number): void {
    const confirmar = confirm("Tem certeza que deseja remover este chaveamento?");
    if (!confirmar) {
      alert("Remoção cancelada.");
      return;
    }

    if (id && id !== 0) {
      // Remove do banco e do array local
      this.chaveamentoService.deleteChaveamento(id).subscribe({
        next: () => {
          this.chaveamentos = this.chaveamentos.filter(c => c.id !== id);
          this.chaveamentosPorCategoria[this.categoriaSelecionada!] = this.chaveamentos;
          alert("Chaveamento removido com sucesso.");
        },
        error: (err) => {
          console.error('Erro ao remover chaveamento:', err);
          alert("Erro ao remover chaveamento. Verifique o console.");
        }
      });
    } else if (tempId) {
      // Apenas remove localmente
      this.chaveamentos = this.chaveamentos.filter(c => c.tempId !== tempId);
      this.chaveamentosPorCategoria[this.categoriaSelecionada!] = this.chaveamentos;
      alert("Chaveamento temporário removido com sucesso.");
    }
  }

  atualizarNomeChaveamento(id: number, tempId: number, novoNome: string) {
    const chaveamento = this.chaveamentos.find(c => c.id === id || c.tempId === tempId);
    if (chaveamento) chaveamento.nome = novoNome;
  }

  salvarChaveamentosDaCategoriaAtual(): void {
    if (!this.categoriaSelecionadaAtual || this.chaveamentos.length === 0) return;


    this.diagramas.forEach((diagrama, index) => {
      const chaveamento = this.chaveamentos[index];
      const arvore = diagrama.obterArvoreAtual(); // método que você cria no componente filho
      if (arvore) {
        console.log(`Chaveamento ${chaveamento.id} serializado:`, arvore);
        chaveamento.arvoreConfrontos = arvore; // Atualiza o objeto chaveamento
      }
    });

    this.chaveamentosPorCategoria[this.categoriaSelecionadaAtual] = this.chaveamentos;
  }


  restaurarChaveamentosDaCategoriaSelecionada(): void {
    const modelosSalvos = this.chaveamentosPorCategoria[this.categoriaSelecionada];

    if (modelosSalvos) {
      console.log('Modelos salvos para a categoria selecionada:', modelosSalvos);

      // Agora cada item já é um objeto chaveamento completo
      this.chaveamentos = modelosSalvos.map((chaveamento: any) => ({
        ...chaveamento, // já contém id, categoriaId, etc.
        tempId: 0,
        participantes: [], // caso ainda vá ser preenchido ou resetado
        // Garante que 'arvoreConfrontos' está em formato serializado (string)
        arvoreConfrontos: chaveamento.arvoreConfrontos || ''
      }));
      console.log('Chaveamentos restaurados:', this.chaveamentos);
    }
  }


  salvarChaveamento(chaveamento: any): void {
    const confirmar = confirm("Deseja realmente salvar este chaveamento?");
    if (!confirmar) {
      alert("Operação de salvamento cancelada.");
      return;
    }

    let index = this.chaveamentos.findIndex(c => c.id === chaveamento.id);
    if (index === -1) {
      index = this.chaveamentos.findIndex(c => c.tempId === chaveamento.tempId);
    }

    if (index !== -1) {
      this.salvarArvoreConfrontos(index);
    }

    const payload = {
      id: chaveamento.id > 0 ? chaveamento.id : 0,
      nome: chaveamento.nome,
      categoriaId: this.categoriaSelecionada,
      arvoreConfrontos: chaveamento.arvoreConfrontos
    };

    console.log('Salvando chaveamento:', payload);

    if (payload.id === 0) {
      // Novo chaveamento
      this.chaveamentoService.createChaveamento(payload).subscribe({
        next: (res) => {
          console.log('Chaveamento criado:', res);
          chaveamento.id = res.id;
          alert("Chaveamento criado com sucesso!");
        },
        error: (err) => {
          console.error('Erro ao criar chaveamento:', err);
          alert("Erro ao criar chaveamento. Verifique o console.");
        }
      });
    } else {
      // Atualizar chaveamento existente
      this.chaveamentoService.updateChaveamento(payload.id, payload).subscribe({
        next: (res) => {
          console.log('Chaveamento atualizado:', res);
          alert("Chaveamento atualizado com sucesso!");
        },
        error: (err) => {
          console.error('Erro ao atualizar chaveamento:', err);
          alert("Erro ao atualizar chaveamento. Verifique o console.");
        }
      });
    }
  }


  salvarArvoreConfrontos(index: number): void {
    const diagrama = this.diagramas.toArray()[index];
    const chaveamento = this.chaveamentos[index];

    if (!diagrama || !chaveamento) {
      console.warn(`Diagrama ou chaveamento não encontrado no índice ${index}`);
      return;
    }

    const arvore = diagrama.obterArvoreAtual();
    if (arvore) {
      console.log(`Chaveamento ${chaveamento.id} serializado:`, arvore);
      chaveamento.arvoreConfrontos = arvore;
    }
  }
}