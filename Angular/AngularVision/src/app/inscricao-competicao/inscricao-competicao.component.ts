import { Component } from '@angular/core';

@Component({
  selector: 'app-inscricao-competicao',
  templateUrl: './inscricao-competicao.component.html',
  styleUrls: ['./inscricao-competicao.component.css']
})
export class InscricaoCompeticaoComponent {
  categorias = [
    { nome: 'Categoria 1' },
    { nome: 'Categoria 2' },
    { nome: 'Categoria 3' }
  ];
  valorTotal = 0;
  isModalOpen = false;
  isQrCodeGenerated = false;
  qrCodeUrl = '';
  categoriaSelecionada: any = {};
  competidor = { nome: '' };

  openInscricaoModal(categoria: any) {
    this.categoriaSelecionada = categoria;
    this.isModalOpen = true;
  }

  closeInscricaoModal() {
    this.isModalOpen = false;
  }

  inscreverCompetidor() {
    // Lógica para inscrever o competidor na categoria selecionada
    this.valorTotal += 100; // Exemplo de incremento do valor total
    this.isModalOpen = false;
    this.generateQrCode();
  }

  generateQrCode() {
    // Lógica para gerar o QR Code
    this.qrCodeUrl = 'https://example.com/qrcode'; // URL de exemplo
    this.isQrCodeGenerated = true;
  }
}