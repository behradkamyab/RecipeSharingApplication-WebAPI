
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;

namespace DataAccessLayerLibrary.DataPersistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<RecipeModel>(entity =>
            {
                entity.Property(e => e.Name)
                .HasColumnType("citext");
                entity.Property(e => e.Description)
                .HasColumnType("citext");
                entity.Property(r => r.Category)
                .HasColumnType("citext");
                entity.Property(r => r.Cuisine)
                .HasColumnType("citext");
                entity.HasOne(r => r.User) // has one user
                .WithMany(u => u.CreatedRecipe) // every user has many recipes
                .HasForeignKey(r => r.UserId) // FK is userid in recipe model
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<IngredientRecipe>()
                .HasOne(i => i.Recipe) // has one recipe
                .WithMany(r => r.Ingredients) // every recipe has many ingredients
                .HasForeignKey(i => i.RecipeId) // FK is recipeId in ingredient model
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InstructionRecipe>()
                .HasOne(i => i.Recipe) // has one recipe
                .WithMany(r => r.Instructions) // every recipe has many instructions
                .HasForeignKey(i => i.RecipeId) // Fk is recipeId in instruction model
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<UserFavoriteRecipe>(entity =>
            {

                entity.HasKey(uf => new { uf.UserId, uf.RecipeId });
                entity.HasOne(uf => uf.User)
                .WithMany(u => u.FavoriteRecipes)
                .HasForeignKey(uf => uf.UserId);

                // for many-to-many you must break it into two one-to-many relationships
                entity.HasOne(uf => uf.Recipe)
                .WithMany()
                .HasForeignKey(uf => uf.RecipeId);
            });



            builder.Entity<UserProfileModel>(entity =>
            {
                entity.HasOne(up => up.User)  // one-to-one
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfileModel>(u => u.UserId);
            });


            builder.Entity<Follow>(entity =>
            {
                entity.HasKey(f => new { f.FollowerId, f.FollowedId });

                entity.HasOne(f => f.FollowerUsers)
                .WithMany(u => u.Followings)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.FollowedUsers)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable("Follows");

            });


            builder.Entity<Like>(entity =>
            {
                entity.HasKey(l => new { l.UserId, l.RecipeId });

                entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne<RecipeModel>()
                .WithMany(r => r.Likes)
                .HasForeignKey(l => l.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }

        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<IngredientRecipe> Ingredients { get; set; }
        public DbSet<InstructionRecipe> Instructions { get; set; }
        public DbSet<UserFavoriteRecipe> UserFavoriteRecipes { get; set; }
        public DbSet<UserProfileModel> UserProfiles { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}
