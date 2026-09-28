using System.Globalization;

var royalties = new List<Royalty>
{
    new AdvanceRoyalty("Аванс за роман", 18_000),
    new PerCopyRoyalty("Тираж збірки оповідань", 32.50, 1_200)
};

Console.WriteLine("Нараховані роялті:");
foreach (Royalty royalty in royalties)
{
    // Викликається реалізація Total() фактичного типу об'єкта.
    Console.WriteLine($"{royalty.Title}: {royalty.Total().ToString("C", CultureInfo.GetCultureInfo("uk-UA"))}");
}

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