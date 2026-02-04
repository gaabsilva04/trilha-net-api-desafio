using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TrilhaApiDesafio.Context
{
    public class OrganizadorContextFactory : IDesignTimeDbContextFactory<OrganizadorContext>
    {
        public OrganizadorContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OrganizadorContext>();

            // Tenta obter a connection string via variável de ambiente 'TRILHA_CONNECTION'.
            // Caso não exista, usa LocalDB para desenvolvimento. Para produção, defina a connection string
            // em `appsettings.json` (chave: ConnectionStrings:ConexaoPadrao) ou através de uma variável de
            // ambiente (ex.: TRILHA_CONNECTION). Exemplo:
            //   Server=YOUR_SERVER;Database=TrilhaApiDesafioDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
            var connection = Environment.GetEnvironmentVariable("TRILHA_CONNECTION")
                             ?? "Server=(localdb)\\mssqllocaldb;Database=TrilhaApiDesafioDb;Trusted_Connection=True;MultipleActiveResultSets=true";

            builder.UseSqlServer(connection);
            return new OrganizadorContext(builder.Options);



        }
    }
}

