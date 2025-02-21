export interface Notificacao {
    id: number;
    titulo: string;
    descricao?: string;
    dataPublicacao: Date;
    dataExpiracao?: Date;
    anuncianteId: number;
    link?: string;
    tipoAnuncio?: string;
}