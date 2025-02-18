export interface Inscricao {
    id: number;
    categoriaId: number;
    pagamentoId: number|null;
    competidorId: number;
    posicao: number;
    wo: boolean;
    premioResgatavelId: number|null;
    status: number; // pendente, aceita, recusada
  }