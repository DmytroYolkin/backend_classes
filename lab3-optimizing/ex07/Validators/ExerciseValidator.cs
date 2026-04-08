using ex07.DTOs;
using FluentValidation;

namespace ex07.Validators;

public class ExerciseValidator : AbstractValidator<CreateExerciseDto>
{
    public ExerciseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.MuscleGroup).NotEmpty().MaximumLength(50);
    }
}
