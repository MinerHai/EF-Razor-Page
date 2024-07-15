using Microsoft.Extensions.DependencyInjection;
using DBWeb;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var mailSetting = configuration.GetSection("MailSetting");

// Thêm service ( Dotnet bản dưới là Start Up).
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyBlogContext>(options =>
{
    string? connectString = configuration.GetConnectionString("MyBlogContext");
    options.UseSqlServer(connectString);
});
builder.Services.Configure<MailSetting>(mailSetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();

builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MyBlogContext>()
                .AddDefaultTokenProviders();
// builder.Services.AddDefaultIdentity<AppUser>().AddEntityFrameworkStores<MyBlogContext>().AddDefaultTokenProviders();



// Truy cập IdentityOptions
builder.Services.Configure<Microsoft.AspNetCore.Identity.IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

});


var app = builder.Build();

// Định nghĩa địa chỉ https
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();

app.Run();
