import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfrontoInscricao } from '../interfaces/ConfrontoInscricao';

@Injectable({
    providedIn: 'root'
})
export class ConfrontoInscricaoService {
    private baseUrl = 'http://localhost:5000/api/ConfrontoInscricao'; // Substitua pela URL correta do seu backend

    constructor(private http: HttpClient) { }

    getConfrontoInscricoes(): Observable<ConfrontoInscricao[]> {
        return this.http.get<ConfrontoInscricao[]>(`${this.baseUrl}`);
    }

    getConfrontoInscricao(id: number): Observable<ConfrontoInscricao> {
        return this.http.get<ConfrontoInscricao>(`${this.baseUrl}/${id}`);
    }

    getConfrontoInscricoesPorConfronto(idConfronto: number): Observable<ConfrontoInscricao[]> {
        return this.http.get<ConfrontoInscricao[]>(`${this.baseUrl}/buscar-de-confronto/${idConfronto}`);
    }

    createConfrontoInscricao(confrontoInscricao: ConfrontoInscricao): Observable<ConfrontoInscricao> {
        return this.http.post<ConfrontoInscricao>(`${this.baseUrl}`, confrontoInscricao);
    }

    updateConfrontoInscricao(id: number, confrontoInscricao: ConfrontoInscricao): Observable<ConfrontoInscricao> {
        return this.http.put<ConfrontoInscricao>(`${this.baseUrl}/${id}`, confrontoInscricao);
    }

    deleteConfrontoInscricao(id: number): Observable<any> {
        return this.http.delete(`${this.baseUrl}/${id}`);
    }
}