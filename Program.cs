using DBWeb;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Thêm service ( Dotnet bản dưới là Start Up).
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyBlogContext>(options =>
{
    string? connectString = configuration.GetConnectionString("MyBlogContext");
    options.UseSqlServer(connectString);
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

app.MapRazorPages();

app.Run();
