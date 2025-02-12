import { Component, OnInit } from '@angular/core';
import { CompeticaoService } from '../services/competicao.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-minhas-competicoes',
    templateUrl: 'minhas-competicoes.component.html',
    styleUrls: ['./minhas-competicoes.component.css']
})
export class MinhasCompeticoesComponent implements OnInit {
    minhasCompeticoes: any[] = [];
    userId: number = 0; // Substitua pelo ID do usuário logado

    constructor(private competicaoService: CompeticaoService,
        private router: Router,
        private userService: UserService
    ) { }

    ngOnInit(): void {
        this.userService.getUsuarioLogado().subscribe(usuario => {
            this.userId = usuario.id;
            this.buscarMinhasCompeticoes();
        });
    }

    buscarMinhasCompeticoes(): void {
        this.competicaoService.buscarCompeticoesDoUsuario(this.userId).subscribe(competicoes => {
            this.minhasCompeticoes = competicoes;
        }, error => console.log("Erro ao buscar competições: ", error));
    }

    editarCompeticao(id: number): void {
        this.router.navigate(['/editar-competicao', id]);
    }

    excluirCompeticao(id: number): void {
        if (confirm("Tem certeza que deseja excluir esta competição?")) {
            this.competicaoService.deleteCompeticao(id).subscribe(() => {
                this.buscarMinhasCompeticoes(); // Atualiza a lista
            }, error => console.log("Erro ao excluir competição: ", error));
        }
    }
}
