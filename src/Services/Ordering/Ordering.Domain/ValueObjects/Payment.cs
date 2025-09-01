namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        public string? CardName { get; init; } = default!;
        public string? CardNumber { get; init; } = default!;
        public string? Expiration { get; init; } = default!;
        public string? CVV { get; init; } = default!;
        public int PaymentMethod { get; init; } = default!;

        protected Payment() { }

        private Payment(string? cardName, string? cardNumber, string? expiration, string? cvv, int paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }

        public static Payment Of(string? cardName, string? cardNumber, string? expiration, string? cvv, int paymentMethod)
        {
            // You can add validation logic here if needed

            ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

            return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
        }

    }
}
