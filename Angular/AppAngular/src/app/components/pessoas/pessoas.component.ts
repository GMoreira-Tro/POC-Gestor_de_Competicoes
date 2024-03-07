import { PessoasService } from './../../pessoas.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Pessoa } from '../../Pessoa';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-pessoas',
  templateUrl: './pessoas.component.html',
  styleUrls: ['./pessoas.component.css'],
})
export class PessoasComponent implements OnInit {
  formulario: any;
  tituloFormulario!: string;
  pessoas!: Pessoa[];
  nomePessoa!: string;
  id!: number;

  visibilidadeTabela: boolean = true;
  visibilidadeFormulario: boolean = false;
  
  modalRef!: BsModalRef;

  constructor(private pessoasService: PessoasService,
    private modalService : BsModalService) {}

  ngOnInit(): void {
    this.pessoasService.PegarTodos()
      .subscribe((resposta) => {
        this.pessoas = resposta;
      });
      console.log(this.pessoas);
  }

  ExibirFormularioCadastro(): void {
    this.MostrarFormulario();

    this.tituloFormulario = 'Nova Pessoa';
    this.formulario = new FormGroup({
      nome: new FormControl(null),
      sobrenome: new FormControl(null)
    });
  }

  ExibirFormularioAtualizacao(id: number): void {
    this.MostrarFormulario();
    alert(id)
    this.pessoasService.PegarPeloId(id).subscribe(resultado => {
      this.tituloFormulario = 'Atualizar ' + resultado.nome + " " + resultado.sobrenome;
      this.formulario = new FormGroup({
        id: new FormControl(resultado.id),
        nome: new FormControl(resultado.nome),
        sobrenome: new FormControl(resultado.sobrenome)
      })
    });
  }

  EnviarFormulario(): void{
    const pessoa: Pessoa = this.formulario.value;
    alert(pessoa.id);

    if(pessoa.id) {
      this.pessoasService.AtualizarPessoa(pessoa).subscribe(resultado => {
        alert(pessoa.nome + " " + pessoa.sobrenome + " atualizada");
        this.AtualizarLista();
        this.Voltar();
      });
    }
    else {
      this.pessoasService.SalvarPessoa(pessoa).subscribe(()=>{
        alert('Pessoa salva');
        this.AtualizarLista();
        this.Voltar();
      });
    }
  }

  AtualizarLista(): void {
    this.pessoasService.PegarTodos().subscribe ((resultado) => {
      this.pessoas = resultado;
    });
  }

  Voltar(): void {
    this.visibilidadeFormulario = false;
    this.visibilidadeTabela = true;
  }

  MostrarFormulario(): void {
    this.visibilidadeFormulario = true;
    this.visibilidadeTabela = false;
  }

  ExibirConfirmacaoExclusao(id: number, nome: string, conteudoModal: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(conteudoModal);
    this.id = id;
    this.nomePessoa = nome;

  }

  ExcluirPessoa(id: number): void {
    this.pessoasService.ExcluirPessoa(id).subscribe((resultado) => {
      this.modalService.hide();
      alert(" excluída");
      this.AtualizarLista();
    });
  }
}
