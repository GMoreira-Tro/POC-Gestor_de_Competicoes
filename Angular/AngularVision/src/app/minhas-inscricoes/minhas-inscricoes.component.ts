import { Component, OnInit } from '@angular/core';
import { InscricaoService } from '../services/inscricao.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-minhas-inscricoes',
    templateUrl: './minhas-inscricoes.component.html',
    styleUrls: ['./minhas-inscricoes.component.css']
})
export class MinhasInscricoesComponent implements OnInit {
    minhasInscricoes: any[] = [];
    userId: number = 0;

    constructor(private inscricaoService: InscricaoService,
                private router: Router,
                private userService: UserService) { }

    ngOnInit(): void {
        this.userService.getUsuarioLogado().subscribe(usuario => {
            this.userId = usuario.id;
            this.buscarMinhasInscricoes();
        });
    }

    buscarMinhasInscricoes(): void {
        this.inscricaoService.buscarInscricoesDoUsuario(this.userId).subscribe(inscricoes => {
            this.minhasInscricoes = inscricoes;
        }, error => console.log("Erro ao buscar inscrições: ", error));
    }

    editarInscricao(id: number): void {
        this.router.navigate(['/editar-inscricao', id]);
    }

    excluirInscricao(id: number): void {
        if (confirm("Tem certeza que deseja excluir esta inscrição?")) {
            this.inscricaoService.deletarInscricao(id).subscribe(() => {
                this.buscarMinhasInscricoes(); // Atualiza a lista
            }, error => console.log("Erro ao excluir inscrição: ", error));
        }
    }
}