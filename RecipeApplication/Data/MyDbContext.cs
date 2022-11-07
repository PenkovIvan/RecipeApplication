using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RecipeApplication.Data
{
    public class MyDbContext :IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        { 
        }
        public DbSet<Recipe> Recipes { get; set; }
    }
}
