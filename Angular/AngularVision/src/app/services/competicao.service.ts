import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Competicao } from '../interfaces/Competicao';

@Injectable({
  providedIn: 'root'
})
export class CompeticaoService {
  private baseUrl = 'http://localhost:5000/api/Competicao'; // Substitua pela URL correta do seu backend

  constructor(private http: HttpClient) { }

  getCompeticoes(): Observable<Competicao[]> {
    return this.http.get<Competicao[]>(`${this.baseUrl}`);
  }

  getCompeticao(id: number): Observable<Competicao> {
    return this.http.get<Competicao>(`${this.baseUrl}/${id}`);
  }

  createCompeticao(competicao: Competicao): Observable<Competicao> {
    return this.http.post<Competicao>(`${this.baseUrl}`, competicao);
  }

  updateCompeticao(id: number, competicao: Competicao): Observable<Competicao> {
    return this.http.put<Competicao>(`${this.baseUrl}/${id}`, competicao);
  }

  deleteCompeticao(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  buscarCompeticoes(filtro: any): Observable<Competicao[]> {
    const params = new HttpParams({ fromObject: filtro });
    return this.http.get<Competicao[]>(`${this.baseUrl}/buscar`, { params });
  }

  buscarCompeticoesDoUsuario(userId: number): Observable<Competicao[]> {
    return this.http.get<Competicao[]>(`${this.baseUrl}/buscar-do-usuario/${userId}`);
  }

  uploadImagem(id: number, imagem: File): Observable<any> {
    const formData = new FormData();
    formData.append('imagem', imagem);

    return this.http.post<any>(`${this.baseUrl}/${id}/upload-imagem`, formData);
  }
}
