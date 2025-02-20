import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notificacao } from '../interfaces/Notificacao'; // Certifique-se de que a interface Notificacao está definida

@Injectable({
  providedIn: 'root'
})
export class NotificacaoService {
  private apiUrl = 'http://localhost:5000/api/Notificacao'; // Substitua pela URL correta do seu backend

  constructor(private http: HttpClient) {}

  cadastrarNotificacao(notificacao: Notificacao): Observable<Notificacao> {
    return this.http.post<Notificacao>(`${this.apiUrl}`, notificacao);
  }

  listarNotificacoes(): Observable<Notificacao[]> {
    return this.http.get<Notificacao[]>(`${this.apiUrl}`);
  }

  obterNotificacao(id: number): Observable<Notificacao> {
    return this.http.get<Notificacao>(`${this.apiUrl}/${id}`);
  }

  atualizarNotificacao(id: number, notificacao: Notificacao): Observable<Notificacao> {
    return this.http.put<Notificacao>(`${this.apiUrl}/${id}`, notificacao);
  }

  deletarNotificacao(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  buscarNotificacoes(filtro: any): Observable<Notificacao[]> {
    const params = new HttpParams({ fromObject: filtro });
    return this.http.get<Notificacao[]>(`${this.apiUrl}/buscar`, { params });
  }

  buscarNotificacoesDoUsuario(userId: number): Observable<Notificacao[]> {
    return this.http.get<Notificacao[]>(`${this.apiUrl}/buscar-do-usuario/${userId}`);
  }
}
