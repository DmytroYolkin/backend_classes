namespace ex07.DTOs;

public record WorkoutDto(Guid Id, Guid ExerciseId, int Weight, int Reps, DateTime CreatedAt);
public record CreateWorkoutDto(Guid ExerciseId, int Weight, int Reps);
