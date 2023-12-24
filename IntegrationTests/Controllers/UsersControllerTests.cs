using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
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
using IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace IntegrationTests.Controllers
{

    [TestFixture]
    public class UsersControllerTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Program> _factory;
        private JsonSerializerOptions _jsonSerializerOptions;
        
        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();
                
                Utilities.InitializeDbForTests(db);
            }
        }
    }
}
