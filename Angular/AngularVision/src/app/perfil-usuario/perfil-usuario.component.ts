import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-perfil-usuario',
  templateUrl: './perfil-usuario.component.html',
  styleUrls: ['./perfil-usuario.component.css'],
})
export class PerfilUsuarioComponent implements OnInit {
  usuario: any = {};
  novoNome: string = '';
  imagemSelecionada: File | null = null;

  constructor(private userService: UserService, private http: HttpClient) {}

  ngOnInit(): void {
    this.carregarUsuario();
  }

  carregarUsuario(): void {
    this.userService.getUsuarioLogado().subscribe((data) => {
      this.usuario = data;
      this.novoNome = this.usuario.nome;
    });
  }

  atualizarNome(): void {
    this.userService.atualizarNome(this.usuario.id, this.novoNome).subscribe(() => {
      this.usuario.nome = this.novoNome;
    });
  }

  selecionarImagem(event: any): void {
    this.imagemSelecionada = event.target.files[0];
  }

  uploadImagem(): void {
    if (!this.imagemSelecionada) return;
    const formData = new FormData();
    formData.append('imagem', this.imagemSelecionada);
    this.http.post(`${environment.apiUrl}/usuario/${this.usuario.id}/upload-imagem`, formData)
      .subscribe(() => {
        this.carregarUsuario();
      });
  }
}
