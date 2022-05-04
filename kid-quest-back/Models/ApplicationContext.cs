using Microsoft.EntityFrameworkCore;

namespace KidQquest.Models;

public class ApplicationContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<ResetPasswordModel> ResetPasswords { get; set; }
    public DbSet<PreviewModel> Previews { get; set; }
    public DbSet<QuestModel> Quests { get; set; }
    public DbSet<QuestionModel> Questions { get; set; }
    public DbSet<QuestCategoryModel> QuestCategories { get; set; }
    public DbSet<QuestionTypeModel> QuestionTypes { get; set; }
    public DbSet<FactModel> Facts { get; set; }
    public DbSet<AnswerVariantModel> AnswerVariants { get; set; }
    public DbSet<AttemptModel> Attempts { get; set; }
    public DbSet<AwardTypeModel> AwardTypes { get; set; }
    public DbSet<UserAwardModel> UserAwards { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}