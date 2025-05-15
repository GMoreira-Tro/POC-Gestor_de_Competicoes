import { Competidor } from "./Competidor";

export interface Confronto {
  id: number;
  chaveamentoId: number;
  dataInicio?: string;
  dataTermino?: string;
  local: string;

  competidorA?: Competidor | null;
  competidorB?: Competidor | null;
  vencedor?: Competidor | null;

  // IDs dos confrontos que alimentam esse confronto
  pais?: number[];

  // Opcional: identificador para rastreamento local
  tempId?: string;
}
