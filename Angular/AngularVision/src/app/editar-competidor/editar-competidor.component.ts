import { Component, OnInit } from '@angular/core';
import { Competidor } from '../interfaces/Competidor';
import { CompetidorService } from '../services/competidor.service';
import { ActivatedRoute, Router } from '@angular/router';

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
          : `http://localhost:5000/${this.competidor.imagemUrl}`;
      },
      error => console.log('Erro ao carregar competidor:', error)
    );
  }

  onSubmit(): void {
    if (!this.competidor) return;

    this.competidor.tipo = Number(this.competidor.tipo);
    this.competidorService.atualizarCompetidor(this.competidor.id, this.competidor).subscribe(
      () => {
      },
      error => {
        console.log('Erro ao atualizar competidor:', error);
        alert('Erro ao atualizar o competidor. Tente novamente.');
      }
    );
    this.uploadImagem();
    alert('Competidor atualizado com sucesso!');
    this.router.navigate(['/meus-competidores']);
  }

  selecionarImagem(event: any): void {
    this.imagemSelecionada = event.target.files[0] as File;
  }

  uploadImagem(): void {
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

  cancel(): void {
    this.router.navigate(['/meus-competidores']);
  }
}