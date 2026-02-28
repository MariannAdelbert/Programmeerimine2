using System;
using System.Net.Http;
using KooliProjekt.Application.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;

        public WebApplicationFactory<FakeStartup> Factory { get; }
        public HttpClient Client { get; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();
            Client = Factory.CreateClient();
        }

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext != null)
                {
                    return _dbContext;
                }

                _dbContext = Factory.Services.GetService<ApplicationDbContext>();
                return _dbContext;
            }
        }

        public void Dispose()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();
        }

        // Add your other helper methods here
    }
}