using Step1_WebApi_Initials.Logger;
using Step1_WebApi_Initials.Models;

namespace Step1_WebApi_Initials;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // NOTE: global cors policy needed for JS and React frontends
        builder.Services.AddCors();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();
        

        //Dependency Injection for the controller class constructors
        builder.Services.AddSingleton<ILoggerProvider, InMemoryLoggerProvider>();
        builder.Services.AddSingleton<IMockupData, MockupData>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();


        // NOTE: global cors policy needed for JS and React frontends
        // the call to UseCors() must be done here, just before app.UseAuthorization();
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()); // allow credentials

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

