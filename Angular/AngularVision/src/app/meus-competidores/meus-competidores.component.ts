import { Component, OnInit } from '@angular/core';
import { CompetidorService } from '../services/competidor.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-meus-competidores',
    templateUrl: 'meus-competidores.component.html',
    styleUrls: ['./meus-competidores.component.css']
})
export class MeusCompetidoresComponent implements OnInit {
    meusCompetidores: any[] = [];
    userId: number = 0; // Substitua pelo ID do usuário logado

    constructor(private competidorService: CompetidorService,
        private router: Router,
        private userService: UserService
    ) { }

    ngOnInit(): void {
        this.userService.getUsuarioLogado().subscribe(usuario => {
            this.userId = usuario.id;
            this.buscarMeusCompetidores();
        });
    }

    buscarMeusCompetidores(): void {
        this.competidorService.buscarCompetidoresDoUsuario(this.userId).subscribe(competidores => {
            this.meusCompetidores = competidores;
        }, error => console.log("Erro ao buscar competidores: ", error));
    }

    editarCompetidor(id: number): void {
        this.router.navigate(['/editar-competidor', id]);
    }

    excluirCompetidor(id: number): void {
        if (confirm("Tem certeza que deseja excluir este competidor?")) {
            this.competidorService.deletarCompetidor(id).subscribe(() => {
                this.buscarMeusCompetidores(); // Atualiza a lista
            }, error => console.log("Erro ao excluir competidor: ", error));
        }
    }
}