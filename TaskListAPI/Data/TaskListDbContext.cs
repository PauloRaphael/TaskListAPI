using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaskListAPI.Model.Entities;

namespace TaskListAPI.Data
{
    public class TaskListDbContext : DbContext
    {
        public TaskListDbContext(DbContextOptions<TaskListDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<LoginSessao> LoginSessoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("lista_tarefas");

            // Configurações para PostgreSQL (snake_case)
            modelBuilder.UseIdentityByDefaultColumns();


            // Configuração para a enum StatusEnum como string no DB
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Status)
                .HasConversion<string>();

            // Configuração das chaves estrangeiras (exemplo)
            modelBuilder.Entity<Categoria>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .IsRequired();

            modelBuilder.Entity<Tarefa>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(t => t.UsuarioId)
                .IsRequired();

            modelBuilder.Entity<Tarefa>()
                .HasOne<Categoria>()
                .WithMany()
                .HasForeignKey(t => t.CategoriaId)
                .IsRequired(false); // CategoriaId é nullable

            modelBuilder.Entity<LoginSessao>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(ls => ls.UsuarioId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}