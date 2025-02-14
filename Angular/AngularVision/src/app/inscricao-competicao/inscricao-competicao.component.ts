import { Component, OnInit } from '@angular/core';
import { Categoria } from '../interfaces/Categoria';
import { Competidor } from '../interfaces/Competidor';
import { CategoriaService } from '../services/categoria.service';
import { CompetidorService } from '../services/competidor.service';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-inscricao-competicao',
  templateUrl: './inscricao-competicao.component.html',
  styleUrls: ['./inscricao-competicao.component.css']
})
export class InscricaoCompeticaoComponent implements OnInit {
  categorias: Categoria[] = [];
  competidores: Competidor[] = [];
  valorTotal = 0;
  isModalOpen = false;
  isQrCodeGenerated = false;
  qrCodeUrl = '';
  categoriaSelecionada: any = {};
  competidorSelecionado: any = {};
  competidor = { nome: '' };
  isFinalizarModalOpen = false;
  competicaoId: number = 0;

  constructor(private categoriaService: CategoriaService,
    private competidorService: CompetidorService,
    private userService: UserService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.competicaoId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadCategorias();
    this.loadCompetidores();
  }

  loadCategorias() {
    this.categoriaService.getCategoriasPorCompeticao(this.competicaoId).subscribe(categorias => {
      this.categorias = categorias;
    });
  }
  
  loadCompetidores() {
    this.userService.getUsuarioLogado().subscribe(usuario => {
      this.competidorService.buscarCompetidoresDoUsuario(usuario.id).subscribe(competidores => {
        this.competidores = competidores;
        });
    });
  }

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
  }

  finalizarInscricoes() {
    this.isFinalizarModalOpen = true;
  }

  closeFinalizarModal() {
    this.isFinalizarModalOpen = false;
  }
}