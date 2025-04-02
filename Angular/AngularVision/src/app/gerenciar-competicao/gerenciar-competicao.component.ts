import { Component, OnInit } from '@angular/core';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-gerenciar-competicao',
  templateUrl: './gerenciar-competicao.component.html',
  styleUrls: ['./gerenciar-competicao.component.css']
})
export class GerenciarCompeticaoComponent implements OnInit {
criarConfronto() {
throw new Error('Method not implemented.');
}

  competicao: any = {};
  categorias: any[] = [];
  categoriaSelecionada: any = {};
  confrontos: any[] = [];
  chaveamentos: any[] = [];
  novoConfronto: any = { equipeA: '', equipeB: '', data: '', local: '' };
  novoChaveamento: any = { nome: '', etapas: [] };

  constructor(
    private competicaoService: CompeticaoService,
    private categoriaService: CategoriaService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.carregarCompeticao(Number(id));
    }

    this.carregarConfrontos();
    this.carregarChaveamentos();
  }

  carregarCompeticao(id: number): void {
    this.competicaoService.getCompeticao(id).subscribe(
      competicao => {
        console.log(competicao);
        this.userService.getUsuarioLogado().subscribe(usuario => {
          const userId = usuario.id;
          if (competicao.criadorUsuarioId !== userId) {
            console.log("Usuário tentou editar uma competição que não lhe pertence.");
            this.router.navigate(['/']);
            return;
          }

        }
        );
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

  carregarConfrontos(): void {
    // Simulação de carregamento de confrontos (substituir por chamada de API)
    this.confrontos = [
      { equipeA: 'Equipe 1', equipeB: 'Equipe 2', data: '2023-10-01', local: 'Estádio A' },
      { equipeA: 'Equipe 3', equipeB: 'Equipe 4', data: '2023-10-02', local: 'Estádio B' }
    ];
  }

  carregarChaveamentos(): void {
    // Simulação de carregamento de chaveamentos (substituir por chamada de API)
    this.chaveamentos = [
      { nome: 'Chave Principal', etapas: [{ nome: 'Quartas de Final', confrontos: [] }] }
    ];
  }

  adicionarConfronto(): void {
    if (this.novoConfronto.equipeA && this.novoConfronto.equipeB && this.novoConfronto.data && this.novoConfronto.local) {
      this.confrontos.push({ ...this.novoConfronto });
      this.novoConfronto = { equipeA: '', equipeB: '', data: '', local: '' };
    }
  }

  removerConfronto(index: number): void {
    this.confrontos.splice(index, 1);
  }

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
    if (confronto.equipeA && confronto.equipeB && confronto.data && confronto.local) {
      this.chaveamentos[chaveamentoIndex].etapas[etapaIndex].confrontos.push({ ...confronto });
    }
  }
}