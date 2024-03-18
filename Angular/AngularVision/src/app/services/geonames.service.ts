import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GeoNamesService {
  private apiUrl = 'http://api.geonames.org';
  private username = 'Guiru'; // Substitua 'your_username' pelo seu nome de usuário da GeoNames

  constructor(private http: HttpClient) { }

  getAllCountries() {
    const url = `${this.apiUrl}/countryInfoJSON?username=${this.username}`;
    return this.http.get(url);
  }

  // Método para obter os estados ou províncias de um país específico
  getStatesByCountry(geonameID: string) {
    const maxRows = 10000;
    const url = `${this.apiUrl}/childrenJSON?geonameId=${geonameID}&username=${this.username}&maxRows=${maxRows}`;
    console.log(url)
    return this.http.get(url);
  }

  getCitiesByState(geonameID: string) {
    const maxRows = 10000;
    const url = `${this.apiUrl}/childrenJSON?geonameId=${geonameID}&username=${this.username}&maxRows=${maxRows}`;
    return this.http.get(url);
  }
}
