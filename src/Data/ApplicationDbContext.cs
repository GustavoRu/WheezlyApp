using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WheezlyApp.Models;

namespace WheezlyApp.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<BuyerZipCode> BuyerZipCodes { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Make> Makes { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Quote> Quotes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusHistory> StatusHistories { get; set; }

    public virtual DbSet<SubModel> SubModels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ZipCode> ZipCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,14334;Database=WheezlyDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Buyers__3214EC076180BDA2");

            entity.HasIndex(e => e.Email, "UK_Buyers_Email").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BuyerZipCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BuyerZip__3214EC07E5928DB1");

            entity.HasIndex(e => new { e.BuyerId, e.ZipCodeId }, "UK_BuyerZipCodes_Buyer_ZipCode").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Buyer).WithMany(p => p.BuyerZipCodes)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BuyerZipCodes_Buyers");

            entity.HasOne(d => d.ZipCode).WithMany(p => p.BuyerZipCodes)
                .HasForeignKey(d => d.ZipCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BuyerZipCodes_ZipCodes");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cars__3214EC07C6EF2114");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Make).WithMany(p => p.Cars)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_Makes");

            entity.HasOne(d => d.Model).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_Models");

            entity.HasOne(d => d.SubModel).WithMany(p => p.Cars)
                .HasForeignKey(d => d.SubModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_SubModels");

            entity.HasOne(d => d.ZipCode).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ZipCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_ZipCodes");
        });

        modelBuilder.Entity<Make>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Makes__3214EC07CFE95E2D");

            entity.HasIndex(e => e.Name, "UK_Makes_Name").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Models__3214EC0740E77B11");

            entity.HasIndex(e => new { e.MakeId, e.Name }, "UK_Models_MakeId_Name").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Make).WithMany(p => p.Models)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Models_Makes");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotes__3214EC074780B8CC");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CurrentAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Buyer).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quotes_Buyers");

            entity.HasOne(d => d.Car).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quotes_Cars");

            entity.HasOne(d => d.ZipCode).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.ZipCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Quotes_ZipCodes");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07D0D5C492");

            entity.ToTable("Status");

            entity.HasIndex(e => e.Name, "UK_Status_Name").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<StatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StatusHi__3214EC07D174854F");

            entity.ToTable("StatusHistory");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Car).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_Cars");

            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.ChangedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_Users");

            entity.HasOne(d => d.Status).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_Status");
        });

        modelBuilder.Entity<SubModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubModel__3214EC070E300AA3");

            entity.HasIndex(e => new { e.ModelId, e.Name }, "UK_SubModels_ModelId_Name").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Model).WithMany(p => p.SubModels)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubModels_Models");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E55041D5");

            entity.HasIndex(e => e.Email, "UK_Users_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UK_Users_Username").IsUnique();

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<ZipCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ZipCodes__3214EC0723A62F2B");

            entity.HasIndex(e => e.ZipCode1, "UK_ZipCodes_ZipCode").IsUnique();

            entity.Property(e => e.ZipCode1)
                .HasMaxLength(10)
                .HasColumnName("ZipCode");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
