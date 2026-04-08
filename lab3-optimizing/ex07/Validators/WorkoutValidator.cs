using FluentValidation;
using ex07.DTOs;

namespace ex07.Validators;

public class CreateWorkoutValidator : AbstractValidator<CreateWorkoutDto>
{
    public CreateWorkoutValidator()
    {
        RuleFor(x => x.ExerciseId).NotEmpty();
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.Reps).GreaterThan(0);
    }
}
