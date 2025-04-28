import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class GeoNamesService {
  private apiUrl = `${environment.apiBaseUrl}/api/Geonames`;
  private username = 'Guiru'; // Substitua 'your_username' pelo seu nome de usuário da GeoNames

  constructor(private http: HttpClient) { }

  getAllCountries() {
    const url = `${this.apiUrl}/countries`;
    return this.http.get(url);
  }

  getStatesByCountry(geonameID: string) {
    const url = `${this.apiUrl}/states/${geonameID}`;
    return this.http.get(url);
  }

  getCitiesByState(geonameID: string) {
    const url = `${this.apiUrl}/cities/${geonameID}`;
    return this.http.get(url);
  }
}
