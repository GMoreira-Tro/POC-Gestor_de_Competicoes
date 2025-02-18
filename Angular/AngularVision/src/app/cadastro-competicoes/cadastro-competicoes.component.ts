import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { GeoNamesService } from '../services/geonames.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-cadastro-competicoes',
  templateUrl: 'cadastro-competicoes.component.html',
  styleUrls: ['./cadastro-competicoes.component.css']
})
export class CadastroCompeticoesComponent implements OnInit {
  @ViewChild('form') form!: NgForm;

  competicao: any = {
    titulo: '',
    descricao: '',
    modalidade: '',
    pais: '',
    estado: '',
    cidade: '',
    bannerImagem: null,
    id: 0,
    dataInicio: new Date(),
    dataFim: new Date(),
    criadorUsuarioId: 0,  // Ajustar depois para o ID do usuário logado
    status: 0
  };
  categorias: Categoria[] = [];
  imagemSelecionada: File | null = null;

  listaPaises: any;
  listaEstados: any;
  listaCidades: any;

  constructor(private competicaoService: CompeticaoService, private categoriaService: CategoriaService,
    private geonamesService: GeoNamesService,
    private router: Router,
    private userService: UserService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    // Inicializa o formulário após a exibição da visualização do componente
    setTimeout(() => {
      //this.form.control.markAsTouched();
      this.cdr.detectChanges(); // Detecta as alterações manualmente após a inicialização do formulário
    });

    this.userService.getUsuarioLogado().subscribe(usuario => {
      this.competicao.criadorUsuarioId = usuario.id;
    }
    );

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
    if (this.categorias.length === 0) {
      alert("A competição deve ter pelo menos uma categoria.");
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

    this.competicao.status = 1; // Ajusta o status para publicada

    this.competicaoService.createCompeticao(this.competicao).subscribe(
      async novaCompeticao => {

        await this.uploadImagem();

        this.categorias.forEach(async categoria => {
          categoria.competicaoId = novaCompeticao.id;
          await this.categoriaService.createCategoria(categoria).subscribe(
            () => {
            },
            error => console.log("Erro ao criar categoria: ", error)
          );
        });

        this.router.navigate(['/minhas-competicoes']);
        alert("Competição criada com sucesso!");
      },
      error => {
        console.log("Erro ao criar competição: ", error);
        alert("Erro ao criar a competição. Tente novamente.");
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

  adicionarCategoria(): void {
    const novaCategoria: Categoria = {
      id: 0,  // O backend deve gerar esse ID
      nome: '',
      descricao: '',
      competicaoId: 0,
      valorInscricao: 0,
      inscricoes: []
    };
    this.categorias.push(novaCategoria);
  }

  removerCategoria(index: number): void {
    this.categorias.splice(index, 1);
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
    this.competicao.estado = ''; // Limpa o estado selecionado
    this.competicao.cidade = ''; // Limpa a cidade selecionada

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
    this.competicao.cidade = ''; // Limpa a cidade selecionada

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
