import { Component, ViewChild, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { Usuario } from '../interfaces/Usuario';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { GeoNamesService } from '../services/geonames.service';
import { validarCpfCnpj } from '../utils/validarCpfCnpj';
import { AuthService } from '../services/auth.service';

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
    emailConfirmado: false,
    senhaHash: '',
    senhaValidada: false,
    pais: '',
    estado: '',
    cidade: '',
    dataNascimento: new Date(1900, 0, 1),
    cpfCnpj: '',
    role: 0
  };
  signUpButtonPressed: boolean = false;
  paisesDoMundo: any;
  estados: any;
  cidades: any;

  constructor(public userService: UserService, private router: Router, private geonamesService: GeoNamesService, 
    private authService: AuthService,
    private cdr: ChangeDetectorRef) { }

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
    if(this.userData.nome === '')
    {
      alert("O campo nome é obrigatório");
      return;
    }
    if(this.userData.sobrenome === '')
    {
      alert("O campo sobrenome é obrigatório");
      return;
    }
    if(this.userData.email === '')
    {
      alert("O campo e-mail é obrigatório");
      return;
    }
    if(this.userData.senhaHash === '')
    {
      alert("O campo senha é obrigatório");
      return;
    }
    if(this.userData.pais === '')
    {
      alert("O campo país é obrigatório");
      return;
    }
    if(this.userData.estado === '')
    {
      alert("O campo estado é obrigatório");
      return;
    }
    if(this.userData.cidade === '')
    {
      alert("O campo cidade é obrigatório");
      return;
    }
    if(validarCpfCnpj(this.userData.cpfCnpj) === false)
    {
      alert("CPF/CNPJ inválido");
      return;
    }
    this.userService.getUserByEmail(this.userData.email).subscribe(
      (usuario) => {
        if (usuario) {
          alert("E-mail já cadastrado");
          return;
        }
        // Continuação do cadastro...
      },
      (error) => {
        console.error("Erro ao buscar usuário:", error);
      }
    );    

    console.log('Enviando formulário:', this.userData);
    this.userService.createUser(this.userData).subscribe(
      response => {
        alert('Usuário cadastrado com sucesso:');
        this.authService.register(this.userData).subscribe(() => {
          this.router.navigate(['/aguardando-confirmacao']);
        });
        
      },
      error => {
        console.error('Erro ao cadastrar usuário:', error);
        alert(error.error || 'Erro ao cadastrar usuário. Por favor, tente novamente.');
      }
    );
  }

  // Função chamada quando o país selecionado é alterado
  onCountryChange() {
    // Limpa a lista de estados
    this.estados = [];
    this.cidades = [];
    this.userData.estado = ''; // Limpa o estado selecionado
    this.userData.cidade = ''; // Limpa a cidade selecionado

    // Obtém os estados/províncias do país selecionado
    const pais = this.paisesDoMundo?.geonames.find((country: any) => country.countryCode === this.userData.pais);
    if (!pais) return;

    this.geonamesService.getStatesByCountry(pais.geonameId).subscribe(
      (estados: any) => {
        // Extrai os nomes dos estados/províncias da resposta
        this.estados = estados;
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
    this.userData.cidade = ''; // Limpa a cidade selecionada

    // Obtém as cidades do estado selecionado
    const estado = this.estados?.geonames.find((state: any) => state.name === this.userData.estado);
    if (!estado) return;

    this.geonamesService.getCitiesByState(estado.geonameId).subscribe(
      (cidades: any) => {
        // Extrai os nomes das cidades da resposta
        this.cidades = cidades;
        this.cdr.detectChanges(); // Detecta as alterações manualmente após a obtenção das cidades
      },
      error => {
        console.error('Erro ao obter as cidades:', error);
        // Trate o erro conforme necessário
      }
    );
  }
}
