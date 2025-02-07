import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { GeoNamesService } from '../services/geonames.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cadastro-competicoes',
  templateUrl: 'cadastro-competicoes.component.html',
  styleUrls: ['./cadastro-competicoes.component.css']
})
export class CadastroCompeticoesComponent implements OnInit {
  @ViewChild('form') form!: NgForm;
  
  competicao: Competicao = {
    titulo: '',
    descricao: '',
    modalidade: '',
    pais: '',
    estado: '',
    cidade: '',
    bannerImagem: undefined,
    categorias: [],
    id: 0,
    dataInicio: new Date(),
    dataFim: new Date(),
    criadorUsuarioId: 1,  // Ajustar depois para o ID do usuário logado
    status: 0
  };

  listaPaises: any;
  listaEstados: any;
  listaCidades: any;

  constructor(private competicaoService: CompeticaoService, private categoriaService: CategoriaService,
    private geonamesService: GeoNamesService,
    private router: Router,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    // Inicializa o formulário após a exibição da visualização do componente
    setTimeout(() => {
      //this.form.control.markAsTouched();
      this.cdr.detectChanges(); // Detecta as alterações manualmente após a inicialização do formulário
    });

    // Carrega a lista de países do mundo
    this.geonamesService.getAllCountries().subscribe(
      paises => {
        this.listaPaises = paises;
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção dos países
      },
      error => {
        console.error('Erro ao obter a lista de países:', error);
      });
   }

   onSubmit(): void {
    if (this.competicao.categorias.length === 0) {
      alert("A competição deve ter pelo menos uma categoria.");
      return;
    }
  
    this.competicao.status = 1; // Ajusta o status para publicada
    console.log(this.competicao);
  
    this.competicaoService.createCompeticao(this.competicao).subscribe(
      novaCompeticao => {  
        let categoriasCriadas = 0; // Contador de categorias criadas
  
        this.competicao.categorias.forEach(categoria => {
          categoria.competicaoId = novaCompeticao.id;
          this.categoriaService.createCategoria(categoria).subscribe(
            () => {
              categoriasCriadas++;
              // Se todas as categorias foram criadas, exibe alerta e redireciona
              if (categoriasCriadas === this.competicao.categorias.length) {
                alert("Competição criada com sucesso!");
                this.router.navigate(['/']); // Redireciona para a tela inicial
              }
            },
            error => console.log("Erro ao criar categoria: ", error)
          );
        });
      },
      error => {
        console.log("Erro ao criar competição: ", error);
        alert("Erro ao criar a competição. Tente novamente.");
      }
    );
  }  
  
  onFileSelected(event: any): void {
    this.competicao.bannerImagem = event.target.files[0] as File;
  }

  adicionarCategoria(): void {
    const novaCategoria: Categoria = {
      id: 0,  // O backend deve gerar esse ID
      nome: '',
      descricao: '',
      competicaoId: 0,
      inscricoes: []
    };
    this.competicao.categorias.push(novaCategoria);
  }

  removerCategoria(index: number): void {
    this.competicao.categorias.splice(index, 1);
  }

    // Função que chama a API do GeoNames para buscar Estado e Cidade com base no País
    buscarEstadoCidade(): void {
      this.geonamesService.getAllCountries().subscribe(paises => {
        this.listaPaises = paises;
      });
    }

    // Função chamada quando o país selecionado é alterado
  onCountryChange() {
    // Limpa a lista de estados
    this.listaEstados = [];
    this.listaCidades = [];

    // Obtém os estados/províncias do país selecionado
    const pais = this.listaPaises?.geonames.find((country: any) => country.countryCode === this.competicao.pais);
    if (!pais) return;

    this.geonamesService.getStatesByCountry(pais.geonameId).subscribe(
      (estados: any) => {
        // Extrai os nomes dos estados/províncias da resposta
        this.listaEstados = estados;
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção dos estados
      },
      error => {
        console.error('Erro ao obter os estados/províncias:', error);
        // Trate o erro conforme necessário
      }
    );
  }

  // Função chamada quando o estado selecionado é alterado
  onStateChange() {
    // Limpa a lista de cidades
    this.listaCidades = [];

    // Obtém as cidades do estado selecionado
    const estado = this.listaEstados?.geonames.find((state: any) => state.name === this.competicao.estado);
    if (!estado) return;

    this.geonamesService.getCitiesByState(estado.geonameId).subscribe(
      (cidades: any) => {
        // Extrai os nomes das cidades da resposta
        this.listaCidades = cidades;
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção das cidades
      },
      error => {
        console.error('Erro ao obter as cidades:', error);
        // Trate o erro conforme necessário
      }
    );
  }
}
