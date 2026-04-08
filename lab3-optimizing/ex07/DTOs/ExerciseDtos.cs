namespace ex07.DTOs;

public record ExerciseDto(Guid Id, string Name, string MuscleGroup);
public record CreateExerciseDto(string Name, string MuscleGroup);
public record UpdateExerciseDto(string Name, string MuscleGroup);
