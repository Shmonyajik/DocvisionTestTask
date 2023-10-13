using FluentValidation;
using LettersRegistration.WebAPI;
using LettersRegistration.WebAPI.DAL;
using LettersRegistration.WebAPI.Domain;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

RegisterServices(builder.Services);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var app = builder.Build();

Configure(app);

app.ConfigureApi();
   
app.Run();


void RegisterServices(IServiceCollection services)
{

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    
    services.AddDbContext<LettersDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    services.AddScoped<IBaseRepository<Letter>, LetterRepository>();
    services.AddScoped<IValidator<Letter>, LetterValidator>();
}

void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSerilogRequestLogging();
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LettersDbContext>();
        db.Database.EnsureCreated();
    }
    app.UseHttpsRedirection();
}