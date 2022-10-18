using BookStore.BL.BackgroundService;
using BookStore.BL.Interfaces;
using BookStore.BL.Kafka;
using BookStore.BL.Services;
using BookStore.DL.Interfaces;
using BookStore.DL.Repositories.MongoRepo;
using BookStore.DL.Repositories.MsSql;
using BookStore.Models.Models;

namespace BookStore.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IPersonRepository, PersonSqlRepository>();
            services.AddSingleton<IAuthorRepository, AuthorSqlRepository>();
            services.AddSingleton<IBookRepository, BookSqlRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeSqlRepository>();
            services.AddSingleton<IUserInfoRepository, UserInfoRepository>();
            services.AddSingleton<IPurchaseRepo, PurchaseRepo>();
            services.AddSingleton<IShoppingCartRepo, ShoppingCartRepo>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IAuthorService, AuthorService>();
            services.AddSingleton<IBookService, BookService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddSingleton<Producer<int, Book>>();
            services.AddSingleton<Producer<int, Person>>();
            services.AddHostedService<KafkaHostedService>();
            services.AddSingleton<Consumer<int, Book>>();
            services.AddSingleton<IPurchaseService, PurchaseService>();
            services.AddSingleton<IShoppingCartService, ShoppingCartService>();
            
            return services;
        }
    }
}
