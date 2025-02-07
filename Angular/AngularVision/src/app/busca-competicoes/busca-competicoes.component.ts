import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CompeticaoService } from '../services/competicao.service';
import { GeoNamesService } from '../services/geonames.service';

@Component({
  selector: 'app-buscar-competicoes',
  templateUrl: 'busca-competicoes.component.html',
  styleUrls: ['./busca-competicoes.component.css']
})
export class BuscaCompeticoesComponent implements OnInit {
  filtro: any = {
    nome: '',
    modalidade: '',
    descricao: '',
    pais: '',
    estado: '',
    cidade: ''
  };

  competicoes: any[] = [];
  listaPaises: any;
  listaEstados: any;
  listaCidades: any;

  constructor(private http: HttpClient, private competicaoService: CompeticaoService,
    private geonamesService: GeoNamesService, private cdr: ChangeDetectorRef
  ) {}

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

  // Função que chama a API do GeoNames para buscar Estado e Cidade com base no País
  buscarEstadoCidade(): void {
    this.geonamesService.getAllCountries().subscribe(paises => {
      this.listaPaises = paises;
    });
  }

  buscarCompeticoes(): void {
    console.log('Filtro:', this.filtro);
    this.competicaoService.buscarCompeticoes(this.filtro).subscribe(
      (res) => {
        console.log('Competições encontradas:', res);
        this.competicoes = res;
      },
      (error) => {
        console.error('Erro ao buscar competições:', error);
      }
    );
  }

  // Função chamada quando o país selecionado é alterado
  onCountryChange() {
    // Limpa a lista de estados
    this.listaEstados = [];
    this.listaCidades = [];

    // Obtém os estados/províncias do país selecionado
    const pais = this.listaPaises?.geonames.find((country: any) => country.countryCode === this.filtro.pais);
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
    const estado = this.listaEstados?.geonames.find((state: any) => state.name === this.filtro.estado);
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
  carregarMais(): void {
    // Lógica para carregar mais competições se houver paginação
  }
}
