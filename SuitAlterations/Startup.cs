using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuitAlterations.Core.Data;
using SuitAlterations.Core.Services;
using SuitAlterations.ServiceBusTopic;
using SuitAlterations.ServiceBusTopic.Notifications;

namespace SuitAlterations {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services) {
			services.AddRazorPages();
			services.AddServerSideBlazor();
			
			services.AddMediatR(typeof(Startup));
			services.AddAutoMapper(typeof(Startup));
			
			services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(Configuration["Database:ConnectionString"]));
			services.AddScoped<ISuitAlterationRepository, SuitAlterationRepository>();
			services.AddScoped<CustomerRepository>();
			
			services.AddSingleton<ISuitAlterationTopicSubscription, SuitAlterationTopicSubscription>();
			services.AddTransient<ISuitAlterationsService, SuitAlterationsService>();
			services.AddTransient<ISuitAlterationNotificationService, SuitAlterationNotificationService>();
			services.AddApplicationInsightsTelemetry();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints => {
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});

			var topicSubscription = app.ApplicationServices.GetService<ISuitAlterationTopicSubscription>();
			topicSubscription.RegisterMessageReceivingHandler();
		}
	}
}