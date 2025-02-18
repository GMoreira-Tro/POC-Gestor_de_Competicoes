import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Inscricao } from '../interfaces/Inscricao'; // Certifique-se de que a interface Inscricao está definida

@Injectable({
  providedIn: 'root'
})
export class InscricaoService {
  private apiUrl = 'http://localhost:5000/api/Inscricao'; // Substitua pela URL correta do seu backend

  constructor(private http: HttpClient) {}

  cadastrarInscricao(inscricao: Inscricao): Observable<Inscricao> {
    return this.http.post<Inscricao>(`${this.apiUrl}`, inscricao);
  }

  listarInscricoes(): Observable<Inscricao[]> {
    return this.http.get<Inscricao[]>(`${this.apiUrl}`);
  }

  obterInscricao(id: number): Observable<Inscricao> {
    return this.http.get<Inscricao>(`${this.apiUrl}/${id}`);
  }

  atualizarInscricao(id: number, inscricao: Inscricao): Observable<Inscricao> {
    return this.http.put<Inscricao>(`${this.apiUrl}/${id}`, inscricao);
  }

  deletarInscricao(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  buscarInscricoes(filtro: any): Observable<Inscricao[]> {
    const params = new HttpParams({ fromObject: filtro });
    return this.http.get<Inscricao[]>(`${this.apiUrl}/buscar`, { params });
  }

  buscarInscricoesDoUsuario(userId: number): Observable<Inscricao[]> {
    return this.http.get<Inscricao[]>(`${this.apiUrl}/buscar-do-usuario/${userId}`);
  }
}
