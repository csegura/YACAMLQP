#region Copyright(c) Carlos Segura Sanz, All right reserved.

//  -----------------------------------------------------------------------------
//  Copyright(c) 2008-2009 Carlos Segura Sanz, All right reserved.
// 
//  csegura@ideseg.com
//  http://www.ideseg.com
// 
//     * Attribution. You must attribute the work in the manner specified by the author or
//        licensor (but not in any way that suggests that they endorse you or your use of the
//        work).
// 
//     * Noncommercial. You may not use this work for commercial purposes.
// 
//     * No Derivative Works. You may not alter, transform, or build upon this work without        author authorization
//     * For any reuse or distribution, you must make clear to others the license terms of this           work. The best way to do this is contact with author.
//     * Any of the above conditions can be waived if you get permission from the copyright            holder.
//     * Nothing in this license impairs or restricts the author's moral rights.
// 
//  THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED
//  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
//  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
//  EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED  TO,  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF  LIABILITY,  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING   NEGLIGENCE OR  OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  SOFTWARE, EVEN IF  ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  -----------------------------------------------------------------------------

#endregion

using IdeSeg.SharePoint.Caml.QueryParser.LexScanner;
using NUnit.Framework;

namespace YACAMLQP_Tests
{
    [TestFixture]
    public class T_Scanner
    {
        [Test]
        public void Constructor()
        {
            var scanner = new Scanner("TEST");
            Assert.IsNotNull(scanner);
        }

        [Test]
        public void Interfaces()
        {
            var scanner = new Scanner("TEST");
            //Assert.IsInstanceOfType(typeof(ITokenizer), scanner);
            //Assert.IsInstanceOfType(typeof(ITokenValue), scanner);
        }

        [Test]
        [ExpectedException(typeof(ScannerException))]
        public void Constructor_Exception_EmptyInput()
        {
            var input = string.Empty;
            var scanner = new Scanner(input);
            Assert.IsNull(scanner);
        }


        [Test]
        public void LastToken()
        {
            var input = "WHERE Column1 = \"Value1\"";

            var scanner = new Scanner(input);

            Token token = scanner.GetToken();

            Assert.IsTrue(token.Ttype == TokenType.WHERE);
        }

        [Test]
        public void Position()
        {
            //           012345678901234
            var input = "WHERE Column1 = \"Value1\"";

            var scanner = new Scanner(input);

            scanner.GetToken();
            Assert.AreEqual(5, scanner.CurrentPosition);

            scanner.GetToken();
            Assert.AreEqual(13, scanner.CurrentPosition);
        }

        [Test]
        public void StartPosition()
        {
            //           012345678901234
            var input = "WHERE Column1 = \"Value1\"";

            var scanner = new Scanner(input);

            scanner.GetToken();
            Assert.AreEqual(0, scanner.StartPosition);

            scanner.GetToken();
            Assert.AreEqual(6, scanner.StartPosition);
        }

        [Test]
        public void GetToken_Reserved()
        {
            var input = "WHERE = ORDERBY = GROUPBY,AND,OR";

            var scanner = new Scanner(input);

            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.WHERE);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.EQ);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.ORDERBY);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.EQ);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.GROUPBY);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.COMMA);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.AND);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.COMMA);
            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.OR);
        }


        [Test]
        public void GetToken_Field()
        {
            var input = "WHERE Column1 = \"Value1\"";

            var scanner = new Scanner(input);

            Assert.IsTrue(scanner.GetToken().Ttype == TokenType.WHERE);

            Token token = scanner.GetToken();
            Assert_FieldToken(token,"Column1");

        }

        [Test]
        public void GetToken_FieldList()
        {
            var input = "Column1,Column2,Column3";

            var scanner = new Scanner(input);

            Token token = scanner.GetToken();
            Assert_FieldToken(token,"Column1");

            token = scanner.GetToken();
            Assert.IsTrue(token.Ttype == TokenType.COMMA);

            token = scanner.GetToken();
            Assert_FieldToken(token,"Column2");

            token = scanner.GetToken();
            Assert.IsTrue(token.Ttype == TokenType.COMMA);

            token = scanner.GetToken();
            Assert_FieldToken(token,"Column3");
        }

        [Test]
        public void GetToken_Value()
        {
            var input = "WHERE Column1 = \"Value1\" ";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();

            Assert.IsTrue(token.Ttype == TokenType.WHERE);

            token = scanner.GetToken();
            Assert_FieldToken(token,"Column1");

            token = scanner.GetToken();
            Assert.IsTrue(token.Ttype == TokenType.EQ);

            token = scanner.GetToken();
            Assert_ValueToken(token,"Value1",TokenValueType.Text);
        }

        [Test]
        public void ScanDate()
        {
            var input = "#01/01/01#";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();

            Assert_ValueToken(token, "01/01/01", TokenValueType.DateTime);
        }

        [Test]
        [ExpectedException(typeof(ScannerException))]
        public void ScanDate_NotClosed()
        {
            var input = "#01/01/01 Id";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();            
        }

        [Test]
        public void ScanString()
        {
            var input = "\"This is a string\"";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();

            Assert_ValueToken(token, "This is a string", TokenValueType.Text);
        }

        [Test]
        [ExpectedException(typeof(ScannerException))]
        public void ScanString_NotClosed()
        {
            var input = "\"This is a string";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();
        }

        [Test]
        public void ScanSpecialField()
        {
            var input = "[This is a long field name]";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();

            Assert_FieldToken(token, "This is a long field name");
        }

        [Test]
        [ExpectedException(typeof(ScannerException))]
        public void ScanSpecialField_NotClosed()
        {
            var input = "[This is a long field name";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();
        }

        [Test]
        public void ScanNumeric()
        {
            var input = "123";

            var scanner = new Scanner(input);

            Token token = scanner.GetToken();

            Assert_ValueToken(token, "123", TokenValueType.Numeric);
        }

        [Test]
        public void ScanNumeric_Decimal()
        {
            var input = "1024,34";

            var scanner = new Scanner(input);

            Token token = scanner.GetToken();

            Assert_ValueToken(token, "1024,34", TokenValueType.Numeric);
        }

        [Test]
        public void ScanNumeric_Decimal_Point()
        {
            var input = "1024.34";

            var scanner = new Scanner(input);
            Token token = scanner.GetToken();

            Assert_ValueToken(token, "1024.34", TokenValueType.Numeric);
        }

        [Test]
        public void ScanOperator()
        {
            var input = "> >= != <> , ( < <= && || = == )";

            var scanner = new Scanner(input);

            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.GREATER);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.GREATER_EQ);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.NOT_EQ);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.NOT_EQ);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.COMMA);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.LEFT_PARENTHESIS);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.LESS);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.LESS_EQ);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.AND);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.OR);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.EQ);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.EQ);
            Assert.IsNotNull(scanner.GetToken().Ttype == TokenType.RIGHT_PARENTHESIS);
        }
       
        [Test]
        [ExpectedException(typeof(ScannerException))]
        public void ScanOperator_Invalid()
        {
            var input = "|> ,,";

            var scanner = new Scanner(input);
            scanner.GetToken();
        }

        [Test]
        [ExpectedException(typeof(ScannerException))]
        public void Scan_Exception_UnknowChar()
        {
            var input = "\\";

            var scanner = new Scanner(input);

            Token token = scanner.GetToken();
        }

        [Test]
        public void Scan_GetBack()
        {
            var input = "Column <> NULL";
            var scanner = new Scanner(input);

            Token token = scanner.GetToken();

            Assert.AreEqual(TokenType.FIELD, token.Ttype);

            scanner.BackToken();

            token = scanner.GetToken();

            Assert.AreEqual(TokenType.FIELD,token.Ttype);

            token = scanner.GetToken();

            Assert.AreEqual(TokenType.NOT_EQ, token.Ttype);

            token = scanner.GetToken();

            Assert.AreEqual(TokenType.NULL, token.Ttype);
        }

        private void Assert_FieldToken(Token token, string name)
        {
            Assert.IsTrue(token.Ttype == TokenType.FIELD);
            Assert.AreEqual(name, token.Value);
            Assert.AreEqual(TokenValueType.Identifier, token.ValueType);
        }


        private void Assert_ValueToken(Token token, string value, TokenValueType type)
        {
            Assert.IsTrue(token.Ttype == TokenType.VALUE);
            Assert.AreEqual(value, token.Value);
            Assert.AreEqual(type, token.ValueType);
        }

        [Test]
        public void Test_Char_IsLetterOrDigit()
        {
            Assert.IsFalse(char.IsLetterOrDigit(' '));
        }

        [Test]
        public void Test_Parenthesis_IsPuntuation()
        {
            Assert.IsTrue(char.IsPunctuation('('));            
        }
    }
}