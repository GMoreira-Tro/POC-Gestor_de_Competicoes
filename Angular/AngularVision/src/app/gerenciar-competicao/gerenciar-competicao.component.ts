import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriaService } from '../services/categoria.service';
import { InscricaoService } from '../services/inscricao.service';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { CompetidorService } from '../services/competidor.service';

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

  chaveamentos: any[] = [];

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

    this.carregouInscricao = false;
    this.inscricaoService.getInscricoesPorCategoria(this.categoriaSelecionada).subscribe(
      res => {
        this.inscricoes = res.filter(i => i.status === 1);
        const inscricoesQuant = this.inscricoes.length;
        let inscricoesCarregadas = 0;

        this.inscricoes.forEach(inscricao => {
          this.inscricaoService.getInfoInscricao(inscricao.id).subscribe(inscricaoInfo => {
            inscricao.inscricaoInfo = inscricaoInfo;
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
      confrontos: []
    };
    this.chaveamentos.push(novo);
  }

  adicionarConfronto(chaveamentoIndex: number, equipeA: any, equipeB: any): void {
    const confronto = {
      equipeA,
      equipeB,
      vencedor: null
    };
    this.chaveamentos[chaveamentoIndex].confrontos.push(confronto);
  }
}
