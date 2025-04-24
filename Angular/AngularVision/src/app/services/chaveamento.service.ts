import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chaveamento } from '../interfaces/Chaveamento';
import { environment } from '../../environments/environment.prod';

@Injectable({
    providedIn: 'root'
})
export class ChaveamentoService {
    private baseUrl = `${environment.apiBaseUrl}/api/Chaveamento`; // Substitua pela URL correta do seu backend

    constructor(private http: HttpClient) { }

    getChaveamentos(): Observable<Chaveamento[]> {
        return this.http.get<Chaveamento[]>(`${this.baseUrl}`);
    }

    getChaveamento(id: number): Observable<Chaveamento> {
        return this.http.get<Chaveamento>(`${this.baseUrl}/${id}`);
    }

    getChaveamentosPorCategoria(idCategoria: number): Observable<Chaveamento[]> {
        return this.http.get<Chaveamento[]>(`${this.baseUrl}/buscar-de-categoria/${idCategoria}`);
    }

    createChaveamento(chaveamento: Chaveamento): Observable<Chaveamento> {
        return this.http.post<Chaveamento>(`${this.baseUrl}`, chaveamento);
    }

    updateChaveamento(id: number, chaveamento: Chaveamento): Observable<Chaveamento> {
        return this.http.put<Chaveamento>(`${this.baseUrl}/${id}`, chaveamento);
    }

    deleteChaveamento(id: number): Observable<any> {
        return this.http.delete(`${this.baseUrl}/${id}`);
    }
}