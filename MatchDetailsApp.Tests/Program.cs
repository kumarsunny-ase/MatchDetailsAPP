using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MatchDetailsApp.Tests
{
	public class Program
	{
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting xUnit tests...");

            // Run the tests programmatically
            var testAssembly = typeof(XmlFileControllerTests).Assembly;
            
        }
    }
}

