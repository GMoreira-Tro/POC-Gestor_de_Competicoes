import { Inscricao } from "./Inscricao";

export interface Usuario {
    id: number;
    nome: string;
    email: string;
    senhaHash: string;
    inscricoes: Inscricao[];
  }