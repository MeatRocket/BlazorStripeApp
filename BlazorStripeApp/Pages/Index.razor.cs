using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Stripe;
using BlazorStripeApp.Data;
using Newtonsoft.Json;
using AngryMonkey.Core;

namespace BlazorStripeApp.Pages
{
    public partial class Index
    {
        [Inject]
        IJSRuntime? _js { get; set; }
        private static readonly string PayText = "Pay";
        private static readonly string loadingText = "Loading...";
        private bool submitted;
        private PaymentInfo paymentinfo = new();
        private string pulishableKey;
        private string ButtonText;
        private IJSObjectReference? module;
        private DotNetObjectReference<Index>? _objRef;
        private bool isNew { get; set; }
        MarkupString Desc => (MarkupString)Description;
        private string? Description { get; set; }

        public void Dispose()
        {
            _objRef?.Dispose();
        }

        protected override async Task OnInitializedAsync()
        {
            pulishableKey = StripeClients.Settings.PublishableKey;
            ButtonText = PayText;

            ClientPayment payment = await StripeClients.Data.GetUserPayment();
            if (payment != null)
            {
                isNew = false;
                paymentinfo.PaymentId = payment.PaymentId;
            }
            else
                isNew = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            module = await _js.InvokeAsync<IJSObjectReference>("import", "./Pages/Index.razor.js");
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is null)
                return;
            await module.DisposeAsync();
        }

        public async Task ProcessPaymentAsync()
        {
           
            Description = "";

            PaymentIntentService service = new PaymentIntentService();

            if (isNew)
            {
                PaymentIntent paymentIntent = new();
                int number;

                if (string.IsNullOrEmpty(paymentinfo.CustomerName))
                {
                    paymentinfo.CustomerAvialable = false;
                    Description = "Please fill the customer name";

                }
                else
                    paymentinfo.CustomerAvialable = true;

                if (string.IsNullOrEmpty(paymentinfo.Amount))
                {
                    Description = Description + @"<br/>Please fill the amount";
                    paymentinfo.AmountAvialable = false;
                    return;

                }
                else
                {
                    if (!int.TryParse(paymentinfo.Amount, out number))
                    {
                        paymentinfo.AmountAvialable = false;
                        Description = Description + @"<br/>Please enter a valid number";
                        return;
                    }
                    paymentinfo.AmountAvialable = true;
                }
                ButtonText = loadingText;
                Description = loadingText;
                try
                {
                    Customer customer = new CustomerService().Create(new CustomerCreateOptions
                    {
                        Name = paymentinfo.CustomerName
                    });
                    paymentIntent = service.Create(new PaymentIntentCreateOptions
                    {
                        Amount = number * 100,
                        Currency = "usd",
                        Customer = customer.Id,
                        Description = $"A payment From {paymentinfo.CustomerName}"
                    });
                    paymentinfo.PaymentId = paymentIntent.Id;

                    await CheckPaymentMethod(paymentIntent, true, service);
                }
                catch (Exception ex)
                {
                    Description = "There was an error ... Please try again later";
                    return;
                }
            }
            else
            {
                try
                {
                    ButtonText = loadingText;
                    await CheckPaymentMethod(service.Get(paymentinfo.PaymentId), false, service);

                }
                catch (StripeException ex)
                {
                    Description = ex.Message + @"<br/> Please contact your admin";
                    ButtonText = PayText;
                }
            }
        }
        private async Task CheckPaymentMethod(PaymentIntent paymentIntent, bool newClient, PaymentIntentService service)
        {
            try
            {
                //_objRef = DotNetObjectReference.Create(this);

                object paymentIntentObject = await module.InvokeAsync<dynamic>("createPaymentMethodServer", paymentIntent.ClientSecret);

                PaymentIntentModel payment = JsonConvert.DeserializeObject<PaymentIntentModel>(paymentIntentObject.ToString());

                if (payment.error == null && payment.paymentintent.status == "succeeded")
                {
                    PaymentIntent intent = service.Get(paymentIntent.Id);
                    BalanceTransaction balance = new BalanceTransactionService().Get(intent.Charges.First().BalanceTransactionId);
                    Description = $"Payment succeeded <br/> Amount Paid:{balance.Amount / 100} <br/> Net Fee : {balance.Net / 100} <br/> Fee:{balance.Fee / 100}";
                    submitted = true;

                    if (!newClient)
                        await StripeClients.Data.DeleteClientPayment(paymentIntent.Id);
                }
                else
                {
                    ClientPayment client = new()
                    {
                        ClientName = paymentinfo.CustomerName,
                        PaymentAmount = paymentinfo.Amount,
                        PaymentId = paymentIntent.Id,
                    };
                    if (newClient)
                        await StripeClients.Data.AddClientPayment(client);


                    Description = payment.error.message;
                    paymentinfo.PaymentId = paymentIntent.Id;

                    ButtonText = PayText;
                    isNew = false;
                    // paymentinfo.ClientSecret = string.IsNullOrEmpty(paymentinfo.ClientSecret) ? paymentIntent.ClientSecret : paymentinfo.ClientSecret;
                    // Description = confirmPaymentIntent.LastPaymentError.Message;
                }

            }
            catch (Exception ex)
            {
                ClientPayment client = new()
                {
                    ClientName = paymentinfo.CustomerName,
                    PaymentAmount = paymentinfo.Amount,
                    PaymentId = paymentIntent.Id,
                };

                Description = "There was an error while loading... Please try again later";
                ButtonText = PayText;
                paymentinfo.ClientSecret = string.IsNullOrEmpty(paymentinfo.ClientSecret) ? paymentIntent.ClientSecret : paymentinfo.ClientSecret;
                isNew = false;

                if (newClient)
                    await StripeClients.Data.AddClientPayment(client);
            }

        }

        [JSInvokable("Subscribe")]
        public async Task Subscribe(string clientSecret, string message, bool paymentApproved)
        {
            //var service = new PaymentIntentService();
            //var payment = service.Get(paymentId);
            //Description = "";
            //Result = "";

            ButtonText = PayText;
            paymentinfo.ClientSecret = clientSecret;
            isNew = false;


            //var serviceCharge = new ChargeService();
            //try
            //{
            //    Charge charge = await serviceCharge.CreateAsync(new ChargeCreateOptions { Amount = Int32.Parse(payment.Amount) * 100, Currency = "usd", Description = $"A payment from {payment.CustomerName}", Source = tokenId });
            //    if (charge.Paid)
            //    {
            //        payment.Amount = "";
            //        payment.CustomerName = "";
            //        await module.InvokeVoidAsync("ResetCard");
            //        Description = "Success";
            //        submited = true;
            //    }
            //    else
            //        Description = "Failed";
            //}
            //catch (StripeException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //PaymentIntent m = service.Update("pi_3KO3v3EKoXqfsULc16BGM0cX", new PaymentIntentUpdateOptions
            //{
            //    Amount = number * 100,
            //    Currency = "usd",

            //    Description = $"A payment From fffff"
            //});

            // if (m.Status == "succeeded")
            //{

            //    BalanceTransaction o = new BalanceTransactionService().Get(m.Charges.First().BalanceTransactionId);

            //    Description = $"succeddedd + {o.Fee}";
            //}
            //else
            //{
            //    Description = m.LastPaymentError.Message;
            //    paymentinfo.ClientSecret = paymentIntent.ClientSecret;
            //    isNew = false;
            //}

        }
    }
}