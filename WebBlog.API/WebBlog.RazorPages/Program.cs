using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
namespace WebBlog.RazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        //opt.Audience = builder.Configuration["Jwt:Audience"]; // Cấu hình cấp cao, sẽ tự đông gán audience nếu như không có trong token
            //        //Authority là url chỉ định IDP - nơi phát hành token (issuer)
            //        //MetadataAddress là url trỏ trực tiếp đến file OIDC 
            //        opt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = true,
            //            ValidAudience = builder.Configuration["Jwt:Audience"],
            //        };
            //    });
            builder.Services.AddHttpClient();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
