import { Competidor } from "./Competidor";
import { Inscricao } from "./Inscricao";
import { UsuarioNotificacao } from "./UsuarioNotificacao";

export interface Usuario {
    id: number;
    nome: string;
    sobrenome: string;
    email: string;
    emailConfirmado: boolean
    senhaHash: string;
    senhaValidada: boolean
    pais: string;
    estado: string;
    cidade: string;
    dataNascimento: Date;
    cpfCnpj: string;
    role: number
    inscricoes: Inscricao[];
    competidores: Competidor[];
    anunciosRecebidos: UsuarioNotificacao[];
  }