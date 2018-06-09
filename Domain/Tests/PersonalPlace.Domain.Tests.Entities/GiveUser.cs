using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pactor.Infra.Crosscutting.Security;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.Tests.Base;
using PersonalPlace.Domain.Common.RBAC;
using PersonalPlace.Domain.Common.ValueObjects;
using PersonalPlace.Domain.Entities.Security;
using PersonalPlace.Domain.Entities.Structure;

namespace PersonalPlace.Domain.Tests.Entities
{
    [TestClass]
    public class GiveUser : IntegratedBaseIsolatedExecutionTestClass
    {
        #region Test User Values
        private readonly string _externalId = "B109DF8D-6EB9-4F4B-ADDE-FFE3F31FB8AB";
        private readonly string _firstName = "New";
        private readonly string _lastName = "User Test";
        private readonly string _email = "newusertest@teste.com";
        private readonly string _password = new ShuffleService().ComputeHash("sEnH4Usu4ri0");
        private readonly bool _changePasswordRequired = true;
        private readonly string[] _userAuthorizationRoles = { Roles.User, Roles.Administrator };
        private readonly UserStateType _stateType = UserStateType.Active;

        #endregion

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            StartEnvironment();
        }

        [TestMethod]
        [TestCategory("Domain.Entities")]
        [TestCategory("Integrated")]
        public void WhenSaveUserThenICanRecover()
        {
            //arrange
            var uow = Container.Resolve<IUnitOfWork>();
            var userRepository = Container.Resolve<IRepository<User>>();
            var user = GetUser();

            //act
            userRepository.SaveAll(user);
            uow.Clear();
            var recoveredUser = userRepository.FindOne(user.Id);

            //assert
            Assert.IsNotNull(recoveredUser);
            Assert.AreEqual(_externalId, recoveredUser.ExternalId);
            Assert.AreEqual(_firstName, recoveredUser.FirstName);
            Assert.AreEqual(_lastName, recoveredUser.LastName);
            Assert.AreEqual(_email, recoveredUser.Email);
            Assert.AreEqual(_password, recoveredUser.Password);
            Assert.AreEqual(_changePasswordRequired, recoveredUser.ChangePasswordRequired);
            Assert.AreEqual(_stateType, recoveredUser.State.StateType);
        }

        [TestMethod]
        [TestCategory("Domain.Entities")]
        [TestCategory("Integrated")]
        public void WhenAssignAuthorizationThenTheAuthorizationIsAssigned()
        {
            //arrange
            var uow = Container.Resolve<IUnitOfWork>();
            var userRepository = Container.Resolve<IRepository<User>>();
            var user = GetUser();
            var authorization = new Authorization(user, GetUnit(), _userAuthorizationRoles);
            user.AddAuthorization(authorization);

            //act
            userRepository.SaveAll(user);
            uow.Clear();
            var recoveredUser = userRepository.FindOne(user.Id);

            //assert
            Assert.IsNotNull(recoveredUser);
            Assert.IsTrue(authorization.Roles.All(x => user.Authorizations.SelectMany(a => a.Roles).Contains(x)));
        }

        [TestMethod]
        [TestCategory("Domain.Entities")]
        [TestCategory("Integrated")]
        public void WhenAssignPropertyThenThePropertyIsAssigned()
        {
            //arrange
            const string propertyKey = "Test.TestProperty";
            const string propertyValue = "{\"Width\": 800, \"Height\": 600, \"Title\": \"View from 15th Floor\", \"IDs\": [116, 943, 234, 38793]}";
            var uow = Container.Resolve<IUnitOfWork>();
            var userRepository = Container.Resolve<IRepository<User>>();
            var user = GetUser();

            //act
            user.AssignProperty(propertyKey, propertyValue);
            userRepository.SaveAll(user);
            uow.Clear();
            var recoveredUser = userRepository.FindOne(user.Id);

            //assert
            Assert.IsNotNull(recoveredUser);
            var recoveredProperty = recoveredUser.Properties.SingleOrDefault(x => x.Key == propertyKey);
            Assert.IsNotNull(recoveredProperty);
            Assert.AreEqual(propertyValue, recoveredProperty.Value);
        }

        private User GetUser(string idExterno = null, string email = null, string firstName = null, string lastName = null, string password = null, bool assignAuthorization = false, bool? changePasswordRequired = null)
        {
            var user = new User(firstName ?? _firstName, lastName ?? _lastName, email ?? _email, password ?? _password, _stateType)
            {
                ExternalId = idExterno ?? _externalId,
                ChangePasswordRequired = changePasswordRequired ?? _changePasswordRequired
            };

            if (!assignAuthorization)
                return user;

            var authorization = new Authorization(user, GetUnit(), _userAuthorizationRoles);
            user.AddAuthorization(authorization);

            return user;
        }

        private Unit GetUnit()
        {
            var unitRepository = Container.Resolve<IRepository<Unit>>();
            return unitRepository.FindOne(new Guid("3200826B-DB6F-42D8-B2F1-1B426810F34A"));
        }
    }
}
