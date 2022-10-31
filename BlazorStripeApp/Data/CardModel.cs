using System.ComponentModel.DataAnnotations;

namespace BlazorStripeApp.Data
{
    public class CardModel
    {
        [Required]
        public string CardHolderName { get; set; }

        [Required, CreditCard]
        public string CardNumber { get; set; }

        [Required, Range(1, 12, ErrorMessage = "ExpiryMonth must be between 1 and 12")]
        public long? CardExpiryMonth { get; set; }

        [Required, Range(22, 99, ErrorMessage = $"ExpiryYear must be between more than 2022 ")]
        public long? CardExpiryYear { get; set; }

        [Required,RegularExpression("^[0-9]*$", ErrorMessage = "CVC security code can only contain numbers")]
        public string CardCvc { get; set; }

        [Required, Range(0, 99999999, ErrorMessage = $"Enter valid amount")]
        public string Amount { get; set; }

      
    }
}

