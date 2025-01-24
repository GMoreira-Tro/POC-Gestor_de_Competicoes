import { ConfrontoInscricao } from "./ConfrontoInscricao";

export interface Confronto {
    id: number;
    dataInicio?: Date;
    dataTermino?: Date;
    local: string;
    confrontoInscricoes: ConfrontoInscricao[];
}