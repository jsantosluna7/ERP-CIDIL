using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Modelos;

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

    public virtual DbSet<ReporteFalla> ReporteFallas { get; set; }

    public virtual DbSet<ReservaDeEspacio> ReservaDeEspacios { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SolicitudPrestamosDeEquipo> SolicitudPrestamosDeEquipos { get; set; }

    public virtual DbSet<SolicitudReservaDeEspacio> SolicitudReservaDeEspacios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosPendiente> UsuariosPendientes { get; set; }

    //nuevo
    public DbSet<UsuarioPublico> UsuarioPublicos { get; set; }//revisar esto usuariopublico
    public virtual DbSet<Anuncio> Anuncios { get; set; }
    public virtual DbSet<Comentario> Comentarios { get; set; }
    public virtual DbSet<Like> Likes { get; set; }
    public virtual DbSet<Curriculum> Curriculums { get; set; }

    //nuevo

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
            entity.Property(e => e.ActivadoHorario)
                .HasDefaultValue(true)
                .HasColumnName("activado_horario");
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
            entity.Property(e => e.FechaFinal).HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
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

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activado)
                .HasDefaultValue(true)
                .HasColumnName("activado");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
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
            entity.Property(e => e.ImagenEquipo)
                .HasMaxLength(100)
                .HasColumnName("imagen_equipo");
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
                .HasConstraintName("inventario_equipos_id_estado_fisico_fkey");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.InventarioEquipos)
                .HasForeignKey(d => d.IdLaboratorio)
                .HasConstraintName("inventario_equipos_id_laboratorio_fkey");
        });

        modelBuilder.Entity<Iot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iot_pkey");

            entity.ToTable("iot");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activado)
                .HasDefaultValue(true)
                .HasColumnName("activado");
            entity.Property(e => e.Actuador)
                .HasDefaultValue(false)
                .HasColumnName("actuador");
            entity.Property(e => e.HoraEntrada)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("hora_entrada");
            entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");
            entity.Property(e => e.IdPlaca)
                .HasMaxLength(50)
                .HasColumnName("id_placa");
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
            entity.Property(e => e.Activado)
                .HasDefaultValue(true)
                .HasColumnName("activado");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.CodigoDeLab)
                .HasMaxLength(50)
                .HasColumnName("codigo_de_lab");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Piso)
                .HasDefaultValue(1)
                .HasColumnName("piso");
        });

        modelBuilder.Entity<PrestamosEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prestamos_equipos_pkey");

            entity.ToTable("prestamos_equipos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activado)
                .HasDefaultValue(true)
                .HasColumnName("activado");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
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

        modelBuilder.Entity<ReporteFalla>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("reporte_falla_pkey");

            entity.ToTable("reporte_falla");

            entity.Property(e => e.IdReporte).HasColumnName("id_reporte");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaUltimaActualizacion)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_ultima_actualizacion");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");
            entity.Property(e => e.Lugar).HasColumnName("lugar");
            entity.Property(e => e.NombreSolicitante)
                .HasMaxLength(200)
                .HasColumnName("nombre_solicitante");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.ReporteFallas)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reporte_falla_id_estado_fkey");

            entity.HasOne(d => d.IdLaboratorioNavigation).WithMany(p => p.ReporteFallas)
                .HasForeignKey(d => d.IdLaboratorio)
                .HasConstraintName("reporte_falla_id_laboratorio_fkey");
        });

        modelBuilder.Entity<ReservaDeEspacio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prestamos_espacios_pkey");

            entity.ToTable("reserva_de_espacios");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('prestamos_espacios_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Activado)
                .HasDefaultValue(true)
                .HasColumnName("activado");
            entity.Property(e => e.ComentarioAprobacion).HasColumnName("comentario_aprobacion");
            entity.Property(e => e.FechaAprobacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_aprobacion");
            entity.Property(e => e.FechaFinal).HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
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

        modelBuilder.Entity<SolicitudPrestamosDeEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("solicitud_prestamos_de_equipos_pkey");

            entity.ToTable("solicitud_prestamos_de_equipos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
            entity.Property(e => e.FechaFinal).HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_solicitud");
            entity.Property(e => e.IdEstado)
                .HasDefaultValue(2)
                .HasColumnName("id_estado");
            entity.Property(e => e.IdInventario).HasColumnName("id_inventario");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Motivo).HasColumnName("motivo");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.SolicitudPrestamosDeEquipos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("solicitud_prestamos_de_equipos_id_estado_fkey");

            entity.HasOne(d => d.IdInventarioNavigation).WithMany(p => p.SolicitudPrestamosDeEquipos)
                .HasForeignKey(d => d.IdInventario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_prestamos_de_equipos_id_inventario_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.SolicitudPrestamosDeEquipos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("solicitud_prestamos_de_equipos_id_usuario_fkey");
        });

        modelBuilder.Entity<SolicitudReservaDeEspacio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("solicitud_reserva_de_espacios_pkey");

            entity.ToTable("solicitud_reserva_de_espacios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaFinal).HasColumnName("fecha_final");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
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

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("usuarios_id_rol_fkey");
        });

        modelBuilder.Entity<UsuariosPendiente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pendientes_pkey");

            entity.ToTable("usuarios_pendientes");

            entity.HasIndex(e => e.CorreoInstitucional, "usuarios_pendientes_correo_key").IsUnique();

            entity.HasIndex(e => e.IdMatricula, "usuarios_pendientes_id_matricula_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
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
            entity.Property(e => e.IdMatricula).HasColumnName("id_matricula");
            entity.Property(e => e.IdRol)
                .HasDefaultValue(4)
                .HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.OtpExpira).HasColumnName("otp_expira");
            entity.Property(e => e.OtpHash).HasColumnName("otp_hash");
            entity.Property(e => e.OtpIntentos)
                .HasDefaultValue(0)
                .HasColumnName("otp_intentos");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            //nuevo

            // --- Configuración de Anuncio ---
            modelBuilder.Entity<Anuncio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("anuncios");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsRequired()
                    .HasColumnName("titulo");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion");

                entity.Property(e => e.ImagenUrl)
                    .HasColumnName("imagen_url");

                entity.Property(e => e.FechaPublicacion)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("fecha_publicacion");

                // Relaciones
                entity.HasMany(a => a.Comentarios)
                      .WithOne(c => c.Anuncio)
                      .HasForeignKey(c => c.AnuncioId)
                      .HasConstraintName("comentarios_anuncio_fk")
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.Likes)
                      .WithOne(l => l.Anuncio)
                      .HasForeignKey(l => l.AnuncioId)
                      .HasConstraintName("likes_anuncio_fk")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Configuración de Comentario ---
            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("comentarios");

                entity.Property(e => e.Texto)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("texto");

                entity.Property(e => e.Fecha)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("fecha");

                // Relación con UsuarioPublico
                entity.HasOne(c => c.Usuario)
                      .WithMany(u => u.Comentarios)
                      .HasForeignKey(c => c.UsuarioId)
                      .HasConstraintName("comentarios_usuario_fk")
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con Anuncio ya configurada arriba
            });

            // --- Configuración de Like ---
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("likes");

                entity.Property(e => e.IpUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("ip_usuario");

                entity.Property(e => e.Fecha)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("fecha");

                // Relación con UsuarioPublico
                entity.HasOne(l => l.Usuario)
                      .WithMany(u => u.Likes)
                      .HasForeignKey(l => l.UsuarioId)
                      .HasConstraintName("likes_usuario_fk")
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con Anuncio ya configurada arriba
            });

            // --- Configuración de Curriculum ---
            modelBuilder.Entity<Curriculum>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("curriculums");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.ArchivoUrl)
                    .IsRequired()
                    .HasColumnName("archivo_url");

                entity.Property(e => e.FechaEnvio)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("fecha_envio");
            });








            //nuevo



        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
