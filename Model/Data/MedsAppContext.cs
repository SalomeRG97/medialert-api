using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace Model.Data;

public partial class MedsAppContext : DbContext
{
    public MedsAppContext()
    {
    }

    public MedsAppContext(DbContextOptions<MedsAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Intake> Intakes { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=meds_app;user=meds_user;password=meds_pass", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Intake>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.ScheduleId, "ScheduleId");

            entity.Property(e => e.Status).HasColumnType("enum('planned','taken','missed','skipped')");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Intakes)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedules");
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Form)
                .HasMaxLength(50)
                .HasDefaultValueSql("'0'");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasDefaultValueSql("'0'");
            entity.Property(e => e.Strength)
                .HasMaxLength(50)
                .HasDefaultValueSql("'0'");

            entity.HasOne(d => d.User).WithMany(p => p.Medications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medications_Users");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.MedicationId, "MedicationId");

            entity.Property(e => e.DoseAmount).HasPrecision(10, 2);
            entity.Property(e => e.Pattern).HasMaxLength(255);

            entity.HasOne(d => d.Medication).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.MedicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Medications");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValueSql("'0'");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("'0'");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasDefaultValueSql("'0'");
            entity.Property(e => e.Timezone)
                .HasMaxLength(50)
                .HasDefaultValueSql("'America/Bogota'");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
