using Microsoft.EntityFrameworkCore;
using WebApiLab2_TF.Models;

namespace WebApiLab2_TF.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options ) : base(options) 
        {
            
        }

        public DbSet<Book> Books {  get; set; }
    }
}
