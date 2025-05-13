using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Reservas.Modelos;

public partial class DbErpContext : DbContext
{
    public DbErpContext()
    {
    }

    public DbErpContext(DbContextOptions<DbErpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstadoFisico> EstadoFisicos { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<InventarioEquipo> InventarioEquipos { get; set; }

    public virtual DbSet<Iot> Iots { get; set; }

    public virtual DbSet<Laboratorio> Laboratorios { get; set; }

    public virtual DbSet<PrestamosEquipo> PrestamosEquipos { get; set; }

    public virtual DbSet<ReservaDeEspacio> ReservaDeEspacios { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SolicitudReservaDeEspacio> SolicitudReservaDeEspacios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=10.122.120.52;Database=dbERP;Username=jean;Password=1701");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estado_pkey");

            entity.ToTable("estado");

            entity.HasIndex(e => e.Estado1, "estado_estado_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado1)
                .HasMaxLength(50)
                .HasColumnName("estado");
        });

        modelBuilder.Entity<EstadoFisico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estado_fisico_pkey");

            entity.ToTable("estado_fisico");

            entity.HasIndex(e => e.EstadoFisico1, "estado_fisico_estado_fisico_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EstadoFisico1)
                .HasMaxLength(50)
                .HasColumnName("estado_fisico");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("horario_pkey");

            entity.ToTable("horario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Asignatura).HasColumnName("asignatura");
            entity.Property(e => e.Dia)
                .HasMaxLength(10)
                .HasColumnName("dia");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.HoraFinal).HasColumnName("hora_final");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");
            entity.Property(e => e.Profesor).HasColumnName("profesor");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdLaboratorio)
                .HasConstraintName("horario_id_laboratorio_fkey");
        });

        modelBuilder.Entity<InventarioEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inventario_equipos_pkey");

            entity.ToTable("inventario_equipos");

            entity.HasIndex(e => e.Serial, "inventario_equipos_serial_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Departamento)
                .HasMaxLength(100)
                .HasColumnName("departamento");
            entity.Property(e => e.DescripcionLarga).HasColumnName("descripcion_larga");
            entity.Property(e => e.Disponible)
                .HasDefaultValue(true)
                .HasColumnName("disponible");
            entity.Property(e => e.Fabricante)
                .HasMaxLength(100)
                .HasColumnName("fabricante");
            entity.Property(e => e.FechaTransaccion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_transaccion");
            entity.Property(e => e.IdEstadoFisico)
                .HasDefaultValue(1)
                .HasColumnName("id_estado_fisico");
            entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");
            entity.Property(e => e.ImagenEquipo).HasColumnName("imagen_equipo");
            entity.Property(e => e.ImporteActivo)
                .HasPrecision(12, 2)
                .HasColumnName("importe_activo");
            entity.Property(e => e.Modelo)
                .HasMaxLength(100)
                .HasColumnName("modelo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.NombreCorto)
                .HasMaxLength(50)
                .HasColumnName("nombre_corto");
            entity.Property(e => e.Perfil).HasColumnName("perfil");
            entity.Property(e => e.Serial)
                .HasMaxLength(100)
                .HasColumnName("serial");
            entity.Property(e => e.ValidacionPrestamo)
                .HasDefaultValue(true)
                .HasColumnName("validacion_prestamo");

            entity.HasOne(d => d.IdEstadoFisicoNavigation).WithMany(p => p.InventarioEquipos)
                .HasForeignKey(d => d.IdEstadoFisico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inventario_equipos_id_estado_fisico_fkey");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.InventarioEquipos)
                .HasForeignKey(d => d.IdLaboratorio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inventario_equipos_id_laboratorio_fkey");
        });

        modelBuilder.Entity<Iot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iot_pkey");

            entity.ToTable("iot");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actuador)
                .HasDefaultValue(false)
                .HasColumnName("actuador");
            entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");
            entity.Property(e => e.IdPlaca).HasColumnName("id_placa");
            entity.Property(e => e.Sensor1).HasColumnName("sensor1");
            entity.Property(e => e.Sensor2).HasColumnName("sensor2");
            entity.Property(e => e.Sensor3).HasColumnName("sensor3");
            entity.Property(e => e.Sensor4).HasColumnName("sensor4");
            entity.Property(e => e.Sensor5).HasColumnName("sensor5");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.Iots)
                .HasForeignKey(d => d.IdLaboratorio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iot_id_laboratorio_fkey");
        });

        modelBuilder.Entity<Laboratorio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("laboratorios_pkey");

            entity.ToTable("laboratorios");

            entity.HasIndex(e => e.CodigoDeLab, "laboratorios_codigo_de_lab_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.CodigoDeLab)
                .HasMaxLength(50)
                .HasColumnName("codigo_de_lab");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
        });

        modelBuilder.Entity<PrestamosEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prestamos_equipos_pkey");

            entity.ToTable("prestamos_equipos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ComentarioAprobacion).HasColumnName("comentario_aprobacion");
            entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaFinal).HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdInventario).HasColumnName("id_inventario");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdUsuarioAprobador).HasColumnName("id_usuario_aprobador");
            entity.Property(e => e.Motivo).HasColumnName("motivo");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.PrestamosEquipos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("prestamos_equipos_id_estado_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.PrestamosEquipoIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("prestamos_equipos_id_usuario_fkey");

            entity.HasOne(d => d.IdUsuarioAprobadorNavigation).WithMany(p => p.PrestamosEquipoIdUsuarioAprobadorNavigations)
                .HasForeignKey(d => d.IdUsuarioAprobador)
                .HasConstraintName("prestamos_equipos_id_usuario_aprobador_fkey");
        });

        modelBuilder.Entity<ReservaDeEspacio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prestamos_espacios_pkey");

            entity.ToTable("reserva_de_espacios");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('prestamos_espacios_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ComentarioAprobacion).HasColumnName("comentario_aprobacion");
            entity.Property(e => e.FechaAprobacion).HasColumnName("fecha_aprobacion");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_solicitud");
            entity.Property(e => e.HoraFinal).HasColumnName("hora_final");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdUsuarioAprobador).HasColumnName("id_usuario_aprobador");
            entity.Property(e => e.Motivo).HasColumnName("motivo");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.ReservaDeEspacios)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("prestamos_espacios_id_estado_fkey");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.ReservaDeEspacios)
                .HasForeignKey(d => d.IdLaboratorio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("prestamos_espacios_id_laboratorio_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ReservaDeEspacioIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("prestamos_espacios_id_usuario_fkey");

            entity.HasOne(d => d.IdUsuarioAprobadorNavigation).WithMany(p => p.ReservaDeEspacioIdUsuarioAprobadorNavigations)
                .HasForeignKey(d => d.IdUsuarioAprobador)
                .HasConstraintName("prestamos_espacios_id_usuario_aprobador_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Rol, "roles_rol_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<SolicitudReservaDeEspacio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("solicitud_reserva_de_espacios_pkey");

            entity.ToTable("solicitud_reserva_de_espacios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_solicitud");
            entity.Property(e => e.HoraFinal).HasColumnName("hora_final");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdEstado)
                .HasDefaultValue(2)
                .HasColumnName("id_estado");
            entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Motivo).HasColumnName("motivo");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.SolicitudReservaDeEspacios)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("solicitud_reserva_de_espacios_id_estado_fkey");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.SolicitudReservaDeEspacios)
                .HasForeignKey(d => d.IdLaboratorio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_reserva_de_espacios_id_laboratorio_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.SolicitudReservaDeEspacios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_reserva_de_espacios_id_usuario_fkey");
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

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("usuarios_id_rol_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
