using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ServicesLibrary.Generators;
using DataAccessLayerLibrary.Interfaces;
using ServicesLibrary.Interfaces;
using DataAccessLayerLibrary.Repositories;
using ServicesLibrary.Managers;
using DataAccessLayerLibrary.DataPersistence;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;
using Microsoft.AspNetCore.ResponseCompression;
using RecipeSharingWebApi.Hubs;


namespace RecipeSharingWebApi
{
    // no logger , no cqrs , no mediatr , No notification
    // posgtress , User Identity , different design patterns (repo, strategy, dependency injection ) , clean architecture, EF CORE, web api
    // user and recipes => favorite lists , feed , Filter and search based on name , cuisine , categories , profile managment (followers , followings , favorite recipes)
    // ,JWT , error handling , DAL
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //add EF CORE db context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("RecipeDB")); //appsettings json

            });
            // add ASPNET core identity for users
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            // add response compression and using the octet stream to have the best connectivity for signalR (as optimize as possible)
            builder.Services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .SetIsOriginAllowed((host) => true) // Allow any origin
                          .AllowCredentials(); // Allow credentials (cookies, headers)
                });
            });
            //builder.Services.AddOpenApi();

            // aspnet core identity authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // setup jwt token
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                    // Configure SignalR to use the JWT token for authentication
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();
            

            // user manager service to manage users such as login , register , add recipe to user  favorite list, follow,unfollow , profile and ...
            builder.Services.AddScoped<IUserManager, UserManager>();
            // user repository for talking the user-related tables in db
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRecipeRepository, RecipeModelRepository>();
            //recipe manager for crud operations for recipe object, crud operation for ingredient , instructions
            builder.Services.AddScoped<IRecipeManager, RecipeManager>();

            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationManager, NotificationManager>();
            // create jwt token for every users authentication
            builder.Services.AddSingleton<JwtProvider>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                //app.UseSwaggerUI(options =>
                //{
                //    options.SwaggerEndpoint("/openapi/v1.json", "Recipe sharing API");

                //});
            }
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<NotificationHub>("notificationHub");
            app.MapControllers();
            


            app.Run();  
           
        }
    }
}
