using ex07.DTOs;
using ex07.Exceptions;
using ex07.Models;
using ex07.Repositories;
using FluentValidation;

namespace ex07.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _repository;
    private readonly IValidator<CreateExerciseDto> _validator;

    public ExerciseService(IExerciseRepository repository, IValidator<CreateExerciseDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<List<ExerciseDto>> GetAllAsync()
    {
        var exercises = await _repository.GetAllAsync();
        return exercises.Select(x => new ExerciseDto(x.Id, x.Name, x.MuscleGroup)).ToList();
    }

    public async Task<ExerciseDto> GetByIdAsync(Guid id)
    {
        var exercise = await _repository.GetByIdAsync(id);
        if (exercise is null) throw new ResourceNotFoundException("Exercise", id);
        return new ExerciseDto(exercise.Id, exercise.Name, exercise.MuscleGroup);
    }

    public async Task<ExerciseDto> CreateAsync(CreateExerciseDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var exercise = new Exercise { Id = Guid.NewGuid(), Name = dto.Name, MuscleGroup = dto.MuscleGroup };
        await _repository.AddAsync(exercise);
        return new ExerciseDto(exercise.Id, exercise.Name, exercise.MuscleGroup);
    }

    public async Task DeleteAsync(Guid id)
    {
        var exercise = await _repository.GetByIdAsync(id);
        if (exercise is null) throw new ResourceNotFoundException("Exercise", id);
        await _repository.DeleteAsync(exercise);
    }
}
