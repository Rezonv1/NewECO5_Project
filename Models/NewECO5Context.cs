using Microsoft.EntityFrameworkCore;
using NewECO5.Models.Enum;
using NewECO5.Models;


namespace NewECO5.Models
{
    public class NewECO5DBContext : DbContext
    {
        /// <summary>
        /// 通訊設定資料集
        /// </summary>
        public DbSet<MeterSetting> MeterSettings { get; set; } = null!;

        /// <summary>
        /// 電表設定資料集
        /// </summary>
        public DbSet<PowerMeterSetting> PowerMeterSettings { get; set; } = null!;

        public DbSet<Protocol> Protocols { get; set; }
        public DbSet<MeterRealtimeRecord> MeterRealtimeRecords { get; set; }

        public DbSet<Comm_UnitSetting> Comm_UnitSettings { get; set; } = null!;
        public NewECO5DBContext(DbContextOptions<NewECO5DBContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 配置 MeterSettings 表
            modelBuilder.Entity<MeterSetting>(entity => {
                entity.ToTable("MeterSettings");
                entity.HasIndex(e => e.SerialNr)
                      .IsUnique();
            });

            // 配置 PowerMeterSettings 表
            modelBuilder.Entity<PowerMeterSetting>(entity => {
                entity.ToTable("PowerMeterSettings");
                entity.HasOne(e => e.MeterSetting)
                      .WithOne()
                      .HasForeignKey<PowerMeterSetting>(e => e.SerialNr)
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();
            });
        }

    }
}
