using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Ectensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            //Service interface and class added as scoped.(Alive until request is done)
            services.AddScoped<ITokenService, TokenService>();
            //Injection to use DataContext for other classes
            IServiceCollection serviceCollections = services.AddDbContext<DataContext>(options => {
                //Through _config we get access to connection string defined in .json file
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
