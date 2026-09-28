using System.Globalization;

var firstAdvance = new AdvanceRoyalty("Аванс за детектив", 12_000);
var secondAdvance = new AdvanceRoyalty("Аванс за фантастику", 10_500);

AdvanceRoyalty combinedAdvance = firstAdvance + secondAdvance;
Console.WriteLine($"{combinedAdvance.Title}: {FormatMoney(combinedAdvance.Total())}");
Console.WriteLine($"Чи рівні аванси: {firstAdvance == secondAdvance}");

Royalty[] royalties =
{
    combinedAdvance,
    new AdvanceRoyalty("Невеликий аванс", 8_000),
    new PerCopyRoyalty("Роялті за тираж", 38, 750)
};

foreach (Royalty royalty in royalties)
{
    Console.WriteLine($"{royalty.Title}: {Classify(royalty)}");
}

static string FormatMoney(double value) => value.ToString("C", CultureInfo.GetCultureInfo("uk-UA"));

static string Classify(Royalty royalty) => royalty switch
{
    AdvanceRoyalty advance when advance.Total() > 20_000 => "велике роялті",
    AdvanceRoyalty => "звичайне нарахування",
    PerCopyRoyalty => "роялті за тираж",
    _ => "невідомий вид роялті"
};

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

    public static AdvanceRoyalty operator +(AdvanceRoyalty first, AdvanceRoyalty second) =>
        new($"{first.Title} + {second.Title}", first.baseValue + second.baseValue);

    public static bool operator ==(AdvanceRoyalty? first, AdvanceRoyalty? second) =>
        ReferenceEquals(first, second) ||
        (first is not null && second is not null && first.baseValue == second.baseValue);

    public static bool operator !=(AdvanceRoyalty? first, AdvanceRoyalty? second) => !(first == second);

    public override bool Equals(object? obj) =>
        obj is AdvanceRoyalty other && baseValue == other.baseValue;

    public override int GetHashCode() => baseValue.GetHashCode();
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