import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { RodapeModule } from "../rodape/rodape.module";
import { InscricaoService } from '../services/inscricao.service';
import { CategoriaService } from '../services/categoria.service';

@Component({
  selector: 'app-aprovar-inscricao',
  templateUrl: './aprovar-inscricao.component.html',
  styleUrls: ['./aprovar-inscricao.component.css']
})
export class AprovarInscricaoComponent implements OnInit {
  competicaoId: number = 0;
  userId: number = 0;
  infoInscricoes: any[] = [];
  categorias: { [categoriaId: number]: any } = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private competicaoService: CompeticaoService,
    private inscricaoService: InscricaoService,
    private categoriaService: CategoriaService
  ) {}

  ngOnInit(): void {
    this.competicaoId = Number(this.route.snapshot.paramMap.get('id'));
    this.competicaoService.getCompeticao(this.competicaoId).subscribe(
      (competicao) => {
        this.userService.getUsuarioLogado().subscribe(user =>
        {
          if(competicao.criadorUsuarioId !== user.id)
          {
            console.log("Usuário tentou editar uma competição que não lhe pertence.");
            this.router.navigate(['/']);
            return;
          }
        }
        )
      },
      () => {
        this.router.navigate(['/']);
      }
    );

    this.inscricaoService.getInscricoesDeCompeticao(this.competicaoId).subscribe(inscricoes => {
      inscricoes.forEach(inscricao => {
        this.inscricaoService.getInfoInscricao(inscricao.id).subscribe(infoInscricao =>
        {
          infoInscricao.status = inscricao.status;
          infoInscricao.categoriaId = inscricao.categoriaId;
          this.infoInscricoes.push(infoInscricao);
        });

        this.categoriaService.getCategoria(inscricao.categoriaId).subscribe(categoria =>
        {
          this.categorias[inscricao.categoriaId] = categoria;
        }
        );
      })
    });
  }

  aprovarInscricao()
  {
    alert("gay")
  }
}