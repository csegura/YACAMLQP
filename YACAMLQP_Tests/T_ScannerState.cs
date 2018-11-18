using System;
using System.Collections.Generic;
using System.Text;
using IdeSeg.SharePoint.Caml.QueryParser.LexScanner;
using NUnit.Framework;

namespace YACAMLQP_Tests
{
    [TestFixture]
    public class T_ScannerState
    {
        [Test]
        public void Constructor()
        {
            ScannerState scannerState = new ScannerState();
            Assert.IsNotNull(scannerState);
            Assert.AreEqual(0,scannerState.StartPosition);
            Assert.AreEqual(0,scannerState.CurrentPosition);
            Assert.IsNull(scannerState.ReadToken);
        }

        [Test]
        public void ConstructorFromState()
        {
            ScannerState scannerState = new ScannerState();
            scannerState.StartPosition = 1;
            scannerState.CurrentPosition = 2;
            scannerState.ReadToken = new Token(TokenType.TRUE);

            ScannerState scannerStateFromState = new ScannerState(scannerState);
            Assert.IsNotNull(scannerStateFromState);
            Assert.AreEqual(scannerState.StartPosition, scannerStateFromState.StartPosition);
            Assert.AreEqual(scannerState.CurrentPosition, scannerStateFromState.CurrentPosition);
            Assert.AreEqual(scannerState.ReadToken, scannerStateFromState.ReadToken);
        }
    }
}
