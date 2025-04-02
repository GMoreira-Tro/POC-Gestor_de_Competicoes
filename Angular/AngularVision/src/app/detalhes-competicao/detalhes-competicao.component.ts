import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { UserService } from '../services/user.service';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface BracketMatch {
  id: number;
  round: number;
  player1: string;
  player2: string;
  winner?: string;
  nextMatchId?: number;
}

@Component({
  selector: 'app-detalhes-competicao',
  templateUrl: './detalhes-competicao.component.html',
  styleUrls: ['./detalhes-competicao.component.css']
})
export class DetalhesCompeticaoComponent implements OnInit {
  competicao: Competicao | null = null;
  categorias: Categoria[] = [];
  brackets: { [key: string]: BracketMatch[] } = {};
  isOrganizador: boolean = false;
  bracketsPublicos: boolean = false;
  competicaoId: number = 0;
  userId: number = 0;

  constructor(
    private route: ActivatedRoute,
    private competicaoService: CompeticaoService,
    private categoriaService: CategoriaService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.competicaoId = +params['id'];
      this.loadCompeticao();
      this.loadCategorias();
      this.checkOrganizador();
    });
  }

  loadCompeticao(): void {
    this.competicaoService.getCompeticao(this.competicaoId).subscribe(
      competicao => {
        this.competicao = competicao;
        this.loadBrackets();
      },
      error => {
        console.error('Erro ao carregar competição:', error);
      }
    );
  }

  loadCategorias(): void {
    this.categoriaService.getCategoriasPorCompeticao(this.competicaoId).subscribe(
      (categorias: Categoria[]) => {
        this.categorias = categorias;
      },
      error => {
        console.error('Erro ao carregar categorias:', error);
      }
    );
  }

  checkOrganizador(): void {
    this.userService.getUsuarioLogado().subscribe(
      usuario => {
        this.userId = usuario.id;
        this.isOrganizador = this.competicao?.criadorUsuarioId === this.userId;
      }
    );
  }

  loadBrackets(): void {
    // Aqui você implementaria a lógica para carregar os brackets do backend
    // Por enquanto, vamos criar um exemplo
    this.brackets = {
      'Categoria A': [
        {
          id: 1,
          round: 1,
          player1: 'Jogador 1',
          player2: 'Jogador 2',
          nextMatchId: 3
        },
        {
          id: 2,
          round: 1,
          player1: 'Jogador 3',
          player2: 'Jogador 4',
          nextMatchId: 3
        },
        {
          id: 3,
          round: 2,
          player1: '',
          player2: '',
          nextMatchId: 5
        }
      ]
    };
  }

  toggleBracketsPublicos(): void {
    if (this.isOrganizador) {
      this.bracketsPublicos = !this.bracketsPublicos;
      // Aqui você implementaria a lógica para salvar no backend
    }
  }

  podeVerBrackets(): boolean {
    return this.bracketsPublicos || this.isOrganizador;
  }

  getUniqueRounds(matches: BracketMatch[]): number[] {
    return [...new Set(matches.map(match => match.round))].sort((a, b) => a - b);
  }

  getMatchesByRound(matches: BracketMatch[], round: number): BracketMatch[] {
    return matches.filter(match => match.round === round);
  }
}
