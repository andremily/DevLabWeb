using DevLabWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true);
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IClientes, Clientes>();
builder.Services.AddSingleton<IProductos, Productos>();

builder.Services.AddCors(options =>
    {
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
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
