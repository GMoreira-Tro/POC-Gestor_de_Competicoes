import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { UserService } from '../services/user.service';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { ChaveamentoService } from '../services/chaveamento.service';

interface BracketMatch {
  nomeChaveamento: string;
  arvoreConfrontos: string;
}

@Component({
  selector: 'app-detalhes-competicao',
  templateUrl: './detalhes-competicao.component.html',
  styleUrls: ['./detalhes-competicao.component.css']
})
export class DetalhesCompeticaoComponent implements OnInit {
  competicao: Competicao | null = null;
  categorias: Categoria[] = [];
  bracketsPorCategoria: { [categoriaNome: string]: BracketMatch[] } = {};
  isOrganizador: boolean = false;
  bracketsPublicos: boolean = false;
  competicaoId: number = 0;
  userId: number = 0;

  constructor(
    private route: ActivatedRoute,
    private competicaoService: CompeticaoService,
    private categoriaService: CategoriaService,
    private chaveamentoService: ChaveamentoService, // Supondo que o serviço de chaveamento seja o mesmo
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.competicaoId = +params['id'];
      this.loadCompeticao();
      this.loadCategorias();
    });
  }

  loadCompeticao(): void {
    this.competicaoService.getCompeticao(this.competicaoId).subscribe({
      next: competicao => {
        this.competicao = competicao;
        this.checkOrganizador(); // só depois da competição carregada
      },
      error: err => console.error('Erro ao carregar competição:', err)
    });
  }

  loadCategorias(): void {
    this.categoriaService.getCategoriasPorCompeticao(this.competicaoId).subscribe({
      next: categorias => {
        this.categorias = categorias;
        this.carregarChaveamentosPorCategoria();
      },
      error: err => console.error('Erro ao carregar categorias:', err)
    });
  }

  checkOrganizador(): void {
    this.userService.getUsuarioLogado().subscribe({
      next: usuario => {
        this.userId = usuario.id;
        this.isOrganizador = this.competicao?.criadorUsuarioId === this.userId;
      },
      error: err => console.error('Erro ao verificar organizador:', err)
    });
  }

  toggleBracketsPublicos(): void {
    if (!this.isOrganizador) return;
    this.bracketsPublicos = !this.bracketsPublicos;
    // Chamada ao backend para salvar visibilidade pode ser feita aqui
  }

  podeVerBrackets(): boolean {
    return this.bracketsPublicos || this.isOrganizador;
  }

  carregarChaveamentosPorCategoria(): void {
    this.bracketsPorCategoria = {};

    this.categorias.forEach(categoria => {
      this.chaveamentoService.getChaveamentosPorCategoria(categoria.id).subscribe(chaveamentos => {
        this.bracketsPorCategoria[categoria.nome] = chaveamentos.map(ch => ({
          nomeChaveamento: ch.nome,
          arvoreConfrontos: ch.arvoreConfrontos // ou 'arvore', dependendo do que vem do backend
        }));
      });
    });
  }
}
