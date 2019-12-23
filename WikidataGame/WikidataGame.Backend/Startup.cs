using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;
using WikidataGame.Backend.Validators;

namespace WikidataGame.Backend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Wikidata Game API", Version = "v1" });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile));

                c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            if (!string.IsNullOrWhiteSpace(Configuration.GetConnectionString("SQL")))
            {
                services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("SQL"))
                    .UseLazyLoadingProxies());
            }
            else
            {
                //services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb").UseLazyLoadingProxies());
                services.AddDbContext<DataContext>(x => x.UseSqlite("Filename=qwiki.db").UseLazyLoadingProxies());
            }
            
            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AuthSecret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                        var user = await userManager.GetUserAsync(context.Principal);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            //configure identity
            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<DataContext>()
                .AddUserManager<UserManager<User>>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.RequireUniqueEmail = false;
            });
            services.AddSingleton((provider) => new MapperConfiguration(cfg =>
                cfg.AddProfile(new AutomapperProfile(provider))
            ).CreateMapper());
            services.AddTransient<IUserValidator<User>, UserValidator>();
            services.AddSingleton<INotificationService>(new NotificationService(Configuration.GetConnectionString("NotificationHub")));
            services.AddScoped<UserManager<User>>();
            services.AddSingleton(new AuthService(Configuration.GetConnectionString("GoogleClientSecret")));
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IMinigameRepository, MinigameRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IRepository<Category, Guid>, Repository<Category, Guid>>();
            services.AddScoped<IRepository<Friend, Guid>, Repository<Friend, Guid>>();
            services.AddScoped<IRepository<GameRequest, Guid>, Repository<GameRequest, Guid>>();
            services.AddScoped<IMinigameService, MultipleChoiceMinigameService>();
            services.AddScoped<IMinigameService, SortingMinigameService>();
            services.AddSingleton<CategoryCacheService, CategoryCacheService>();
            //services.AddScoped<IMinigameService, BlurryImageMinigameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wikidata Game API V1");
            });

            app.Run(async (context) => await Task.Run(() => context.Response.Redirect("/swagger")));

            var dataContext = app.ApplicationServices.GetService<DataContext>();
            if (dataContext.Database.IsSqlite())
            {
                //SQLite is missing major migration functionality, but is only used for testing purposes
                dataContext.Database.EnsureCreated();
            }
            else
            {
                dataContext.Database.Migrate();
            }

            await app.ApplicationServices.GetService<CategoryCacheService>().InitializeAsync();
        }
    }
}
