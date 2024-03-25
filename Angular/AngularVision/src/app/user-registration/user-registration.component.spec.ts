import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { UserRegistrationComponent } from './user-registration.component';
import { Usuario } from '../interfaces/Usuario';

const mockNome = "Guilherme";
const mockSobrenome = "dos Santos Moreira";
const mockSenha = "Gm$1403s";
const mockEmail = "guilherme.dsmoreira@gmail.com"
const mockPais = "Brazil";
const mockEstado = "Rio Grande do Sul";
const mockCidade = "São Leopoldo";
const mockDataNascimento = new Date(2000, 4, 25)
const mockCpf = "046.239.580-43"
const mockInvalidCpf = "111.111.111-11";
const existingEmail = 'user@example.com';

const mockedUser: Usuario = {
  id: 0,
  nome: mockNome,
  sobrenome: mockSobrenome,
  senhaHash: mockSenha,
  email: mockEmail,
  pais: mockPais,
  estado: mockEstado,
  cidade: mockCidade,
  dataNascimento: mockDataNascimento,
  cpfCnpj: mockCpf,
  inscricoes: []
}

describe('UserRegistrationComponent', () => {
  let component: UserRegistrationComponent;
  let fixture: ComponentFixture<UserRegistrationComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [UserRegistrationComponent],
      imports: [FormsModule, HttpClientTestingModule]
    }).compileComponents();
  }));

  beforeEach((done: DoneFn) => {
    fixture = TestBed.createComponent(UserRegistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    setTimeout(() => {
      done(); // Indica ao Jasmine que a configuração está completa
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty fields', () => {
    // Corrigido para usar o nome correto dos campos do formulário
    expect(component.userData).toEqual({
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
    });
  });

  it('should require name field', () => {
    // Corrigido para usar o nome correto dos campos do formulário
    const nameControl = component.form.control.get('nome');
    nameControl?.setValue('');
    expect(nameControl?.valid).toBeFalsy();
    expect(nameControl?.errors?.['required']).toBeTruthy();
  });

  it('should require email field', () => {
    // Corrigido para usar o nome correto dos campos do formulário
    const emailControl = component.form.control.get('email');
    emailControl?.setValue('');
    expect(emailControl?.valid).toBeFalsy();
    expect(emailControl?.errors?.['required']).toBeTruthy();
  });

  it('should require email to be unique', async () => {
    component.userData = mockedUser;
    component.userData.email = existingEmail;

    try {
      // Faz uma chamada mockada para simular a verificação de e-mail único
       component.userService.createUser(component.userData);
      // Se a criação do usuário for bem-sucedida, falha no teste
      fail('Expected an error to be thrown');
    } catch (error) {
      // Verifica se o erro corresponde ao esperado
      expect(error).toBeTruthy();
      expect(error).toEqual('Email already registered');
    }
  });


  it('should require password field', () => {
    // Corrigido para usar o nome correto dos campos do formulário
    const passwordControl = component.form.control.get('senha');
    passwordControl?.setValue('');
    expect(passwordControl?.valid).toBeFalsy();
    expect(passwordControl?.errors?.['required']).toBeTruthy();
  });

  it('should require pais field', () => {
    // Corrigido para usar o nome correto dos campos do formulário
    const control = component.form.control.get('pais');
    control?.setValue('');
    expect(control?.valid).toBeFalsy();
    expect(control?.errors?.['required']).toBeTruthy();
  });

  it('should require estado field', () => {
    // Corrigido para usar o nome correto dos campos do formulário
    const control = component.form.control.get('estado');
    control?.setValue('');
    expect(control?.valid).toBeFalsy();
    expect(control?.errors?.['required']).toBeTruthy();
  });

  it('should require cidade field', () => {
    // Corrigido para usar o nome correto dos campos do formulário
    const control = component.form.control.get('cidade');
    control?.setValue('');
    expect(control?.valid).toBeFalsy();
    expect(control?.errors?.['required']).toBeTruthy();
  });
});
