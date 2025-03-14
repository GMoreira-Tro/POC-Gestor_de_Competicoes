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
import { CadastroCompetidoresComponent } from './cadastro-competidores/cadastro-competidores.component';
import { MeusCompetidoresComponent } from './meus-competidores/meus-competidores.component';
import { EditarCompetidorComponent } from './editar-competidor/editar-competidor.component';
import { InscricaoCompeticaoComponent } from './inscricao-competicao/inscricao-competicao.component';
import { MinhasInscricoesComponent } from './minhas-inscricoes/minhas-inscricoes.component';
import { AprovarInscricaoComponent } from './aprovar-inscricao/aprovar-inscricao.component';
import { FaleConoscoComponent } from './fale-conosco/fale-conosco.component';

const routes: Routes = [
  { path: '', component: TelaInicialComponent },
  { path: 'register', component: UserRegistrationComponent },
  { path: 'login', component:LoginComponent },
  { path: 'busca', component: BuscaCompeticoesComponent },
  { path: 'cadastro-competicao', component: CadastroCompeticoesComponent },
  { path: 'cadastro-competidores', component: CadastroCompetidoresComponent },
  { path: 'minhas-competicoes', component: MinhasCompeticoesComponent },
  { path: 'meus-competidores', component: MeusCompetidoresComponent },
  { path: 'minhas-inscricoes', component: MinhasInscricoesComponent },
  { path: 'editar-competicao/:id', component: EditarCompeticaoComponent },
  { path: 'editar-competidor/:id', component: EditarCompetidorComponent },
  { path: 'inscricao-competicao/:id', component: InscricaoCompeticaoComponent },
  { path: 'aprovar-inscricao/:id', component: AprovarInscricaoComponent },
  { path: 'aguardando-confirmacao', component: AguardandoConfirmacaoComponent },
  { path: 'email-confirmado/:token', component: EmailConfirmadoComponent }, // Adicione a rota para a confirmação de e-mail
  { path: 'perfil-usuario', component: PerfilUsuarioComponent },
  { path: 'fale-conosco', component: FaleConoscoComponent },
  { path: '', redirectTo: '/register', pathMatch: 'full' }, // Defina uma rota padrão
  { path: '**', redirectTo: '/register' } // Redireciona para o registro se a rota não for encontrada
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
