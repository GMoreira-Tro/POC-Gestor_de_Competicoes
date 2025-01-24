export interface ConfrontoInscricao {
    id: number;
    confrontoId: number;
    inscricaoId: number;
    confrontoInscricaoPaiId?: number;
    confrontoInscricaoPai?: ConfrontoInscricao;
}