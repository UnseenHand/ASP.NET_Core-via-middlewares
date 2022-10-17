var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseStaticFiles();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<UserListMiddleware>();

app.Run();