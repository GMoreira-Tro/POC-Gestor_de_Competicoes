import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { Usuario } from '../interfaces/Usuario';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})
export class UserRegistrationComponent {
  userData : Usuario = {
    nome: '', email: '', senhaHash: '',
    id: 0,
    inscricoes: []
  };

  constructor(private userService: UserService) {}

  submitForm() {
    this.userService.createUser(this.userData).subscribe(
      response => {
        console.log('Usuário cadastrado com sucesso:', response);
        // Lógica adicional, como redirecionar para outra página
      },
      error => {
        console.error('Erro ao cadastrar usuário:', error);
        // Tratar erros de cadastro, exibir mensagens de erro, etc.
      }
    );
  }
}
