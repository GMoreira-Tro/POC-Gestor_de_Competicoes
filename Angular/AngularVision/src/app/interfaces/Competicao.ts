import { Categoria } from "./Categoria";
import { Usuario } from "./Usuario";

export interface Competicao {
    id: number;
    titulo: string;
    descricao: string;
    dataInicio: Date;
    dataFim: Date;
    idCriadorUsuario: number;
    usuario: Usuario;
    categorias: Categoria[];
  }