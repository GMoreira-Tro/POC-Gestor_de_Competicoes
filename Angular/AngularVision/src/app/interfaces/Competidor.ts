export interface Competidor {
    id: number;
    nome: string;
    email: string;
    tipo: number;
    imagemUrl: File|null; 
    criadorId: number;
}