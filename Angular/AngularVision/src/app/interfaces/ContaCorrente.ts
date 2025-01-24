import { PagamentoContaCorrente } from "./PagamentoContaCorrente";

export interface ContaCorrente {
    id: number;
    saldo: number;
    usuarioId: number;
    pagamentoContasCorrente: PagamentoContaCorrente[];
}