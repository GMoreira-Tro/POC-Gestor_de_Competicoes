import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { InscricaoService } from '../services/inscricao.service';
import { CategoriaService } from '../services/categoria.service';
import { NotificacaoService } from '../services/notificacao.service';
import { PaymentService } from '../services/payment.service';

@Component({
  selector: 'app-pagar-inscricao',
  templateUrl: './pagar-inscricao.component.html',
  styleUrls: ['./pagar-inscricao.component.css']
})
export class PagarInscricaoComponent implements OnInit {
  infoInscricao: any;
  user: any;
  qrCodeUrl: string = '';
  pollingInterval: any; // Armazena o intervalo do polling
  paymentCompleted: boolean = false;
  txid: string = ''; // ID da transação PIX
  inscricaoId: number = 0;
  pixEnviadoAoOrganizador = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private inscricaoService: InscricaoService,
    private userService: UserService,
    private paymentService: PaymentService
  ) { }

  ngOnInit(): void {
    this.inscricaoId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.inscricaoId) {
      this.inscricaoService.obterInscricao(this.inscricaoId).subscribe(
        (inscricao) => {
          this.inscricaoService.getInfoInscricao(inscricao.id).subscribe((infoInscricao) => {
            this.infoInscricao = infoInscricao;
            this.infoInscricao.status = inscricao.status;
            this.userService.getUsuarioLogado().subscribe(usuario => {
              this.user = usuario;
              this.paymentCompleted = this.infoInscricao.status === 2;
              if (!this.paymentCompleted)
                this.gerarQRCode();
            });
          });
        },
        (error) => {
          this.router.navigate(['/']);
        }
      );
    } else {
      this.router.navigate(['/']);
    }
  }

  gerarQRCode() {
    const valorOriginal = this.infoInscricao.valorCategoria.toFixed(2);

    this.paymentService.generateQRCodeLocation({
      "calendario": {
        "expiracao": 36000
      },
      "devedor": {
        "cpf": this.user.cpfCnpj,
        "nome": this.user.nome,
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
                pagadorId: this.user.id,
                favorecidoId: this.infoInscricao.organizadorId,
                infoPagamento: 'Inscrição em competição'
              }
            ).subscribe((pagamento) =>
            {
              this.inscricaoService.obterInscricao(this.inscricaoId).subscribe(inscricao => {
                inscricao.status = 2;
                inscricao.pagamentoId = pagamento.id;
                this.inscricaoService.atualizarInscricao(inscricao.id, inscricao).subscribe(() => {
                  this.infoInscricao.status = 2;
                  this.paymentService.receberEmailInscricao(this.inscricaoId).subscribe(() => {
                    console.log('Email de inscrição recebido com sucesso!');
                    alert('✅ Pagamento realizado com sucesso!'); // Exibe o alerta
                  });
                });
              });
            });
          }
        },
        (error) => {
          console.error('Erro ao verificar status do pagamento:', error);
        }
      );
    }, 5000); // Intervalo de 5 segundos
  }

  enviarPix() {
    if (this.pixEnviadoAoOrganizador) return;

    const chaveFavorecido = prompt('Digite a chave PIX do favorecido:');
    const valor = "20.00";
    if (!chaveFavorecido || !valor) {
      alert('Chave PIX e valor são obrigatórios.');
      return;
    }

    const pixSent = {
      valor: parseFloat(valor).toFixed(2),
      pagador: {
        chave: 'a5d98ae0-2416-457f-86a1-c543e08c07a4', // Chave da sua conta Efi Bank
        infoPagador: this.user.nome,
      },
      favorecido: {
        chave: chaveFavorecido,
      },
    };

    const idEnvio = `envio${Date.now()}`; // Gera um identificador único para o envio

    this.paymentService.sendPix(idEnvio, pixSent).subscribe(
      (response) => {
        this.pixEnviadoAoOrganizador = true;
        alert('✅ PIX enviado com sucesso!');
        console.log('Resposta do envio:', response);
      },
      (error) => {
        console.error('Erro ao enviar PIX:', error);
        alert('❌ Erro ao enviar PIX.');
      }
    );
  }
}