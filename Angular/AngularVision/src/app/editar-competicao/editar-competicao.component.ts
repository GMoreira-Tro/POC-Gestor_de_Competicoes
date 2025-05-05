import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { GeoNamesService } from '../services/geonames.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { PaymentService } from '../services/payment.service';
import { InscricaoService } from '../services/inscricao.service';
import { Inscricao } from '../interfaces/Inscricao';
import { CompetidorService } from '../services/competidor.service';
import { environment } from '../../environments/environment.prod';
import { countriesDict } from '../utils/paisesDict';

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
  categorias: any[] = [];
  imagemSelecionada: File | null = null;
  mostrarModal = false;
  categoriasMap: { [key: number]: any } = {};
  competidoresUsuario: any[] = [];
  etapaAtual = 1;
  tempIdCategoria = 0;
  qrCodeUrl = '';
  usuario: any;
  txid: string = ''; // ID da transação PIX
  pollingInterval: any; // Armazena o intervalo do polling
  paymentCompleted: boolean = false;

  constructor(
    private competicaoService: CompeticaoService,
    private categoriaService: CategoriaService,
    private geonamesService: GeoNamesService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
    private paymentService: PaymentService,
    private inscricaoService: InscricaoService,
    private competidorService: CompetidorService,
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
        for (let i = 0; i < this.listaPaises.geonames.length; i++) {
          this.listaPaises.geonames[i].countryName = countriesDict[this.listaPaises.geonames[i].countryName] ||
            this.listaPaises.geonames[i].countryName;
        }
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

    this.userService.getUsuarioLogado().subscribe(usuario => {
      this.usuario = usuario;
      this.competidorService.buscarCompetidoresDoUsuario(usuario.id).subscribe(competidores => {
        this.competidoresUsuario = competidores;
      });
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
            : `${environment.apiBaseUrl}/${this.competicao.bannerImagem}`;
        }
        this.onCountryChange();
      },
      error => console.log('Erro ao carregar competição:', error)
    );

    this.categoriaService.getCategoriasPorCompeticao(id).subscribe(
      categorias => {
        this.categorias = categorias;
        for (let i = 0; i < this.categorias.length; i++) {
          this.categorias[i].tempId = this.tempIdCategoria++;
        }
      },
      error => console.log('Erro ao carregar categorias:', error)
    );
  }

  onSubmit(idPagamento: number): void {
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
      async competicaoAtualizada => {

        this.categorias.forEach(async categoria => {
          const categoriaMap = this.categoriasMap[categoria.tempId];
          delete categoria.tempId;
          categoria.competicaoId = this.competicao.id;
          if (categoria.id === 0) {
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
              error => console.log("Erro ao criar categoria: ", error)
            )
          }
          else {
            await this.categoriaService.updateCategoria(categoria.id, categoria).subscribe(
              (categoriaAtualizada) => {
                if (categoriaMap.competidores && categoriaMap.competidores.length > 0) {
                  categoriaMap.competidores.forEach((competidor: any) => {
                    const inscricao: Inscricao = {
                      id: 0, // O backend deve gerar esse ID
                      competidorId: competidor.id,
                      categoriaId: categoriaAtualizada.id,
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

        await this.uploadImagem()
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
        alert("Competição atualizada com sucesso!");
        this.router.navigate(['/minhas-competicoes']);
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
    const novaCategoria: any = {
      id: 0,  // O backend deve gerar esse ID
      tempId: this.tempIdCategoria++, // ID temporário para identificar a categoria
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

    delete this.categoriasMap[this.categorias[index].tempId];

    this.idCategoriasParaDeletar.push(this.categorias[index].id);
    this.categorias.splice(index, 1);
  }

  cancel(): void {
    this.router.navigate(['/minhas-competicoes']);
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

  async abrirModalCompetidores(categoriaId: number, categoriaTempId: number): Promise<void> {
    if (categoriaId !== 0) {
      await this.inscricaoService.getInscricoesPorCategoria(categoriaId).subscribe(
        inscricoes => {
          this.competidoresUsuario = this.competidoresUsuario.filter(competidor =>
            !inscricoes.some((inscricao: any) => inscricao.competidorId === competidor.id)
          )
          this.categoriasMap[categoriaTempId].mostrarModal = true;
          console.log(inscricoes);
        });
    }
    else {
      this.categoriasMap[categoriaTempId].mostrarModal = true;
    }

    if (this.categoriasMap[categoriaTempId] === undefined)
      this.categoriasMap[categoriaTempId] = { mostrarModal: false, competidores: [] };
    for (let i = 0; i < this.competidoresUsuario.length; i++) {
      if (this.competidoresUsuario[i].selecionadoPorCategoria === undefined)
        this.competidoresUsuario[i].selecionadoPorCategoria = {};
      if (this.competidoresUsuario[i].selecionadoPorCategoria[categoriaTempId] === undefined)
        this.competidoresUsuario[i].selecionadoPorCategoria[categoriaTempId] = false;
    }
    console.log(categoriaTempId)
  }

  fecharModalCompetidores(categoriaTempId: number): void {
    this.userService.getUsuarioLogado().subscribe(usuario => {
      this.usuario = usuario;
      this.competidorService.buscarCompetidoresDoUsuario(usuario.id).subscribe(competidores => {
        this.competidoresUsuario = competidores;
      });
    }
    );
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