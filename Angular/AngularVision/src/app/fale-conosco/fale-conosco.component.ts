import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-fale-conosco',
  templateUrl: './fale-conosco.component.html',
  styleUrl: './fale-conosco.component.css'
})
export class FaleConoscoComponent implements OnInit {
  userId: number = 0; 
  assunto: string = '';
  mensagem: string = '';
  constructor(public router: Router,
    public userService: UserService
  ) { }

  ngOnInit(): void {
    // Inicialize o usuário (exemplo)
    this.userService.getUsuarioLogado().subscribe((data) => {
      this.userId = data.id;
    });
  }

  enviarEmail(): void {
    alert('E-mail enviado com sucesso!');
    this.userService.enviarEmailSuporte(this.userId, this.assunto, this.mensagem).subscribe(() => {
      // Redirecionar para a página inicial após o envio do e-mail
      this.router.navigate(['/tela-inicial']);
    }); 
  }
}