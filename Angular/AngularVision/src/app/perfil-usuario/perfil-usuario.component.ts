import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';
import { GeoNamesService } from '../services/geonames.service';

@Component({
  selector: 'app-perfil-usuario',
  templateUrl: './perfil-usuario.component.html',
  styleUrls: ['./perfil-usuario.component.css'],
})
export class PerfilUsuarioComponent implements OnInit {
  usuario: any = {};
  countryName: string = '';
  imagemSelecionada: File | null = null;

  // Flags para edição inline
  editandoNome = false;
  editandoSobrenome = false;
  editandoNascimento = false;
  editandoLocalizacao = false;

  // Listas para dropdowns de localização
  listaPaises: any;
  listaEstados: any;
  listaCidades: any;

  constructor(private userService: UserService, private http: HttpClient,
    private geonamesService: GeoNamesService, private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.carregarUsuario();

    setTimeout(() => {
      //this.form.control.markAsTouched();
      this.cdr.detectChanges(); // Detecta as alterações manualmente após a inicialização do formulário
    });

    // Carrega a lista de países
    this.geonamesService.getAllCountries().subscribe(
      paises => {
        this.listaPaises = paises;
        if (this.usuario.pais) {
          const pais = this.listaPaises?.geonames.find((country: any) => country.countryCode === this.usuario.pais);
          if (pais) {
            this.countryName = pais.countryName;
            this.geonamesService.getStatesByCountry(pais.geonameId).subscribe(
              estados => {
                this.listaEstados = estados;
                if (this.usuario.estado) {
                  const estado = this.listaEstados?.geonames.find((state: any) => state.name === this.usuario.estado);
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

  carregarUsuario(): void {
    this.userService.getUsuarioLogado().subscribe((data) => {
      this.usuario = data;
      this.usuario.imagemUrl = this.usuario.imagemUrl?.startsWith('http')
        ? this.usuario.imagemUrl
        : `http://localhost:5000/${this.usuario.imagemUrl}`;
    },
      error => {
        console.error("Erro ao carregar o usuário", error);
      }
    );
  }

  atualizarUsuario(): void {
    this.userService.updateUser(this.usuario.id, this.usuario).subscribe(() => {
      alert("Usuário atualizado com sucesso");
    }, error => {
      console.log(error);
      alert(error.error);
    });
  }

  selecionarImagem(event: any): void {
    this.imagemSelecionada = event.target.files[0];
  }

  uploadImagem(): void {
    if (!this.imagemSelecionada) return;
    this.userService.uploadImagem(this.usuario.id, this.imagemSelecionada).subscribe(
      (response) => {
        console.log("Imagem enviada com sucesso!", response);
        this.usuario.imagemUrl = response.imagemUrl; // Atualiza a imagem na interface

        this.usuario.imagemUrl = this.usuario.imagemUrl?.startsWith('http')
          ? this.usuario.imagemUrl
          : `http://localhost:5000/${this.usuario.imagemUrl}`;
      },
      (error) => {
        console.error("Erro ao fazer upload da imagem", error);
      }
    );
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
    this.usuario.estado = ''; // Limpa o estado selecionado
    this.usuario.cidade = ''; // Limpa a cidade selecionada

    // Obtém os estados/províncias do país selecionado
    this.carregarEstados();
  }

  carregarEstados(): void {
    const pais = this.listaPaises?.geonames.find((country: any) => country.countryCode === this.usuario.pais);
    if (!pais) return;

    this.countryName = pais.countryName; // Atualiza o nome do país selecionado
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
    this.usuario.cidade = ''; // Limpa a cidade selecionada

    // Obtém as cidades do estado selecionado
    this.carregarCidades();
  }

  carregarCidades(): void {
    const estado = this.listaEstados?.geonames.find((state: any) => state.name === this.usuario.estado);
    if (!estado) return;

    this.geonamesService.getCitiesByState(estado.geonameId).subscribe(
      (cidades: any) => {
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
