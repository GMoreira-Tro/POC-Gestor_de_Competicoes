import { Confronto } from "./Confronto";

export interface Chaveamento {
    id: number;
    nome: string;
    categoriaId: number;
    arvoreConfrontos: string; // JSON da árvore de confrontos
}