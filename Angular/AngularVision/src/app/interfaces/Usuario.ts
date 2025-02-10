
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
  }