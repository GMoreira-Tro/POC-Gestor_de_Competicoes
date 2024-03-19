import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TelaInicialComponent } from './tela-inicial.component';
import { CabecalhoModule } from '../cabecalho/cabecalho.module'; // Importando o CabecalhoModule
import { RodapeModule } from '../rodape/rodape.module';

@NgModule({
  declarations: [
    TelaInicialComponent
  ],
  imports: [
    CommonModule,
    CabecalhoModule,
    RodapeModule
    // Se precisar de outros módulos, importe-os aqui
  ],
  exports: [
    TelaInicialComponent
  ]
})
export class TelaInicialModule { }
