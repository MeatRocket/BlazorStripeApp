
let card;
let startcard: boolean = true;
let stripeVariable: stripe.Stripe;
const message: HTMLDivElement = document.querySelector('#result-message') as HTMLDivElement;
try {

    const key = (document.getElementById("publishKey") as HTMLInputElement).value;

    stripeVariable = Stripe(key, {
        apiVersion: '2020-08-27',
    });

    const elements: stripe.elements.Elements = stripeVariable.elements();
    const style = {

        base: {
            color: '#32325d',
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: 'antialiased',
            fontSize: '16px',
            '::placeholder': {
                color: '#aab7c4'
            }
        },
        invalid: {
            color: '#fa755a',
            iconColor: '#fa755a'
        }
    };
    if (startcard) {
        card = elements.create('card', {
            style: style
        });
        startcard = false;
    }
    card.mount('#card-element');


}
catch (ex) { console.log("Stripe error: " + ex.message); }


//export function Initiate() {
//	return;

//}

export async function createPaymentMethodServer(clieint:string) {
    message.textContent = "Loading...";
    stripeVariable.confirmCardPayment(clieint,card)
   // return await createPaymentMethod();
}


async function createPaymentMethod() {
    

    return (await stripeVariable.createToken(card)).token.id;
}

function createSubscription(dotnetHelper: any, clientSecret: string, resultMessage: string, approved: boolean) {

    const button: HTMLSpanElement = document.querySelector('#button') as HTMLSpanElement;
    const nameInput: HTMLInputElement = document.querySelector('#CustomerName') as HTMLInputElement;
    const amountInput: HTMLInputElement = document.querySelector('#Amount') as HTMLInputElement;
    message.textContent = "";
    if (approved) {
        button.classList.add('_submitted');
        message.textContent = "Payment Succeded";
        nameInput.value = "";
        amountInput.value = "";
        card.clear();
    }
    else {
        message.textContent = resultMessage;
        button.textContent = "Pay";
        dotnetHelper.invokeMethodAsync('Subscribe', clientSecret, resultMessage, approved);
        dotnetHelper.dispose();
    }



}
//function displayError(result) {

//    const errorMessage: HTMLDivElement = document.querySelector('#result-message') as HTMLDivElement;
//    errorMessage.classList.remove("hidden");
//    errorMessage.textContent = result;

//    setTimeout(function () {
//        errorMessage.textContent = "";
//    }, 4000);
//}

//function orderComplete() {
//    const resultMessage: HTMLDivElement = document.querySelector('#result-message') as HTMLDivElement;
//    resultMessage.classList.remove("hidden");
//    resultMessage.textContent = "Payment Succeded";
//    ResetCard();
//}
//function ResetCard() {

//    card.clear();
//}