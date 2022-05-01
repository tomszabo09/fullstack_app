using BV3N92_HFT_2021221.Data;
using BV3N92_HFT_2021221.Endpoint.Services;
using BV3N92_HFT_2021221.Logic;
using BV3N92_HFT_2021221.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Endpoint
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<DbContext, ParliamentAdministrationDbContext>();

            services.AddSingleton<IParliamentRepository, ParliamentRepository>();
            services.AddSingleton<IPartyRepository, PartyRepository>();
            services.AddSingleton<IPartyMemberRepository, PartyMemberRepository>();

            services.AddSingleton<IParliamentLogic, ParliamentLogic>();
            services.AddSingleton<IPartyLogic, PartyLogic>();
            services.AddSingleton<IPartyMemberLogic, PartyMemberLogic>();

            services.AddSignalR();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BV3N92_HFT_2021221.Endpoint", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BV3N92_HFT_2021221.Endpoint v1"));
            }

            app.UseCors(x => x.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:63555"));

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/hub");
            });
        }
    }
}
