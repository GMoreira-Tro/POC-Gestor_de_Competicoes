import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-busca-competicoes',
  templateUrl: './busca-competicoes.component.html',
  styleUrls: ['./busca-competicoes.component.css']
})
export class BuscaCompeticoesComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  buscarCompeticao(): void {
    // Lógica para buscar competições
  }

}
