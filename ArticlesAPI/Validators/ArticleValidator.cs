using ArticlesAPI.Models;
using FluentValidation;

namespace ArticlesAPI.Validators
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Abstract).NotEmpty();
            RuleFor(x => x.PublicationDate).NotEmpty();
            RuleFor(x => x.Authors).NotEmpty().WithMessage("There must be at least one author.");
            RuleFor(x => x.ReadingEstimatedTime).GreaterThan(0).WithMessage("Reading estimated time should be greater than 0.");
        }
    }
}
