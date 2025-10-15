namespace TestProject1;

using System.Data;
using Mythology.Core;

public class RulesTest
{
    [Theory]
    [InlineData("Zeus", "Ares", true)]
    [InlineData("Zeus", "Poseidon", true)]
    [InlineData("Ares", "Zeus", false)]
    [InlineData("Ares", "Poseidon", false)]
    public void Test1(string god1, string god2, bool expected)
    {
        var rules = new Rules();
        var result = rules.Beats(god1, god2);
        Assert.Equal(expected, result);
    }
}
