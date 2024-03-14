import { Inscricao } from "./Inscricao";

export interface Usuario {
    id: number;
    pais: string;
    estado: string;
    cidade: string;
    dataNascimento: Date;
    cpfCnpj: string;
    nome: string;
    sobrenome: string;
    email: string;
    senhaHash: string;
    inscricoes: Inscricao[];
  }