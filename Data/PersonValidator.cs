using FluentValidation;
using schedule_api.Models;

namespace schedule_api.Data
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(person => person.Name).NotEmpty().WithMessage("The name can't be blank");

        }
    }
}
