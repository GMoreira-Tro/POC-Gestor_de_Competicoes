import { Usuario } from "./Usuario";

/**
 * Interface representing a competitor, which can be an athlete or a club.
 * Registered by a user.
 */
export interface Competidor {
    /**
     * Unique identifier for the competitor.
     */
    id: number;

    /**
     * Name of the competitor.
     */
    nome: string;

    /**
     * Email address of the competitor.
     */
    email: string;

    /**
     * Indicates whether the competitor is an athlete or a club.
     */
    tipo: number;

    /**
     * ID of the user responsible for creating this competitor.
     */
    criadorId: number;

    /**
     * User responsible for creating this competitor.
     */
    criador?: Usuario;
}