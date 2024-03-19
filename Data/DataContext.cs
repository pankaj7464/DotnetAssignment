using DotnetAssignment1.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotnetAssignment1.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext>  option):base(option)
        {
            
        }
           public DbSet<IotData> IotDatas { get; set; }

       
    }
}
