import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { GeoNamesService } from '../services/geonames.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-editar-competicao',
  templateUrl: 'editar-competicao.component.html',
  styleUrls: ['./editar-competicao.component.css']
})
export class EditarCompeticaoComponent implements OnInit {
  @ViewChild('form') form!: NgForm;

  competicao: Competicao = {} as Competicao;
  listaPaises: any;
  listaEstados: any;
  listaCidades: any;
  idCategoriasParaDeletar: number[] = [];
  primeiroLoad = true;
  categorias: Categoria[] = [];

  constructor(
    private competicaoService: CompeticaoService,
    private categoriaService: CategoriaService,
    private geonamesService: GeoNamesService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.carregarCompeticao(Number(id));
    }

    // Carrega a lista de países
    this.geonamesService.getAllCountries().subscribe(
      paises => {
        this.listaPaises = paises;
        if (this.competicao.pais) {
          const pais = this.listaPaises?.geonames.find((country: any) => country.countryCode === this.competicao.pais);
          if (pais) {
            this.geonamesService.getStatesByCountry(pais.geonameId).subscribe(
              estados => {
                this.listaEstados = estados;
                if (this.competicao.estado) {
                  const estado = this.listaEstados?.geonames.find((state: any) => state.name === this.competicao.estado);
                  if (estado) {
                    this.geonamesService.getCitiesByState(estado.geonameId).subscribe(
                      cidades => {
                        this.listaCidades = cidades;
                        this.cdr.detectChanges();
                      },
                      error => {
                        console.error('Erro ao obter as cidades:', error);
                      }
                    );
                  }
                }
              },
              error => {
                console.error('Erro ao obter os estados:', error);
              }
            );
          }
        }
      },
      error => {
        console.error('Erro ao obter a lista de países:', error);
      }
    );
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
        }
      );
        this.competicao = competicao;
        this.onCountryChange();
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

  onSubmit(): void {
    if (!this.competicao) return;

    if (this.categorias.length === 0) {
      alert('A competição deve ter pelo menos uma categoria.');
      return;
    }

    console.log(this.competicao);

    this.competicaoService.updateCompeticao(this.competicao.id, this.competicao).subscribe(
      () => {
        let categoriasCriadas = 0; // Contador de categorias criadas

        this.categorias.forEach(categoria => {
          categoriasCriadas++;
          if (categoria.id === 0) {
            this.categoriaService.createCategoria(categoria).subscribe(
              () => {
                // Se todas as categorias foram criadas, exibe alerta e redireciona
                if (categoriasCriadas === this.categorias.length) {
                  alert("Competição atualizada com sucesso!");
                  this.router.navigate(['/']); // Redireciona para a tela inicial
                }
              },
              error => console.log("Erro ao criar categoria: ", error)
            )
          }
          else {
            this.categoriaService.updateCategoria(categoria.id, categoria).subscribe(
              () => {
                if (categoriasCriadas === this.categorias.length) {
                  alert("Competição criada com sucesso!");
                  this.router.navigate(['/']); // Redireciona para a tela inicial
                }
              },
              error => console.log("Erro ao atualizar categoria: ", error)
            )
          };
        });

        this.idCategoriasParaDeletar.forEach(id => {
          if (id !== 0) {
            this.categoriaService.deleteCategoria(id).subscribe(
              () => {
                console.log("Categoria deletada com sucesso!");
              },
              error => console.log("Erro ao deletar categoria: ", error)
            );
          }
        });

      },
      error => {
        console.log('Erro ao atualizar competição:', error);
        alert('Erro ao atualizar a competição. Tente novamente.');
      }
    );
  }

  onFileSelected(event: any): void {
    if (this.competicao) {
      this.competicao.bannerImagem = event.target.files[0] as File;
    }
  }

  onCountryChange(): void {
    if (this.primeiroLoad) {
      this.primeiroLoad = false;
      return;
    }

    this.listaEstados = [];
    this.listaCidades = [];
    this.competicao.estado = '';
    this.competicao.cidade = '';

    const pais = this.listaPaises?.geonames.find((country: any) => country.countryCode === this.competicao.pais);
    if (!pais) return;

    this.geonamesService.getStatesByCountry(pais.geonameId).subscribe(
      (estados: any) => {
        this.listaEstados = estados;
        this.cdr.detectChanges();
      },
      error => {
        console.error('Erro ao obter os estados:', error);
      }
    );
  }

  onStateChange(): void {
    this.listaCidades = [];
    this.competicao.cidade = '';

    const estado = this.listaEstados?.geonames.find((state: any) => state.name === this.competicao.estado);
    if (!estado) return;

    this.geonamesService.getCitiesByState(estado.geonameId).subscribe(
      (cidades: any) => {
        this.listaCidades = cidades;
        this.cdr.detectChanges();
      },
      error => {
        console.error('Erro ao obter as cidades:', error);
      }
    );
  }

  adicionarCategoria(): void {
    const novaCategoria: Categoria = {
      id: 0,  // O backend deve gerar esse ID
      nome: '',
      descricao: '',
      competicaoId: this.competicao.id,
      valorInscricao: 0,
      inscricoes: []
    };
    this.categorias.push(novaCategoria);
  }

  removerCategoria(index: number): void {
    console.log(this.categorias[index]);

    this.idCategoriasParaDeletar.push(this.categorias[index].id);
    this.categorias.splice(index, 1);
  }

  cancel(): void {
    this.router.navigate(['/minhas-competicoes']);
  }
}