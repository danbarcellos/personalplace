using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Pactor.Infra.Crosscutting.IoC.Core;

namespace Pactor.Infra.Crosscutting.IoC.Tests
{
    [TestClass]
    public class DadoUmContainerBootstrap
    {
        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.IoC")]
        public void QuandoInstancioEntaoObtenhoUmaInstancia()
        {
            //act
            var containerBuilder = new ContainerBootstrap();

            //assert
            Assert.IsNotNull(containerBuilder);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.IoC")]
        public void QuandoSolicitoAConstrucaoDoContainerEntaoObtenhoUmContainer()
        {
            //arrange
            var containerBuilderMock = new Mock<ContainerBootstrap> { CallBase = true };
            containerBuilderMock.Protected()
                .Setup<string[]>("GetIoCModulesAssemblies")
                .Returns(new string[]{});

            //act
            var container = containerBuilderMock.Object.Config();

            //assert
            containerBuilderMock.Verify();
            Assert.IsNotNull(container);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.IoC")]
        public void QuandoSolicitoAConstrucaoDoContainerEntaoRegistraUmModuloIoC()
        {
            //arrange
            var containerBuilderMock = new Mock<ContainerBootstrap> { CallBase = true };
            containerBuilderMock.Protected()
                .Setup<string[]>("GetIoCModulesAssemblies")
                .Returns(new[] { GetType().Assembly.Location });

            //act
            var container = containerBuilderMock.Object.Config();

            //assert
            containerBuilderMock.Verify();
            Assert.IsNotNull(container);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.IoC")]
        public void QuandoSolicitoAConstrucaoDoContainerEFalhaACargaDoAssemblyDeUmModuloIoCEntaoProssegueSemErro()
        {
            //arrange
            var containerBuilderMock = new Mock<ContainerBootstrap> { CallBase = true };
            containerBuilderMock.Protected()
                .Setup<string[]>("GetIoCModulesAssemblies")
                .Returns(new[] { GetType().Assembly.Location });
            containerBuilderMock.Protected()
                .Setup<Assembly>("LoadIoCModuleAssembly", ItExpr.IsAny<string>())
                .Throws<FileLoadException>();

            //act
            var container = containerBuilderMock.Object.Config();

            //assert
            containerBuilderMock.Verify();
            Assert.IsNotNull(container);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.IoC")]
        [ExpectedException(typeof(Exception))]
        public void QuandoSolicitoAConstrucaoDoContainerEAconteceUmaExcecaoNaoPrevistaNaCargaDoAssemblyDeUmModuloIoCEntaoDisparaExcecao()
        {
            //arrange
            var containerBuilderMock = new Mock<ContainerBootstrap> { CallBase = true };
            containerBuilderMock.Protected()
                .Setup<string[]>("GetIoCModulesAssemblies")
                .Returns(new[] { GetType().Assembly.Location });
            containerBuilderMock.Protected()
                .Setup<Assembly>("LoadIoCModuleAssembly", ItExpr.IsAny<string>())
                .Throws<Exception>();
            
            //act
            containerBuilderMock.Object.Config();
        }
    }
}
