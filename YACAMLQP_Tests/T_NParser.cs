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

using IdeSeg.SharePoint.Caml.QueryParser.AST;
using IdeSeg.SharePoint.Caml.QueryParser.AST.Base;
using IdeSeg.SharePoint.Caml.QueryParser.Parser;
using NUnit.Framework;

namespace YACAMLQP_Tests
{
    [TestFixture]
    public class T_NParser
    {
        [Test]
        public void Constructor()
        {
            var parser = new NParser("TEST");
            Assert.IsNotNull(parser);
        }

        [Test]
        public void WhereExpression_Test()
        {
            var input = "WHERE Column1 = \"Value1\" ";

            var parser = new NParser(input);
            var caml = parser.Parse();

            Assert.IsInstanceOfType(typeof(Query), caml);
            Assert.IsInstanceOfType(typeof(Sequence), caml.LeftNode);
            Assert.IsInstanceOfType(typeof(Where), caml.LeftNode.LeftNode);

            Assert_WellFormedExpression(caml.LeftNode.LeftNode.LeftNode);           
        }

        [Test]
        public void WhereExpression_Test_And()
        {
            var input = "WHERE Column1 = \"Value1\" AND Column2=2";

            var parser = new NParser(input);
            var caml = parser.Parse();

            Assert.IsInstanceOfType(typeof(Query), caml);
            Assert.IsInstanceOfType(typeof(Sequence), caml.LeftNode);
            Assert.IsInstanceOfType(typeof(Where), caml.LeftNode.LeftNode);

            Assert.IsInstanceOfType(typeof(BooleanAnd), caml.LeftNode.LeftNode.LeftNode);

            Assert_WellFormedExpression(caml.LeftNode.LeftNode.LeftNode.LeftNode); 
            Assert_WellFormedExpression(caml.LeftNode.LeftNode.LeftNode.RightNode); 

        }

        [Test]
        public void WhereExpression_Test_Or()
        {
            var input = "WHERE Column1 = \"Value1\" OR Column2=2";

            var parser = new NParser(input);
            var caml = parser.Parse();

            Assert.IsInstanceOfType(typeof(Query), caml);
            Assert.IsInstanceOfType(typeof(Sequence), caml.LeftNode);
            Assert.IsInstanceOfType(typeof(Where), caml.LeftNode.LeftNode);

            Assert.IsInstanceOfType(typeof(BooleanOr), caml.LeftNode.LeftNode.LeftNode);

            Assert_WellFormedExpression(caml.LeftNode.LeftNode.LeftNode.LeftNode);
            Assert_WellFormedExpression(caml.LeftNode.LeftNode.LeftNode.RightNode); 
        }

        [Test]
        public void WhereExpression_Test_Or_And()
        {
            var input = "WHERE Column1 = \"Value1\" OR Column2=2 AND Column3=#01/01/01#";

            var parser = new NParser(input);
            var caml = parser.Parse();

            Assert.IsInstanceOfType(typeof(Query), caml);
            Assert.IsInstanceOfType(typeof(Sequence), caml.LeftNode);
            Assert.IsInstanceOfType(typeof(Where), caml.LeftNode.LeftNode);

            Where where = caml.LeftNode.LeftNode as Where;

            Assert.IsInstanceOfType(typeof(BooleanOr), where.LeftNode);

            Assert_WellFormedExpression(where.LeftNode.LeftNode);

            Assert.IsInstanceOfType(typeof(BooleanAnd), where.LeftNode.RightNode);

            Assert_WellFormedExpression(where.LeftNode.RightNode.LeftNode);
            Assert_WellFormedExpression(where.LeftNode.RightNode.RightNode);
        }

        [Test]
        public void WhereExpression_Test_Or_And_Parenthesis()
        {
            var input = "WHERE (Column1 = \"Value1\" OR Column2=2) AND Column3=#01/01/01#";

            var parser = new NParser(input);
            var caml = parser.Parse();

            Assert.IsInstanceOfType(typeof(Query), caml);
            Assert.IsInstanceOfType(typeof(Sequence), caml.LeftNode);
            Assert.IsInstanceOfType(typeof(Where), caml.LeftNode.LeftNode);

            Where where = caml.LeftNode.LeftNode as Where;

            Assert.IsInstanceOfType(typeof(BooleanAnd), where.LeftNode);
            Assert.IsInstanceOfType(typeof(BooleanOr), where.LeftNode.LeftNode);

            Assert_WellFormedExpression(where.LeftNode.RightNode);

            Assert_WellFormedExpression(where.LeftNode.LeftNode.LeftNode);
            Assert_WellFormedExpression(where.LeftNode.LeftNode.RightNode);
        }


        //TODO: OrderBy GroupBy TESTS

        public void CheckSingleExpression(ASTNode ast)
        {
            Assert.IsInstanceOfType(typeof(FieldOperation), ast);
            CheckComparationExpression(ast.LeftNode);
        }

        public void CheckComparationExpression(ASTNode ast)
        {
            Assert.IsInstanceOfType(typeof(Operation), ast);
            Assert.IsInstanceOfType(typeof(FieldNode), ast.LeftNode);
            Assert.IsInstanceOfType(typeof(ValueNode), ast.RightNode);
        }

        public void Assert_WellFormedExpression(ASTNode node)
        {
            Assert.IsInstanceOfType(typeof(Expression), node);
            Assert.IsInstanceOfType(typeof(Operation), node.LeftNode);
            Assert.IsInstanceOfType(typeof(FieldNode), node.LeftNode.LeftNode);            
            Assert.IsInstanceOfType(typeof(ValueNode), node.LeftNode.RightNode);
        }
    }
}