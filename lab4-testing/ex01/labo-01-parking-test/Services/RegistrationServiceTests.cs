namespace labo_01_parking_test.Services;

public class RegistrationServiceTests
{
    private readonly FakeCarRepository _carRepository;
    private readonly FakeRegistrationRepository _registrationRepository;
    private readonly IPricingService _pricingService;
    private readonly IMapper _mapper;

    public RegistrationServiceTests()
    {
        _carRepository = new FakeCarRepository();
        _registrationRepository = new FakeRegistrationRepository();
        _pricingService = new PricingService();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    }

    [Fact]
    public async Task StopParking_WithValidRegistration_SendsEmailWithCorrectData()
    {
        var mockEmailService = new global::Moq.Mock<IEmailService>();
        
        var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", Plate = "ABC-123", Color = "Blue" };
        var registration = new Registration 
        { 
            Id = 1, 
            Plate = "ABC-123",
            CarId = 1,
            Start = DateTime.UtcNow.AddHours(-1),
            End = null,
            TotalPrice = 0m,
            IsFinished = false
        };

        await _carRepository.AddAsync(car);
        await _registrationRepository.AddAsync(registration);

        var service = new RegistrationService(
            _registrationRepository,
            _carRepository,
            _pricingService,
            mockEmailService.Object,
            _mapper);

        var result = await service.StopParkingAsync(1);

        result.Should().NotBeNull();
        result!.IsFinished.Should().BeTrue();
        mockEmailService.Verify(
            x => x.SendEmailAsync(
                global::Moq.It.Is<EmailMessage>(msg => 
                    msg.To == "owner@car-ABC-123.local" &&
                    msg.Subject == "Parking Session Completed" &&
                    msg.Body.Contains("ABC-123")),
                global::Moq.It.IsAny<CancellationToken>()),
            global::Moq.Times.Once);
    }

    [Fact]
    public async Task StopParking_WithValidRegistration_CalculatesPriceCorrectly()
    {
        var mockEmailService = new global::Moq.Mock<IEmailService>();
        
        var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", Plate = "ABC-123", Color = "Blue" };
        var startTime = DateTime.UtcNow.AddHours(-2);
        var registration = new Registration 
        { 
            Id = 1, 
            Plate = "ABC-123",
            CarId = 1,
            Start = startTime,
            End = null,
            TotalPrice = 0m,
            IsFinished = false
        };

        await _carRepository.AddAsync(car);
        await _registrationRepository.AddAsync(registration);

        var service = new RegistrationService(
            _registrationRepository,
            _carRepository,
            _pricingService,
            mockEmailService.Object,
            _mapper);

        var result = await service.StopParkingAsync(1);

        result.Should().NotBeNull();
        result!.TotalPrice.Should().BeGreaterThanOrEqualTo(5.00m);
    }

    [Fact]
    public async Task StopParking_WithAlreadyFinishedRegistration_ReturnsNull()
    {
        var mockEmailService = new global::Moq.Mock<IEmailService>();
        
        var car = new Car { Id = 1, Brand = "Toyota", Model = "Corolla", Plate = "ABC-123", Color = "Blue" };
        var registration = new Registration 
        { 
            Id = 1, 
            Plate = "ABC-123",
            CarId = 1,
            Start = DateTime.UtcNow.AddHours(-1),
            End = DateTime.UtcNow,
            TotalPrice = 2.50m,
            IsFinished = true
        };

        await _carRepository.AddAsync(car);
        await _registrationRepository.AddAsync(registration);

        var service = new RegistrationService(
            _registrationRepository,
            _carRepository,
            _pricingService,
            mockEmailService.Object,
            _mapper);

        var result = await service.StopParkingAsync(1);

        result.Should().BeNull();
        mockEmailService.Verify(
            x => x.SendEmailAsync(global::Moq.It.IsAny<EmailMessage>(), global::Moq.It.IsAny<CancellationToken>()),
            global::Moq.Times.Never);
    }

    [Fact]
    public async Task StopParking_WithNonExistentRegistration_ReturnsNull()
    {
        var mockEmailService = new global::Moq.Mock<IEmailService>();

        var service = new RegistrationService(
            _registrationRepository,
            _carRepository,
            _pricingService,
            mockEmailService.Object,
            _mapper);

        var result = await service.StopParkingAsync(999);

        result.Should().BeNull();
        mockEmailService.Verify(
            x => x.SendEmailAsync(global::Moq.It.IsAny<EmailMessage>(), global::Moq.It.IsAny<CancellationToken>()),
            global::Moq.Times.Never);
    }
}
