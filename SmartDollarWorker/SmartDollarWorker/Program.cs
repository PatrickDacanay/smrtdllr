using Microsoft.Extensions.Configuration;
using SmartDollarWorker.Access.DbApps;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Smart Dollar Running...");
        var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();
        var console = new SmartDollarApp(config);

        console.GetTransactions();
    }
}