export interface Notificacao {
    id: number;
    titulo: string;
    descricao?: string;
    dataPublicacao: Date;
    dataExpiracao?: Date;
    anuncianteId: number;
    bannerImagem?: string;
    tipoAnuncio?: string;
}