using ArticlesAPI.Models;
using FluentValidation;

namespace ArticlesAPI.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
