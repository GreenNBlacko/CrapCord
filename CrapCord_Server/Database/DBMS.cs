using CrapCord_Entities;
using Microsoft.EntityFrameworkCore;

namespace CrapCord_Server.Database;

/// <summary>
///     Database management system, using Entity Framework Core and Pomelo 
/// </summary>
public class DBMS : DbContext {
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected DBMS(DbContextOptions<DBMS> options) : base(options) { }

    public DBMS(string ip, string port, string database, string username, string password)
        : base(new DbContextOptionsBuilder<DBMS>()
            .UseMySql($"Server={ip};Port={port};Database={database};User={username};Password={password};",
                ServerVersion.AutoDetect($"Server={ip};Port={port};Database={database};User={username};Password={password};"))
            .EnableSensitiveDataLogging()
            .Options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Room>()
            .HasMany(r => r.Members)
            .WithMany();
        
        modelBuilder.Entity<Room>()
            .HasMany(r => r.Messages)
            .WithOne();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tokens)
            .WithOne();
        
        base.OnModelCreating(modelBuilder);
    }
}