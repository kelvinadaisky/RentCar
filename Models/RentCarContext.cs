using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RentCar.Models;

public partial class RentCarContext : DbContext
{
    public RentCarContext()
    {
    }

    public RentCarContext(DbContextOptions<RentCarContext> options)
        : base(options)
    {
    }
    public DbSet<RevenueByCar> RevenuesByCar { get; set; }
    public DbSet<TopCustomer> TopCustomers { get; set; }
    public DbSet<MonthlyRevenueByAgency> MonthlyRevenuesByAgence { get; set; }
    // Table-valued functions
    public IQueryable<RevenueByCar> GetRevenueByCar() =>
        RevenuesByCar.FromSqlRaw("SELECT * FROM GetRevenueByCar()");
    public IQueryable<TopCustomer> GetTopCustomers(int n) =>
    TopCustomers.FromSqlRaw("SELECT * FROM gettopcustomers({0})", n);
    public IQueryable<MonthlyRevenueByAgency> GetMonthlyRevenueByAgence() =>
        MonthlyRevenuesByAgence.FromSqlRaw("SELECT * FROM GetMonthlyRevenueByAjans()");

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Ajan> Ajans { get; set; }

    public virtual DbSet<Arac> Aracs { get; set; }

    public virtual DbSet<AracDurumu> AracDurumus { get; set; }

    public virtual DbSet<Bakim> Bakims { get; set; }

    public virtual DbSet<Calisan> Calisans { get; set; }

    public virtual DbSet<Ehliyet> Ehliyets { get; set; }

    public virtual DbSet<Fatura> Faturas { get; set; }

    public virtual DbSet<HasarDurumu> HasarDurumus { get; set; }

    public virtual DbSet<Kisi> Kisis { get; set; }

    public virtual DbSet<Musteri> Musteris { get; set; }

    public virtual DbSet<Odeme> Odemes { get; set; }

    public virtual DbSet<Sigortum> Sigorta { get; set; }

    public virtual DbSet<Sozlesme> Sozlesmes { get; set; }

    public virtual DbSet<Teslimat> Teslimats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Rent_Car;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RevenueByCar>().HasNoKey();
        modelBuilder.Entity<TopCustomer>().HasNoKey();
        modelBuilder.Entity<MonthlyRevenueByAgency>().HasNoKey();
        modelBuilder.Entity<MonthlyRevenue>(entity =>
        {
            entity.HasNoKey(); // No primary key
        });
        modelBuilder.Entity<StatsView>(entity =>
        {
            entity.HasNoKey(); // This indicates that it does not have a primary key
        });
        modelBuilder.Entity<TotalCarsCount>(entity =>
        {
            entity.HasNoKey(); // No primary key
        });
        modelBuilder.Entity<TotalCustomersCount>(entity =>
        {
            entity.HasNoKey(); // No primary key
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Tc).HasName("Admin_pkey");

            entity.ToTable("Admin");

            entity.Property(e => e.Tc)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("TC");
            entity.Property(e => e.Sifre).HasMaxLength(255);

            entity.HasOne(d => d.TcNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.Tc)
                .HasConstraintName("Admin_TC_fkey");
        });

        modelBuilder.Entity<Ajan>(entity =>
        {
            entity.HasKey(e => e.IdAjans).HasName("Ajans_pkey");

            entity.Property(e => e.IdAjans).HasColumnName("ID_ajans");
            entity.Property(e => e.Adres).HasMaxLength(255);
            entity.Property(e => e.AjansAdi)
                .HasMaxLength(100)
                .HasColumnName("Ajans_adi");
            entity.Property(e => e.EPosta)
                .HasMaxLength(100)
                .HasColumnName("E_posta");
            entity.Property(e => e.Telefon).HasMaxLength(15);
        });

        modelBuilder.Entity<Arac>(entity =>
        {
            entity.HasKey(e => e.IdAraba).HasName("Araç_pkey");

            entity.ToTable("Arac");

            entity.HasIndex(e => e.PlakaNumarasi, "Arac_Plaka_numarasi_key").IsUnique();

            entity.Property(e => e.IdAraba)
                .HasDefaultValueSql("nextval('\"Araç_ID_araba_seq\"'::regclass)")
                .HasColumnName("ID_araba");
            entity.Property(e => e.IdAjans).HasColumnName("ID_ajans");
            entity.Property(e => e.Marka).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.PlakaNumarasi)
                .HasMaxLength(20)
                .HasColumnName("Plaka_numarasi");
            entity.Property(e => e.Renk).HasMaxLength(20);
            entity.Property(e => e.UretimYili).HasColumnName("Uretim_yili");

            entity.HasOne(d => d.IdAjansNavigation).WithMany(p => p.Aracs)
                .HasForeignKey(d => d.IdAjans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Arac_ID_ajans_fkey");
        });

        modelBuilder.Entity<AracDurumu>(entity =>
        {
            entity.HasKey(e => e.IdDurum).HasName("Araç_Durumu_pkey");

            entity.ToTable("Arac_Durumu");

            entity.HasIndex(e => e.IdAraba, "unique_id_araba").IsUnique();

            entity.Property(e => e.IdDurum)
                .HasDefaultValueSql("nextval('\"Araç_Durumu_ID_durum_seq\"'::regclass)")
                .HasColumnName("ID_durum");
            entity.Property(e => e.Aciklama).HasDefaultValueSql("'Araç mevcut'::text");
            entity.Property(e => e.GuncellemeTarihi).HasColumnName("Guncelleme_tarihi");
            entity.Property(e => e.IdAraba).HasColumnName("ID_araba");

            entity.HasOne(d => d.IdArabaNavigation).WithOne(p => p.AracDurumu)
                .HasForeignKey<AracDurumu>(d => d.IdAraba)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("link_Arac_AracDurum");
        });

        modelBuilder.Entity<Bakim>(entity =>
        {
            entity.HasKey(e => e.IdBakim).HasName("Bakım_pkey");

            entity.ToTable("Bakim");

            entity.HasIndex(e => e.IdAraba, "Bakim_ID_araba_key").IsUnique();

            entity.Property(e => e.IdBakim)
                .HasDefaultValueSql("nextval('\"Bakım_ID_bakım_seq\"'::regclass)")
                .HasColumnName("ID_bakim");
            entity.Property(e => e.BakimTuru)
                .HasMaxLength(50)
                .HasColumnName("Bakim_turu");
            entity.Property(e => e.BaslangicTarihi).HasColumnName("Baslangic_tarihi");
            entity.Property(e => e.BitisTarihi).HasColumnName("Bitis_tarihi");
            entity.Property(e => e.IdAraba).HasColumnName("ID_araba");
            entity.Property(e => e.Maliyet).HasPrecision(10, 2);

            entity.HasOne(d => d.IdArabaNavigation).WithOne(p => p.Bakim)
                .HasForeignKey<Bakim>(d => d.IdAraba)
                .HasConstraintName("Bakim_ID_araba_fkey");
        });

        modelBuilder.Entity<Calisan>(entity =>
        {
            entity.HasKey(e => e.Tc).HasName("Çalışan_pkey");

            entity.ToTable("Calisan");

            entity.Property(e => e.Tc)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("TC");
            entity.Property(e => e.IdAjans).HasColumnName("ID_ajans");
            entity.Property(e => e.Maas).HasPrecision(10, 2);
            entity.Property(e => e.Pozisyon).HasMaxLength(50);

            entity.HasOne(d => d.IdAjansNavigation).WithMany(p => p.Calisans)
                .HasForeignKey(d => d.IdAjans)
                .HasConstraintName("Calisan_ID_ajans_fkey");

            entity.HasOne(d => d.TcNavigation).WithOne(p => p.Calisan)
                .HasForeignKey<Calisan>(d => d.Tc)
                .HasConstraintName("Calisan_TC_fkey");
        });

        modelBuilder.Entity<Ehliyet>(entity =>
        {
            entity.HasKey(e => e.IdEhliyet).HasName("Ehliyet_pkey");

            entity.ToTable("Ehliyet");

            entity.HasIndex(e => e.MusteriTc, "Ehliyet_Musteri_TC_key").IsUnique();

            entity.Property(e => e.IdEhliyet).HasColumnName("ID_ehliyet");
            entity.Property(e => e.EhliyetNo)
                .HasMaxLength(20)
                .HasColumnName("Ehliyet_no");
            entity.Property(e => e.EhliyetTarihi).HasColumnName("Ehliyet_tarihi");
            entity.Property(e => e.Kategori).HasMaxLength(20);
            entity.Property(e => e.MusteriTc)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("Musteri_TC");
            entity.Property(e => e.SonGecerlilikTarihi).HasColumnName("Son_gecerlilik_tarihi");

            entity.HasOne(d => d.MusteriTcNavigation).WithOne(p => p.Ehliyet)
                .HasForeignKey<Ehliyet>(d => d.MusteriTc)
                .HasConstraintName("link_Musteri_Ehliyet");
        });

        modelBuilder.Entity<Fatura>(entity =>
        {
            entity.HasKey(e => e.IdFatura).HasName("Fatura_pkey");

            entity.ToTable("Fatura");

            entity.HasIndex(e => e.IdSozlesme, "Fatura_ID_sozlesme_key").IsUnique();

            entity.Property(e => e.IdFatura).HasColumnName("ID_fatura");
            entity.Property(e => e.DuzenlenmeTarihi).HasColumnName("Duzenlenme_tarihi");
            entity.Property(e => e.IdSozlesme).HasColumnName("ID_sozlesme");
            entity.Property(e => e.OdenenTutar)
                .HasPrecision(10, 2)
                .HasColumnName("Odenen_tutar");
            entity.Property(e => e.VadeTarihi).HasColumnName("Vade_tarihi");

            entity.HasOne(d => d.IdSozlesmeNavigation).WithOne(p => p.Fatura)
                .HasForeignKey<Fatura>(d => d.IdSozlesme)
                .HasConstraintName("Fatura_ID_sozlesme_fkey");
        });

        modelBuilder.Entity<HasarDurumu>(entity =>
        {
            entity.HasKey(e => e.IdHasarDurumu).HasName("Hasar_Durumu_pkey");

            entity.ToTable("Hasar_Durumu");

            entity.HasIndex(e => e.IdTeslimat, "Hasar_Durumu_ID_teslimat_key").IsUnique();

            entity.Property(e => e.IdHasarDurumu).HasColumnName("ID_Hasar_Durumu");
            entity.Property(e => e.Durumu).HasMaxLength(50);
            entity.Property(e => e.IdTeslimat).HasColumnName("ID_teslimat");

            entity.HasOne(d => d.IdTeslimatNavigation).WithOne(p => p.HasarDurumu)
                .HasForeignKey<HasarDurumu>(d => d.IdTeslimat)
                .HasConstraintName("Hasar_Durumu_ID_teslimat_fkey");
        });

        modelBuilder.Entity<Kisi>(entity =>
        {
            entity.HasKey(e => e.Tc).HasName("Kişi_pkey");

            entity.ToTable("Kisi");

            entity.Property(e => e.Tc)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("TC");
            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.EPosta)
                .HasMaxLength(100)
                .HasColumnName("E_posta");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.Soyad).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(15);
        });

        modelBuilder.Entity<Musteri>(entity =>
        {
            entity.HasKey(e => e.Tc).HasName("Müşteri_pkey");

            entity.ToTable("Musteri");

            entity.Property(e => e.Tc)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("TC");
            entity.Property(e => e.DogumTarihi).HasColumnName("Dogum_Tarihi");

            entity.HasOne(d => d.TcNavigation).WithOne(p => p.Musteri)
                .HasForeignKey<Musteri>(d => d.Tc)
                .HasConstraintName("Musteri_TC_fkey");
        });

        modelBuilder.Entity<Odeme>(entity =>
        {
            entity.HasKey(e => e.IdOdeme).HasName("Ödeme_pkey");

            entity.ToTable("Odeme");

            entity.HasIndex(e => e.IdFatura, "Odeme_ID_fatura_key").IsUnique();

            entity.Property(e => e.IdOdeme)
                .HasDefaultValueSql("nextval('\"Ödeme_ID_ödeme_seq\"'::regclass)")
                .HasColumnName("ID_odeme");
            entity.Property(e => e.IdFatura).HasColumnName("ID_fatura");
            entity.Property(e => e.OdemeDurumu)
                .HasMaxLength(50)
                .HasColumnName("Odeme_durumu");
            entity.Property(e => e.OdemeTarihi).HasColumnName("Odeme_tarihi");
            entity.Property(e => e.OdemeYontemi)
                .HasMaxLength(50)
                .HasColumnName("Odeme_yontemi");
            entity.Property(e => e.Tutar).HasPrecision(10, 2);

            entity.HasOne(d => d.IdFaturaNavigation).WithOne(p => p.Odeme)
                .HasForeignKey<Odeme>(d => d.IdFatura)
                .HasConstraintName("Odeme_ID_fatura_fkey");
        });

        modelBuilder.Entity<Sigortum>(entity =>
        {
            entity.HasKey(e => e.IdSigorta).HasName("Sigorta_pkey");

            entity.HasIndex(e => e.IdAraba, "Sigorta_ID_araba_key").IsUnique();

            entity.Property(e => e.IdSigorta).HasColumnName("ID_sigorta");
            entity.Property(e => e.IdAraba).HasColumnName("ID_araba");
            entity.Property(e => e.SigortaFirma)
                .HasMaxLength(100)
                .HasColumnName("Sigorta_firma");
            entity.Property(e => e.SigortaTuru)
                .HasMaxLength(50)
                .HasColumnName("Sigorta_turu");
            entity.Property(e => e.SonTarih).HasColumnName("Son_tarih");

            entity.HasOne(d => d.IdArabaNavigation).WithOne(p => p.Sigortum)
                .HasForeignKey<Sigortum>(d => d.IdAraba)
                .HasConstraintName("Sigorta_ID_araba_fkey");
        });

        modelBuilder.Entity<Sozlesme>(entity =>
        {
            entity.HasKey(e => e.IdSozlesme).HasName("Sözleşme_pkey");

            entity.ToTable("Sozlesme");

            entity.Property(e => e.IdSozlesme)
                .HasDefaultValueSql("nextval('\"Sözleşme_ID_sözleşme_seq\"'::regclass)")
                .HasColumnName("ID_sozlesme");
            entity.Property(e => e.CikisTarihi).HasColumnName("cikis_tarihi");
            entity.Property(e => e.DonusTarihi).HasColumnName("donus_tarihi");
            entity.Property(e => e.Durum)
                .HasDefaultValueSql("'Aktif'::character varying")
                .HasColumnType("character varying");
            entity.Property(e => e.IdAraba).HasColumnName("ID_araba");
            entity.Property(e => e.IdMusteri)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("ID_musteri");
            entity.Property(e => e.KiraSekli)
                .HasMaxLength(50)
                .HasDefaultValueSql("'daily'::character varying")
                .HasColumnName("Kira_sekli");
            entity.Property(e => e.ToplamTutar).HasColumnName("Toplam_tutar");

            entity.HasOne(d => d.IdArabaNavigation).WithMany(p => p.Sozlesmes)
                .HasForeignKey(d => d.IdAraba)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Sozlesme_ID_araba_fkey");

            entity.HasOne(d => d.IdMusteriNavigation).WithMany(p => p.Sozlesmes)
                .HasForeignKey(d => d.IdMusteri)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Sozlesme_ID_musteri_fkey");
        });

        modelBuilder.Entity<Teslimat>(entity =>
        {
            entity.HasKey(e => e.IdTeslimat).HasName("Teslimat_pkey");

            entity.ToTable("Teslimat");

            entity.HasIndex(e => e.IdSozlesme, "Teslimat_ID_sozlesme_key").IsUnique();

            entity.Property(e => e.IdTeslimat).HasColumnName("ID_teslimat");
            entity.Property(e => e.Durum).HasMaxLength(20);
            entity.Property(e => e.GeriDonusTarihi).HasColumnName("Geri_donus_tarihi");
            entity.Property(e => e.IdSozlesme).HasColumnName("ID_sozlesme");
            entity.Property(e => e.TeslimatTarihi).HasColumnName("Teslimat_tarihi");

            entity.HasOne(d => d.IdSozlesmeNavigation).WithOne(p => p.Teslimat)
                .HasForeignKey<Teslimat>(d => d.IdSozlesme)
                .HasConstraintName("Teslimat_ID_sozlesme_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
