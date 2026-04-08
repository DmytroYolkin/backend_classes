using ex07.DTOs;
using ex07.Exceptions;
using ex07.Models;
using ex07.Repositories;
using FluentValidation;

namespace ex07.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _repository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IValidator<CreateWorkoutDto> _validator;

    public WorkoutService(IWorkoutRepository repository, IExerciseRepository exerciseRepository, IValidator<CreateWorkoutDto> validator)
    {
        _repository = repository;
        _exerciseRepository = exerciseRepository;
        _validator = validator;
    }

    public async Task<List<WorkoutDto>> GetAllAsync()
    {
        var workouts = await _repository.GetAllAsync();
        return workouts.Select(x => new WorkoutDto(x.Id, x.ExerciseId, x.Weight, x.Reps, x.CreatedAt)).ToList();
    }

    public async Task<WorkoutDto> GetByIdAsync(Guid id)
    {
        var workout = await _repository.GetByIdAsync(id);
        if (workout is null) throw new ResourceNotFoundException("Workout", id);
        return new WorkoutDto(workout.Id, workout.ExerciseId, workout.Weight, workout.Reps, workout.CreatedAt);
    }

    public async Task<WorkoutDto> CreateAsync(CreateWorkoutDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var exercise = await _exerciseRepository.GetByIdAsync(dto.ExerciseId);
        if (exercise is null) throw new ResourceNotFoundException("Exercise", dto.ExerciseId);

        var workout = new Workout { Id = Guid.NewGuid(), ExerciseId = dto.ExerciseId, Weight = dto.Weight, Reps = dto.Reps, CreatedAt = DateTime.UtcNow };
        await _repository.AddAsync(workout);
        return new WorkoutDto(workout.Id, workout.ExerciseId, workout.Weight, workout.Reps, workout.CreatedAt);
    }

    public async Task DeleteAsync(Guid id)
    {
        var workout = await _repository.GetByIdAsync(id);
        if (workout is null) throw new ResourceNotFoundException("Workout", id);
        await _repository.DeleteAsync(workout);
    }
}
