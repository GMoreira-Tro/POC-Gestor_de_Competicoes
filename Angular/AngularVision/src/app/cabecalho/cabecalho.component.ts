import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cabecalho',
  templateUrl: './cabecalho.component.html',
  styleUrls: ['./cabecalho.component.css']
})
export class CabecalhoComponent implements OnInit {

  constructor(public router: Router) { }
  ngOnInit(): void {
  }

  retornarTelaInicial() {
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

  private usuarioLogado(): boolean {
    // Implemente sua lógica para verificar se o usuário está logado aqui
    return true; // Exemplo simples - substitua por sua lógica real
  }

  login() {
    this.router.navigate(['/login']);
  }
}
