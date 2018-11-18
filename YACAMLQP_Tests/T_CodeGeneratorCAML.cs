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

using System;
using System.Diagnostics;
using IdeSeg.SharePoint.Caml.QueryParser;
using IdeSeg.SharePoint.Caml.QueryParser.AST;
using IdeSeg.SharePoint.Caml.QueryParser.AST.Base;
using IdeSeg.SharePoint.Caml.QueryParser.Parser;
using NUnit.Framework;

namespace YACAMLQP_Tests
{
    [TestFixture]
    public class T_CodeGeneratorCAML
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_Exception()
        {
            CodeGenerator generator = new CodeGenerator(null);
        }

        [Test]
        public void CAMLSimpleWhere()
        {
            const string textQuery = "WHERE Test=\"Value\"";
            const string spectedCAML = "<Query><Where><Eq>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "<Value Type=\"Text\">Value</Value>" +
                                       "</Eq></Where></Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleWhere2()
        {
            const string textQuery = "WHERE Test <> NULL";
            const string spectedCAML = "<Query>" +
                                       "<Where>" +
                                       "<IsNotNull>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "</IsNotNull>" +
                                       "</Where>" +
                                       "</Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleWhereAnd()
        {
            const string textQuery = "WHERE Test=\"Value\" AND Other=2";
            const string spectedCAML = "<Query><Where><And>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "<Value Type=\"Text\">Value</Value>" +
                                       "</Eq>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Other\" />" +
                                       "<Value Type=\"Numeric\">2</Value>" +
                                       "</Eq>" +
                                       "</And></Where></Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleWhereOr()
        {
            const string textQuery = "WHERE Test=\"Value\" OR Other=2";
            const string spectedCAML = "<Query><Where><Or>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "<Value Type=\"Text\">Value</Value>" +
                                       "</Eq>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Other\" />" +
                                       "<Value Type=\"Numeric\">2</Value>" +
                                       "</Eq>" +
                                       "</Or></Where></Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleWhereParenthesis()
        {
            const string textQuery = "WHERE Test=\"Value\" OR (Other=2 AND Test=1)";
            const string spectedCAML = "<Query>"+
                "<Where>"+
                "<Or>"+
                "<Eq>"+
                "<FieldRef Name=\"Test\" />"+
                "<Value Type=\"Text\">Value</Value>"+
                "</Eq>"+
                "<And>"+
                "<Eq>"+
                "<FieldRef Name=\"Other\" />"+
                "<Value Type=\"Numeric\">2</Value>"+
                "</Eq>"+
                "<Eq>" +
                "<FieldRef Name=\"Test\" />" +
                "<Value Type=\"Numeric\">1</Value>" +
                "</Eq>" +
                "</And>"+
                "</Or>"+
                "</Where>"+
                "</Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleWhereParenthesis1()
        {
            const string textQuery = "WHERE ((Column1 = \"Value1\") AND " +
                                     "(Column2 = #01/01/2007#)) ";
            const string spectedCAML = "<Query>" +
                                       "<Where>" +
                                       "<And>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Column1\" />" +
                                       "<Value Type=\"Text\">Value1</Value>" +
                                       "</Eq>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Column2\" />" +
                                       "<Value Type=\"DateTime\">01/01/2007</Value>" +
                                       "</Eq>" +
                                       "</And>" +
                                       "</Where>" +
                                       "</Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }


        [Test]
        public void CAMLSimpleWhereGroupBy()
        {
            const string textQuery = "WHERE Test=\"Value\" AND Other=2 GROUPBY Column1";
            const string spectedCAML = "<Query>" +
                                       "<Where>" +
                                       "<And>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "<Value Type=\"Text\">Value</Value>" +
                                       "</Eq>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Other\" />" +
                                       "<Value Type=\"Numeric\">2</Value>" +
                                       "</Eq>" +
                                       "</And>" +
                                       "</Where>" +
                                       "<Group>" +
                                       "<FieldRef Name=\"Column1\" />" +
                                       "</Group>" +
                                       "</Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }


        [Test]
        public void CAMLSimpleWhereParenthesis2()
        {
            const string textQuery = "WHERE ((Column1 = \"Value1\") AND " +
                                     "(Column2 = #01/01/2007#)) OR " +
                                     "((Column3 = 10) AND (Column3 <> 10)) " +
                                     "ORDERBY Column1, Column2 ASC, Column3 DESC " +
                                     "GROUPBY Column1 ";

            const string spectedCAML = "<Query>" +
                                       "<Where>" +
                                       "<Or>" +
                                       "<And>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Column1\" />" +
                                       "<Value Type=\"Text\">Value1</Value>" +
                                       "</Eq>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Column2\" />" +
                                       "<Value Type=\"DateTime\">01/01/2007</Value>" +
                                       "</Eq>" +
                                       "</And>" +
                                       "<And>" +
                                       "<Eq>" +
                                       "<FieldRef Name=\"Column3\" />" +
                                       "<Value Type=\"Numeric\">10</Value>" +
                                       "</Eq>" +
                                       "<Neq>" +
                                       "<FieldRef Name=\"Column3\" />" +
                                       "<Value Type=\"Numeric\">10</Value>" +
                                       "</Neq>" +
                                       "</And>" +
                                       "</Or>" +
                                       "</Where>" +
                                       "<Order>" +
                                       "<FieldRef Name=\"Column1\" />" +
                                       "<FieldRef Name=\"Column2\" Ascending=\"True\" />" +
                                       "<FieldRef Name=\"Column3\" Ascending=\"False\" />" +
                                       "</Order>" +
                                       "<Group>" +
                                       "<FieldRef Name=\"Column1\" />" +
                                       "</Group>" +
                                       "</Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        [ExpectedException(typeof(ParserException))]
        public void CAMLSimpleWhereParenthesisException()
        {
            const string textQuery = "WHERE Test=\"Value\" OR (Other=2 AND Test=1";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);

            generator.Generate();
        }


        [Test]
        public void CAMLSimpleOrderBy()
        {
            const string textQuery = "ORDERBY Test";
            const string spectedCAML = "<Query><Order>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "</Order></Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            CodeGenerator generator = new CodeGenerator(parser.Parse());

            Assert.IsNotNull(generator);
            Debug.WriteLine(generator.Code);
            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleOrderBy_Fields()
        {
            const string textQuery = "ORDERBY Test, Column2,Column3";
            const string spectedCAML = "<Query><Order>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "<FieldRef Name=\"Column2\" />" +
                                       "<FieldRef Name=\"Column3\" />" +
                                       "</Order></Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            Query query = parser.Parse();

            CodeGenerator generator = new CodeGenerator(query);

            Assert.IsNotNull(generator);
            Debug.WriteLine(generator.Code);
            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleOrderBy_Fields_AscendingDescending()
        {
            const string textQuery = "ORDERBY Test, Column2 DESC,Column3";
            const string spectedCAML = "<Query><Order>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "<FieldRef Name=\"Column2\" Ascending=\"False\" />" +
                                       "<FieldRef Name=\"Column3\" />" +
                                       "</Order></Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            Query query = parser.Parse();

            CodeGenerator generator = new CodeGenerator(query);

            Assert.IsNotNull(generator);
            Debug.WriteLine(generator.Code);
            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }

        [Test]
        public void CAMLSimpleGroupBy_Fields()
        {
            const string textQuery = "GROUPBY Test,Column2,Column3";
            const string spectedCAML = "<Query><Group>" +
                                       "<FieldRef Name=\"Test\" />" +
                                       "<FieldRef Name=\"Column2\" />" +
                                       "<FieldRef Name=\"Column3\" />" +
                                       "</Group></Query>";

            NParser parser = new NParser(textQuery, new ASTNodeCAMLFactory());

            Query query = parser.Parse();

            CodeGenerator generator = new CodeGenerator(query);

            Assert.IsNotNull(generator);
            Debug.WriteLine(generator.Code);
            generator.Generate();

            Assert.AreEqual(spectedCAML, generator.Code);
        }
    }
}