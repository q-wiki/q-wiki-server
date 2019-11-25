using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
                        var userRepo = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var user = await userRepo.SingleOrDefaultAsync(u => u.Id == context.Principal.Identity.Name);
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
                    ValidateAudience = false
                };
            });

            services.AddSingleton<INotificationService>(new NotificationService(Configuration.GetConnectionString("NotificationHub")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton(new AuthService(Configuration.GetConnectionString("FirebaseAuth")));
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IMinigameRepository, MinigameRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddSingleton<IRepository<Category, string>, Repository<Category, string>>();
            services.AddScoped<IRepository<Friend, string>, Repository<Friend, string>>();
            services.AddScoped<IMinigameService, MultipleChoiceMinigameService>();
            services.AddScoped<IMinigameService, SortingMinigameService>();
            services.AddSingleton<CategoryCacheService, CategoryCacheService>();
            //services.AddScoped<IMinigameService, BlurryImageMinigameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.ApplicationServices.GetService<DataContext>().Database.Migrate();
        }
    }
}
