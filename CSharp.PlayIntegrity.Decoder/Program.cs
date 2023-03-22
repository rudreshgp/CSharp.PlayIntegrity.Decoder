using CSharp.PlayIntegrity.Decoder.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;

public class Program
{
    public static void Main(string[] args)
    {
        var app = Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((context, builder) =>
             {
                 builder.SetBasePath(Directory.GetCurrentDirectory());
                 builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
             })
             .ConfigureServices((context, services) =>
             {
                 services.AddLogging(configure =>
                 {
                     if (context.HostingEnvironment.IsDevelopment())
                     {
                         configure.AddConsole();
                     }
                     else
                     {
                         configure.AddEventSourceLogger();
                     }
                 }
                 );
                 
                 services.Configure<ApiKeys>(context.Configuration.GetSection("ApiKeys"));

                 services.AddSingleton<IPlayIntegrityTokenDecryptor, PlayIntegrityTokenDecryptor>()
                 .AddSingleton<ISecurityTokenValidator, JwtSecurityTokenHandler>()
                 
                 //TODO: use one of them based on your framework. dotnet core or dotnet framework
                 .AddSingleton<ISignatureKeyService, SignatureKeyServiceCore>()
                 //.AddSingleton<ISignatureKeyService, SignatureKeyServiceFramework>()
                 ;
             })
            .Build();
        
        //TODO: paste your generated attestation statement or play integrity token below
        var encryptedToken = "";

        var logger = app.Services.GetService<ILogger<Program>>();
        try
        {
            var tokenDecyptor = app.Services.GetService<IPlayIntegrityTokenDecryptor>();
            var playIntegrityData = tokenDecyptor.DecryptAndVerifyToken(encryptedToken);
            if (playIntegrityData != default)
            {
                Console.Write("Integrity token decrypted and verified successfully");
                Console.WriteLine(JsonConvert.SerializeObject(playIntegrityData));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "an error occured");
        }
        Console.Read();
    }
}