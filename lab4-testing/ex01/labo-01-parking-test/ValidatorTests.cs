namespace labo_01_parking_test;

public class LicensePlateValidatorTests
{
    private readonly ILicensePlateValidator _validator = new LicensePlateValidator();

    [Theory]
    [InlineData("ABC-123")]
    [InlineData("ABC123")]
    [InlineData("XX-1234")]
    [InlineData("TEST-PLATE")]
    [InlineData("A1B2C3D")]
    public void IsValidLicensePlate_WithValidPlates_ReturnsTrue(string plate)
    {
        var result = _validator.IsValidLicensePlate(plate);
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("AB")]
    [InlineData("ABCDEFGHIJKLMNOPQRST1")]
    [InlineData("ABC@123")]
    [InlineData("ABC 123")]
    [InlineData("a?-123")]
    public void IsValidLicensePlate_WithInvalidPlates_ReturnsFalse(string? plate)
    {
        var result = _validator.IsValidLicensePlate(plate!);
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidLicensePlate_WithLowercasePlate_ReturnsTrue()
    {
        var result = _validator.IsValidLicensePlate("abc-123");
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidLicensePlate_WithMixedCasePlate_ReturnsTrue()
    {
        var result = _validator.IsValidLicensePlate("AbC-123");
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidLicensePlate_WithLeadingAndTrailingSpaces_ReturnsTrue()
    {
        var result = _validator.IsValidLicensePlate("  ABC-123  ");
        result.Should().BeTrue();
    }
}

public class CreateCarValidatorTests
{
    private readonly CreateCarValidator _validator;
    private readonly ILicensePlateValidator _licensePlateValidator;

    public CreateCarValidatorTests()
    {
        _licensePlateValidator = new LicensePlateValidator();
        _validator = new CreateCarValidator(_licensePlateValidator);
    }

    [Fact]
    public async Task Validate_WithValidCar_ReturnsSuccess()
    {
        var dto = new CreateCarDto("Toyota", "Corolla", "ABC-123", "Blue");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithInvalidLicensePlate_ReturnsFalse()
    {
        var dto = new CreateCarDto("Toyota", "Corolla", "INVALID@", "Blue");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Plate");
    }

    [Fact]
    public async Task Validate_WithEmptyBrand_ReturnsFalse()
    {
        var dto = new CreateCarDto("", "Corolla", "ABC-123", "Blue");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Brand");
    }

    [Fact]
    public async Task Validate_WithEmptyModel_ReturnsFalse()
    {
        var dto = new CreateCarDto("Toyota", "", "ABC-123", "Blue");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Model");
    }

    [Fact]
    public async Task Validate_WithEmptyPlate_ReturnsFalse()
    {
        var dto = new CreateCarDto("Toyota", "Corolla", "", "Blue");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Plate");
    }

    [Fact]
    public async Task Validate_WithEmptyColor_ReturnsFalse()
    {
        var dto = new CreateCarDto("Toyota", "Corolla", "ABC-123", "");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Color");
    }

    [Fact]
    public async Task Validate_WithTooLongBrand_ReturnsFalse()
    {
        var longBrand = new string('A', 51);
        var dto = new CreateCarDto(longBrand, "Corolla", "ABC-123", "Blue");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("ABC-123")]
    [InlineData("XX-1234")]
    [InlineData("TEST-PLATE")]
    public async Task Validate_WithVariousValidPlates_ReturnsSuccess(string plate)
    {
        var dto = new CreateCarDto("Toyota", "Corolla", plate, "Blue");
        var result = await _validator.ValidateAsync(dto);
        result.IsValid.Should().BeTrue();
    }
}
