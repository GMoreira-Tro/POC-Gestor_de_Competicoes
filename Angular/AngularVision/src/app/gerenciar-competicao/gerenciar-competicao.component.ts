import { Component, ElementRef, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriaService } from '../services/categoria.service';
import { InscricaoService } from '../services/inscricao.service';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { CompetidorService } from '../services/competidor.service';
import * as go from 'gojs';

@Component({
  selector: 'app-gerenciar-competicao',
  templateUrl: './gerenciar-competicao.component.html',
  styleUrls: ['./gerenciar-competicao.component.css']
})
export class GerenciarCompeticaoComponent implements OnInit, AfterViewInit {
  @ViewChild('diagramaRef', { static: false }) diagramaRef!: ElementRef;

  competicaoId!: number;
  categorias: any[] = [];
  categoriaSelecionada: any;
  inscricoes: any[] = [];
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
}