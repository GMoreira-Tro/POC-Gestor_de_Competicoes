import { PagamentoContaCorrente } from "./PagamentoContaCorrente";

export enum TipoPagamento {
    CartaoDeCredito = 1,
    Boleto = 2,
    DebitoOnLineTEF = 3,
    SaldoPagSeguro = 4,
    OiPago = 5,
    DepositoEmConta = 6,
    Dinheiro = 7
}

export enum Status {
    Pendente,
    Paga,
    Aceita,
    Recusada
}

export interface Pagamento {
    id: number;
    valor: number;
    moeda: string;
    dataRequisicao: Date;
    dataRecebimento?: Date;
    aprovadorId: number;
    motivo: string;
    status: Status;
    tipoPagamento: TipoPagamento;
    tokenPagSeguro: string;
    pagamentoContasCorrente: PagamentoContaCorrente[];
}