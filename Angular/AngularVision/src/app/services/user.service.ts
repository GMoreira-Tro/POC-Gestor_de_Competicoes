import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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

  createUser(user: Usuario): Observable<Usuario> {
    return this.http.post<Usuario>(`${this.baseUrl}`, user);
  }

  updateUser(id: number, user: Usuario): Observable<Usuario> {
    return this.http.put<Usuario>(`${this.baseUrl}/${id}`, user);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  sendConfirmationEmail(email: string): Observable<any> {
    const emailData = { email: email };
    return this.http.post<any>(`${this.baseUrl}/email-confirmation`, emailData); // Substitua pela rota correta no backend para enviar o e-mail de confirmação
  }
}
