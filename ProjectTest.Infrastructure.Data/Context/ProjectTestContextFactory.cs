using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectTest.Infrastructure.Data.Context
{
    public class ProjectTestContextFactory : IDesignTimeDbContextFactory<ProjectTestContext>
    {
        public ProjectTestContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectTestContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.SetBasePath(@"C:\_Source\eclipseworks\ProjectTest") 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("ConnectionString");

            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("ProjectTest.Infrastructure.Data"));

            return new ProjectTestContext(optionsBuilder.Options, null);
        }
    }
}
