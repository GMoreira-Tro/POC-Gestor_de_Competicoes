import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HousingLocation } from '../housing-location';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-housing-location',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <section class="listing">
      <img class="listing-photo" [src]="housingLocation.photo"
       alt="Foto de {{housingLocation.name}}">
      <h2 class="listing-header">{{housingLocation.name}}</h2>
      <p class="listing-location">Cidade: {{housingLocation.city}}\nEstado: {{housingLocation.state}}</p>
      <a [routerLink]="['Details', housingLocation.id]">Detalhes</a>
    </section>
  `,
  styleUrls: ['./housing-location.component.css']
})
export class HousingLocationComponent {
  @Input() housingLocation!:HousingLocation
}
