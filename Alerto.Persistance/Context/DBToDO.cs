using Alerto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Alerto.Persistance.Context;

public partial class DBToDO : DbContext
{
    public DBToDO()
    {
    }

    
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<Conta> Contas { get; set; }
    public DbSet<Lista> Listas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }

    public DBToDO(DbContextOptions<DBToDO> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notificacao>()
            .HasOne(n => n.Conta)
            .WithMany(c => c.Notificacoes)
            .HasForeignKey(n => n.ContaId);

        modelBuilder.Entity<Tarefa>()
            .HasOne(t => t.Categoria)
            .WithMany(c => c.Tarefas)
            .HasForeignKey(c => c.CategoriaId)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<Lista>()
            .HasMany(l => l.Tarefas)
            .WithOne(t => t.Lista)
            .HasForeignKey(t => t.ListaId)
            .OnDelete(DeleteBehavior.ClientCascade);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}