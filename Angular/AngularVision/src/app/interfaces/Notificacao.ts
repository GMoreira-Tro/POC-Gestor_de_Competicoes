import { UsuarioNotificacao } from "./UsuarioNotificacao";

export interface Notificacao {
    id: number;
    pagamentoId?: number;
    titulo: string;
    descricao?: string;
    dataPublicacao: Date;
    dataExpiracao?: Date;
    anuncianteId: number;
    bannerImagem?: string;
    tipoAnuncio?: string;
    usuariosAlvo: UsuarioNotificacao[];
}