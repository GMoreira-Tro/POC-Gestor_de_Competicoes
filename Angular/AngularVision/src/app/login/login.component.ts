import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements AfterViewInit {
  @ViewChild('form') form!: NgForm;
  userData: any = {
    email: '',
    senhaHash: ''
  };
  loginButtonPressed: boolean = false;

  constructor(private userService: UserService, private router: Router) { }

  ngAfterViewInit() {
    this.form.control.markAsTouched();
  }

  highlightRequiredFields() {
    this.loginButtonPressed = true;
    Object.keys(this.form.controls).forEach(field => {
      const control = this.form.controls[field];
      control.markAsTouched();
    });
  }

  submitForm() {
    this.highlightRequiredFields();
    if (this.form.valid) {
      // Aqui você pode adicionar a lógica para enviar os dados de login para o servidor
      console.log('Dados do formulário de login:', this.userData);

      // Exemplo de chamada do serviço de login
      this.userService.login(this.userData).subscribe(
        (response: any) => {
          console.log('Usuário autenticado com sucesso:', response);
          // Navegar para a página inicial ou outra página desejada após o login
          this.router.navigate(['']);
        },
        (error: any) => {
          console.error('Erro ao autenticar usuário:', error);
          // Tratar erros de autenticação, como exibir mensagens de erro
        }
      );
    }
  }

  onKeyPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      alert("ENTER")
      this.submitForm();
    }
  }
}
