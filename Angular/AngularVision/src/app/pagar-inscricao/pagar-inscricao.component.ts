import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { InscricaoService } from '../services/inscricao.service';
import { CategoriaService } from '../services/categoria.service';
import { NotificacaoService } from '../services/notificacao.service';

@Component({
  selector: 'app-pagar-inscricao',
  templateUrl: './pagar-inscricao.component.html',
  styleUrls: ['./pagar-inscricao.component.css']
})
export class PagarInscricaoComponent implements OnInit {
  infoInscricao: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private inscricaoService: InscricaoService
  ) {}

  ngOnInit(): void {
    const inscricaoId = Number(this.route.snapshot.paramMap.get('id'));
    if (inscricaoId) {
      this.inscricaoService.obterInscricao(inscricaoId).subscribe(
        (inscricao) => {
          this.inscricaoService.getInfoInscricao(inscricao.id).subscribe((infoInscricao) => {
            this.infoInscricao = infoInscricao;
            this.infoInscricao.status = inscricao.status;
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

  gerarQRCode(infoInscricao: any): string {
    // Simulating QR Code generation logic
    return `https://api.qrserver.com/v1/create-qr-code/?data=Pagamento:${infoInscricao.id}&size=200x200`;
  }

}