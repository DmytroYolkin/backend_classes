using ex03_ef_postgresql.DTOs;
using ex03_ef_postgresql.Models;
using ex03_ef_postgresql.Repositories;
using FluentValidation;

namespace ex03_ef_postgresql.Services;

public class TravelerService : ITravelerService
{
    private readonly ITravelerRepository _travelerRepository;
    private readonly IDestinationRepository _destinationRepository;
    private readonly IValidator<Traveler> _validator;

    public TravelerService(ITravelerRepository travelerRepository, 
                           IDestinationRepository destinationRepository,
                           IValidator<Traveler> validator)
    {
        _travelerRepository = travelerRepository;
        _destinationRepository = destinationRepository;
        _validator = validator;
    }

    public async Task<List<TravelerDto>> GetAllTravelersAsync()
    {
        var travelers = await _travelerRepository.GetAllAsync();
        return travelers.Select(t => new TravelerDto(t.Id, t.FullName, t.Passport?.PassportNumber)).ToList();
    }

    public async Task<TravelerDto?> GetTravelerByIdAsync(int id)
    {
        var traveler = await _travelerRepository.GetByIdAsync(id);
        if (traveler == null) return null;
        
        return new TravelerDto(traveler.Id, traveler.FullName, traveler.Passport?.PassportNumber);
    }

    public async Task<TravelerDto> AddTravelerAsync(TravelerInputDto input)
    {
        if (await _travelerRepository.ExistsByPassportNumberAsync(input.PassportNumber))
        {
            throw new ArgumentException("Passport number already exists.");
        }

        var traveler = new Traveler
        {
            FullName = input.FullName,
            Passport = new Passport { PassportNumber = input.PassportNumber }
        };

        var validationResult = await _validator.ValidateAsync(traveler);
        if (!validationResult.IsValid)
        {
             throw new ValidationException(validationResult.Errors);
        }

        await _travelerRepository.AddAsync(traveler);

        return new TravelerDto(traveler.Id, traveler.FullName, traveler.Passport?.PassportNumber);
    }

    public async Task<TravelerDetailDto?> AddTravelerToDestinationAsync(int travelerId, int destinationId)
    {
        var traveler = await _travelerRepository.GetByIdAsync(travelerId);
        if (traveler == null) return null;

        var destination = await _destinationRepository.GetByIdAsync(destinationId);
        if (destination == null) return null; // Or throw EntityNotFoundException

        if (!traveler.Destinations.Any(d => d.Id == destinationId))
        {
            traveler.Destinations.Add(destination);
            await _travelerRepository.UpdateAsync(traveler);
        }

        return new TravelerDetailDto(
            traveler.Id, 
            traveler.FullName, 
            traveler.Passport?.PassportNumber,
            traveler.Destinations.Select(d => new DestinationDto(d.Id, d.Name)).ToList()
        );
    }
}
