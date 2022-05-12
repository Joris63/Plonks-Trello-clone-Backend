using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Plonks.Auth.Helpers;
using Plonks.Auth.Models;
using Plonks.Auth.Services;
using Plonks.Auth.Test.MockDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Plonks.Auth.Test
{
    public class AuthServiceTests
    {
        private IAuthService accountService;
        IConfiguration _configuration;

        public AuthServiceTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"JWT:Issuer", "http://localhost:5010"},
                {"JWT:Audience", "http://localhost:3000/"},
                {"JWT:Secret", "5798587460471803C2E2AE6E90DC51F1609DB1EFB370D802CF73B6CE3CEE0FBB"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        public static DbContextOptions<AppDbContext> CreateOptions()
        {
            //This creates the SQLite connection string to in-memory database
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();

            //This creates a SqliteConnectionwith that string
            var connection = new SqliteConnection(connectionString);

            //The connection MUST be opened here
            connection.Open();

            //Now we have the EF Core commands to create SQLite options
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlite(connection);

            return builder.Options;
        }

        [Fact]
        public void RegisterGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                context.Database.EnsureCreated();

                RegisterRequest create = new RegisterRequest()
                {
                    Username = "Test",
                    Email = "Test@test.com",
                    Password = "Test123"
                };

                //ATTEMPT
                Task<AuthenticateResponse> service = accountService.Register(create);
                AuthenticateResponse response = service.Result;

                //VERIFY
                Assert.NotNull(response.AccessToken);
            }
        }

        [Fact]
        public void RegisterBadFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                context.Database.EnsureCreated();
                UsersDBMock.SeedDbOneUser(context);

                RegisterRequest create = new RegisterRequest()
                {
                    Username = "Test",
                    Email = "Test@test.com",
                    Password = "Test123"
                };

                //ATTEMPT
                Task<AuthenticateResponse> service = accountService.Register(create);
                AuthenticateResponse response = service.Result;

                //VERIFY
                Assert.Null(response.AccessToken);
                Assert.Equal("This email is already registered.", response.Message);
            }
        }

        [Fact]
        public void LoginGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                context.Database.EnsureCreated();
                UsersDBMock.SeedDbOneUser(context);

                LoginRequest user = new LoginRequest()
                {
                    Email = "Test@test.com",
                    Password = "Tester123"
                };

                //ATTEMPT
                Task<AuthenticateResponse> service = accountService.Login(user);
                AuthenticateResponse response = service.Result;

                //VERIFY
                Assert.NotNull(response.AccessToken);
            }
        }

        [Fact]
        public void LoginBadFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                context.Database.EnsureCreated();

                LoginRequest user = new LoginRequest()
                {
                    Email = "Test@test.com",
                    Password = "Tester123"
                };

                //ATTEMPT
                Task<AuthenticateResponse> service = accountService.Login(user);
                AuthenticateResponse response = service.Result;

                //VERIFY
                Assert.Null(response.AccessToken);
                Assert.Equal("Incorrect email or password.", response.Message);
            }
        }

        [Fact]
        public void RefreshTokenGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                string token = "645l5abfRPkTOm3bjj+0zqAMr8ReqQU86KR6uvOciiPU8BJ5s+cxOQ2zK+IqMeb/T5+KPe0Hc52KUatVc1VRzw==";

                context.Database.EnsureCreated();
                UsersDBMock.SeedDbOneUser(context);

                //ATTEMPT
                Task<RefreshTokenResponse> service = accountService.RefreshToken(token);
                RefreshTokenResponse response = service.Result;

                //VERIFY
                Assert.NotNull(response.AccessToken);
            }
        }

        [Fact]
        public void RefreshTokenBadFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                string token = "645l5abfRPkTOm3bjj+0zqAMr8ReqQU86KR6uvOciiPU8BJ5s+cxOQ2zK+IqMeb/T5+KPe0Hc52KUatVc1VRzw==";

                context.Database.EnsureCreated();

                //ATTEMPT
                Task<RefreshTokenResponse> service = accountService.RefreshToken(token);
                RefreshTokenResponse response = service.Result;

                //VERIFY
                Assert.Null(response.AccessToken);
                Assert.Equal("Invalid refresh token.", response.Message);
            }
        }

        [Fact]
        public void RevokeTokenGoodFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                context.Database.EnsureCreated();
                UsersDBMock.SeedDbOneUser(context);

                RevokeTokenRequest request = new RevokeTokenRequest()
                {
                    UserId = Guid.Parse("4861d24e-0af5-4e05-b9ca-0d77f8397ec0")
                };

                //ATTEMPT
                Task<bool> service = accountService.RevokeToken(request);
                bool response = service.Result;

                //VERIFY
                Assert.True(response);
            }
        }

        [Fact]
        public void RevokeTokenBadFlow()
        {
            //SETUP
            var options = CreateOptions();

            using (var context = new AppDbContext(options))
            {
                accountService = new AuthService(context, _configuration);

                context.Database.EnsureCreated();

                RevokeTokenRequest request = new RevokeTokenRequest()
                {
                    UserId = Guid.Parse("4861d24e-0af5-4e05-b9ca-0d77f8397ec0")
                };

                //ATTEMPT
                Task<bool> service = accountService.RevokeToken(request);
                bool response = service.Result;

                //VERIFY
                Assert.False(response);
            }
        }
    }
}
