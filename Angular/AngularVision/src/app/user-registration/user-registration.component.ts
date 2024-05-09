import { Component, ViewChild, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { Usuario } from '../interfaces/Usuario';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { GeoNamesService } from '../services/geonames.service';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css']
})
export class UserRegistrationComponent implements AfterViewInit {
  @ViewChild('form') form!: NgForm;
  userData: Usuario = {
    id: 0,
    nome: '',
    sobrenome: '',
    email: '',
    senhaHash: '',
    pais: '',
    estado: '',
    cidade: '',
    dataNascimento: new Date(1900, 0, 1),
    cpfCnpj: '',
    inscricoes: []
  };
  signUpButtonPressed: boolean = false;
  paisesDoMundo: any;
  estados: any;
  cidades: any;

  constructor(public userService: UserService, private router: Router, private geonamesService: GeoNamesService, private cdr: ChangeDetectorRef) { }

  ngAfterViewInit() {
    // Inicializa o formulário após a exibição da visualização do componente
    setTimeout(() => {
      this.form.control.markAsTouched();
      this.cdr.detectChanges(); // Detecta as alterações manualmente após a inicialização do formulário
    });


    // Carrega a lista de países do mundo
    this.geonamesService.getAllCountries().subscribe(
      paises => {
        this.paisesDoMundo = paises;
        console.log(this.paisesDoMundo);
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção dos países
      },
      error => {
        console.error('Erro ao obter a lista de países:', error);
        // Trate o erro conforme necessário
      }
    );
  }

  highlightRequiredFields() {
    this.signUpButtonPressed = true;
    Object.keys(this.form.controls).forEach(field => {
      const control = this.form.controls[field];
      control.markAsTouched();
    });
  }

  submitForm() {
    this.highlightRequiredFields(); // Certifica-se de que os campos obrigatórios são destacados antes de enviar o formulário
    // Verifica se o formulário é válido antes de enviar
    this.userService.createUser(this.userData).subscribe(
      response => {
        console.log('Usuário cadastrado com sucesso:', response);
        // Envie o e-mail de confirmação
        // this.userService.sendConfirmationEmail(this.userData.email).subscribe(
        //   emailResponse => {
        //     console.log('E-mail de confirmação enviado:', emailResponse);
        //     this.router.navigate(['/email-confirmation']);
        //   },
        //   error => {
        //     console.error('Erro ao enviar e-mail de confirmação:', error);
        //     alert('Erro ao enviar e-mail de confirmação. Por favor, tente novamente.');
        //   }
        // );
      },
      error => {
        console.error('Erro ao cadastrar usuário:', error);
        let errorMessage = 'Erro ao cadastrar usuário';
        if (error.error && typeof error.error === 'string') {
          errorMessage = error.error;
          alert(errorMessage);
        }
        alert(errorMessage);
        // Tratar erros de cadastro, exibir mensagens de erro, etc.
      }
    );
  }

  // Função chamada quando o país selecionado é alterado
  onCountryChange() {
    // Limpa a lista de estados
    this.estados = [];
    this.cidades = [];

    // Obtém os estados/províncias do país selecionado
    const pais = this.paisesDoMundo?.geonames.find((country: any) => country.countryName === this.userData.pais);
    if (!pais) return;

    this.geonamesService.getStatesByCountry(pais.geonameId).subscribe(
      (estados: any) => {
        // Extrai os nomes dos estados/províncias da resposta
        this.estados = estados;
        console.log(this.estados.geonames);
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção dos estados
      },
      error => {
        console.error('Erro ao obter os estados/províncias:', error);
        // Trate o erro conforme necessário
      }
    );
  }

  // Função chamada quando o estado selecionado é alterado
  onStateChange() {
    // Limpa a lista de cidades
    this.cidades = [];

    // Obtém as cidades do estado selecionado
    const estado = this.estados?.geonames.find((state: any) => state.name === this.userData.estado);
    if (!estado) return;

    this.geonamesService.getCitiesByState(estado.geonameId).subscribe(
      (cidades: any) => {
        // Extrai os nomes das cidades da resposta
        this.cidades = cidades;
        console.log(this.cidades.geonames);
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção das cidades
      },
      error => {
        console.error('Erro ao obter as cidades:', error);
        // Trate o erro conforme necessário
      }
    );
  }
}
