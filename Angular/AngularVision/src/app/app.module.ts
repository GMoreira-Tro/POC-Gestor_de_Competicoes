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

@NgModule({
  declarations: [
    AppComponent,
    UserRegistrationComponent,
    LoginComponent,
    TelaInicialComponent,
    BuscaCompeticoesComponent,
    CadastroCompeticoesComponent,
    CabecalhoComponent,
    RodapeComponent
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
