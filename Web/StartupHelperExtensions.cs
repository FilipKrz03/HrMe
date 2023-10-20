using Application;
using FluentValidation;
using Infrastructure;
using System.Reflection;

namespace Web
{
    internal static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            var connectionString = builder.Configuration["Connection_String_HrMe"];

            builder.Services.AddAplication();
            builder.Services.AddInfrastucture();

            ValidatorOptions.Global.LanguageManager.Enabled = false;

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddlewareAplication();

            app.MapControllers();

            return app;
        }

    }
}
