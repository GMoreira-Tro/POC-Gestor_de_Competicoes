export interface PixCharge {
    calendario: Calendario;
    devedor: Devedor;
    valor: Valor;
    chave: string;
    solicitacaoPagador: string;
}

export interface Calendario {
    expiracao: number;
}

export interface Devedor {
    cpf: string;
    nome: string;
}

export interface Valor {
    original: string;
}