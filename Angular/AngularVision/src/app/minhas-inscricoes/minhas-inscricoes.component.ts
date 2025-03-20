import { Component, OnInit } from '@angular/core';
import { InscricaoService } from '../services/inscricao.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { CompeticaoService } from '../services/competicao.service';
import { CompetidorService } from '../services/competidor.service';
import { CategoriaService } from '../services/categoria.service';

@Component({
    selector: 'app-minhas-inscricoes',
    templateUrl: './minhas-inscricoes.component.html',
    styleUrls: ['./minhas-inscricoes.component.css']
})
export class MinhasInscricoesComponent implements OnInit {
    minhasInscricoes: any[] = [];
    dadosInscricoes: {
        [inscricaoId: number]:
        {
            tituloCompeticao: string,
            nomeCategoria: string,
            nomeCompetidor: string,
            emailCompetidor: string
        }
    } = {};
    userId: number = 0;

    constructor(private inscricaoService: InscricaoService,
        private router: Router,
        private userService: UserService,
        private competicaoService: CompeticaoService,
        private competidorService: CompetidorService,
        private categoriaService: CategoriaService) { }

    ngOnInit(): void {
        this.userService.getUsuarioLogado().subscribe(usuario => {
            this.userId = usuario.id;
            this.buscarMinhasInscricoes();
        });
    }

    buscarMinhasInscricoes(): void {
        this.inscricaoService.buscarInscricoesDoUsuario(this.userId).subscribe(inscricoes => {
            this.minhasInscricoes = inscricoes;
            this.minhasInscricoes.forEach(inscricao => {
                this.getDados(inscricao.id);
            })
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

    getDados(id: number) {
        this.dadosInscricoes[id] = {
            tituloCompeticao: '',
            nomeCategoria: '',
            nomeCompetidor: '',
            emailCompetidor: ''
        };

        this.inscricaoService.getInfoInscricao(id).subscribe(infoInscricao => {
            this.dadosInscricoes[id].tituloCompeticao = infoInscricao.tituloCompeticao;
            this.dadosInscricoes[id].nomeCategoria = infoInscricao.nomeCategoria;
            this.dadosInscricoes[id].nomeCompetidor = infoInscricao.nomeCompetidor;
            this.dadosInscricoes[id].emailCompetidor = infoInscricao.emailCompetidor;
        }
        );
    }

    pagarInscricao(id: number) {
        this.router.navigate(['/pagar-inscricao', id]);
    }
}