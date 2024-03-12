import { Categoria } from "./interfaces/Categoria";
import { Usuario } from "./interfaces/Usuario";

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