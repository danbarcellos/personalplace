using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using Pactor.Infra.Crosscutting.LogCore;

namespace Pactor.Infra.Crosscutting.Log.Tests
{
    [TestClass]
    public class DadoUmaInstanciaDeNLogLogger
    {
        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoInstancioEntaoObtenhoUmaInstancia()
        {
            //act
            var nLogLogger = new NLogLogger(ObterMockILogDecorado().Object);

            //assert
            Assert.IsNotNull(nLogLogger);
        }

        
        // -------- Debug

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoDebugComParametroMensagemEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Debug(It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Debug(message);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoDebugComParametrosMensagemEExceptionEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var exception = new Exception(message);
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Debug(It.IsAny<Exception>(), It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Debug(message, exception);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoDebugFormatComParametrosStringEUmParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Debug(It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.DebugFormat(message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoDebugFormatComParametrosStringEDoisParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Debug(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.DebugFormat(message, parametroUm, parametroDois);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoDebugFormatComParametrosStringETresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Debug(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.DebugFormat(message, parametroUm, parametroDois, parametroTres);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoDebugFormatComParametrosStringComMaisDeTresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            const int parametroQuatro = 4;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Debug(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.DebugFormat(message, parametroUm, parametroDois, parametroTres, parametroQuatro);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoDebugFormatComParametrosFormatProviderEStringEParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var formatProviderStub = new Mock<IFormatProvider>().Object;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Debug(It.IsAny<IFormatProvider>(), It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.DebugFormat(formatProviderStub, message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        // -------- Info

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoInfoComParametroMensagemEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Info(It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Info(message);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoInfoComParametrosMensagemEExceptionEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var exception = new Exception(message);
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Info(It.IsAny<Exception>(), It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Info(message, exception);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoInfoFormatComParametrosStringEUmParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Info(It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.InfoFormat(message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoInfoFormatComParametrosStringEDoisParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Info(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.InfoFormat(message, parametroUm, parametroDois);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoInfoFormatComParametrosStringETresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Info(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.InfoFormat(message, parametroUm, parametroDois, parametroTres);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoInfoFormatComParametrosStringComMaisDeTresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            const int parametroQuatro = 4;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Info(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.InfoFormat(message, parametroUm, parametroDois, parametroTres, parametroQuatro);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoInfoFormatComParametrosFormatProviderEStringEParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var formatProviderStub = new Mock<IFormatProvider>().Object;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Info(It.IsAny<IFormatProvider>(), It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.InfoFormat(formatProviderStub, message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        // -------- Warn

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoWarnComParametroMensagemEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Warn(It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Warn(message);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoWarnComParametrosMensagemEExceptionEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var exception = new Exception(message);
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Warn(It.IsAny<Exception>(), It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Warn(message, exception);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoWarnFormatComParametrosStringEUmParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Warn(It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.WarnFormat(message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoWarnFormatComParametrosStringEDoisParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Warn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.WarnFormat(message, parametroUm, parametroDois);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoWarnFormatComParametrosStringETresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Warn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.WarnFormat(message, parametroUm, parametroDois, parametroTres);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoWarnFormatComParametrosStringComMaisDeTresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            const int parametroQuatro = 4;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Warn(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.WarnFormat(message, parametroUm, parametroDois, parametroTres, parametroQuatro);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoWarnFormatComParametrosFormatProviderEStringEParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var formatProviderStub = new Mock<IFormatProvider>().Object;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Warn(It.IsAny<IFormatProvider>(), It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.WarnFormat(formatProviderStub, message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        // -------- Error

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoErrorComParametroMensagemEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Error(It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Error(message);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoErrorComParametrosMensagemEExceptionEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var exception = new Exception(message);
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Error(message, exception);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoErrorFormatComParametrosStringEUmParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.ErrorFormat(message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoErrorFormatComParametrosStringEDoisParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.ErrorFormat(message, parametroUm, parametroDois);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoErrorFormatComParametrosStringETresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.ErrorFormat(message, parametroUm, parametroDois, parametroTres);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoErrorFormatComParametrosStringComMaisDeTresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            const int parametroQuatro = 4;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.ErrorFormat(message, parametroUm, parametroDois, parametroTres, parametroQuatro);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoErrorFormatComParametrosFormatProviderEStringEParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var formatProviderStub = new Mock<IFormatProvider>().Object;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Error(It.IsAny<IFormatProvider>(), It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.ErrorFormat(formatProviderStub, message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        // -------- Fatal

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoFatalComParametroMensagemEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Fatal(It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Fatal(message);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoFatalComParametrosMensagemEExceptionEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            var exception = new Exception(message);
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Fatal(It.IsAny<Exception>(), It.IsAny<string>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.Fatal(message, exception);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoFatalFormatComParametrosStringEUmParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Fatal(It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.FatalFormat(message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoFatalFormatComParametrosStringEDoisParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Fatal(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.FatalFormat(message, parametroUm, parametroDois);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoFatalFormatComParametrosStringETresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Fatal(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.FatalFormat(message, parametroUm, parametroDois, parametroTres);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoFatalFormatComParametrosStringComMaisDeTresParametrosDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            const int parametroDois = 2;
            const int parametroTres = 3;
            const int parametroQuatro = 4;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Fatal(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.FatalFormat(message, parametroUm, parametroDois, parametroTres, parametroQuatro);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoLogoFatalFormatComParametrosFormatProviderEStringEParametroDeFormatacaoEntaoOMetodoDecoradoEhExecutado()
        {
            //arrange
            const string message = "TesteDeLog";
            const int parametroUm = 1;
            var formatProviderStub = new Mock<IFormatProvider>().Object;
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.Fatal(It.IsAny<IFormatProvider>(), It.IsAny<string>(), It.IsAny<int>()));
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            nLogLogger.FatalFormat(formatProviderStub, message, parametroUm);

            //assert
            logDecoradoMock.Verify();
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoAcessoPropriedadeIsDebugEnabledEntaoAPropriedadeDecoradaEhAcessada()
        {
            //arrange
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.IsDebugEnabled).Returns(true);
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            var isDebugEnabled = nLogLogger.IsDebugEnabled;

            //assert
            logDecoradoMock.Verify();
            Assert.IsTrue(isDebugEnabled);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoAcessoPropriedadeIsInfoEnabledEntaoAPropriedadeDecoradaEhAcessada()
        {
            //arrange
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.IsInfoEnabled).Returns(true);
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            var isInfoEnabled = nLogLogger.IsInfoEnabled;

            //assert
            logDecoradoMock.Verify();
            Assert.IsTrue(isInfoEnabled);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoAcessoPropriedadeIsWarnEnabledEntaoAPropriedadeDecoradaEhAcessada()
        {
            //arrange
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.IsWarnEnabled).Returns(true);
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            var isWarnEnabled = nLogLogger.IsWarnEnabled;

            //assert
            logDecoradoMock.Verify();
            Assert.IsTrue(isWarnEnabled);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoAcessoPropriedadeIsErrorEnabledEntaoAPropriedadeDecoradaEhAcessada()
        {
            //arrange
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.IsErrorEnabled).Returns(true);
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            var isErrorEnabled = nLogLogger.IsErrorEnabled;

            //assert
            logDecoradoMock.Verify();
            Assert.IsTrue(isErrorEnabled);
        }

        [TestMethod]
        [TestCategory("Unit")]
        [TestCategory("Infra.Crosscutting.Log")]
        public void QuandoAcessoPropriedadeIsFatalEnabledEntaoAPropriedadeDecoradaEhAcessada()
        {
            //arrange
            var logDecoradoMock = ObterMockILogDecorado();
            logDecoradoMock.Setup(x => x.IsFatalEnabled).Returns(true);
            var nLogLogger = new NLogLogger(logDecoradoMock.Object);

            //act
            var isFatalEnabled = nLogLogger.IsFatalEnabled;

            //assert
            logDecoradoMock.Verify();
            Assert.IsTrue(isFatalEnabled);
        }

        private Mock<NLog.ILogger> ObterMockILogDecorado()
        {
            return new Mock<NLog.ILogger>();
        }
    }
}
