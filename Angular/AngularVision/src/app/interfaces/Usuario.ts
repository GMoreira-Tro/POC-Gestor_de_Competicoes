
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
    dataNascimento: string;
    cpfCnpj: string;
    role: number
  }