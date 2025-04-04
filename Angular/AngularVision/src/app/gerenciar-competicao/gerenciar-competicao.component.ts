import { Component, OnInit } from '@angular/core';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../services/user.service';
import { ConfrontoService } from '../services/confronto.service';
import { ChaveamentoService } from '../services/chaveamento.service';
import { InscricaoService } from '../services/inscricao.service';
import { ConfrontoInscricaoService } from '../services/confrontoInscricao.service';

@Component({
  selector: 'app-gerenciar-competicao',
  templateUrl: './gerenciar-competicao.component.html',
  styleUrls: ['./gerenciar-competicao.component.css']
})
export class GerenciarCompeticaoComponent implements OnInit {
  competicao: any = {};
  categorias: any[] = [];
  categoriaSelecionada: any = undefined;
  chaveamentos: any[] = [];
  inscricoes: any[] = [];
  novoConfronto: any = { equipeA: '', equipeB: '', dataInicio: '', local: '' };
  novoChaveamento: any = { nome: '', etapas: [] };
  competicaoId!: number;

  constructor(
    private competicaoService: CompeticaoService,
    private categoriaService: CategoriaService,
    private userService: UserService,
    private confrontoService: ConfrontoService,
    private confrontoInscricaoService: ConfrontoInscricaoService,
    private chaveamentoService: ChaveamentoService,
    private inscricaoService: InscricaoService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.competicaoId = Number(id);
      this.carregarCompeticao(this.competicaoId);
    }
  }

  carregarCompeticao(id: number): void {
    this.competicaoService.getCompeticao(id).subscribe(
      competicao => {
        this.userService.getUsuarioLogado().subscribe(usuario => {
          const userId = usuario.id;
          if (competicao.criadorUsuarioId !== userId) {
            console.log("Usuário tentou editar uma competição que não lhe pertence.");
            this.router.navigate(['/']);
            return;
          }
        });
        this.competicao = competicao;
      },
      error => console.log('Erro ao carregar competição:', error)
    );

    this.categoriaService.getCategoriasPorCompeticao(id).subscribe(
      categorias => {
        this.categorias = categorias;
      },
      error => console.log('Erro ao carregar categorias:', error)
    );
  }

  onCategoriaSelecionada(): void {
    if (!this.categoriaSelecionada?.id) return;

    console.log('Categoria selecionada:', this.categoriaSelecionada.id);
    this.inscricaoService.getInscricoesPorCategoria(this.categoriaSelecionada.id).subscribe(
      inscricoes => {
        this.inscricoes = inscricoes;
        console.log('Inscrições:', inscricoes);
      },
      error => console.error('Erro ao carregar inscrições:', error)
    );

    this.chaveamentoService.getChaveamentosPorCategoria(this.categoriaSelecionada.id).subscribe(
      chaveamentos => {
        this.chaveamentos = chaveamentos;
        console.log('Chaveamentos:', chaveamentos);
        for (const chaveamento of this.chaveamentos) {
          this.confrontoService.getConfrontosPorChaveamento(chaveamento.id).subscribe(
            confrontos => {
              chaveamento.confrontos = confrontos;
              console.log('Confrontos:', confrontos);
              for (const confronto of chaveamento.confrontos) {
                this.confrontoInscricaoService.getConfrontoInscricoesPorConfronto(confronto.id).subscribe(
                  confrontoInscricoes => {
                    confronto.confrontoInscricoes = confrontoInscricoes;
                  },
                  error => console.error('Erro ao carregar inscrições de confronto:', error)
                );
              }
            },
            error => console.error('Erro ao carregar confrontos:', error)
          );
        }
      },
      error => console.error('Erro ao carregar chaveamentos:', error)
    );
  }

  carregarChaveamentos(): void {
    // Placeholder por enquanto
    this.chaveamentos = [
      { nome: 'Chave Principal', etapas: [{ nome: 'Quartas de Final', confrontos: [] }] }
    ];
  }

  criarConfronto(): void {
    if (this.novoConfronto.equipeA && this.novoConfronto.equipeB && this.novoConfronto.dataInicio && this.novoConfronto.local) {
      const confrontoParaSalvar = {
        dataInicio: this.novoConfronto.dataInicio,
        local: this.novoConfronto.local,
        confrontoInscricoes: [
          { inscricaoId: this.novoConfronto.equipeA },
          { inscricaoId: this.novoConfronto.equipeB }
        ]
      };

      // this.confrontoService.createConfronto(confrontoParaSalvar).subscribe(
      //   confrontoSalvo => {
      //     this.confrontos.push(confrontoSalvo);
      //     this.novoConfronto = { equipeA: '', equipeB: '', dataInicio: '', local: '' };
      //   },
      //   error => console.error('Erro ao criar confronto:', error)
      // );
    }
  }

  // removerConfronto(index: number): void {
  //   this.confrontos.splice(index, 1);
  // }

  criarChaveamento(): void {
    if (this.novoChaveamento.nome) {
      this.chaveamentos.push({ ...this.novoChaveamento, etapas: [] });
      this.novoChaveamento = { nome: '', etapas: [] };
    }
  }

  adicionarEtapaAoChaveamento(chaveamentoIndex: number, etapaNome: string): void {
    if (etapaNome) {
      this.chaveamentos[chaveamentoIndex].etapas.push({ nome: etapaNome, confrontos: [] });
    }
  }

  adicionarConfrontoNaEtapa(chaveamentoIndex: number, etapaIndex: number, confronto: any): void {
    if (confronto.equipeA && confronto.equipeB && confronto.dataInicio && confronto.local) {
      this.chaveamentos[chaveamentoIndex].etapas[etapaIndex].confrontos.push({ ...confronto });
    }
  }
}
