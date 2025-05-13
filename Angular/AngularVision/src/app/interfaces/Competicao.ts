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
    dataInicio: string;
    dataFim: string;
    criadorUsuarioId: number;
    status: number;
    chavePix: string;
  }