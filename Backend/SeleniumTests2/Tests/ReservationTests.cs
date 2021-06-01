using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace SeleniumTests2.Tests
{
    public class ReservationTests : TestsBase
    {
        public ReservationTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ReserveBikeShouldSucceed()
        {
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(GetUniqueString(), adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);

            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(station.Name);
            stationsPage.ReserveBike(bike.Id);

            stationsPage.HasBike(bike.Id).Should().BeFalse();
            stationsPage.ContainsSnackbar().Should().BeTrue();
            var reservationsPage = stationsPage.GoToReservations();
            reservationsPage.HasBike(bike.Id).Should().BeTrue();
        }

        [Fact]
        public async Task CancelReservationShouldSucceed()
        {
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(GetUniqueString(), adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);

            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(station.Name);
            stationsPage.ReserveBike(bike.Id);

            var reservationsPage = stationsPage.GoToReservations();
            reservationsPage.HasBike(bike.Id).Should().BeTrue();
            reservationsPage.CancelBike(bike.Id);
            reservationsPage.HasBike(bike.Id).Should().BeFalse();
            stationsPage = reservationsPage.GoToStations();
            stationsPage.OpenBikesList(station.Name);
            stationsPage.HasBike(bike.Id);
        }

        [Fact]
        public async Task RentReservedBikeShouldSucceed()
        {
            var adminToken = await Api.LogInAsAdmin();
            var station = await Api.AddStation(GetUniqueString(), adminToken);
            var bike = await Api.AddBike(station.Id, adminToken);

            var stationsPage = await LoginAsSomeUser();
            stationsPage.OpenBikesList(station.Name);
            stationsPage.ReserveBike(bike.Id);

            var reservationsPage = stationsPage.GoToReservations();
            reservationsPage.RentBike(bike.Id);
            reservationsPage.HasBike(bike.Id).Should().BeFalse();
            var rentalsPage = reservationsPage.GoToRentals();
            rentalsPage.HasBike(bike.Id);
        }
    }
}
