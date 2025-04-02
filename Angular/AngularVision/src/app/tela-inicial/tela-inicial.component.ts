import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { CompeticaoService } from '../services/competicao.service';
import { Router } from '@angular/router';

interface Competicao {
  nome: string;
}

@Component({
  selector: 'app-tela-inicial',
  templateUrl: './tela-inicial.component.html',
  styleUrls: ['./tela-inicial.component.css']
})
export class TelaInicialComponent implements OnInit {
  competicoes: any[] = [];
  filtroSelecionado: string = 'municipal';
  usuarioLogado: any

  constructor(private userService: UserService,
    private competicaoService: CompeticaoService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.userService.getUsuarioLogado().subscribe(usuario => {
      this.usuarioLogado = usuario;
      this.buscarMinhasCompeticoes();
    });
  }

  buscarMinhasCompeticoes(): void {
    const params: {
      pais?: string,
      estado?: string,
      cidade?: string
    } = {};
    switch (this.filtroSelecionado) {
      case 'mundial':
        // No filter needed for mundial
        break;
      case 'nacional':
        params.pais = this.usuarioLogado.pais;
        break;
      case 'estadual':
        params.pais = this.usuarioLogado.pais;
        params.estado = this.usuarioLogado.estado;
        break;
      case 'municipal':
        params.pais = this.usuarioLogado.pais;
        params.estado = this.usuarioLogado.estado;
        params.cidade = this.usuarioLogado.cidade;
        break;
      default:
        // Handle default case if needed
        break;
    }

    this.competicaoService.buscarCompeticoes(params).subscribe(competicoes => {
      this.competicoes = competicoes;

      this.competicoes.forEach(competicao => {
        if (competicao.bannerImagem !== null) {
          competicao.bannerImagem = competicao.bannerImagem?.startsWith('http')
            ? competicao.bannerImagem
            : `http://localhost:5000/${competicao.bannerImagem}`;
        }
      });
    }, error => console.log("Erro ao buscar competições: ", error));
  }

  // Função para se inscrever em uma competição
  inscreverCompeticao(id: number): void {
    this.router.navigate([`/inscricao-competicao/${id}`]);
  }

  verInformacoes(id: number): void {
    this.router.navigate([`/informacoes/${id}`]);
  }
}