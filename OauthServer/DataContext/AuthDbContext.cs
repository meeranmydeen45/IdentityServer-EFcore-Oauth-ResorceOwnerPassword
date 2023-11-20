using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OauthServer.Models;

namespace OauthServer
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        public DbSet<Client> Client { get; set; }
        public DbSet<ClientGrantTypes> ClientGrantTypes { get; set; }
        public DbSet<ClientSecrets> ClientSecrets { get; set; }
        public DbSet<ClientScopes> ClientScopes { get; set; }
        public DbSet<ApiResources> ApiResources { get; set; }
        public DbSet<ApiResourceScopes> ApiResourceScopes { get; set; }
        public DbSet<User> User { get; set; }
    }
}
