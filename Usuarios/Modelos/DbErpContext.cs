using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Usuarios.Modelos;

public partial class DbErpContext : DbContext
{
    public DbErpContext()
    {
    }

    public DbErpContext(DbContextOptions<DbErpContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=dbERP;Username=postgres;Password=060408");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Rol, "roles_rol_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.CorreoInstitucional, "usuarios_correo_institucional_key").IsUnique();

            entity.HasIndex(e => e.IdMatricula, "usuarios_id_matricula_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApellidoUsuario)
                .HasMaxLength(50)
                .HasColumnName("apellido_usuario");
            entity.Property(e => e.ContrasenaHash).HasColumnName("contrasena_hash");
            entity.Property(e => e.CorreoInstitucional)
                .HasMaxLength(100)
                .HasColumnName("correo_institucional");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaUltimaModificacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("fecha_ultima_modificacion");
            entity.Property(e => e.IdMatricula).HasColumnName("id_matricula");
            entity.Property(e => e.IdRol).HasColumnName("id_rol")
                .HasDefaultValueSql("4");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.ResetToken).HasColumnName("reset_token");
            entity.Property(e => e.ResetTokenExpira)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("reset_token_expira");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarios_id_rol_fkey");

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
