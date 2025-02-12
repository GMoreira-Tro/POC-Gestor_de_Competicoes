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
  competidor: Competidor = {
    id: 0,
    nome: '',
    email: '',
    tipo: 0,
    criadorId: 0
  }
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

  onSubmit() {
    this.competidor.criadorId = this.userId;
    this.competidorService.cadastrarCompetidor(this.competidor).subscribe(competidor => {
      console.log(competidor);
      this.router.navigate(['/meus-competidores']);
    },
      error => {
        if (typeof error.error === 'string') {
          alert(error.error);
        } else {
          alert("Preencha todos os campos");
        }
      });
  }
}
