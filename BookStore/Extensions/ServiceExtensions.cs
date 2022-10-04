using BookStore.BL.Interfaces;
using BookStore.BL.Services;
using BookStore.DL.Interfaces;
using BookStore.DL.Repositories.InMemotyRepositories;
using BookStore.DL.Repositories.MsSql;

namespace BookStore.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IPersonRepository, UserInMemoryRepository>();
            services.AddSingleton<IAuthorRepository, AuthorSqlRepository>();
            services.AddSingleton<IBookRepository, BookSqlRepository>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IPersonService, PersonService>();
            services.AddSingleton<IAuthorService, AuthorService>();
            services.AddSingleton<IBookService, BookService>();

            return services;
        }
    }
}
