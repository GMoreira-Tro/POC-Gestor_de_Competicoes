import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Usuario } from '../interfaces/Usuario';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'http://localhost:5000/api/Usuario'; // Substitua pela URL correta do seu backend

  constructor(private http: HttpClient) { }

  getUsers(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(`${this.baseUrl}`);
  }

  getUser(id: number): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.baseUrl}/${id}`);
  }

  getUserByEmail(email: string): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.baseUrl}/email/${email}`);
  }

  createUser(user: Usuario): Observable<Usuario> {
    return this.http.post<Usuario>(`${this.baseUrl}`, user);
  }

  updateUser(id: number, user: Usuario): Observable<Usuario> {
    return this.http.put<Usuario>(`${this.baseUrl}/${id}`, user);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  confirmarEmail(token: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/confirmar-email/${token}`);
  }

  reenviarEmailConfirmacao(email: string): Observable<any> {
      return this.http.get(`${this.baseUrl}/reenviar-email-confirmacao/${email}`);
  }

  getUsuarioLogado(): Observable<Usuario> {
    return this.getUser(Number(localStorage.getItem('userId')));
  }

  uploadImagem(id: number, imagem: File): Observable<any> {
    const formData = new FormData();
    formData.append('imagem', imagem);

    return this.http.post<any>(`${this.baseUrl}/${id}/upload-imagem`, formData);
  }
}
