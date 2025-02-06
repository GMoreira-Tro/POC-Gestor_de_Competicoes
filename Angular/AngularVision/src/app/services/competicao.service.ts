import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Competicao } from '../interfaces/Competicao';

@Injectable({
  providedIn: 'root'
})
export class CompeticaoService {
  private baseUrl = 'http://localhost:5000/api/Competicao'; // Substitua pela URL correta do seu backend

  constructor(private http: HttpClient) { }

  getCompeticoes(): Observable<Competicao[]> {
    return this.http.get<Competicao[]>(`${this.baseUrl}`).pipe(
      catchError(this.handleError)
    );
  }

  getCompeticao(id: number): Observable<Competicao> {
    return this.http.get<Competicao>(`${this.baseUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  createCompeticao(competicao: Competicao): Observable<Competicao> {
    return this.http.post<Competicao>(`${this.baseUrl}`, competicao).pipe(
      catchError(this.handleError)
    );
  }

  updateCompeticao(id: number, competicao: Competicao): Observable<Competicao> {
    return this.http.put<Competicao>(`${this.baseUrl}/${id}`, competicao).pipe(
      catchError(this.handleError)
    );
  }

  deleteCompeticao(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    console.error('Erro:', error);
    return throwError(error);
  }
}
