using ex03_ef_postgresql.DTOs;
using ex03_ef_postgresql.Models;
using ex03_ef_postgresql.Repositories;
using FluentValidation;

namespace ex03_ef_postgresql.Services;

public class DestinationService : IDestinationService
{
    private readonly IDestinationRepository _repository;
    private readonly IValidator<Destination> _validator;

    public DestinationService(IDestinationRepository repository, IValidator<Destination> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<List<DestinationDto>> GetAllDestinationsAsync()
    {
        var destinations = await _repository.GetAllAsync();
        return destinations.Select(d => new DestinationDto(d.Id, d.Name)).ToList();
    }

    public async Task<DestinationDto> AddDestinationAsync(DestinationDto input)
    {
        var destination = new Destination { Name = input.Name };
        
        var validationResult = await _validator.ValidateAsync(destination);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _repository.AddAsync(destination);
        return new DestinationDto(destination.Id, destination.Name);
    }
}
