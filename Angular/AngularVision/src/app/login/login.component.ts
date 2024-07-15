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
    senha: ''
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
      console.log('Dados do formulário de login:', this.userData);

      this.userService.login(this.userData).subscribe(
        (response: any) => {
          console.log('Usuário autenticado com sucesso:', response);
          this.router.navigate(['']);
        },
        () => {
          alert("Erro ao autenticar usuário.")
          // Tratar erros de autenticação, como exibir mensagens de erro
        }
      );
    }
    else {
      alert("Erro ao autenticar usuário. Formulário inválido!");
    }
  }

  onKeyPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.submitForm();
    }
  }
}
