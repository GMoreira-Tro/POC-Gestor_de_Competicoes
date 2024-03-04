import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { HousingService } from '../housing.service';
import { HousingLocation } from '../housing-location';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
   <article>
    <img class="listing photo" [src]="housingLocation?.photo">
    <section class="listing description">
      <h2 class="listing-heading">{{housingLocation?.name}}</h2>
      <p class="listing-location">{{housingLocation?.city}}, {{housingLocation?.state}}</p>
    </section>
    <section class="listing-features">
      <h2 class="section-header">Sobre esta casa</h2>
      <ul>
        <li>Unidades disponíveis: {{housingLocation?.availableUnits}}</li>
        <li>{{hasWifiText}}</li>
        <li>Tem laundry? {{housingLocation?.laundry}}</li>
      </ul>
    <section class="listing-apply">
      <h2 class="section-header">Comprar casa agora</h2>
      <form [formGroup]="applyForm" (submit)="submitApplyForm()">
        <label form="nome">Nome</label>
        <input id="nome" type="text" formControlName="nome">
        <label form="sobrenome">Sobrenome</label>
        <input id="sobrenome" type="text" formControlName="sobrenome">
        <label form="email">Email</label>
        <input id="email" type="text" formControlName="email">
        <button type="submit" class="Primary">Comprar</button>
      </form>
  `,
  styleUrls: ['./details.component.css']
})
export class DetailsComponent {
  route: ActivatedRoute = inject(ActivatedRoute);
  housingService: HousingService = inject(HousingService);
  housingLocation: HousingLocation | undefined;
  hasWifiText: String;
  applyForm = new FormGroup({
    nome: new FormControl(''),
    sobrenome: new FormControl(''),
    email: new FormControl('')
  })

  constructor() {
    const housingLocationId = Number(this.route.snapshot.params['id']);
    console.log(housingLocationId);
    this.housingService.getHousingLocationById(housingLocationId).then(hl => 
    {
      this.housingLocation = hl;
    });
    console.log(this.housingLocation);

    this.hasWifiText = this.housingLocation?.wifi ? "Tem wifi" : "Não tem wifi";
  }

  submitApplyForm(){
    this.housingService.submitApp(
      this.applyForm.value.nome ?? '',
      this.applyForm.value.sobrenome ?? '',
      this.applyForm.value.email ?? ''
    )
  }
}
