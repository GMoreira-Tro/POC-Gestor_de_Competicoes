import { Categoria } from "./Categoria";

export interface Competicao {
    id: number;
    titulo: string;
    descricao: string;
    modalidade: string;
    pais: string;
    estado: string;
    cidade: string;
    bannerImagem: File|null;
    dataInicio: Date;
    dataFim: Date;
    criadorUsuarioId: number;
    status: number;
  }