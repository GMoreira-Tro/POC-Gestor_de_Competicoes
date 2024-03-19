import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuscaCompeticoesComponent } from './busca-competicoes.component';
import { CabecalhoModule } from '../cabecalho/cabecalho.module'; // Importando o CabecalhoModule
import { RodapeModule } from '../rodape/rodape.module';

@NgModule({
  declarations: [
    BuscaCompeticoesComponent
  ],
  imports: [
    CommonModule,
    CabecalhoModule,
    RodapeModule
    // Se precisar de outros módulos, importe-os aqui
  ],
  exports: [
    BuscaCompeticoesComponent
  ]
})
export class TelaInicialModule { }
