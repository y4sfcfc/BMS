using BMS.DAL.AutoMappers;
using BMS.DAL.Data;
using BMS.DAL.Repository;
using BMS.DAL.Services.Implementations;
using BMS.DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.WEBUI.ServiceRegistrations
{
	public static class ServiceRegister
	{
		public static void Register(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddDbContext<BMSDbContext>(opts =>
			{
				opts.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"));

			});
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
			services.AddScoped(typeof(IBookService), typeof(BookService));
			services.AddScoped(typeof(IAuthorService), typeof(AuthorService));
			services.AddScoped(typeof(IAuthorContactService), typeof(AuthorContactService));
			services.AddScoped(typeof(IPublisherService), typeof(PublisherService));
		    services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
			
		}
	}
}
