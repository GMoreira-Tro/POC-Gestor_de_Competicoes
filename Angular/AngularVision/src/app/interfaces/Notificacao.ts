export interface Notificacao {
    id: number;
    titulo: string;
    descricao?: string;
    dataPublicacao: string;
    dataExpiracao?: string;
    anuncianteId: number;
    link?: string;
    tipoAnuncio?: string;
}