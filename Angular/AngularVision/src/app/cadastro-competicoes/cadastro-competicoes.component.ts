import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Competicao } from '../interfaces/Competicao';

@Component({
  selector: 'app-cadastro-competicoes',
  templateUrl: './cadastro-competicoes.component.html',
  styleUrls: ['./cadastro-competicoes.component.css']
})
export class CadastroCompeticoesComponent implements OnInit {

  competicao: Competicao = {
    titulo: '',
    descricao: '',
    bannerImagem: undefined,
    categorias: [],
    id: 0,
    dataInicio: new Date(),
    dataFim: new Date(),
    idCriadorUsuario: 0,
    usuario: undefined
  }

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    // Lógica para enviar os dados do formulário para o backend e cadastrar a competição
    console.log('Formulário enviado:', this.competicao);
  }

  onFileSelected(event: any): void {
    // Obtém a imagem selecionada pelo usuário
    this.competicao.bannerImagem = event.target.files[0] as File;
  }

  adicionarCategoria(): void {
    // Adiciona uma nova categoria à lista de categorias
    //this.competicao.categorias.push('');
  }

  removerCategoria(index: number): void {
    // Remove a categoria da lista de categorias com base no índice
    //this.categorias.splice(index, 1);
  }

}
