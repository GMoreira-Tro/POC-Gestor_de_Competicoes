import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { GeoNamesService } from '../services/geonames.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

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
  primeiroLoad = true;
  
  constructor(
    private competicaoService: CompeticaoService,
    private categoriaService: CategoriaService,
    private geonamesService: GeoNamesService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

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
        this.competicao = competicao;
        this.onCountryChange();
      },
      error => console.log('Erro ao carregar competição:', error)
    );
  }

  onSubmit(): void {
    if (!this.competicao) return;
    
    if (this.competicao.categorias.length === 0) {
      alert('A competição deve ter pelo menos uma categoria.');
      return;
    }
    
    console.log(this.competicao);
    
    this.competicaoService.updateCompeticao(this.competicao.id, this.competicao).subscribe(
      () => {
        alert('Competição atualizada com sucesso!');
        this.router.navigate(['/minhas-competicoes']);
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
    if(this.primeiroLoad) {
      alert('Primeiro load');
      this.primeiroLoad = false;
      return;
    }

    this.listaEstados = [];
    this.listaCidades = [];

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
      inscricoes: []
    };
    this.competicao.categorias.push(novaCategoria);
  }
  
  removerCategoria(index: number): void {
    if (!this.competicao) return;
    this.competicao.categorias.splice(index, 1);
  }
  
  cancel(): void {
    this.router.navigate(['/minhas-competicoes']);
  }
}