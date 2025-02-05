    public class PixRefundResponse
    {
        /// <summary>
        /// Status da devolução (ex.: "success", "failed").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mensagem informativa sobre a operação.
        /// </summary>
        public string Message { get; set; }

        // Adicione outros campos retornados pela API, se necessário.
    }