import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CabecalhoModule } from '../cabecalho/cabecalho.module'; // Importando o CabecalhoModule
import { RodapeModule } from '../rodape/rodape.module';
import { CadastroCompeticoesComponent } from './cadastro-competicoes.component';

@NgModule({
  declarations: [
    CadastroCompeticoesComponent
  ],
  imports: [
    CommonModule,
    CabecalhoModule,
    RodapeModule
    // Se precisar de outros módulos, importe-os aqui
  ],
  exports: [
    CadastroCompeticoesComponent
  ]
})
export class CadastroCompeticoesModule { }
