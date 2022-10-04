using BookStore.Models.Requests;
using FluentValidation;

namespace BookStore.Validators
{
    public class AddBookValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.AuthorId).NotNull().NotEmpty();
        }
    }
}
