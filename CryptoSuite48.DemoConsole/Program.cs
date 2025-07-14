using Autofac;
using Autofac.Extensions.DependencyInjection;
using CryptoSuite48.DemoConsole.Demos;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CryptoSuite48.DemoConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var builder = new ContainerBuilder();

            builder.Populate(serviceCollection);
            builder.RegisterModule<CryptoSuiteModule>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                Console.WriteLine("=== CryptoSuite Demo Console ===\n");

                var aesDemo = scope.Resolve<AesDemo>();
                aesDemo.Run();

                Console.WriteLine();

                var rsaDemo = scope.Resolve<RsaDemo>();
                rsaDemo.Run();

                Console.WriteLine();

                var eccDemo = scope.Resolve<EccDemo>();
                eccDemo.Run();

                Console.WriteLine("\n=== Demo 結束 ===");
            }
        }
    }
}