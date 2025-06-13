import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { InscricaoService } from '../services/inscricao.service';
import { CategoriaService } from '../services/categoria.service';
import { NotificacaoService } from '../services/notificacao.service';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-aprovar-inscricao',
  templateUrl: './aprovar-inscricao.component.html',
  styleUrls: ['./aprovar-inscricao.component.css']
})
export class AprovarInscricaoComponent implements OnInit {
  competicaoId: number = 0;
  tituloCompeticao: string = "";
  userId: number = 0;
  infoInscricoes: any[] = [];
  categorias: { [categoriaId: number]: any } = {};
  carregouInscricoes: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private competicaoService: CompeticaoService,
    private inscricaoService: InscricaoService,
    private categoriaService: CategoriaService,
    private notificacaoService: NotificacaoService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.competicaoId = Number(this.route.snapshot.paramMap.get('id'));
    this.competicaoService.getCompeticao(this.competicaoId).subscribe(
      (competicao) => {
        this.userService.getUsuarioLogado().subscribe(user => {
          if (competicao.criadorUsuarioId !== user.id) {
            console.log("Usuário tentou editar uma competição que não lhe pertence.");
            this.router.navigate(['/']);
            return;
          }
          this.tituloCompeticao = competicao.titulo;
        }
        )
      },
      () => {
        this.router.navigate(['/']);
      }
    );

    this.inscricaoService.getInscricoesDeCompeticao(this.competicaoId).subscribe(inscricoes => {
      const requisicoes = inscricoes.map(inscricao =>
        this.inscricaoService.getInfoInscricao(inscricao.id).toPromise().then(infoInscricao => {
          infoInscricao.status = inscricao.status;
          infoInscricao.categoriaId = inscricao.categoriaId;
          infoInscricao.inscricao = inscricao;
          return infoInscricao;
        })
      );

      Promise.all(requisicoes).then(infoList => {
        this.infoInscricoes = infoList; // 🔁 Nova referência!
        this.carregouInscricoes = true;
        this.cdr.detectChanges(); // agora sim funciona!
      });

      // Carrega categorias (isso pode ficar separado mesmo)
      inscricoes.forEach(inscricao => {
        this.categoriaService.getCategoria(inscricao.categoriaId).subscribe(categoria => {
          this.categorias[inscricao.categoriaId] = categoria;
        });
      });
    });

  }

  aprovarInscricao(infoInscricao: any) {
    if (!confirm("Você tem certeza que deseja aprovar esta inscrição? Esta ação não pode ser desfeita.")) {
      return;
    }

    infoInscricao.inscricao.status = 1;
    infoInscricao.status = 1;
    this.inscricaoService.atualizarInscricao(infoInscricao.inscricao.id, infoInscricao.inscricao).subscribe(() => {
      this.notificacaoService.cadastrarNotificacaoDeInscricao(infoInscricao.inscricao.id).subscribe();
    });
  }

  recusarInscricao(infoInscricao: any) {
    if (!confirm("Você tem certeza que deseja recusar esta inscrição? Esta ação não pode ser desfeita.")) {
      return;
    }
    infoInscricao.inscricao.status = 3;
    infoInscricao.status = 3;
    this.inscricaoService.atualizarInscricao(infoInscricao.inscricao.id, infoInscricao.inscricao).subscribe(() => {
      this.notificacaoService.cadastrarNotificacaoDeInscricao(infoInscricao.inscricao.id).subscribe();
    });
  }
}