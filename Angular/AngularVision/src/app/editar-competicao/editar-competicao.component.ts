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

  competicao: any = {};
  listaPaises: any;
  listaEstados: any;
  listaCidades: any;
  idCategoriasParaDeletar: number[] = [];
  primeiroLoad = true;
  categorias: Categoria[] = [];
  imagemSelecionada: File | null = null;

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
        if (competicao.bannerImagem) {
          this.competicao.bannerImagem = this.competicao.bannerImagem?.startsWith('http')
            ? this.competicao.bannerImagem
            : `http://localhost:5000/${this.competicao.bannerImagem}`;
        }
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

    for (let i = 0; i < this.categorias.length; i++) {
      const categoria = this.categorias[i];
      if (categoria.nome === '') {
        alert('Todas as categorias devem conter um nome.');
        return;
      }
      if (categoria.valorInscricao < 15.99) {
        alert('O valor de inscrição das categorias deve ser maior ou igual a R$ 15,99');
        return;
      }
    }

    console.log(this.competicao);

    this.competicaoService.updateCompeticao(this.competicao.id, this.competicao).subscribe(
      async () => {

        await this.uploadImagem();

        this.categorias.forEach(async categoria => {
          if (categoria.id === 0) {
            await this.categoriaService.createCategoria(categoria).subscribe(
              () => {
              },
              error => console.log("Erro ao criar categoria: ", error)
            )
          }
          else {
            await this.categoriaService.updateCategoria(categoria.id, categoria).subscribe(
              () => {
              },
              error => console.log("Erro ao atualizar categoria: ", error)
            )
          };
        });

        this.idCategoriasParaDeletar.forEach(async id => {
          if (id !== 0) {
            await this.categoriaService.deleteCategoria(id).subscribe(
              () => {
              },
              error => console.log("Erro ao deletar categoria: ", error)
            );
          }
        });

        alert("Competição atualizada com sucesso!");
        this.router.navigate(['/minhas-competicoes']);
      },
      error => {
        console.log('Erro ao atualizar competição:', error);
        alert('Erro ao atualizar a competição. Tente novamente.');
      }
    );

  }

  selecionarImagemBanner(event: any): void {
    this.imagemSelecionada = event.target.files[0] as File;
  }

  async uploadImagem(): Promise<void> {
    if (!this.imagemSelecionada) return;
    this.competicaoService.uploadImagem(this.competicao.id, this.imagemSelecionada).subscribe(
      () => {
      },
      (error) => {
        console.error("Erro ao fazer upload da imagem", error);
      }
    );
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