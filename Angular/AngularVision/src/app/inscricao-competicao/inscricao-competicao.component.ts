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
  gerarCamposCompetidores() {
    throw new Error('Method not implemented.');
  }
  categorias: Categoria[] = [];
  competidores: Competidor[] = [];
  valorTotal = 0;
  isModalOpen = false;
  isQrCodeGenerated = false;
  qrCodeUrl = '';
  categoriaSelecionada: any = {};
  competidor = { nome: '' };
  isFinalizarModalOpen = false;
  competicaoId: number = 0;
  infoModals: { quantidadeInscricoes: number, competidoresSelecionados: any[]}[] = [];
  indexCategoria: number = 0;

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
      this.infoModals = categorias.map(() => ({ quantidadeInscricoes: 1, competidoresSelecionados: [] }));
    });
  }

  loadCompetidores() {
    this.userService.getUsuarioLogado().subscribe(usuario => {
      this.competidorService.buscarCompetidoresDoUsuario(usuario.id).subscribe(competidores => {
        this.competidores = competidores;
      });
    });
  }

  openInscricaoModal(categoria: any, index: number) {
    this.categoriaSelecionada = categoria;
    this.indexCategoria = index;
    this.isModalOpen = true;
  }

  closeInscricaoModal() {
    this.isModalOpen = false;
  }

  inscreverCompetidor() {
    if (this.infoModals[this.indexCategoria].competidoresSelecionados.length !== this.infoModals[this.indexCategoria].quantidadeInscricoes) {
      alert('Você precisa selecionar todos os competidores da categoria.');
      return;
    }
    for(let i = 0; i < this.infoModals[this.indexCategoria].competidoresSelecionados.length; i++)
    {
      if(this.infoModals[this.indexCategoria].competidoresSelecionados[i] === undefined)
      {
        alert('Você precisa selecionar todos os competidores da categoria.');
        return;
      }
    }

    this.valorTotal = 0;
    for (let i = 0; i < this.infoModals.length; i++) {
      this.valorTotal += this.categorias[i].valorInscricao * this.infoModals[i].quantidadeInscricoes;
    }
    this.closeInscricaoModal();
  }

  finalizarInscricoes() {
    this.isFinalizarModalOpen = true;
  }

  closeFinalizarModal() {
    this.isFinalizarModalOpen = false;
  }
}