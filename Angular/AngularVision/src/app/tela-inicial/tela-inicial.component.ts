import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tela-inicial',
  templateUrl: './tela-inicial.component.html',
  styleUrls: ['./tela-inicial.component.css']
})
export class TelaInicialComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  cadastrar(): void {
    this.router.navigate(['/register']);
  }

  buscarCompeticao(): void {
    if (!this.usuarioLogado()) {
      this.router.navigate(['/login']);
    } else {
      // Lógica para buscar competições
    }
  }

  cadastrarCompeticao(): void {
    if (!this.usuarioLogado()) {
      this.router.navigate(['/login']);
    } else {
      // Lógica para cadastrar competição
    }
  }

  private usuarioLogado(): boolean {
    // Implemente sua lógica para verificar se o usuário está logado aqui
    return false; // Exemplo simples - substitua por sua lógica real
  }

  login() {
    this.router.navigate(['/login']);
  }
}
