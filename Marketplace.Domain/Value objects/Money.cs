using System.Globalization;
using Marketplace.Framework;

namespace Marketplace.Domain.Value_objects;

public class Money : Value<Money>
{
    private const string DefaultCurrency = "EUR";

    public static Money FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup) =>
        new Money(amount: amount, currencyCode: currency, currencyLookup: currencyLookup);

    public static Money FromString(string amount, string currency, ICurrencyLookup currencyLookup)
    {
        return new Money(amount: decimal.Parse(amount, CultureInfo.InvariantCulture), currencyCode: currency, currencyLookup: currencyLookup);
    }

    protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
    {
        if (string.IsNullOrEmpty(currencyCode))
            throw new ArgumentNullException(paramName: nameof(currencyCode),
                message: "Currency code must be specified");

        CurrencyDetails currency = currencyLookup.FindCurrency(currencyCode: currencyCode);

        if (!currency.InUse)
            throw new ArgumentException(message: $"Currency {currencyCode} is not valid");
        
        if (decimal.Round(
                amount, currency.DecimalPlaces) != amount)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                message: $"Amount in {
                    currencyCode} cannot have more than {
                        currency.DecimalPlaces} decimals");
        
        Amount = amount;
        Currency = currency;
    }

    private Money(decimal amount, CurrencyDetails currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    public decimal Amount { get; }
    public CurrencyDetails Currency { get; }

    public Money Add(Money summand)
    {
        if (Currency != summand.Currency)
            throw new CurrencyMismatchException(
                message: "Cannot sum amounts with different currencies");
        return new Money(amount: Amount + summand.Amount, currency: Currency);
    }
    public Money Subtract(Money subtrahend)
    {
        if (Currency != subtrahend.Currency)
            throw new CurrencyMismatchException(
                message: "Cannot subtract amounts with different currencies");
        return new Money(amount: Amount - subtrahend.Amount, currency: Currency);
    }
    public static Money operator +(
        Money summand1, Money summand2) =>
        summand1.Add(summand2);
    public static Money operator -(
        Money minuend, Money subtrahend) =>
        minuend.Subtract(subtrahend);
    
    public override string ToString() => $"{
        Currency.CurrencyCode} {Amount}";
    
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) :
            base(message)
        {
        }
    }
}