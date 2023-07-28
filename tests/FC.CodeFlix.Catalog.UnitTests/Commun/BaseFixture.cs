using Bogus;

namespace FC.CodeFlix.Catalog.UnitTests.Commun;
public abstract class BaseFixture
{
    public Faker Faker { get; set; }
    protected BaseFixture
        () => Faker = new Faker("pt_BR");
}
