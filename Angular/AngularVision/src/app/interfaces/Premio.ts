export interface Premio {
    id: number;
    nome: string;
    descricao?: string;
    dataEntrega: Date;
    pagamentoId?: number;
}