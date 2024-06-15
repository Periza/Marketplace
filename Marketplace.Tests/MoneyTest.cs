using Marketplace.Domain;
using Marketplace.Domain.Value_objects;

namespace Marketplace.Tests;

public class MoneyTest
{
    private static readonly ICurrencyLookup CurrencyLookup =
        new FakeCurrencyLookup();
    
    [Fact]
    public void Two_of_same_amount_should_be_equal()
    {
        Money firstAmount = Money.FromDecimal(amount: 5, currency: "EUR",
            CurrencyLookup);
        Money secondAmount = Money.FromDecimal(amount: 5, currency: "EUR",
            CurrencyLookup);
        Assert.Equal(expected: firstAmount, actual: secondAmount);
    }

    [Fact]
    public void Two_of_same_amount_but_different_Currencies_should_not_be_equal()
    {
        Money firstAmount = Money.FromDecimal(amount: 5, currency: "EUR",
            CurrencyLookup);
        Money secondAmount = Money.FromDecimal(amount: 5, currency: "USD",
            CurrencyLookup);
        Assert.NotEqual(expected: firstAmount, actual: secondAmount);
    }
    
    [Fact]
    public void FromString_and_FromDecimal_should_be_equal()
    {
        Money firstAmount = Money.FromDecimal(amount: 5, currency: "EUR",
            CurrencyLookup);
        Money secondAmount = Money.FromString(amount: "5.00", currency: "EUR",
            CurrencyLookup);
        Assert.Equal(expected: firstAmount, actual: secondAmount);
    }

    [Fact]
    public void Sum_of_money_gives_full_amount()
    {
        Money coin1 = Money.FromDecimal(amount: 1, currency: "EUR", CurrencyLookup);
        Money coin2 = Money.FromDecimal(amount: 2, currency: "EUR", CurrencyLookup);
        Money coin3 = Money.FromDecimal(amount: 2, currency: "EUR", CurrencyLookup);
        Money banknote = Money.FromDecimal(amount: 5, currency: "EUR", CurrencyLookup);
        Assert.Equal(expected: banknote, actual: coin1 + coin2 + coin3);
    }
    
    [Fact]
    public void Unused_currency_should_not_be_allowed()
    {
        Assert.Throws<ArgumentException>(testCode: () =>
            Money.FromDecimal(amount: 100, currency: "DEM", CurrencyLookup)
        );
    }
    
    [Fact]
    public void Unknown_currency_should_not_be_allowed()
    {
        Assert.Throws<ArgumentException>(testCode: () =>
            Money.FromDecimal(amount: 100, currency: "WHAT?", CurrencyLookup)
        );
    }
    [Fact]
    public void Throw_when_too_many_decimal_places()
    {
        Assert.Throws<ArgumentOutOfRangeException>(testCode: () =>
            Money.FromDecimal(amount: 100.123m, currency: "EUR", CurrencyLookup)
        );
    }
    [Fact]
    public void Throws_on_adding_different_currencies()
    {
        Money firstAmount = Money.FromDecimal(amount: 5, currency: "USD",
            CurrencyLookup);
        Money secondAmount = Money.FromDecimal(amount: 5, currency: "EUR",
            CurrencyLookup);
        Assert.Throws<Money.CurrencyMismatchException>(testCode: () =>
            firstAmount + secondAmount
        );
    }

    [Fact]
    public void Throws_on_substracting_different_currencies()
    {
        Money firstAmount = Money.FromDecimal(amount: 5, currency: "USD",
            CurrencyLookup);
        Money secondAmount = Money.FromDecimal(amount: 5, currency: "EUR",
            CurrencyLookup);
        Assert.Throws<Money.CurrencyMismatchException>(testCode: () =>
            firstAmount - secondAmount);
    }
}