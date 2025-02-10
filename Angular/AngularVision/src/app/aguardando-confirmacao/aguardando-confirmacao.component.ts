import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-aguardando-confirmacao',
  templateUrl: './aguardando-confirmacao.component.html',
  styleUrls: ['./aguardando-confirmacao.component.css']
})
export class AguardandoConfirmacaoComponent {
  constructor(private router: Router) {}

  login() {
    this.router.navigate(['/login']);
  }
}
