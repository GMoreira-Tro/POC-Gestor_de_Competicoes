import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserRegistrationComponent } from './user-registration/user-registration.component';
import { EmailConfirmadoComponent } from './email-confirmation/email-confirmado.component'; // Suponha que você tenha um componente para a confirmação de e-mail
import { TelaInicialComponent } from './tela-inicial/tela-inicial.component';
import { LoginComponent } from './login/login.component';
import { BuscaCompeticoesComponent } from './busca-competicoes/busca-competicoes.component';
import { CadastroCompeticoesComponent } from './cadastro-competicoes/cadastro-competicoes.component';
import { MinhasCompeticoesComponent } from './minhas-competicoes/minhas-competicoes.component';
import { EditarCompeticaoComponent } from './editar-competicao/editar-competicao.component';
import { AguardandoConfirmacaoComponent } from './aguardando-confirmacao/aguardando-confirmacao.component';
import { PerfilUsuarioComponent } from './perfil-usuario/perfil-usuario.component';

const routes: Routes = [
  { path: '', component: TelaInicialComponent },
  { path: 'register', component: UserRegistrationComponent },
  { path: 'login', component:LoginComponent },
  { path: 'busca', component: BuscaCompeticoesComponent },
  { path: 'cadastro-competicao', component: CadastroCompeticoesComponent },
  { path: 'minhas-competicoes', component: MinhasCompeticoesComponent },
  { path: 'minhas-competicoes', component: MinhasCompeticoesComponent },
  { path: 'editar-competicao/:id', component: EditarCompeticaoComponent },
  { path: 'aguardando-confirmacao', component: AguardandoConfirmacaoComponent },
  { path: 'email-confirmado/:token', component: EmailConfirmadoComponent }, // Adicione a rota para a confirmação de e-mail
  { path: 'perfil-usuario', component: PerfilUsuarioComponent },
  { path: '', redirectTo: '/register', pathMatch: 'full' }, // Defina uma rota padrão
  { path: '**', redirectTo: '/register' } // Redireciona para o registro se a rota não for encontrada
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
