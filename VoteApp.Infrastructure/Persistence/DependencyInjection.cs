using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VoteApp.Application.Commons.Interfaces;

namespace VoteApp.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastuctureService(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TestDB"));
            services.AddScoped<IAppDbContext, AppDbContext>();
            return services;
        }
    }
}
