export interface PagamentoContaCorrente {
    id: number;
    pagamentoId: number;
    contaCorrenteId: number;
    contaCorrenteSolicitante: boolean;
    observacao: string;
}