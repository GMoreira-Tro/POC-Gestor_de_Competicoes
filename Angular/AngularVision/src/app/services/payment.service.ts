import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PixCharge } from '../interfaces/PixCharge';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private apiUrl = 'http://localhost:5000/api/Pix'; // URL do seu backend

  constructor(private http: HttpClient) { }

  // Gera a localização do QR Code para pagamento PIX
  generateQRCodeLocation(charge: PixCharge): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/generate`, charge);
  }

  // Obtém o QR Code em Base64 usando a localização gerada
  generateQRCodeBase64(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/base64/${id}`);
  }

  // Solicita o estorno de um pagamento PIX usando o e2eId da transação
  refundPIX(e2eId: string, transacaoId: string, valorEstornado: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/devolucao/${e2eId}/${transacaoId}`, {
      valor: valorEstornado
    });
  }

  // Consulta o saldo disponível na conta Efi Bank
  getBalance(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/consulta`);
  }

  getUserPayments(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/payments/user/${userId}`);
  }
  
  getAllPixPayments(inicio: string, fim: string): Observable<any> {
    const params = new HttpParams()
      .set('inicio', inicio)
      .set('fim', fim);
  
    return this.http.get<any>(`${this.apiUrl}/payments/consulta`, { params });
  }  

  getPixPaymentsByE2eId(e2eId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/payments/consulta/${e2eId}`);
  }

  getPixPaymentByTxid(txid: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/payments/consulta-txid/${txid}`);
  }

  getPixPaymentByLocid(locid: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/payments/consulta-location/${locid}`);
  }

  registerUserPayment(payment: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/payments/user`, payment);
  }

  // Envia um pagamento PIX para um favorecido
  sendPix(idEnvio: string, pixSent: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/envio/${idEnvio}`, pixSent);
  }

  receberEmailInscricao(idInscricao: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/receber-email-inscricao/${idInscricao}`, {});
  }
}
