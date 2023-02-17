namespace WeatherForecastExample.ApplicationCore.UnitTests.Services;

public class Bar
{
    
}

public class Foo
{
    public int Add(int a, int b, Bar bar)
    {
        Console.WriteLine(bar.ToString());
        return a + b;
    }
}

public class TestExample
{
    [Theory]
    [MemberData(nameof(BarScenarios))]
    public void Test(int a, int b, Bar bar, int expectedResult)
    {
        // arrange
        var foo = new Foo();
        
        // act
        var result = foo.Add(a, b, bar);

        // assert
        Assert.Equal(expectedResult, result);
    }


    public static IEnumerable<object[]> BarScenarios => new[]
    {
        new object[]
        {
            1,
            2,
            new Bar(),
            3
        },
        new object[]
        {
            4,
            5,
            new Bar(),
            9
        }
    };
}