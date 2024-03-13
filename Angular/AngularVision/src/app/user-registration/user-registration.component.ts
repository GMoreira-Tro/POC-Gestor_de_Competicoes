import { Component } from '@angular/core';
import { Usuario } from '../interfaces/Usuario';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})

export class UserRegistrationComponent {
  userData: Usuario = {
    id: 0,
    nome: '',
    sobrenome: '',
    email: '',
    senhaHash: '',
    pais: '',
    dataNascimento: new Date(),
    cpfCnpj: '',
    inscricoes: []
  };

  constructor(private userService: UserService, private router: Router) { }

  submitForm() {
    this.userService.createUser(this.userData).subscribe(
      response => {
        console.log('Usuário cadastrado com sucesso:', response);
        // Envie o e-mail de confirmação
        this.userService.sendConfirmationEmail(this.userData.email).subscribe(
          emailResponse => {
            console.log('E-mail de confirmação enviado:', emailResponse);
            this.router.navigate(['/email-confirmation']);
          },
          error => {
            console.error('Erro ao enviar e-mail de confirmação:', error);
            alert('Erro ao enviar e-mail de confirmação. Por favor, tente novamente.');
          }
        );
      },
      error => {
        console.error('Erro ao cadastrar usuário:', error);
        let errorMessage = 'Erro ao cadastrar usuário';
        if (error.error) {
          errorMessage = error.error;
        }
        alert(errorMessage);
        // Tratar erros de cadastro, exibir mensagens de erro, etc.
      }
    );
  }
}
