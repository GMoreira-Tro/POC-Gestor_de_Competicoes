import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Competicao } from '../interfaces/Competicao';
import { Categoria } from '../interfaces/Categoria';
import { CompeticaoService } from '../services/competicao.service';
import { CategoriaService } from '../services/categoria.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-cadastro-competicoes',
  templateUrl: './cadastro-competicoes.component.html',
  styleUrls: ['./cadastro-competicoes.component.css']
})
export class CadastroCompeticoesComponent implements OnInit {

  competicao: Competicao = {
    titulo: '',
    descricao: '',
    modalidade: '',
    pais: '',
    estado: '',
    cidade: '',
    bannerImagem: undefined,
    categorias: [],
    id: 0,
    dataInicio: new Date(),
    dataFim: new Date(),
    criadorUsuarioId: 0,  // Ajustar depois para o ID do usuário logado
    status: 0
  };

  constructor(private competicaoService: CompeticaoService, private categoriaService: CategoriaService,
    private router: Router, private http: HttpClient) { }

  ngOnInit(): void { }

  onSubmit(): void {
    if (this.competicao.categorias.length === 0)
    {
      alert("A competição deve ter pelo menos uma categoria.");
      return;
    }
    this.competicao.status = 1; //Ajusta o status para publicada
    console.log(this.competicao);
    this.competicaoService.createCompeticao(this.competicao).subscribe(novaCompeticao => {  
      this.competicao.categorias.forEach(categoria => 
        {
          categoria.competicaoId = novaCompeticao.id;
          this.categoriaService.createCategoria(categoria).subscribe();
        });
    }, (error) => {
        console.log("Erro ao criar competição: ", error);
      }
    );
  }
  
  onFileSelected(event: any): void {
    this.competicao.bannerImagem = event.target.files[0] as File;
  }

  adicionarCategoria(): void {
    const novaCategoria: Categoria = {
      id: 0,  // O backend deve gerar esse ID
      nome: '',
      descricao: '',
      competicaoId: 0,
      inscricoes: []
    };
    this.competicao.categorias.push(novaCategoria);
  }

  removerCategoria(index: number): void {
    this.competicao.categorias.splice(index, 1);
  }

    // Função que chama a API do GeoNames para buscar Estado e Cidade com base no País
    buscarEstadoCidade(): void {
      const pais = this.competicao.pais;
  
      if (pais) {
        this.http.get(`http://api.geonames.org/searchJSON?name_equals=${pais}&maxRows=10000&username=Guiru`)
          .subscribe((response: any) => {
            if (response.geonames && response.geonames.length > 0) {
              const local = response.geonames[0]; // Pega o primeiro local encontrado
              this.competicao.estado = local.adminName1; // Estado
              this.competicao.cidade = local.name; // Cidade
            }
          }, (error) => {
            console.error('Erro ao buscar localização:', error);
          });
      }
    }
}
