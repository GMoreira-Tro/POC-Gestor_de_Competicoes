import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { NotificacaoService } from '../services/notificacao.service';

@Component({
  selector: 'app-cabecalho',
  templateUrl: './cabecalho.component.html',
  styleUrls: ['./cabecalho.component.css']
})
export class CabecalhoComponent implements OnInit {

  usuario: any; // Substitua pelo tipo real de usuário que você está usando
  dropdownAberto = false;
  numeroDeNotificacoes: number|string = 99;
  notificacoes: any[] = [];
  notificationDropdownAberto = false;

  constructor(public router: Router,
    public userService: UserService,
    private notificacaoService: NotificacaoService
  ) { }

  ngOnInit(): void {
    // Inicialize o usuário (exemplo)
    this.userService.getUsuarioLogado().subscribe((data) => {
      this.usuario = data;
      this.usuario.imagemUrl = this.usuario.imagemUrl?.startsWith('http')
        ? this.usuario.imagemUrl
        : `http://localhost:5000/${this.usuario.imagemUrl}`;

        this.notificacaoService.buscarNotificacoesDoUsuario(data.id).subscribe(notificacoes =>
        {
          this.notificacoes = notificacoes;
          this.numeroDeNotificacoes = this.notificacoes.length <= 99 ? this.notificacoes.length : "+99";
        }
        );
    },
      error => {
        console.error("Erro ao carregar o usuário", error);
      }
    );
  }

  retornarTelaInicial(): void {
    this.router.navigate(['']);
  }

  cadastrar(): void {
    this.router.navigate(['/register']);
  }

  buscarCompeticao(): void {
    if (!this.usuarioLogado()) {
      this.router.navigate(['/login']);
    } else if (this.router.url === '/busca') {
      this.router.navigate(['']);
    } else {
      this.router.navigate(['/busca']);
    }
  }

  cadastrarCompeticao(): void {
    if (!this.usuarioLogado()) {
      this.router.navigate(['/login']);
    } else if (this.router.url === '/cadastro-competicao') {
      this.router.navigate(['']);
    } else {
      this.router.navigate(['/cadastro-competicao']);
    }
  }

  cadastrarCompetidores(): void {
    if (!this.usuarioLogado()) {
      this.router.navigate(['/login']);
    } else if (this.router.url === '/cadastro-competidores') {
      this.router.navigate(['']);
    } else {
      this.router.navigate(['/cadastro-competidores']);
    }
  }

  toggleDropdown(): void {
    this.dropdownAberto = !this.dropdownAberto;
  }

  toggleNotificationDropdown(): void {
    this.notificationDropdownAberto = !this.notificationDropdownAberto;
  }

  logout(): void {
    // Implemente a lógica de logout aqui (exemplo)
    this.usuario = null;
    this.router.navigate(['/login']);
  }

  private usuarioLogado(): boolean {
    // Implemente sua lógica para verificar se o usuário está logado aqui
    return this.usuario !== null; // Exemplo simples - substitua por sua lógica real
  }

  login(): void {
    this.router.navigate(['/login']);
  }
}