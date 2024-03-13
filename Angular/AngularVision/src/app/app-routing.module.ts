import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserRegistrationComponent } from './user-registration/user-registration.component';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component'; // Suponha que você tenha um componente para a confirmação de e-mail

const routes: Routes = [
  { path: 'register', component: UserRegistrationComponent },
  { path: 'email-confirmation', component: EmailConfirmationComponent }, // Adicione a rota para a confirmação de e-mail
  { path: '', redirectTo: '/register', pathMatch: 'full' }, // Defina uma rota padrão
  { path: '**', redirectTo: '/register' } // Redireciona para o registro se a rota não for encontrada
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
