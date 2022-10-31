namespace BlazorStripeApp.Data
{
    public class StripeSetting
    {
        internal string PublishableKey { get; set; }
        private string SecretKey { get; set; }

        public StripeSetting(string pulishableKey)
        {
            PublishableKey = pulishableKey;
        }

    }
}
