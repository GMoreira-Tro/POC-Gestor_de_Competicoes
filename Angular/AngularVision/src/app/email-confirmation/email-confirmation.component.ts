import { Component } from '@angular/core';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent {
  
  constructor() { }

  resendConfirmationEmail() {
    // Lógica para reenviar o e-mail de confirmação
    console.log('E-mail de confirmação reenviado.');
  }
}
