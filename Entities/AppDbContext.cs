using Microsoft.EntityFrameworkCore;

namespace ClienteCadastroApplication.Entities
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        //protected override void OnModelCreating(ModelBuilder mb)
        //{
        //    base.OnModelCreating(mb);
        //}
        //------------------------------------------------------------------------------------------------

        public DbSet<Clientes> Clientes { get; set; }

    }
}
