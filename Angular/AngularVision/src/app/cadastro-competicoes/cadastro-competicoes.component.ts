import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { GeoNamesService } from '../services/geonames.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { CompetidorService } from '../services/competidor.service';
import { Inscricao } from '../interfaces/Inscricao';
import { InscricaoService } from '../services/inscricao.service';
import { PaymentService } from '../services/payment.service';
import { countriesDict } from '../utils/paisesDict';

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
    status: 0,
    chavePix: ''
  };
  categorias: any[] = [];
  mostrarModal = false;
  categoriasMap: { [key: number]: any } = {};
  competidoresUsuario: any[] = [];
  imagemSelecionada: File | null = null;
  etapaAtual = 1;
  tempIdCategoria = 0;
  qrCodeUrl = '';
  usuario: any;
  txid: string = ''; // ID da transação PIX
  pollingInterval: any; // Armazena o intervalo do polling
  paymentCompleted: boolean = false;
  isLoading: boolean = false;

  listaPaises: any;
  listaEstados: any;
  listaCidades: any;

  constructor(private competicaoService: CompeticaoService, private categoriaService: CategoriaService,
    private geonamesService: GeoNamesService,
    private router: Router,
    private userService: UserService,
    private competidorService: CompetidorService,
    private inscricaoService: InscricaoService,
    private paymentService: PaymentService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    // Inicializa o formulário após a exibição da visualização do componente
    setTimeout(() => {
      //this.form.control.markAsTouched();
      this.cdr.detectChanges(); // Detecta as alterações manualmente após a inicialização do formulário
    });

    this.userService.getUsuarioLogado().subscribe(usuario => {
      this.competicao.criadorUsuarioId = usuario.id;
      this.usuario = usuario;
      this.competidorService.buscarCompetidoresDoUsuario(usuario.id).subscribe(competidores => {
        this.competidoresUsuario = competidores;
      });
    }
    );

    // Carrega a lista de países do mundo
    this.geonamesService.getAllCountries().subscribe(
      paises => {
        this.listaPaises = paises;
        for (let i = 0; i < this.listaPaises.geonames.length; i++) {
          this.listaPaises.geonames[i].countryName = countriesDict[this.listaPaises.geonames[i].countryName] ||
            this.listaPaises.geonames[i].countryName;
        }
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção dos países
      },
      error => {
        console.error('Erro ao obter a lista de países:', error);
      });
  }

  validarFormulario(): boolean {
    if (this.competicao.titulo === '') {
      alert('O título da competição é obrigatório.');
      return false;
    }
    if (this.competicao.modalidade === '') {
      alert('A modalidade da competição é obrigatória.');
      return false;
    }
    if (this.categorias.length === 0) {
      alert("A competição deve ter pelo menos uma categoria.");
      return false;
    }
    for (let i = 0; i < this.categorias.length; i++) {
      const categoria = this.categorias[i];
      if (categoria.nome === '') {
        alert('Todas as categorias devem conter um nome.');
        return false;
      }
      if (categoria.valorInscricao < 15.99) {
        alert('O valor de inscrição das categorias deve ser maior ou igual a R$ 15,99');
        return false;
      }
    }

    return true;
  }

  onSubmit(idPagamento: number): void {
    if (this.isLoading) {
      return;
    }
    this.isLoading = true;
    this.competicao.status = 1; // Ajusta o status para publicada

    this.competicaoService.createCompeticao(this.competicao).subscribe(
      async novaCompeticao => {

        this.categorias.forEach(async categoria => {
          const categoriaMap = this.categoriasMap[categoria.tempId];
          delete categoria.tempId;
          categoria.competicaoId = novaCompeticao.id;
          await this.categoriaService.createCategoria(categoria).subscribe(
            (categoriaCriada) => {
              if (categoriaMap.competidores && categoriaMap.competidores.length > 0) {
                categoriaMap.competidores.forEach((competidor: any) => {
                  const inscricao: Inscricao = {
                    id: 0, // O backend deve gerar esse ID
                    competidorId: competidor.id,
                    categoriaId: categoriaCriada.id,
                    status: 2, // Ajuste o status conforme necessário
                    pagamentoId: idPagamento,
                    posicao: 0,
                    wo: false,
                    premioResgatavelId: null
                  };

                  this.inscricaoService.cadastrarInscricao(inscricao).subscribe();
                });
              }
            },
            error => {
              this.isLoading = false;
              console.log("Erro ao criar categoria: ", error);
            }
          );
        });

        this.competicao = novaCompeticao;
        await this.uploadImagem();
      },
      error => {
        this.isLoading = false;
        console.log("Erro ao criar competição: ", error);
        alert("Erro ao criar a competição. Tente novamente.");
      }
    );

  }

  selecionarImagemBanner(event: any): void {
    this.imagemSelecionada = event.target.files[0] as File;
  }

  async uploadImagem(): Promise<void> {
    if (!this.imagemSelecionada) {
      this.router.navigate(['/minhas-competicoes']);
      alert("Competição criada com sucesso!");
      return;
    };
    this.competicaoService.uploadImagem(this.competicao.id, this.imagemSelecionada).subscribe(
      () => {
        this.router.navigate(['/minhas-competicoes']);
        alert("Competição criada com sucesso!");
      },
      (error) => {
        this.isLoading = false;
        console.error("Erro ao fazer upload da imagem", error);
      }
    );
  }

  adicionarCategoria(): void {
    const novaCategoria: any = {
      id: 0, // O backend deve gerar esse ID
      tempId: this.tempIdCategoria++, // ID temporário para identificar a categoria
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
    delete this.categoriasMap[this.categorias[index].tempId];
  }

  // Função que chama a API do GeoNames para buscar Estado e Cidade com base no País
  buscarEstadoCidade(): void {
    this.geonamesService.getAllCountries().subscribe(paises => {
      this.listaPaises = paises;
      for (let i = 0; i < this.listaPaises.geonames.length; i++) {
        this.listaPaises.geonames[i].countryName = countriesDict[this.listaPaises.geonames[i].countryName] ||
          this.listaPaises.geonames[i].countryName;
      }
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

  mudarEtapa(etapa: number): void {
    if (this.etapaAtual === 1 && !this.validarFormulario()) return;
    if (this.etapaAtual === 3 && this.competicao.chavePix === '') {
      alert("Informe a chave PIX para inscrição na competição.");
      return;
    }
    if (etapa === 4) {
      if (this.competicao.chavePix === '') {
        alert("Registre a chave PIX para inscrição na competição.");
        return;
      }
      clearInterval(this.pollingInterval); // Para o polling
      let quantidadeInscricoes = 0;
      Object.values(this.categoriasMap).forEach((categoria: any) => {
        quantidadeInscricoes += categoria.competidores.length;
      });
      if (quantidadeInscricoes === 0) {
        if (confirm("Nenhum competidor foi previamente inscrito. Deseja finalizar o cadastro da competição mesmo assim?")) {
          this.onSubmit(0);
        } else {
          etapa = 2;
        }
        return;
      }
      else
        this.gerarQRCode(quantidadeInscricoes);
    }

    this.etapaAtual = etapa;
  }

  abrirModalCompetidores(categoriaTempId: number): void {
    if (this.categoriasMap[categoriaTempId] === undefined)
      this.categoriasMap[categoriaTempId] = { mostrarModal: false, competidores: [] };
    for (let i = 0; i < this.competidoresUsuario.length; i++) {
      if (this.competidoresUsuario[i].selecionadoPorCategoria === undefined)
        this.competidoresUsuario[i].selecionadoPorCategoria = {};
      if (this.competidoresUsuario[i].selecionadoPorCategoria[categoriaTempId] === undefined)
        this.competidoresUsuario[i].selecionadoPorCategoria[categoriaTempId] = false;
    }
    this.categoriasMap[categoriaTempId].mostrarModal = true;
    console.log(categoriaTempId)
  }

  fecharModalCompetidores(categoriaTempId: number): void {
    this.categoriasMap[categoriaTempId].mostrarModal = false;
  }

  confirmarSelecaoCompetidores(categoriaTempId: number): void {
    const competidoresSelecionados = this.competidoresUsuario.filter(competidor => competidor.selecionadoPorCategoria[categoriaTempId]);
    this.categoriasMap[categoriaTempId].competidores = competidoresSelecionados.map(competidor => competidor);
    this.fecharModalCompetidores(categoriaTempId);
  }

  gerarQRCode(quantidadeInscricoes: number) {

    const valorOriginal = (9.99 * quantidadeInscricoes).toFixed(2);

    this.paymentService.generateQRCodeLocation({
      "calendario": {
        "expiracao": 36000
      },
      "devedor": {
        "cpf": this.usuario.cpfCnpj,
        "nome": this.usuario.nome,
      },
      "valor": {
        "original": valorOriginal
      },
      "chave": "a5d98ae0-2416-457f-86a1-c543e08c07a4",
      "solicitacaoPagador": "Informe o número ou identificador do pedido."
    }).subscribe(
      (response) => {
        try {
          const parsedResponse = JSON.parse(response.qrCodeUrl);
          console.log('Resposta do QR Code:', parsedResponse);
          this.txid = parsedResponse.txid; // Armazena o ID da transação PIX

          this.paymentService.generateQRCodeBase64(parsedResponse.loc.id).subscribe(
            (response) => {
              console.log(response);
              const base64Response = JSON.parse(response.base64QrCode);
              this.qrCodeUrl = base64Response.imagemQrcode;

              // Inicia a verificação periódica do pagamento
              this.checkPixStatusPeriodically();
            }
          );
        } catch (e) {
          console.error('Erro ao processar resposta do QR Code:', e);
        }
      },
      (error) => {
        console.error('Erro ao gerar QR Code:', error);
      }
    );
  }

  checkPixStatusPeriodically() {
    this.pollingInterval = setInterval(() => {
      this.paymentService.getPixPaymentByTxid(this.txid).subscribe(
        (response) => {
          const parsedResponse = JSON.parse(response.message);
          console.log('Resposta do pagamento:', parsedResponse);
          console.log('Status do pagamento:', parsedResponse.status);
          if (parsedResponse && parsedResponse.status === 'CONCLUIDA') {
            clearInterval(this.pollingInterval); // Para o polling
            this.paymentCompleted = true;
            this.paymentService.registerUserPayment(
              {
                txid: this.txid,
                pagadorId: this.usuario.id,
                favorecidoId: this.usuario.id,
                infoPagamento: 'Inscrição em competição de competidor próprio'
              }
            ).subscribe((pagamento) => {
              this.onSubmit(pagamento.id);
            });
          }
        },
        (error) => {
          console.error('Erro ao verificar status do pagamento:', error);
        }
      );
    }, 5000); // Intervalo de 5 segundos
  }
}
