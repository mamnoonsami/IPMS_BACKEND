using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PEI_API.EF
{
    public partial class peiDBContext : DbContext
    {
        public peiDBContext()
        {
        }

        public peiDBContext(DbContextOptions<peiDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PeiCoordinatesPest> PeiCoordinatesPests { get; set; }
        public virtual DbSet<PeiCrop> PeiCrops { get; set; }
        public virtual DbSet<PeiCropimage> PeiCropimages { get; set; }
        public virtual DbSet<PeiCropsdisease> PeiCropsdiseases { get; set; }
        public virtual DbSet<PeiCropspest> PeiCropspests { get; set; }
        public virtual DbSet<PeiDisease> PeiDiseases { get; set; }
        public virtual DbSet<PeiFeedback> PeiFeedbacks { get; set; }
        public virtual DbSet<PeiPest> PeiPests { get; set; }
        public virtual DbSet<PeiPestInfo> PeiPestInfos { get; set; }
        public virtual DbSet<PeiPestimage> PeiPestimages { get; set; }
        public virtual DbSet<PeiReply> PeiReplies { get; set; }
        public virtual DbSet<PeiUser> PeiUsers { get; set; }
        public virtual DbSet<PeiWeed> PeiWeeds { get; set; }
        public virtual DbSet<PeiWeedimage> PeiWeedimages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=peiDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<PeiCoordinatesPest>(entity =>
            {
                entity.HasKey(e => e.CoordId);

                entity.ToTable("pei_coordinatesPest");

                entity.Property(e => e.CoordId).HasColumnName("coord_id");

                entity.Property(e => e.CoordLat)
                    .HasColumnType("decimal(20, 17)")
                    .HasColumnName("coord_lat");

                entity.Property(e => e.CoordLng)
                    .HasColumnType("decimal(20, 17)")
                    .HasColumnName("coord_lng");

                entity.Property(e => e.PId).HasColumnName("p_id");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.PIdNavigation)
                    .WithMany(p => p.PeiCoordinatesPests)
                    .HasForeignKey(d => d.PId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_pei_coordinatesPest_pei_pests");
            });

            modelBuilder.Entity<PeiCrop>(entity =>
            {
                entity.HasKey(e => e.CId);

                entity.ToTable("pei_crops");

                entity.Property(e => e.CId).HasColumnName("c_id");

                entity.Property(e => e.CDescription)
                    .IsRequired()
                    .HasColumnName("c_description");

                entity.Property(e => e.CName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("c_name");

                entity.Property(e => e.CPhotoUrl)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .IsUnicode(false)
                    .HasColumnName("c_photoURL");
            });

            modelBuilder.Entity<PeiCropimage>(entity =>
            {
                entity.HasKey(e => new { e.CiId, e.CId });

                entity.ToTable("pei_cropimages");

                entity.Property(e => e.CiId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ci_id");

                entity.Property(e => e.CId).HasColumnName("c_id");

                entity.Property(e => e.CiPhotoUrl)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .IsUnicode(false)
                    .HasColumnName("ci_photoURL");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.PeiCropimages)
                    .HasForeignKey(d => d.CId)
                    .HasConstraintName("FK_pei_cropimages_pei_crops");
            });

            modelBuilder.Entity<PeiCropsdisease>(entity =>
            {
                entity.HasKey(e => new { e.CId, e.DId });

                entity.ToTable("pei_cropsdiseases");

                entity.Property(e => e.CId).HasColumnName("c_id");

                entity.Property(e => e.DId).HasColumnName("d_id");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.PeiCropsdiseases)
                    .HasForeignKey(d => d.CId)
                    .HasConstraintName("FK_pei_cropsdiseases_pei_crops");

                entity.HasOne(d => d.DIdNavigation)
                    .WithMany(p => p.PeiCropsdiseases)
                    .HasForeignKey(d => d.DId)
                    .HasConstraintName("FK_pei_cropsdiseases_pei_diseases");
            });

            modelBuilder.Entity<PeiCropspest>(entity =>
            {
                entity.HasKey(e => new { e.PId, e.CId });

                entity.ToTable("pei_cropspests");

                entity.Property(e => e.PId).HasColumnName("p_id");

                entity.Property(e => e.CId).HasColumnName("c_id");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.PeiCropspests)
                    .HasForeignKey(d => d.CId)
                    .HasConstraintName("FK_pei_cropspests_pei_crops");

                entity.HasOne(d => d.PIdNavigation)
                    .WithMany(p => p.PeiCropspests)
                    .HasForeignKey(d => d.PId)
                    .HasConstraintName("FK_pei_cropspests_pei_pests");
            });

            modelBuilder.Entity<PeiDisease>(entity =>
            {
                entity.HasKey(e => e.DId);

                entity.ToTable("pei_diseases");

                entity.Property(e => e.DId).HasColumnName("d_id");

                entity.Property(e => e.DDescription).HasColumnName("d_description");

                entity.Property(e => e.DName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("d_name");
            });

            modelBuilder.Entity<PeiFeedback>(entity =>
            {
                entity.HasKey(e => e.FId)
                    .HasName("PK_pei_feedback");

                entity.ToTable("pei_feedbacks");

                entity.Property(e => e.FId).HasColumnName("f_id");

                entity.Property(e => e.FComment)
                    .IsRequired()
                    .HasColumnName("f_comment");

                entity.Property(e => e.FImageName)
                    .HasMaxLength(100)
                    .HasColumnName("f_imageName");

                entity.Property(e => e.FTimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("f_timeStamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_email");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.PeiFeedbacks)
                    .HasForeignKey(d => new { d.UId, d.UEmail })
                    .HasConstraintName("FK_pei_feedbacks_pei_users");
            });

            modelBuilder.Entity<PeiPest>(entity =>
            {
                entity.HasKey(e => e.PId);

                entity.ToTable("pei_pests");

                entity.Property(e => e.PId).HasColumnName("p_id");

                entity.Property(e => e.PDescription)
                    .IsRequired()
                    .HasColumnName("p_description");

                entity.Property(e => e.PName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("p_name");

                entity.Property(e => e.PPhotoUrl)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .IsUnicode(false)
                    .HasColumnName("p_photoURL");
            });

            modelBuilder.Entity<PeiPestInfo>(entity =>
            {
                entity.HasKey(e => e.PId);

                entity.ToTable("pei_pestInfo");

                entity.Property(e => e.PId)
                    .ValueGeneratedNever()
                    .HasColumnName("p_id");

                entity.Property(e => e.PinBiologicalControl)
                    .IsUnicode(false)
                    .HasColumnName("pin_biologicalControl");

                entity.Property(e => e.PinBiologicalinfo)
                    .IsUnicode(false)
                    .HasColumnName("pin_biologicalinfo");

                entity.Property(e => e.PinChemicalControl)
                    .IsUnicode(false)
                    .HasColumnName("pin_chemicalControl");

                entity.Property(e => e.PinControlThreshold)
                    .IsUnicode(false)
                    .HasColumnName("pin_controlThreshold");

                entity.Property(e => e.PinCulturalControl)
                    .IsUnicode(false)
                    .HasColumnName("pin_culturalControl");

                entity.Property(e => e.PinMonitoringMethod)
                    .IsUnicode(false)
                    .HasColumnName("pin_monitoringMethod");

                entity.Property(e => e.PinPhysicalControl)
                    .IsUnicode(false)
                    .HasColumnName("pin_physicalControl");

                entity.HasOne(d => d.PIdNavigation)
                    .WithOne(p => p.PeiPestInfo)
                    .HasForeignKey<PeiPestInfo>(d => d.PId)
                    .HasConstraintName("FK_pei_pestInfo_pei_pests");
            });

            modelBuilder.Entity<PeiPestimage>(entity =>
            {
                entity.HasKey(e => new { e.PiId, e.PId });

                entity.ToTable("pei_pestimages");

                entity.Property(e => e.PiId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("pi_id");

                entity.Property(e => e.PId).HasColumnName("p_id");

                entity.Property(e => e.PiPhotoUrl)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .IsUnicode(false)
                    .HasColumnName("pi_photoURL");

                entity.HasOne(d => d.PIdNavigation)
                    .WithMany(p => p.PeiPestimages)
                    .HasForeignKey(d => d.PId)
                    .HasConstraintName("FK_pei_pestimages_pei_pests");
            });

            modelBuilder.Entity<PeiReply>(entity =>
            {
                entity.HasKey(e => e.RId);

                entity.ToTable("pei_replies");

                entity.Property(e => e.RId).HasColumnName("r_id");

                entity.Property(e => e.FId).HasColumnName("f_id");

                entity.Property(e => e.RReply)
                    .IsRequired()
                    .HasColumnName("r_reply");

                entity.Property(e => e.RTimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("r_timeStamp")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_email");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.FIdNavigation)
                    .WithMany(p => p.PeiReplies)
                    .HasForeignKey(d => d.FId)
                    .HasConstraintName("FK_pei_replies_pei_feedbacks");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.PeiReplies)
                    .HasForeignKey(d => new { d.UId, d.UEmail })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pei_replies_pei_users");
            });

            modelBuilder.Entity<PeiUser>(entity =>
            {
                entity.HasKey(e => new { e.UId, e.UEmail });

                entity.ToTable("pei_users");

                entity.Property(e => e.UId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("u_id");

                entity.Property(e => e.UEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_email");

                entity.Property(e => e.UAccessToken)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_accessToken");

                entity.Property(e => e.UAuthLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_authLevel");

                entity.Property(e => e.UFirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_firstName");

                entity.Property(e => e.ULastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_lastName");

                entity.Property(e => e.UPassword)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("u_password");

                entity.Property(e => e.UStatus).HasColumnName("u_status");

                entity.Property(e => e.UTimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("u_timeStamp")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<PeiWeed>(entity =>
            {
                entity.HasKey(e => e.WId)
                    .HasName("PK_pei_weed");

                entity.ToTable("pei_weeds");

                entity.Property(e => e.WId).HasColumnName("w_id");

                entity.Property(e => e.WDescription)
                    .IsRequired()
                    .HasColumnName("w_description");

                entity.Property(e => e.WName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("w_name");

                entity.Property(e => e.WPhotoUrl)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .IsUnicode(false)
                    .HasColumnName("w_photoURL");
            });

            modelBuilder.Entity<PeiWeedimage>(entity =>
            {
                entity.HasKey(e => new { e.WiId, e.WId });

                entity.ToTable("pei_weedimages");

                entity.Property(e => e.WiId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("wi_id");

                entity.Property(e => e.WId).HasColumnName("w_id");

                entity.Property(e => e.WiPhotoUrl)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .IsUnicode(false)
                    .HasColumnName("wi_photoURL");

                entity.HasOne(d => d.WIdNavigation)
                    .WithMany(p => p.PeiWeedimages)
                    .HasForeignKey(d => d.WId)
                    .HasConstraintName("FK_pei_weedimages_pei_weeds");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
