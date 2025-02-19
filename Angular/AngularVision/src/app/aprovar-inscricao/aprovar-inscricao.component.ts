import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { RodapeModule } from "../rodape/rodape.module";

@Component({
  selector: 'app-aprovar-inscricao',
  templateUrl: './aprovar-inscricao.component.html',
  styleUrls: ['./aprovar-inscricao.component.css']
})
export class AprovarInscricaoComponent implements OnInit {
  competicaoId: number = 0;
  userId: number = 0;
  inscricoes: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private competicaoService: CompeticaoService
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
      (error) => {
        console.error('Error fetching competition', error);
        this.router.navigate(['/error']);
      }
    );
  }

  aprovarInscricao()
  {
    alert("gay")
  }
}