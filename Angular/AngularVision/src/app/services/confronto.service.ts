import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Confronto } from '../interfaces/Confronto';

// filepath: c:\Users\winck\OneDrive\Área de Trabalho\Guilherme\POC-Gestor_de_Competicoes\Angular\AngularVision\src\app\services\confronto.service.ts

@Injectable({
    providedIn: 'root'
})
export class ConfrontoService {
    private baseUrl = 'http://localhost:5000/api/Confronto'; // Substitua pela URL correta do seu backend

    constructor(private http: HttpClient) { }

    getConfrontos(): Observable<Confronto[]> {
        return this.http.get<Confronto[]>(`${this.baseUrl}`);
    }

    getConfronto(id: number): Observable<Confronto> {
        return this.http.get<Confronto>(`${this.baseUrl}/${id}`);
    }

    getConfrontosPorChaveamento(idChaveamento: number): Observable<Confronto[]> {
        return this.http.get<Confronto[]>(`${this.baseUrl}/buscar-de-chaveamento/${idChaveamento}`);
    }

    createConfronto(confronto: Confronto): Observable<Confronto> {
        return this.http.post<Confronto>(`${this.baseUrl}`, confronto);
    }

    updateConfronto(id: number, confronto: Confronto): Observable<Confronto> {
        return this.http.put<Confronto>(`${this.baseUrl}/${id}`, confronto);
    }

    deleteConfronto(id: number): Observable<any> {
        return this.http.delete(`${this.baseUrl}/${id}`);
    }
}