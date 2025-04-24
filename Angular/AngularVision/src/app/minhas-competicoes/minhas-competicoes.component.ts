import { Component, OnInit } from '@angular/core';
import { CompeticaoService } from '../services/competicao.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { CategoriaService } from '../services/categoria.service';
import { environment } from '../../environments/environment.prod';

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
        private userService: UserService,
        private categoriaService: CategoriaService
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

            this.minhasCompeticoes.forEach(competicao => {
                if (competicao.bannerImagem !== null) {
                    competicao.bannerImagem = competicao.bannerImagem?.startsWith('http')
                        ? competicao.bannerImagem
                        : `${environment.apiBaseUrl}/${competicao.bannerImagem}`;
                }
            }
            );
        }, error => console.log("Erro ao buscar competições: ", error));
    }

    editarCompeticao(id: number): void {
        this.router.navigate(['/editar-competicao', id]);
    }

    async excluirCategorias(id: number): Promise<void> {

        this.categoriaService.getCategoriasPorCompeticao(id).subscribe(async (categorias) => {
            console.log(categorias)
            let countCategorias = 0;
            categorias.forEach(async categoria => {
                this.categoriaService.deleteCategoria(categoria.id).subscribe(categoria => {
                    countCategorias++;
                    if (countCategorias === categorias.length) {
                        this.competicaoService.deleteCompeticao(id).subscribe(() => {
                            this.buscarMinhasCompeticoes(); // Atualiza a lista
                        }, error => alert(error.error.message ?? "Erro ao deletar competição."));
                    }
                }, error => console.log("Erro ao deletar categoria")
                );
            });
        });

    }

    async excluirCompeticao(id: number) {
        if (!confirm("Tem certeza que deseja excluir esta competição?")) return;
        await this.excluirCategorias(id);
    }

    gerenciarInscricoes(id: number)
    {
        this.router.navigate(['/aprovar-inscricao', id]);
    }
}
