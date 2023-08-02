using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Casgem_IdentityRole.DAL
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=UNKNOWN\\SQLEXPRESS;initial Catalog=CasgemDbRole;integrated Security=true");
        }
        public DbSet<Product> Products { get; set; }
    }
}
