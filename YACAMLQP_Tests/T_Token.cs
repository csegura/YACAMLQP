using System;
using IdeSeg.SharePoint.Caml.QueryParser.LexScanner;
using NUnit.Framework;

namespace YACAMLQP_Tests
{
    [TestFixture]
    public class T_Token
    {
        [Test]
        public void Constructor()
        {
            Token token = new Token(TokenType.EOL);
            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.EOL, token.Ttype);
            Assert.AreEqual(TokenValueType.None, token.ValueType);
            Assert.AreEqual(string.Empty, token.Value);
        }

       
        [Test]
        public void ConstructorWithValue()
        {
            Token token = new Token(TokenType.VALUE,TokenValueType.Text, "TEST");
            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.VALUE, token.Ttype);
            Assert.AreEqual(TokenValueType.Text, token.ValueType);
            Assert.AreEqual("TEST", token.Value);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithBadTokenTypeAndValue()
        {
            Token token = new Token(TokenType.EOL, TokenValueType.Text, "TEST");
            Assert.IsNotNull(token);            
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithFieldTokenWithoutTypeAndValue()
        {
            Token token = new Token(TokenType.FIELD, TokenValueType.None, "TEST");
            Assert.IsNotNull(token);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithValueTokenWithoutTypeAndValue()
        {
            Token token = new Token(TokenType.VALUE, TokenValueType.None, "TEST");
            Assert.IsNotNull(token);
        }

        [Test]
        public void ConstructorWithValueEmpty()
        {
            Token token = new Token(TokenType.VALUE, TokenValueType.Text, string.Empty);
            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.VALUE,token.Ttype);
            Assert.AreEqual(TokenValueType.Text, token.ValueType);
            Assert.AreEqual(string.Empty, token.Value);
        }

        [Test]
        public void Value()
        {
            Token token = new Token(TokenType.VALUE, TokenValueType.Text, "TEST");
            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.VALUE, token.Ttype);
            Assert.AreEqual(TokenValueType.Text, token.ValueType);
            Assert.AreEqual("TEST", token.Value);
        }


    }
}
