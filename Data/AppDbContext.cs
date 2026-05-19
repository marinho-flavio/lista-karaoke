using ListaKaraoke.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaKaraoke.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Musica> Musicas { get; set; }
}
