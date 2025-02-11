import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-email-confirmado',
  templateUrl: './email-confirmado.component.html',
  styleUrls: ['./email-confirmado.component.css']
})
export class EmailConfirmadoComponent implements OnInit {
  tokenConfirmacao: string | null = null;
  mensagem: string = 'Confirmando e-mail...';
  sucesso: boolean = false;
  
  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.tokenConfirmacao = params.get('token');
      console.log(this.tokenConfirmacao)
      if (this.tokenConfirmacao) {
        this.userService.confirmarEmail(this.tokenConfirmacao).subscribe();
        this.sucesso = true;
      }
    });
  }

  irParaLogin(): void {
    this.router.navigate(['/login']);
  }

  reenviarConfirmacao() {
    //this.userService.reenviarEmailConfirmacao(this.tokenConfirmacao).subscribe();
  }
}
