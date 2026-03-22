

namespace Howest.lab2.ex01_ef_mysql.Validation
{
    public class PersonValidation : AbstractValidator<Person>
    {
        public PersonValidation()
        {
            RuleFor(person => person.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(person => person.Age)
                .InclusiveBetween(0, 119);

            RuleFor(person => person.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
