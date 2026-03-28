using ex03_ef_postgresql.DTOs;
using ex03_ef_postgresql.Repositories;

namespace ex03_ef_postgresql.Services;

public class GuideService : IGuideService
{
    private readonly IGuideRepository _repository;

    public GuideService(IGuideRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<object>> GetAllGuidesAsync(bool includeTours)
    {
        var guides = await _repository.GetAllAsync(includeTours);

        if (includeTours)
        {
            return guides.Select(g => (object)new GuideWithToursDto(
                g.Id, 
                g.Name, 
                g.Tours.Select(t => new TourDto(t.Id, t.Title)).ToList()
            )).ToList();
        }

        return guides.Select(g => (object)new GuideDto(g.Id, g.Name)).ToList();
    }

    public async Task<object?> GetGuideByIdAsync(int id, bool includeTours)
    {
        var guide = await _repository.GetByIdAsync(id, includeTours);
        if (guide == null) return null;

        if (includeTours)
        {
            return new GuideWithToursDto(
                guide.Id, 
                guide.Name, 
                guide.Tours.Select(t => new TourDto(t.Id, t.Title)).ToList()
            );
        }

        return new GuideDto(guide.Id, guide.Name);
    }
}
