import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HousingLocationComponent} from '../housing-location/housing-location.component';
import { HousingLocation } from '../housing-location';
import { HousingService } from '../housing.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, HousingLocationComponent],
  template: `
    <section>
      <form>
        <input type="Text" placeholder="Filtro por cidade" #filter>
        <button class="Primary" type="button" (click)="filterResults
        (filter.value)">Procurar</button>
      </form>
    </section>
    <section>
      <app-housing-location *ngFor="let housingLocation of filteredList"
      [housingLocation]="housingLocation">
      </app-housing-location>
    </section>
  `,
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  housingLocationList: HousingLocation[] = [];
  filteredList: HousingLocation[] = [];
  housingService: HousingService = inject(HousingService);

  constructor() {
    this.housingService.getAllHousingLocations().then((hl: HousingLocation[]) => 
      {
        this.housingLocationList = hl;
        this.filteredList = hl;
      })
  }

  filterResults(text: string) 
  {
    this.filteredList = this.housingLocationList.filter(hl =>
      {
        hl.name.toLowerCase().includes(text.toLowerCase());
      })
  }
}
