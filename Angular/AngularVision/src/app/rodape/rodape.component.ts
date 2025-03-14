import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-rodape',
  templateUrl: './rodape.component.html',
  styleUrl: './rodape.component.css'
})
export class RodapeComponent {
  constructor(private router: Router) {}

  navigateToFaleConosco() {
    this.router.navigate(['/fale-conosco']);
  }
}