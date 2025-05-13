import { Component, OnInit } from '@angular/core';
import { Competidor } from '../interfaces/Competidor';
import { CompetidorService } from '../services/competidor.service';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../environments/environment.prod';

@Component({
  selector: 'app-editar-competidor',
  templateUrl: './editar-competidor.component.html',
  styleUrls: ['./editar-competidor.component.css']
})
export class EditarCompetidorComponent implements OnInit {
  competidor: any = {};
  imagemSelecionada: File | null = null;

  constructor(
    private competidorService: CompetidorService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.carregarCompetidor(Number(id));
    }
  }

  carregarCompetidor(id: number): void {
    this.competidorService.obterCompetidor(id).subscribe(
      competidor => {
        this.competidor = competidor;
        this.competidor.imagemUrl = this.competidor.imagemUrl?.startsWith('http')
          ? this.competidor.imagemUrl
          : `${environment.apiBaseUrl}/${this.competidor.imagemUrl}`;
      },
      error => console.log('Erro ao carregar competidor:', error)
    );
  }

  onSubmit(): void {
    if (!this.competidor) return;

    this.competidor.tipo = Number(this.competidor.tipo);
    this.competidorService.atualizarCompetidor(this.competidor.id, this.competidor).subscribe(
      async () => {
        await this.uploadImagem();
      },
      error => {
        console.log('Erro ao atualizar competidor:', error);
        alert('Erro ao atualizar o competidor. Tente novamente.');
      }
    );
  }

  selecionarImagem(event: any): void {
    this.imagemSelecionada = event.target.files[0] as File;
  }

  async uploadImagem(): Promise<void> {
    if (!this.imagemSelecionada) {
      alert('Competidor atualizado com sucesso!');
      this.router.navigate(['/meus-competidores']);
      return;
    };
    this.competidorService.uploadImagem(this.competidor.id, this.imagemSelecionada).subscribe(
      (response) => {
        console.log("Imagem enviada com sucesso!", response);
        this.competidor.imagemUrl = response.imagemUrl; // Atualiza a imagem na interface

        this.competidor.imagemUrl = this.competidor.imagemUrl?.startsWith('http')
          ? this.competidor.imagemUrl
          : `${environment.apiBaseUrl}/${this.competidor.imagemUrl}`;
          
          alert('Competidor atualizado com sucesso!');
          this.router.navigate(['/meus-competidores']);
      },
      (error) => {
        console.error("Erro ao fazer upload da imagem", error);
      }
    );
  }

  cancel(): void {
    this.router.navigate(['/meus-competidores']);
  }
}