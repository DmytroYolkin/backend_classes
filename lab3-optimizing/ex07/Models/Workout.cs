namespace ex07.Models;

public class Workout
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public int Weight { get; set; }
    public int Reps { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Exercise? Exercise { get; set; }
}
