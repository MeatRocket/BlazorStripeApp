using Microsoft.Azure.Cosmos.Table;

namespace BlazorStripeApp.Data
{
    public class ClientPayment: TableEntity
    {
        public string PaymentId { get; set; }
        public string ClientName { get; set; }
        public string PaymentAmount { get; set; }

    }

    public class PaymentInfo
    {
        public string CustomerName { get; set; }
        public string Amount { get; set; }

        public bool CustomerAvialable { get; set; } = true;
        public bool AmountAvialable { get; set; } = true;
        public string ClientSecret { get; set; }
        public string PaymentId { get; set; }

    }

    public class PaymentIntentModel
    {
        public PaymentIntentClass paymentintent { get; set; }
        public ErrorCLass error { get; set; }

    }

    public class PaymentIntentClass
    {
        public string id { get; set; }
        public string amount { get; set; }
        public string client_secret { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public string status { get; set; }

    }

    public class ErrorCLass
    {
        public string type { get; set; }
        public string message { get; set; }

        public PaymentIntentClass payment_intent { get; set; }

    }
}
