using Autofac;
using CryptoSuite48.DemoConsole.Demos;
using CryptoSuite48.KeyManagement.Factories;
using CryptoSuite48.KeyManagement.Interfaces;
using CryptoSuite48.Services;
using CryptoSuite48.Services.Interfaces;

namespace CryptoSuite48.DemoConsole
{
    public class CryptoSuiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CryptoKeyService>()
                   .As<ICryptoKeyService>()
                   .SingleInstance();

            builder.RegisterType<CryptoService>()
                   .As<ICryptoService>()
                   .SingleInstance();

            builder.RegisterType<KeyGeneratorFactory>()
                   .As<IKeyGeneratorFactory>()
                   .SingleInstance();

            builder.RegisterType<KeyLoaderFactory>()
                   .As<IKeyLoaderFactory>()
                   .SingleInstance();

            // Demo 類別註冊
            builder.RegisterType<AesDemo>().AsSelf().SingleInstance();
            builder.RegisterType<RsaDemo>().AsSelf().SingleInstance();
            builder.RegisterType<EccDemo>().AsSelf().SingleInstance();
        }
    }
}