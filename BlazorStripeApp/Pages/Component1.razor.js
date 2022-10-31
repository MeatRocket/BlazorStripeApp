var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var card;
var startcard = true;
var stripeVariable;
var message = document.querySelector('#result-message');
try {
    var key = document.getElementById("publishKey").value;
    stripeVariable = Stripe(key, {
        apiVersion: '2020-08-27',
    });
    var elements = stripeVariable.elements();
    var style = {
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
catch (ex) {
    console.log("Stripe error: " + ex.message);
}
export function createPaymentMethodServer(clieint) {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            message.textContent = "Loading...";
            stripeVariable.confirmCardPayment(clieint, card);
            return [2];
        });
    });
}
function createPaymentMethod() {
    return __awaiter(this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4, stripeVariable.createToken(card)];
                case 1: return [2, (_a.sent()).token.id];
            }
        });
    });
}
function createSubscription(dotnetHelper, clientSecret, resultMessage, approved) {
    var button = document.querySelector('#button');
    var nameInput = document.querySelector('#CustomerName');
    var amountInput = document.querySelector('#Amount');
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
//# sourceMappingURL=Component1.razor.js.map