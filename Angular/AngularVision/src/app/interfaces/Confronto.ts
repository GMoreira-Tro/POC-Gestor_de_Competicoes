import { ConfrontoInscricao } from "./ConfrontoInscricao";

export interface Confronto {
    id: number;
    chaveamentoId: number;
    dataInicio?: string;
    dataTermino?: string;
    local: string;
}