using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorStripeApp;
using BlazorStripeApp.Shared;
using Stripe.Infrastructure;
using Stripe.Issuing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using BlazorStripeApp.Data;

namespace BlazorStripeApp.Pages
{
    public partial class Component1
    {
        [Inject]
        IJSRuntime? _js { get; set; }

        private static readonly string PayText = "Pay";
        private static readonly string loadingText = "Loading...";
        private PaymentInfo paymentinfo = new();
        private string pulishableKey;
        MarkupString Desc => (MarkupString)Description;
        private string ButtonText;
        private string? Description { get; set; }

        private IJSObjectReference? module;
        private DotNetObjectReference<Index>? _objRef;
        public bool isNew = true;
        public void Dispose()
        {
            _objRef?.Dispose();
        }

        protected override void OnInitialized()
        {
            //StripeSetting setting = new();
            //setting.PublishableKey = "pk_test_51Jv11MEKoXqfsULcCklP3k92Fb74vI7bvkk5gIYxEJwDrrXqrT4wJdZIuEp2fa2J3Dengtlg6qrESV1UzZZi1j7p00STPGRGxc";
            //setting.SecretKey = "sk_test_51Jv11MEKoXqfsULcr96mvrkfXkahxUEvPfN4DcS8m4Q9ctQqjovxEBLHgZkH19EeZM4zDhb4EPN5i3EiSVUrGtYJ00gd5XLKFd";
            //pulishableKey = setting.PublishableKey;
            //StripeConfiguration.ApiKey = setting.SecretKey;
            //ButtonText = PayText;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;
            module = await _js.InvokeAsync<IJSObjectReference>("import", "./Pages/Component1.razor.js");
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is null)
                return;
            await module.DisposeAsync();
        }

        public async Task ProcessPaymentAsync()
        {
            //ButtonText = loadingText;
            //int number;
            //Description = "";
            //if (string.IsNullOrEmpty(paymentinfo.CustomerName))
            //{
            //    paymentinfo.CustomerAvialable = false;
            //    Description = "Please fill the customer name";
            //}
            //else
            //    paymentinfo.CustomerAvialable = true;
            //if (string.IsNullOrEmpty(paymentinfo.Amount))
            //{
            //    Description = Description + @"<br/>Please fill the amount";
            //    paymentinfo.AmountAvialable = false;
            //    return;
            //}
            //else
            //{
            //    if (!int.TryParse(paymentinfo.Amount, out number))
            //    {
            //        paymentinfo.AmountAvialable = false;
            //        Description = Description + @"<br/>Please enter a valid number";
            //        return;
            //    }

            //    paymentinfo.AmountAvialable = true;
            //}
            //string token =  await module.InvokeAsync<string>("createPaymentMethodServer");


            // var serviceCharge = new ChargeService();
            // try
            // {
            //     Charge charge = await serviceCharge.CreateAsync(new ChargeCreateOptions { Amount = Int32.Parse(paymentinfo.Amount) * 100, Currency = "usd", Description = $"A payment from {paymentinfo.CustomerName}", Source = token });


            //     if (charge.Paid)
            //     {

            //         Description = charge.Status + new BalanceTransactionService().Get(charge.BalanceTransactionId).Fee;
            //     }
            //     else
            //         Description = charge.Status;
            // }
            // catch (StripeException ex)
            // {
            //     Description = ex.Message;
            // }
            //-------------------------------------
            // string token = await module.InvokeAsync<string>("createPaymentMethodServer");
            Charge nc =await new ChargeService().GetAsync("ch_3KO5CQEKoXqfsULc0qw1k0y6");

          
         
          
            await module.InvokeVoidAsync("createPaymentMethodServer", new ChargeService().Get("ch_3KO5CQEKoXqfsULc0qw1k0y6").PaymentIntent.ClientSecret);

            Charge n = new ChargeService().Get("ch_3KO5CQEKoXqfsULc0qw1k0y6");
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
        }
    }
}