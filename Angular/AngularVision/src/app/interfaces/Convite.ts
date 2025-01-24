import { ConviteCompetidor } from "./ConviteCompetidor";

export interface Convite {
    id: number;
    titulo: string;
    descricao?: string;
    dataEnvio: Date;
    dataResposta?: Date;
    convitesCompetidor: ConviteCompetidor[];
}