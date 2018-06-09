using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.Tests.Base;
using PersonalPlace.Domain.Entities.Structure;

namespace PersonalPlace.Domain.Tests.Entities
{
    [TestClass]
    public class GiveUnit : IntegratedBaseIsolatedExecutionTestClass
    {
        #region Test Unit Values

        private readonly string _scopeTag = "1.1.3.1.";
        private readonly string _externalId = "B109DF8D-6EB9-4F4B-ADDE-FFE3F31FB8AB";
        private readonly string _description = "Unit for test";
        private readonly string _photoFilePrefix = "B109DF8D-6EB9-4F4B-ADDE-FFE3F31FB8AB";
        private readonly Guid? _externalSourceId = new Guid("761DD27C-7B4F-4913-B47B-7A3D74379E9B");
        private readonly int _dataLineNumber = 10;
        private readonly Guid _upperUnitId = new Guid("3200826B-DB6F-42D8-B2F1-1B426810F34A");
        private readonly int _hierarchicalSequence = 2;
        private readonly string _culture = "en-US";

        #endregion

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            StartEnvironment();
        }

        [TestMethod]
        [TestCategory("Domain.Entities")]
        [TestCategory("Integrated")]
        public void WhenSaveUnitThenCanRecover()
        {
            //arrange
            var uow = Container.Resolve<IUnitOfWork>();
            var unitRepository = Container.Resolve<IRepository<Unit>>();
            var unit = GetUnit();

            //act
            unitRepository.SaveAll(unit);
            uow.Clear();
            var recoveredUnit = unitRepository.FindOne(unit.Id);

            //assert
            Assert.IsNotNull(recoveredUnit);
            Assert.AreEqual(_externalId, recoveredUnit.ExternalId);
            Assert.AreEqual(_externalSourceId, recoveredUnit.ExternalSourceId);
            Assert.AreEqual(_dataLineNumber, recoveredUnit.DataLineNumber);
            Assert.AreEqual(_upperUnitId, recoveredUnit.UpperUnit.Id);
            Assert.AreEqual(_description, recoveredUnit.Description);
            Assert.AreEqual(_photoFilePrefix, recoveredUnit.PhotoFilePrefix);
            Assert.AreEqual(_hierarchicalSequence, recoveredUnit.HierarchicalSequence);
            Assert.AreEqual(_culture, recoveredUnit.Culture);
        }

        private Unit GetUnit()
        {
            var unit = new Unit(_description, hierarchicalSequence: 2, scopeTag: _scopeTag)
            {
                ExternalId = _externalId,
                ExternalSourceId = _externalSourceId,
                DataLineNumber = _dataLineNumber,
                UpperUnit = GetUpperUnit(),
                PhotoFilePrefix = _photoFilePrefix,
                Culture = _culture
            };

            return unit;
        }

        private Unit GetUpperUnit()
        {
            var unitRepository = Container.Resolve<IRepository<Unit>>();
            return unitRepository.Load(_upperUnitId);
        }
    }
}
