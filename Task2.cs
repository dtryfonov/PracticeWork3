using System.Globalization;

var advance = new AdvanceRoyalty("Аванс за навчальний посібник", 15_000);

Console.WriteLine($"Прогноз на 3 місяці: {FormatMoney(advance.Projected(3))}");
Console.WriteLine($"Прогноз на 3 місяці зі зростанням 5%: {FormatMoney(advance.Projected(3, 5))}");

var royalties = new Royalty[]
{
    advance,
    new PerCopyRoyalty("Тираж роману", 45.50, 900)
};

Console.WriteLine($"Сума роялті: {FormatMoney(Royalty.Sum(royalties))}");

static string FormatMoney(double value) => value.ToString("C", CultureInfo.GetCultureInfo("uk-UA"));

abstract class Royalty
{
    protected double baseValue;

    protected Royalty(string title, double baseValue)
    {
        Title = title;
        this.baseValue = baseValue;
    }

    public string Title { get; }

    public abstract double Total();

    public double Projected(int months) => Total() * months;

    public double Projected(int months, double growthPercent)
    {
        double sum = 0;
        double currentPayment = Total();

        for (int month = 0; month < months; month++)
        {
            sum += currentPayment;
            currentPayment *= 1 + growthPercent / 100;
        }

        return sum;
    }

    public static double Sum(params Royalty[] items)
    {
        double total = 0;

        foreach (Royalty royalty in items)
        {
            total += royalty.Total();
        }

        return total;
    }
}

class AdvanceRoyalty : Royalty
{
    public AdvanceRoyalty(string title, double advance) : base(title, advance)
    {
    }

    public override double Total() => baseValue;
}

class PerCopyRoyalty : Royalty
{
    private readonly int copies;

    public PerCopyRoyalty(string title, double paymentPerCopy, int copies)
        : base(title, paymentPerCopy)
    {
        this.copies = copies;
    }

    public override double Total() => baseValue * copies;
}