
using DVG.CRM.XeCung.InfrastructureLayer.Databases.SqlDB.Commands;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.SqlDB.Queries;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Authentication;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Users;
using DVG.CRM.XeCung.ApplicationLayer.Cachings;
using DVG.CRM.XeCung.ApplicationLayer.Interfaces;
using DVG.CRM.XeCung.ApplicationLayer.Repositories;
using DVG.CRM.XeCung.DomainLayer.Repositories;
using DVG.CRM.XeCung.InfrastructureLayer.Caching;
using DVG.CRM.XeCung.InfrastructureLayer.Caching.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.Base.DAL.Interfaces;
using DVG.CRM.XeCung.InfrastructureLayer.IoC;
using DVG.CRM.XeCung.InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVG.CRM.XeCung.InfrastructureLayer.Databases.SqlDB;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Customer;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Videos;
using DVG.CRM.XeCung.ApplicationLayer.AppServices.Expenditure;

namespace DVG.CRM.XeCung.WebAPI.Config
{
    public class IoC
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ICached, StackExchangeRedisCached>();
            services.AddTransient<IUserTokenCache, UserTokenCache>();
            services.AddSingleton<IExpenditureCache, ExpenditureCache>();
            // application services
            services.AddTransient<IAuthenticationAppService, CustomCookieAuthenticationAppService>();
            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<ICustomerAppService, CustomerAppService>();
            services.AddTransient<IVideoAppService, VideoAppService>();
            services.AddTransient<IExpenditureAppService, ExpenditureAppService>();

            // domain services
            services.AddTransient(typeof(ICommandDal<>), typeof(SqlCommandDal<>));
            services.AddTransient(typeof(IDtoQueryDal<>), typeof(SqlDtoQueryDal<>));
            services.AddTransient(typeof(IDtoQueryDal<,>), typeof(SqlDtoQueryDal<,>));

            services.AddTransient(typeof(ICommandDal<,>), typeof(SqlCommandDal<,>));
            services.AddTransient(typeof(IEntityQueryDal<>), typeof(SqlEntityQueryDal<>));
            services.AddTransient(typeof(IEntityQueryDal<,>), typeof(SqlEntityQueryDal<,>));

            //repository
            services.AddTransient<IUserRespository, UserRespository>();
            services.AddTransient<IVideoRepository, VideoRepository>();
            services.AddTransient<ICustomerRespository, CustomerRespository>();
            // unit of work
            services.AddTransient<IUnitOfWork, SqlDbUnitOfWork>();

            DVGServiceLocator.SetLocatorProvider(services.BuildServiceProvider());
        }
    }
}
