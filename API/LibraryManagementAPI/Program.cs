using LibraryManagementAPI.ConfigurationExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLibraryServices();

builder.Services.ConfigureLibraryDatabase(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();