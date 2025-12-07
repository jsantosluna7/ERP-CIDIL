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

    public virtual DbSet<Anuncio> Anuncios { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<ComentariosOrden> ComentariosOrdens { get; set; }

    public virtual DbSet<Curriculum> Curriculums { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstadoFisico> EstadoFisicos { get; set; }

    public virtual DbSet<EstadosTimeline> EstadosTimelines { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<InventarioEquipo> InventarioEquipos { get; set; }

    public virtual DbSet<Iot> Iots { get; set; }

    public virtual DbSet<Laboratorio> Laboratorios { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<OrdenItem> OrdenItems { get; set; }

    public virtual DbSet<OrdenTimeline> OrdenTimelines { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<PrestamosEquipo> PrestamosEquipos { get; set; }

    public virtual DbSet<ReporteFalla> ReporteFallas { get; set; }

    public virtual DbSet<ReservaDeEspacio> ReservaDeEspacios { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SolicitudPrestamosDeEquipo> SolicitudPrestamosDeEquipos { get; set; }

    public virtual DbSet<SolicitudReservaDeEspacio> SolicitudReservaDeEspacios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosPendiente> UsuariosPendientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anuncio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("anuncios_pkey");

            entity.ToTable("anuncios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.EsCarrusel).HasDefaultValue(false);
            entity.Property(e => e.EsPasantia)
                .HasDefaultValue(false)
                .HasColumnName("es_pasantia");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaPublicacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.ImagenUrl).HasColumnName("imagen_url");
            entity.Property(e => e.Titulo)
                .HasMaxLength(200)
                .HasColumnName("titulo");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comentarios_pkey");

            entity.ToTable("comentarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnuncioId).HasColumnName("anuncio_id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
            entity.Property(e => e.Texto).HasColumnName("texto");
            entity.Property(e => e.Usuario)
                .HasMaxLength(150)
                .HasColumnName("usuario");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Anuncio).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.AnuncioId)
                .HasConstraintName("comentario_anuncio_fkey");
        });

        modelBuilder.Entity<ComentariosOrden>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comentarios_orden_pkey");

            entity.ToTable("comentarios_orden");

            entity.HasIndex(e => e.ItemId, "idx_comentarios_item");

            entity.HasIndex(e => e.OrdenId, "idx_comentarios_orden");

            entity.HasIndex(e => e.UsuarioId, "idx_comentarios_usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comentario).HasColumnName("comentario");
            entity.Property(e => e.CreadoEn)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creado_en");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.OrdenId).HasColumnName("orden_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Item).WithMany(p => p.ComentariosOrdens)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comentarios_orden_item_id_fkey");

            entity.HasOne(d => d.Orden).WithMany(p => p.ComentariosOrdens)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comentarios_orden_orden_id_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ComentariosOrdens)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("comentarios_orden_usuario_id_fkey");
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("curriculums_pkey");

            entity.ToTable("curriculums");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnuncioId).HasColumnName("anuncio_id");
            entity.Property(e => e.ArchivoUrl).HasColumnName("archivo_url");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.EsExterno)
                .HasDefaultValue(false)
                .HasColumnName("es_externo");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_envio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Anuncio).WithMany(p => p.Curricula)
                .HasForeignKey(d => d.AnuncioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("curriculums_anuncio_id_fkey");
        });

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

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("likes_pkey");

            entity.ToTable("likes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnuncioId).HasColumnName("anuncio_id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
            entity.Property(e => e.IpUsuario)
                .HasMaxLength(50)
                .HasColumnName("ip_usuario");
            entity.Property(e => e.Usuario)
                .HasMaxLength(150)
                .HasColumnName("usuario");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Anuncio).WithMany(p => p.Likes)
                .HasForeignKey(d => d.AnuncioId)
                .HasConstraintName("like_anuncio_fkey");
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
            entity.Property(e => e.EstadoTimelineId).HasDefaultValue(1).HasColumnName("estado_timeline_id");
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

            entity.HasIndex(e => e.IdUsuario, "fki_reporte_falla_id_usuario_fkey");

            entity.Property(e => e.IdReporte).HasColumnName("id_reporte");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(1)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaUltimaActualizacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_ultima_actualizacion");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Lugar).HasColumnName("lugar");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ReporteFallas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reporte_falla_id_usuario_fkey");
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
            entity.Property(e => e.PersonasCantidad)
                .HasDefaultValue(1)
                .HasColumnName("personas_cantidad");

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
            entity.Property(e => e.PersonasCantidad)
                .HasDefaultValue(1)
                .HasColumnName("personas_cantidad");

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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
