using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Infraestructure.Infraestructure;

public class NotesDbContext : DbContext
{
    public DbSet<NoteRecord> Notes { get; set; }

    public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) 
    { 

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}

