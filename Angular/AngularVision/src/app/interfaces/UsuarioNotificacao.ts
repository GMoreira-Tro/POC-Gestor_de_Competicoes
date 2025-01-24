export interface UsuarioNotificacao {
    id: number;
    usuarioId: number;
    notificacaoId: number;
    lida: boolean;
    dataCriacao: Date;
    dataLeitura?: Date;
}