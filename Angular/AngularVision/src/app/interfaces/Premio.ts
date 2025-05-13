export interface Premio {
    id: number;
    nome: string;
    descricao?: string;
    dataEntrega: string;
    pagamentoId?: number;
}