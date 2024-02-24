using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Repositories;
using Notes.Infraestructure.Infraestructure;

namespace Notes.Infraestructure;

public static class ServiceCollectionExtension 
{
    public static IServiceCollection AddNotesInfraestructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()) );
        services.AddScoped<INoteRepository, EfNoteRepository>();
        services.AddDbContext<NotesDbContext>(options => {
            options.UseInMemoryDatabase("appdb");
        });
        return services;
    }
}