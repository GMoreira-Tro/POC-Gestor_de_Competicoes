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
  competidor: Competidor = {} as Competidor;

  constructor(
    private competidorService: CompetidorService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

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
      },
      error => console.log('Erro ao carregar competidor:', error)
    );
  }

  onSubmit(): void {
    if (!this.competidor) return;
    
    this.competidor.tipo = Number(this.competidor.tipo);
    this.competidorService.atualizarCompetidor(this.competidor.id, this.competidor).subscribe(
      () => {
        alert('Competidor atualizado com sucesso!');
        this.router.navigate(['/meus-competidores']);
      },
      error => {
        console.log('Erro ao atualizar competidor:', error);
        alert('Erro ao atualizar o competidor. Tente novamente.');
      }
    );
  }

  cancel(): void {
    this.router.navigate(['/meus-competidores']);
  }
}