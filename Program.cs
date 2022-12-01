using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EFCoreDemo;

internal class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(builder =>
            {
                builder.AddUserSecrets(typeof(Program).Assembly);
            })
            .ConfigureServices((b, services) =>
            {
                string cs = b.Configuration.GetConnectionString("SqlLocal") ?? throw new ArgumentNullException();
                services.AddDbContextFactory<DataContext>((services, builder) =>
                {
                    builder.UseSqlServer(cs);
                });
            })
            .Build();

        using DataContext context = host.Services.GetRequiredService<IDbContextFactory<DataContext>>().CreateDbContext();


        // throws System.MissingMethodException here
        int deleted = await context.Blogs.Where(b => b.Id > 0).BatchDeleteAsync();

        // standard EF version is fine
        // foreach (Blog blog in await context.Blogs.Where(b => b.Id > 0))
        //    context.Remove(blog);
        //
        // int deleted = await context.SaveChangesAsync();

        Console.WriteLine($"Deleted {deleted} blogs");

        await host.StopAsync();
    }
}
