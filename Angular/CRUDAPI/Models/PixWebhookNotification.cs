public class PixWebhookNotification
{
    public List<PixPaymentInfo> pix { get; set; }
}

public class PixPaymentInfo
{
    public string txid { get; set; }
    public string status { get; set; }
    public int userId { get; set; }
}
