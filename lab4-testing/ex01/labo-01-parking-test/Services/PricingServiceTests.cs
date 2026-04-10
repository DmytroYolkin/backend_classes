namespace labo_01_parking_test.Services;

public class PricingServiceTests
{
    private readonly IPricingService _pricingService = new PricingService();

    [Fact]
    public void CalculatePrice_WithOneHourParking_ReturnsCorrectPrice()
    {
        var start = DateTime.UtcNow;
        var end = start.AddHours(1);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(2.50m);
    }

    [Fact]
    public void CalculatePrice_WithTwoHoursParking_ReturnsCorrectPrice()
    {
        var start = DateTime.UtcNow;
        var end = start.AddHours(2);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(5.00m);
    }

    [Fact]
    public void CalculatePrice_WithThirtyMinutesParking_ReturnsCeiledPrice()
    {
        var start = DateTime.UtcNow;
        var end = start.AddMinutes(30);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(2.50m);
    }

    [Fact]
    public void CalculatePrice_WithNinetyMinutesParking_ReturnsCeiledPrice()
    {
        var start = DateTime.UtcNow;
        var end = start.AddMinutes(90);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(5.00m);
    }

    [Fact]
    public void CalculatePrice_WithTenMinutesParking_ReturnsCeiledPrice()
    {
        var start = DateTime.UtcNow;
        var end = start.AddMinutes(10);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(2.50m);
    }

    [Fact]
    public void CalculatePrice_WithOneSecondParking_ReturnsCeiledPrice()
    {
        var start = DateTime.UtcNow;
        var end = start.AddSeconds(1);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(2.50m);
    }

    [Fact]
    public void CalculatePrice_WithSixHoursParking_ReturnsCorrectPrice()
    {
        var start = DateTime.UtcNow;
        var end = start.AddHours(6);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(15.00m);
    }

    [Fact]
    public void CalculatePrice_WhenEndBeforeStart_ReturnsZero()
    {
        var start = DateTime.UtcNow;
        var end = start.AddHours(-1);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(0m);
    }

    [Fact]
    public void CalculatePrice_WhenStartEqualsEnd_ReturnsZero()
    {
        var start = DateTime.UtcNow;
        var end = start;

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be(0m);
    }

    [Theory]
    [InlineData(1, 2.50)]
    [InlineData(2, 5.00)]
    [InlineData(3, 7.50)]
    [InlineData(4, 10.00)]
    [InlineData(5, 12.50)]
    public void CalculatePrice_WithMultipleHours_ReturnsCorrectPrice(int hours, double expectedPrice)
    {
        var start = DateTime.UtcNow;
        var end = start.AddHours(hours);

        var price = _pricingService.CalculatePrice(start, end);
        price.Should().Be((decimal)expectedPrice);
    }
}
