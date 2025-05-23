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
    const competidores = this.inscricoes.map(i => {
      return i.inscricaoInfo.nomeCompetidor;
    });

    // Gerar IDs únicos para os nós
    let id = 1;
    const folhas = competidores.map(nome => ({ key: id++, name: nome, parent: undefined as number | undefined }));
    const semi1 = { key: id++, name: '', parent: id }; // pai será a final
    const semi2 = { key: id++, name: '', parent: id };
    const final = { key: id++, name: '' };

    // Atribuir pai para folhas
    folhas[0].parent = semi1.key;
    folhas[1].parent = semi1.key;
    folhas[2].parent = semi2.key;
    folhas[3].parent = semi2.key;
    semi1.parent = final.key;
    semi2.parent = final.key;

    return [...folhas, semi1, semi2, final];
  }
}