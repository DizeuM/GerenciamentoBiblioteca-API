using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
    {
    }

    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Exemplar> Exemplares { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }
    public DbSet<Multa> Multas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exemplar>()
            .HasOne(e => e.Livro)
            .WithMany(l => l.Exemplares);

        modelBuilder.Entity<Emprestimo>()
            .HasOne(e => e.Usuario)
            .WithMany(u => u.Emprestimos);

        modelBuilder.Entity<Emprestimo>()
            .HasOne(e => e.Exemplar)
            .WithMany(e => e.Emprestimos);

        modelBuilder.Entity<Multa>()
            .HasOne(m => m.Emprestimo)
            .WithOne(e => e.Multa);

        modelBuilder.Entity<Multa>()
              .HasOne(m => m.Usuario)
              .WithMany(u => u.Multas);
    }
}
