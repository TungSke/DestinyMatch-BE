using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Repository.Models;

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

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Hobby> Hobbies { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<MatchRequest> MatchRequests { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberPackage> MemberPackages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<University> Universities { get; set; }

    public virtual DbSet<Verification> Verifications { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(local);database=DestinyMatch;uid=sa;pwd=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC076A8C801E");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D1053408136640").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("member");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("newbie");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC074F7F552A");

            entity.ToTable("Conversation");

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
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.FirstMember).WithMany(p => p.ConversationFirstMembers)
                .HasForeignKey(d => d.FirstMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__First__6D0D32F4");

            entity.HasOne(d => d.SecondMember).WithMany(p => p.ConversationSecondMembers)
                .HasForeignKey(d => d.SecondMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__Secon__6E01572D");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC075E46D84A");

            entity.ToTable("Feedback");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã Gửi");
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Sender).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Sender__7A672E12");
        });

        modelBuilder.Entity<Hobby>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hobby__3214EC0798D3D28B");

            entity.ToTable("Hobby");

            entity.HasIndex(e => e.Name, "UQ__Hobby__737584F66869A0BA").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Members).WithMany(p => p.Hobbies)
                .UsingEntity<Dictionary<string, object>>(
                    "HobbyMember",
                    r => r.HasOne<Member>().WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HobbyMemb__Membe__534D60F1"),
                    l => l.HasOne<Hobby>().WithMany()
                        .HasForeignKey("HobbyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__HobbyMemb__Hobby__52593CB8"),
                    j =>
                    {
                        j.HasKey("HobbyId", "MemberId").HasName("PK__HobbyMem__9A710F7E894FFE91");
                        j.ToTable("HobbyMember");
                        j.HasIndex(new[] { "HobbyId" }, "idx_HobbyId");
                    });
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Major__3214EC0794C463BE");

            entity.ToTable("Major");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<MatchRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatchReq__3214EC07FCBD8432");

            entity.ToTable("MatchRequest");

            entity.HasIndex(e => e.FromId, "idx_FromId");

            entity.HasIndex(e => e.ToId, "idx_ToId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Phản Hồi");

            entity.HasOne(d => d.From).WithMany(p => p.MatchRequestFroms)
                .HasForeignKey(d => d.FromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MatchRequ__FromI__66603565");

            entity.HasOne(d => d.To).WithMany(p => p.MatchRequestTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MatchReque__ToId__6754599E");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Member__3214EC071B9C4F74");

            entity.ToTable("Member");

            entity.HasIndex(e => e.AccountId, "UQ__Member__349DA5A7629047EC").IsUnique();

            entity.HasIndex(e => e.Gender, "idx_Gender");

            entity.HasIndex(e => e.MajorId, "idx_MajorId");

            entity.HasIndex(e => e.UniversityId, "idx_UniversityId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.Introduce).HasDefaultValue("Tên này rất lười, chả để lại lời nói gì cả!");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Xác Thực");
            entity.Property(e => e.Surplus).HasDefaultValue(0);

            entity.HasOne(d => d.Account).WithOne(p => p.Member)
                .HasForeignKey<Member>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__AccountI__4D94879B");

            entity.HasOne(d => d.Major).WithMany(p => p.Members)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__MajorId__4F7CD00D");

            entity.HasOne(d => d.University).WithMany(p => p.Members)
                .HasForeignKey(d => d.UniversityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__Universi__4E88ABD4");
        });

        modelBuilder.Entity<MemberPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MemberPa__3214EC0790B97912");

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
                .HasConstraintName("FK__MemberPac__Membe__5FB337D6");

            entity.HasOne(d => d.Package).WithMany(p => p.MemberPackages)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPac__Packa__60A75C0F");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3214EC0705C6B0CD");

            entity.ToTable("Message");

            entity.HasIndex(e => e.SenderId, "idx_SenderId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Đã gửi");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__Convers__73BA3083");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__SenderI__74AE54BC");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Package__3214EC079502DB19");

            entity.ToTable("Package");

            entity.HasIndex(e => e.Code, "UQ__Package__A25C5AA763EE6BD9").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Picture__3214EC0722803DA9");

            entity.ToTable("Picture");

            entity.HasIndex(e => e.MemberId, "idx_MemberId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.Pictures)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Picture__MemberI__571DF1D5");
        });

        modelBuilder.Entity<University>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Universi__3214EC07D3CF5663");

            entity.ToTable("University");

            entity.HasIndex(e => e.Code, "UQ__Universi__A25C5AA751A7B15A").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Code).HasMaxLength(20);
        });

        modelBuilder.Entity<Verification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Verifica__3214EC0764FEF9E4");

            entity.ToTable("Verification");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Chưa Duyệt");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");

            entity.HasOne(d => d.Member).WithMany(p => p.Verifications)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Verificat__Membe__7F2BE32F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
