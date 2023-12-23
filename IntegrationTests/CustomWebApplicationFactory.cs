namespace IntegrationTests
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using System.Net.Http;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using System.Security.Claims;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Hosting;
    using Service.Services;
using Services.Abstractions.Interfaces;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using Services.Services;
using Services.Abstractions.DTO;
using Domain.Entities;
using Infrastructure;
using Domain.Interfaces;
using Domain.Specifications;
    using Microsoft.Data.Sqlite;
    using System.Data.Common;

    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {

                var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==   
                    typeof(DbContextOptions<ApplicationContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services.AddSingleton<DbConnection>(container =>
                {   
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<ApplicationContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                    options.DefaultScheme = "Test"; }).AddScheme<AuthenticationSchemeOptions, AuthHandler>("Test", options => { });
            });
        }
    }

}