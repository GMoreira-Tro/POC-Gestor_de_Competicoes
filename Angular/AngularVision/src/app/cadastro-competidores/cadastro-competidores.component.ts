import { Component } from '@angular/core';
import { Competidor } from '../interfaces/Competidor';
import { UserService } from '../services/user.service';
import { CompetidorService } from '../services/competidor.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cadastrar-competidores',
  templateUrl: './cadastro-competidores.component.html',
  styleUrls: ['./cadastro-competidores.component.css']
})
export class CadastroCompetidoresComponent {
  competidor: any = {}
  imagemSelecionada: File | null = null;
  userId: number = 0;

  constructor(private userService: UserService,
    private router: Router,
    private competidorService: CompetidorService) { }

  ngOnInit(): void {
    // Inicialize o usuário (exemplo)
    this.userService.getUsuarioLogado().subscribe((data) => {
      this.userId = data.id;
    },
      error => {
        console.error("Erro ao carregar o usuário", error);
      }
    );
  }

  selecionarImagem(event: any): void {
    this.imagemSelecionada = event.target.files[0] as File;
  }

  async uploadImagem(): Promise<void> {
    if (!this.imagemSelecionada) return;
    this.competidorService.uploadImagem(this.competidor.id, this.imagemSelecionada).subscribe(
      (response) => {
        console.log("Imagem enviada com sucesso!", response);
        this.competidor.imagemUrl = response.imagemUrl; // Atualiza a imagem na interface

        this.competidor.imagemUrl = this.competidor.imagemUrl?.startsWith('http')
          ? this.competidor.imagemUrl
          : `http://localhost:5000/${this.competidor.imagemUrl}`;
      },
      (error) => {
        console.error("Erro ao fazer upload da imagem", error);
      }
    );
  }

  onSubmit() {
    this.competidor.criadorId = this.userId;
    this.competidor.tipo = Number(this.competidor.tipo);
    console.log(this.competidor)
    this.competidorService.cadastrarCompetidor(this.competidor).subscribe(async competidor => {

      await this.uploadImagem();
      this.router.navigate(['/meus-competidores']);
      alert('Competidor cadastrado com sucesso!');
    },
      error => {
        if (typeof error.error === 'string') {
          alert(error.error);
        } else {
          console.log(error)
          alert("Preencha todos os campos");
        }
      });


  }
}
