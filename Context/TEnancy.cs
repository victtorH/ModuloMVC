using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloMVC.Models;
using System.Security.Claims;
using ModuloMVC.Interfaces;

namespace ModuloMVC.Context
{
    public class TEnancyDB : DbContext
    {
        private readonly string UserIdLogado;
        public TEnancyDB(DbContextOptions<TEnancyDB> options, IUserContext userContext)
        : base(options)
        {

            UserIdLogado = userContext.UserId;

        }

        public DbSet<Tarefa> Tarefa { get; set; }
        public DbSet<Contato> Contato { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 3. O Entity Framework é inteligente e vai usar a propriedade dinâmica!
            modelBuilder.Entity<Tarefa>().HasQueryFilter(t => t.UserId == UserIdLogado);
            modelBuilder.Entity<Contato>().HasQueryFilter(c => c.UserId == UserIdLogado);

            // Aquela nossa trava de segurança contra o erro de Cascata
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                // Verificamos se a entidade é uma tabela de "ligação" (Many-to-Many)
                // Geralmente o EF as nomeia combinando os nomes ou usando dicionários
                if (fk.DeclaringEntityType.IsPropertyBag || fk.DeclaringEntityType.Name.Contains("TarefaContato"))
                {
                    // Para tabelas de ligação, MANTEMOS o Cascade
                    fk.DeleteBehavior = DeleteBehavior.Cascade;
                }
                else
                {
                    // Para o restante das tabelas (como Usuario -> Tarefa), mantemos o Restrict
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }

modelBuilder.Entity<Usuario>(entity =>
    {
        entity.ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

        entity.HasKey(u => u.UserId);

        entity.Property(u => u.UserId)
              .HasColumnName("Id"); 

        entity.Property(u => u.NomeUsuario).HasColumnName("NomeUsuario");
        entity.Property(u => u.Email).HasColumnName("Email");
        entity.Property(u => u.Bio).HasColumnName("Bio");
        entity.Property(u => u.DataCriacao).HasColumnName("DataCriacao");
        entity.Property(u => u.DataAtualizacao).HasColumnName("DataAtualizacao");
        entity.Property(u => u.Avatar).HasColumnName("Avatar");
        entity.Property(u => u.ConexaoGoogleAtiva).HasColumnName("ConexaoGoogleAtiva");

        entity.HasQueryFilter(u => u.UserId == UserIdLogado);
    });

    modelBuilder.Entity<Contato>(entity =>
    {
        entity.HasKey(c => c.Id);

        entity.HasOne<Usuario>() 
              .WithMany()
              .HasForeignKey(c => c.UserId)
              .HasPrincipalKey(u => u.UserId) 
              .OnDelete(DeleteBehavior.Restrict); 
              
    });

    modelBuilder.Entity<Tarefa>(entity =>
    {
        entity.HasKey(t => t.Id);
        entity.HasOne<Usuario>()
              .WithMany()
              .HasForeignKey(t => t.UserId)
              .HasPrincipalKey(u => u.UserId)
              .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Ignore<Microsoft.AspNetCore.Identity.IdentityUser>();


            var tarefaBuilder = modelBuilder.Entity<Tarefa>();

            tarefaBuilder.HasKey(t => t.Id);

            tarefaBuilder.Property(t => t.Titulo).IsRequired(false).HasMaxLength(200);
            tarefaBuilder.Property(t => t.Descricao).IsRequired(false).HasMaxLength(2000);

            // A trava de segurança no banco que conversamos
            tarefaBuilder.HasCheckConstraint("CK_Tarefa_TituloOuDescricao_Requerido",
                "([Titulo] IS NOT NULL AND [Titulo] <> '') OR ([Descricao] IS NOT NULL AND [Descricao] <> '')");

            // Ensinando o EF Core a ler a sua lista privada (Backing Field)
            tarefaBuilder.Navigation(t => t.ContatosEnvolvidos)
                         .HasField("_contatosEnvolvidos")
                         .UsePropertyAccessMode(PropertyAccessMode.Field);

            tarefaBuilder.Property(t => t.Status)
                        .HasConversion<string>();

            tarefaBuilder.HasMany(t => t.ContatosEnvolvidos)
                        .WithMany(c => c.TarefasEnvolvidas);


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entradas = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

            foreach (var entrada in entradas)
            {
                var propUserId = entrada.Entity.GetType().GetProperty("UserId");
                if (propUserId != null)
                {
                    // Pegamos o valor que JÁ ESTÁ na entidade (se o service preencheu)
                    var idJaPreenchido = propUserId.GetValue(entrada.Entity) as string;
                    var idLogadoContexto = UserIdLogado; // O que vem do Cookie/Contexto

                    // Se não tem no contexto E não foi preenchido manualmente, aí sim damos o alerta
                    if (string.IsNullOrEmpty(idLogadoContexto) && string.IsNullOrEmpty(idJaPreenchido))
                    {
                        throw new Exception("ALERTA: Tentativa de salvar dados sem um usuário logado validado pelo sistema.");
                    }

                    // Se o Service não preencheu nada, mas temos um usuário logado, nós "carimbamos"
                    if (string.IsNullOrEmpty(idJaPreenchido))
                    {
                        propUserId.SetValue(entrada.Entity, idLogadoContexto);
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}