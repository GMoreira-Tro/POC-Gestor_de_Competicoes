import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-confirmacao-email',
  templateUrl: './confirmacao-email.component.html',
  styleUrls: ['./confirmacao-email.component.css']
})
export class ConfirmacaoEmailComponent implements OnInit {
  mensagem: string = "Confirmando seu e-mail...";

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit() {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (token) {
      ///TODO: Substituir a URL abaixo pela URL correta do backend
      this.http.get(`http://localhost:5000/api/confirmar-email?token=${token}`).subscribe(
        () => {
          this.mensagem = "E-mail confirmado com sucesso! Você pode fazer login.";
        },
        () => {
          this.mensagem = "Erro ao confirmar e-mail. Tente novamente.";
        }
      );
    } else {
      this.mensagem = "Token inválido.";
    }
  }

  login() {
    this.router.navigate(['/login']);
  }
}
