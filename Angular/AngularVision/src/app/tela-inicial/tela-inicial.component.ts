import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { CompeticaoService } from '../services/competicao.service';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.prod';

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
  currentIndex = 0;

  slides = [
    {
      imagem: 'assets/images/criacao.png',
      texto: 'Crie suas próprias competições com categorias personalizadas. Divulgue seu evento e atraia atletas de toda parte.'
    },
    {
      imagem: 'assets/images/registro.png',
      texto: 'Registre seus atletas na plataforma para vinculá-los às suas competições ou inscrevê-los em competições de terceiros.'
    },
    {
      imagem: 'assets/images/inscricao.png',
      texto: 'Solicite a inscrição de seus competidores em eventos criados por outros usuários. Após a aprovação, realize o pagamento diretamente na plataforma.'
    },
    {
      imagem: 'assets/images/gerenciar-inscricoes.png',
      texto: 'Receba inscrições em suas competições e gerencie-as com praticidade. Aprove ou recuse com apenas um clique.'
    },
    {
      imagem: 'assets/images/confrontos.png',
      texto: 'Crie os chaveamentos das categorias da sua competição, registre os vencedores e avance com controle total.'
    }
  ];

  avancar() {
    this.currentIndex = (this.currentIndex + 1) % this.slides.length;
  }

  voltar() {
    this.currentIndex = (this.currentIndex - 1 + this.slides.length) % this.slides.length;
  }

  irParaSlide(index: number) {
    this.currentIndex = index;
  }

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
            : `${environment.apiBaseUrl}/${competicao.bannerImagem}`;
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