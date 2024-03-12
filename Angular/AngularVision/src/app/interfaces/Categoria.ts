import { Competicao } from "./Competicao";
import { Inscricao } from "./Inscricao";

export interface Categoria {
    id: number;
    nome: string;
    descricao: string;
    competicaoId: number;
    competicao: Competicao;
    inscricoes: Inscricao[];
  }