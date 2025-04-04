import { ConfrontoInscricao } from "./ConfrontoInscricao";

export interface Confronto {
    id: number;
    chaveamentoId: number;
    dataInicio?: Date;
    dataTermino?: Date;
    local: string;
}