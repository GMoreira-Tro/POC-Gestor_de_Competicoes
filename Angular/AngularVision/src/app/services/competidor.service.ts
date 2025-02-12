import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Competidor } from '../interfaces/Competidor'; // Certifique-se de que a interface Competidor está definida

@Injectable({
  providedIn: 'root'
})
export class CompetidorService {
  private apiUrl = 'http://localhost:5000/api/Competidor'; // Substitua pela URL correta do seu backend

  constructor(private http: HttpClient) {}

  cadastrarCompetidor(competidor: Competidor): Observable<Competidor> {
    return this.http.post<Competidor>(`${this.apiUrl}`, competidor);
  }

  listarCompetidores(): Observable<Competidor[]> {
    return this.http.get<Competidor[]>(`${this.apiUrl}`);
  }

  obterCompetidor(id: number): Observable<Competidor> {
    return this.http.get<Competidor>(`${this.apiUrl}/${id}`);
  }

  atualizarCompetidor(id: number, competidor: Competidor): Observable<Competidor> {
    return this.http.put<Competidor>(`${this.apiUrl}/${id}`, competidor);
  }

  deletarCompetidor(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  buscarCompetidores(filtro: any): Observable<Competidor[]> {
    const params = new HttpParams({ fromObject: filtro });
    return this.http.get<Competidor[]>(`${this.apiUrl}/buscar`, { params });
  }

  buscarCompetidoresDoUsuario(userId: number): Observable<Competidor[]> {
    return this.http.get<Competidor[]>(`${this.apiUrl}/buscar-do-usuario/${userId}`);
  }
}
