
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(op=>op.EnableAnnotations());
            builder.Services.AddDbContext<BookShopDBContext>(op => op.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("storeConnection")));
            builder.Services.AddScoped<UnitWork>();
            builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<BookShopDBContext>();

            builder.Services.AddAuthentication(
                option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                op =>
                {
                    op.SaveToken = true;
                    #region secretkey

                    string key = "Mohamed Salah is making this secret key using HmacSha256";
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                    #endregion
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = secretKey,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };

                }

                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
