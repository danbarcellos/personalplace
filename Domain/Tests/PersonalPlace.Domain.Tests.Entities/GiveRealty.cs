using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.Tests.Base;
using PersonalPlace.Domain.Common.ValueObjects;
using PersonalPlace.Domain.Entities.Catalog;
using PersonalPlace.Domain.Entities.Component;

namespace PersonalPlace.Domain.Tests.Entities
{
    [TestClass]
    public class GiveRealty : IntegratedBaseIsolatedExecutionTestClass
    {
        #region Test User Values

        private readonly Guid _clientId = new Guid("95F9F8E4-2F30-49C8-821F-DAC984BDE37C");
        private readonly string _addressDetail = "Realty address detail";
        private readonly string _description = "Realty description";
        private readonly bool _furnished = true;
        private readonly bool _disabilityAccess = true;
        private readonly int _totalRooms = 4;
        private readonly int _totalSuites = 3;
        private readonly int _age = 50;
        private readonly decimal _rentValue = 1000.50m;
        private readonly decimal _saleValue = 1000000.50m;
        private readonly string _addrLine = "Address line";
        private readonly string _addrNeighborhood = "Address neighborhood";
        private readonly string _addrPopulatedPlace = "Address populated place";
        private readonly string _addrPostcode = "9990000";
        private readonly string _addrAdminDivision1 = "Address administrative division one";
        private readonly string _addrAdminDivision2 = "Address administrative division two";
        private readonly string _addrState = "Address state";
        private readonly string _addrCountryRegion = "Address country region";
        private readonly string _addrCountry = "Address country";
        private readonly float _addrCoordLatitude = -89;
        private readonly float _addrCoordLongitude = 179;
        private readonly string _floorDescription = "Floorplan description";
        private readonly double _floorDimension = 128;
        private readonly MeasureUnit _floorMesureUnit = MeasureUnit.SquareMeters;
        private readonly Guid _amenityTypeId = new Guid("0E2F0712-0C41-4AEB-A7D3-01D4D380BEA5");
        private readonly string _amenityDescription = "Amenity description";

        #endregion

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            StartEnvironment();
        }

        [TestMethod]
        [TestCategory("Domain.Entities")]
        [TestCategory("Integrated")]
        public void WhenSaveRealtyThenICanRecover()
        {
            //arrange
            var uow = Container.Resolve<IUnitOfWork>();
            var realtyRepository = Container.Resolve<IRepository<Realty>>();
            var realty = GetRealty();

            //act
            realtyRepository.SaveAll(realty);
            uow.Clear();
            var recoveredRealty = realtyRepository.FindOne(realty.Id);
            var recoveredAddress = recoveredRealty?.Address;
            var recoveredFloorplan = recoveredRealty?.Floorplans.FirstOrDefault();
            var recoveredAmenity = recoveredRealty?.Amenities.FirstOrDefault();
            var recoveredCoordinate = recoveredAddress?.Coordinate;

            //assert
            Assert.IsNotNull(recoveredRealty);
            Assert.AreEqual(_addressDetail, recoveredRealty.AddressDetail);
            Assert.AreEqual(_description, recoveredRealty.Description);
            Assert.AreEqual(_furnished, recoveredRealty.Furnished);
            Assert.AreEqual(_disabilityAccess, recoveredRealty.DisabilityAccess);
            Assert.AreEqual(_totalRooms, recoveredRealty.TotalRooms);
            Assert.AreEqual(_totalSuites, recoveredRealty.TotalSuites);
            Assert.AreEqual(_age, recoveredRealty.Age);
            Assert.AreEqual(_rentValue, recoveredRealty.RentValue);
            Assert.AreEqual(_saleValue, recoveredRealty.SaleValue);
            Assert.IsNotNull(recoveredAddress);
            Assert.AreEqual(_addrNeighborhood, recoveredAddress.Neighborhood);
            Assert.AreEqual(_addrPopulatedPlace, recoveredAddress.PopulatedPlace);
            Assert.AreEqual(_addrPostcode, recoveredAddress.Postcode);
            Assert.AreEqual(_addrAdminDivision1, recoveredAddress.AdminDivision1);
            Assert.AreEqual(_addrAdminDivision2, recoveredAddress.AdminDivision2);
            Assert.AreEqual(_addrState, recoveredAddress.State);
            Assert.AreEqual(_addrCountryRegion, recoveredAddress.CountryRegion);
            Assert.AreEqual(_addrCountry, recoveredAddress.Country);
            Assert.IsNotNull(recoveredCoordinate);
            Assert.AreEqual(_addrCoordLatitude, recoveredCoordinate.Latitude);
            Assert.AreEqual(_addrCoordLongitude, recoveredCoordinate.Longitude);
            Assert.IsNotNull(recoveredFloorplan);
            Assert.AreEqual(_floorDescription, recoveredFloorplan.Description);
            Assert.AreEqual(_floorDimension, recoveredFloorplan.Dimension);
            Assert.AreEqual(_floorMesureUnit, recoveredFloorplan.MesureUnit);
            Assert.IsNotNull(recoveredAmenity);
            Assert.AreEqual(_amenityDescription, recoveredAmenity.Description);
            Assert.AreEqual(_amenityTypeId, recoveredAmenity.AmenityType.Id);
        }

        private Realty GetRealty()
        {
            var client = GetClient();
            var amenityType = GetAmenityType();
            var address = new Address(_addrLine,
                                      _addrNeighborhood,
                                      _addrPostcode,
                                      _addrState,
                                      _addrCountry,
                                      new GeoCoordinate(_addrCoordLatitude, _addrCoordLongitude))
            {
                PopulatedPlace = _addrPopulatedPlace,
                AdminDivision1 = _addrAdminDivision1,
                AdminDivision2 = _addrAdminDivision2,
                CountryRegion = _addrCountryRegion
            };

            var realty = new Realty(client, address, _description)
            {
                AddressDetail = _addressDetail,
                Furnished = _furnished,
                DisabilityAccess = _disabilityAccess,
                TotalRooms = _totalRooms,
                TotalSuites = _totalSuites,
                Age = _age,
                RentValue = _rentValue,
                SaleValue = _saleValue
            };
            var floorplan = new Floorplan(realty, _floorDimension, _floorDescription, _floorMesureUnit) {};
            var amenity = new Amenity(realty, amenityType, _amenityDescription);

            realty.AddFloorplan(floorplan);
            realty.AddAmenity(amenity);
            return realty;
        }

        private AmenityType GetAmenityType()
        {
            var amenityTypeRepository = Container.Resolve<IRepository<AmenityType>>();
            var amenityType = amenityTypeRepository.Load(_amenityTypeId);
            return amenityType;
        }

        private Client GetClient()
        {
            var clientRepository = Container.Resolve<IRepository<Client>>();
            var client = clientRepository.Load(_clientId);
            return client;
        }
    }
}