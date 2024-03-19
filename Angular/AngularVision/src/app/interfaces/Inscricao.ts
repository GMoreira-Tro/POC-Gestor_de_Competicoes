import { Categoria } from "./Categoria";
import { Usuario } from "./Usuario";

export interface Inscricao {
    id: number;
    idCategoria: number;
    categoria: Categoria;
    idUsuario: number;
    usuario: Usuario;
    status: string; // pendente, aceita, recusada
    nomeAtleta: string;
    equipe: string
  }