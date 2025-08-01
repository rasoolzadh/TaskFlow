// TaskFlow.WebApp/Program.cs

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("TaskFlowAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7258");
});

// --- Add Cookie Authentication Services ---
builder.Services.AddAuthentication("TaskFlowCookieAuth")
    .AddCookie("TaskFlowCookieAuth", options =>
    {
        options.Cookie.Name = "TaskFlowCookieAuth";
        options.LoginPath = "/Account/Login"; // Redirect here if user is not authenticated
        options.AccessDeniedPath = "/Home/AccessDenied"; // Redirect here if user is not authorized
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- Add Authentication Middleware ---
// IMPORTANT: This must be placed AFTER UseRouting and BEFORE UseAuthorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
