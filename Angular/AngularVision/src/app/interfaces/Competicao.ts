import { Categoria } from "./Categoria";
import { Usuario } from "./Usuario";

export interface Competicao {
    id: number;
    titulo: string;
    descricao: string;
    bannerImagem: File|undefined;
    dataInicio: Date;
    dataFim: Date;
    idCriadorUsuario: number;
    usuario: Usuario|undefined;
    categorias: Categoria[];
  }