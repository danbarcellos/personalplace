using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.Tests.Base;
using PersonalPlace.Domain.Base;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Tests.Entities
{
    [TestClass]
    public class GiveAmenityType : IntegratedBaseIsolatedExecutionTestClass
    {
        #region Test User Values

        private readonly string _name = "Amenity Test Name";

        #endregion

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            StartEnvironment();
        }

        [TestMethod]
        [TestCategory("Domain.Entities")]
        [TestCategory("Integrated")]
        public void WhenSaveAmenityTypeThenICanRecover()
        {
            //arrange
            var uow = Container.Resolve<IUnitOfWork>();
            var amenityTypeRepository = Container.Resolve<IRepository<AmenityType>>();
            var amenityType = GetAmenityType();

            //act
            amenityTypeRepository.SaveAll(amenityType);
            uow.Clear();
            var recoveredAmenityType = amenityTypeRepository.FindOne(amenityType.Id);

            //assert
            Assert.IsNotNull(recoveredAmenityType);
            Assert.AreEqual(_name, recoveredAmenityType.Name);
        }

        private AmenityType GetAmenityType()
        {
            return new AmenityType(_name, DomainContextConstants.DefaultScopeTag);
        }
    }
}