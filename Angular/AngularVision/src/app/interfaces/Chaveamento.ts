import { Confronto } from "./Confronto";

export interface Chaveamento {
    id: number;
    nome: string;
    descricao: string;
    categoriaId: number;
    confrontos: Confronto[];
    editandoNome: boolean;
}