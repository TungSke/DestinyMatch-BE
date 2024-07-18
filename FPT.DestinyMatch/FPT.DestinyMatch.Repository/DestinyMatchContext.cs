using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository;

public partial class DestinyMatchContext : DbContext
{
    public DestinyMatchContext()
    {
    }

    public DestinyMatchContext(DbContextOptions<DestinyMatchContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Hobby> Hobbies { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Matching> Matchings { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberPackage> MemberPackages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<University> Universities { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07C809103D");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ReceiveNotifiEmail).HasColumnName("ReceiveNotifiEMail");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("member");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("newbie");
        });

        modelBuilder.Entity<Hobby>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hobby__3214EC072205D5CA");

            entity.ToTable("Hobby");

            entity.HasIndex(e => e.Name, "UQ__Hobby__737584F6C43927BA").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Members).WithMany(p => p.Hobbies)
                .UsingEntity<Dictionary<string, object>>(
                    "HobbyMember",
                    r => r.HasOne<Member>().WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HobbyMemb__Membe__5441852A"),
                    l => l.HasOne<Hobby>().WithMany()
                        .HasForeignKey("HobbyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HobbyMemb__Hobby__534D60F1"),
                    j =>
                    {
                        j.HasKey("HobbyId", "MemberId").HasName("PK__HobbyMem__9A710F7E9A404862");
                        j.ToTable("HobbyMember");
                        j.HasIndex(new[] { "HobbyId" }, "idx_HobbyId");
                    });
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Major__3214EC079F21472C");

            entity.ToTable("Major");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Matching>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Matching__3214EC0729DB595E");

            entity.ToTable("Matching");

            entity.HasIndex(e => e.FirstMemberId, "idx_FirstMemberId");

            entity.HasIndex(e => e.SecondMemberId, "idx_SecondMemberId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.RecentlyActivity)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SecondName).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Phản Hồi");

            entity.HasOne(d => d.FirstMember).WithMany(p => p.MatchingFirstMembers)
                .HasForeignKey(d => d.FirstMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Matching__FirstM__68487DD7");

            entity.HasOne(d => d.SecondMember).WithMany(p => p.MatchingSecondMembers)
                .HasForeignKey(d => d.SecondMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Matching__Second__693CA210");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Member__3214EC07FBD5A622");

            entity.ToTable("Member");

            entity.HasIndex(e => e.AccountId, "UQ__Member__349DA5A78679FF4E").IsUnique();

            entity.HasIndex(e => e.Gender, "idx_Gender");

            entity.HasIndex(e => e.MajorId, "idx_MajorId");

            entity.HasIndex(e => e.UniversityId, "idx_UniversityId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address).HasDefaultValue("Ở trên mặt đất, ở dưới bầu trời! :3");
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.Introduce).HasDefaultValue("Tên này rất lười, chả để lại lời nói gì cả!");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Xác Thực");
            entity.Property(e => e.Surplus).HasDefaultValue(0);

            entity.HasOne(d => d.Account).WithOne(p => p.Member)
                .HasForeignKey<Member>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__AccountI__4E88ABD4");

            entity.HasOne(d => d.Major).WithMany(p => p.Members)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__MajorId__5070F446");

            entity.HasOne(d => d.University).WithMany(p => p.Members)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__Universi__4F7CD00D");
        });

        modelBuilder.Entity<MemberPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MemberPa__3214EC07B89FB2C9");

            entity.ToTable("MemberPackage");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberPackages)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPac__Membe__60A75C0F");

            entity.HasOne(d => d.Package).WithMany(p => p.MemberPackages)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPac__Packa__619B8048");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC078609E46F");

            entity.ToTable("Message");

            entity.HasIndex(e => e.SenderId, "idx_SenderId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã gửi");

            entity.HasOne(d => d.Matching).WithMany(p => p.Messages)
                .HasForeignKey(d => d.MatchingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__Matchin__6EF57B66");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__SenderI__6FE99F9F");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Package__3214EC072BD14F13");

            entity.ToTable("Package");

            entity.HasIndex(e => e.Code, "UQ__Package__A25C5AA78434BE82").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Picture__3214EC0700564B39");

            entity.ToTable("Picture");

            entity.HasIndex(e => e.MemberId, "idx_MemberId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.Pictures)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Picture__MemberI__5812160E");
        });

        modelBuilder.Entity<University>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Universi__3214EC07825F0AB4");

            entity.ToTable("University");

            entity.HasIndex(e => e.Code, "UQ__Universi__A25C5AA74D7E4521").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
