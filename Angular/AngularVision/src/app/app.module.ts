import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxMaskModule } from 'ngx-mask';
import { NgxCountryFlagModule } from 'ngx-country-flag';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserRegistrationComponent } from './user-registration/user-registration.component';
import { LoginComponent } from './login/login.component';
import { CabecalhoComponent } from './cabecalho/cabecalho.component';
import { TelaInicialComponent } from './tela-inicial/tela-inicial.component';
import { BuscaCompeticoesComponent } from './busca-competicoes/busca-competicoes.component';
import { RodapeComponent } from './rodape/rodape.component';
import { CadastroCompeticoesComponent } from './cadastro-competicoes/cadastro-competicoes.component';

import { UserService } from './services/user.service';
import { MinhasCompeticoesComponent } from './minhas-competicoes/minhas-competicoes.component';
import { EditarCompeticaoComponent } from './editar-competicao/editar-competicao.component';
import { AguardandoConfirmacaoComponent } from './aguardando-confirmacao/aguardando-confirmacao.component';
import { ConfirmacaoEmailComponent } from './confirmacao-email/confirmacao-email.component';
import { EmailConfirmadoComponent } from './email-confirmation/email-confirmado.component';
import { PerfilUsuarioComponent } from './perfil-usuario/perfil-usuario.component';
import { CadastroCompetidoresComponent } from './cadastro-competidores/cadastro-competidores.component';
import { MeusCompetidoresComponent } from './meus-competidores/meus-competidores.component';
import { EditarCompetidorComponent } from './editar-competidor/editar-competidor.component';
import { InscricaoCompeticaoComponent } from './inscricao-competicao/inscricao-competicao.component';
import { MinhasInscricoesComponent } from './minhas-inscricoes/minhas-inscricoes.component';
import { AprovarInscricaoComponent } from './aprovar-inscricao/aprovar-inscricao.component';
import { FiltrarPorCategoriaPipe } from './aprovar-inscricao/filtrar-por-categoria.pipe';
import { FaleConoscoComponent } from './fale-conosco/fale-conosco.component';

@NgModule({
  declarations: [
    AppComponent,
    UserRegistrationComponent,
    LoginComponent,
    TelaInicialComponent,
    BuscaCompeticoesComponent,
    MinhasCompeticoesComponent,
    CadastroCompeticoesComponent,
    EditarCompeticaoComponent,
    AguardandoConfirmacaoComponent,
    ConfirmacaoEmailComponent,
    EmailConfirmadoComponent,
    PerfilUsuarioComponent,
    CadastroCompetidoresComponent,
    MeusCompetidoresComponent,
    EditarCompetidorComponent,
    InscricaoCompeticaoComponent,
    MinhasInscricoesComponent,
    AprovarInscricaoComponent,
    FiltrarPorCategoriaPipe,
    FaleConoscoComponent,
    CabecalhoComponent,
    RodapeComponent,
    
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxCountryFlagModule,
    ModalModule.forRoot(),
    NgxMaskModule.forRoot(),

  ],
  providers: [UserService],
  bootstrap: [AppComponent],
})
export class AppModule { }
