using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Modelos;

public partial class CasaosContext : DbContext
{
    public CasaosContext()
    {
    }

    public CasaosContext(DbContextOptions<CasaosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<EstadosTimeline> EstadosTimelines { get; set; }

    public virtual DbSet<OrdenItem> OrdenItems { get; set; }

    public virtual DbSet<OrdenTimeline> OrdenTimelines { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=100.89.68.57:5432;Database=casaos;Username=CIDIL-SERVER;Password=CIdil-Admin12");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comentarios_pkey");

            entity.ToTable("comentarios");

            entity.HasIndex(e => e.ItemId, "idx_comentarios_item");

            entity.HasIndex(e => e.OrdenId, "idx_comentarios_orden");

            entity.HasIndex(e => e.UsuarioId, "idx_comentarios_usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comentario1).HasColumnName("comentario");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creado_en");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.OrdenId).HasColumnName("orden_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Item).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comentarios_item_id_fkey");

            entity.HasOne(d => d.Orden).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comentarios_orden_id_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("comentarios_usuario_id_fkey");
        });

        modelBuilder.Entity<EstadosTimeline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estados_timeline_pkey");

            entity.ToTable("estados_timeline");

            entity.HasIndex(e => e.Codigo, "estados_timeline_codigo_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Codigo)
                .HasMaxLength(100)
                .HasColumnName("codigo");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .HasColumnName("color");
            entity.Property(e => e.Icono)
                .HasMaxLength(100)
                .HasColumnName("icono");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<OrdenItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orden_items_pkey");

            entity.ToTable("orden_items");

            entity.HasIndex(e => e.EstadoTimelineId, "idx_items_estado");

            entity.HasIndex(e => e.OrdenId, "idx_items_orden");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualizadoEn)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualizado_en");
            entity.Property(e => e.Atencion)
                .HasMaxLength(150)
                .HasColumnName("atencion");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(0)
                .HasColumnName("cantidad");
            entity.Property(e => e.CantidadRecibida)
                .HasDefaultValue(0)
                .HasColumnName("cantidad_recibida");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.DireccionEnvio).HasColumnName("direccion_envio");
            entity.Property(e => e.EnvioVia)
                .HasMaxLength(100)
                .HasColumnName("envio_via");
            entity.Property(e => e.EstadoTimelineId).HasColumnName("estado_timeline_id");
            entity.Property(e => e.FechaEstimadaEntrega).HasColumnName("fecha_estimada_entrega");
            entity.Property(e => e.FechaRecibido).HasColumnName("fecha_recibido");
            entity.Property(e => e.FechaSolicitud).HasColumnName("fecha_solicitud");
            entity.Property(e => e.FechaSubida).HasColumnName("fecha_subida");
            entity.Property(e => e.ImporteLinea)
                .HasPrecision(12, 2)
                .HasColumnName("importe_linea");
            entity.Property(e => e.LinkExterno).HasColumnName("link_externo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroLista)
                .HasMaxLength(10)
                .HasColumnName("numero_lista");
            entity.Property(e => e.OrdenId).HasColumnName("orden_id");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.TerminosEnvio)
                .HasMaxLength(50)
                .HasColumnName("terminos_envio");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(10)
                .HasColumnName("unidad_medida");

            entity.HasOne(d => d.EstadoTimeline).WithMany(p => p.OrdenItems)
                .HasForeignKey(d => d.EstadoTimelineId)
                .HasConstraintName("orden_items_estado_timeline_id_fkey");

            entity.HasOne(d => d.Orden).WithMany(p => p.OrdenItems)
                .HasForeignKey(d => d.OrdenId)
                .HasConstraintName("orden_items_orden_id_fkey");
        });

        modelBuilder.Entity<OrdenTimeline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orden_timeline_pkey");

            entity.ToTable("orden_timeline");

            entity.HasIndex(e => e.EstadoTimelineId, "idx_timeline_estado");

            entity.HasIndex(e => e.OrdenId, "idx_timeline_orden");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.EstadoTimelineId).HasColumnName("estado_timeline_id");
            entity.Property(e => e.Evento)
                .HasMaxLength(250)
                .HasColumnName("evento");
            entity.Property(e => e.FechaEvento)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_evento");
            entity.Property(e => e.OrdenId).HasColumnName("orden_id");

            entity.HasOne(d => d.CreadoPorNavigation).WithMany(p => p.OrdenTimelines)
                .HasForeignKey(d => d.CreadoPor)
                .HasConstraintName("orden_timeline_creado_por_fkey");

            entity.HasOne(d => d.EstadoTimeline).WithMany(p => p.OrdenTimelines)
                .HasForeignKey(d => d.EstadoTimelineId)
                .HasConstraintName("orden_timeline_estado_timeline_id_fkey");

            entity.HasOne(d => d.Orden).WithMany(p => p.OrdenTimelines)
                .HasForeignKey(d => d.OrdenId)
                .HasConstraintName("orden_timeline_orden_id_fkey");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ordenes_pkey");

            entity.ToTable("ordenes");

            entity.HasIndex(e => e.CreadoPor, "idx_ordenes_creado_por");

            entity.HasIndex(e => e.EstadoTimelineId, "idx_ordenes_estado");

            entity.HasIndex(e => e.Codigo, "ordenes_codigo_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualizadoEn)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualizado_en");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .HasColumnName("codigo");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Departamento)
                .HasMaxLength(150)
                .HasColumnName("departamento");
            entity.Property(e => e.EstadoTimelineId).HasColumnName("estado_timeline_id");
            entity.Property(e => e.FechaSolicitud).HasColumnName("fecha_solicitud");
            entity.Property(e => e.FechaSubida).HasColumnName("fecha_subida");
            entity.Property(e => e.ImporteTotal)
                .HasPrecision(12, 2)
                .HasColumnName("importe_total");
            entity.Property(e => e.Moneda)
                .HasMaxLength(10)
                .HasColumnName("moneda");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .HasColumnName("nombre");
            entity.Property(e => e.SolicitadoPor)
                .HasMaxLength(150)
                .HasColumnName("solicitado_por");
            entity.Property(e => e.UnidadNegocio)
                .HasMaxLength(50)
                .HasColumnName("unidad_negocio");

            entity.HasOne(d => d.CreadoPorNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.CreadoPor)
                .HasConstraintName("ordenes_creado_por_fkey");

            entity.HasOne(d => d.EstadoTimeline).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.EstadoTimelineId)
                .HasConstraintName("ordenes_estado_timeline_id_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.CorreoInstitucional, "usuarios_correo_institucional_key").IsUnique();

            entity.HasIndex(e => e.IdMatricula, "usuarios_id_matricula_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Activado)
                .HasDefaultValue(true)
                .HasColumnName("activado");
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
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaUltimaModificacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_ultima_modificacion");
            entity.Property(e => e.IdMatricula).HasColumnName("id_matricula");
            entity.Property(e => e.IdRol)
                .HasDefaultValue(4)
                .HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.ResetToken).HasColumnName("reset_token");
            entity.Property(e => e.ResetTokenExpira).HasColumnName("reset_token_expira");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.UltimaSesion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("ultima_sesion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
