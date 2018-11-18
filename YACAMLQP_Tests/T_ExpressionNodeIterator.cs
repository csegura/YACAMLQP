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
using System.Collections.Generic;
using IdeSeg.SharePoint.Caml.QueryParser.AST;
using IdeSeg.SharePoint.Caml.QueryParser.AST.Base;
using IdeSeg.SharePoint.Caml.QueryParser.Parser;
using NUnit.Framework;

namespace YACAMLQP_Tests
{
    [TestFixture]
    public class T_ASTNodeIterator
    {

        [Test]
        public void Iterator_Test0()
        {
            var input = "WHERE Column1 = \"Value1\" OR Column2=2";

            var parser = new NParser(input);
            var caml = parser.Parse();

            IEnumerable<ASTNode> ienumerable = ASTNodeIterator.PreOrderWalk(caml);

            Type[] typeNodes = {
                                       typeof(Query),
                                       typeof(Sequence),
                                       typeof(Where),
                                       typeof(BooleanOr),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Sequence), // ORDERBY
                                       typeof(Sequence)  // GROUPBY
                               };

            int checkType = 0;

            foreach (ASTNode node in ienumerable)
            {
                Type type = typeNodes[checkType++];
                Assert.AreEqual(type, node.GetType());

                Console.WriteLine(node.GetType());
            }
        }


        [Test]
        public void Iterator_Test()
        {
            var input = "WHERE Column1 = \"Value1\" OR Column2=2 AND Column3=#01/01/01#";

            var parser = new NParser(input);
            var caml = parser.Parse();

            IEnumerable<ASTNode> ienumerable = ASTNodeIterator.PreOrderWalk(caml);

            Type[] typeNodes = {
                                       typeof(Query),
                                       typeof(Sequence),
                                       typeof(Where),
                                       typeof(BooleanOr),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(BooleanAnd),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Sequence),
                                       typeof(Sequence)
                               };

            int checkType = 0;

            foreach (ASTNode node in ienumerable)
            {
                Type type = typeNodes[checkType++];
                Assert.AreEqual(type, node.GetType());

                Console.WriteLine(node.GetType());
            }
        }

        [Test]
        public void Iterator_TestParenthesis()
        {
            var input = "WHERE (Column1 = \"Value1\" OR Column2=2)";

            var parser = new NParser(input);
            var caml = parser.Parse();

            IEnumerable<ASTNode> ienumerable = ASTNodeIterator.PreOrderWalk(caml);

            Type[] typeNodes = {
                                       typeof(Query),
                                       typeof(Sequence),
                                       typeof(Where),
                                       typeof(BooleanOr),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Sequence),
                                       typeof(Sequence)
                               };

            int checkType = 0;

            foreach (ASTNode node in ienumerable)
            {
                Type type = typeNodes[checkType++];
                Assert.AreEqual(type, node.GetType());

                Console.WriteLine(node.GetType());
            }
        }

        [Test]
        public void Iterator_TestParenthesis2()
        {
            var input = "WHERE (Column1 = \"Value1\" OR Column2=2) AND Column3=3";

            var parser = new NParser(input);
            var caml = parser.Parse();

            IEnumerable<ASTNode> ienumerable = ASTNodeIterator.PreOrderWalk(caml);

            Type[] typeNodes = {
                                       typeof(Query),
                                       typeof(Sequence),
                                       typeof(Where),
                                       typeof(BooleanAnd),
                                       typeof(BooleanOr),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Sequence),
                                       typeof(Sequence)
                               };

            int checkType = 0;

            foreach (ASTNode node in ienumerable)
            {
                Type type = typeNodes[checkType++];
                Assert.AreEqual(type, node.GetType());

                Console.WriteLine(node.GetType());
            }
        }
        [Test]
        public void Iterator_Test2()
        {
            var input = "ORDERBY Column1,Column2 DESC,Column3";

            var parser = new NParser(input);
            var caml = parser.Parse();

            IEnumerable<ASTNode> ienumerable = ASTNodeIterator.PreOrderWalk(caml);

            Type[] typeNodes = {
                                       typeof(Query),
                                       typeof(Sequence),
                                       typeof(Sequence),
                                       typeof(OrderBy),
                                       typeof(FieldList),
                                       typeof(FieldNode),
                                       typeof(FieldList),
                                       typeof(FieldNode),
                                       typeof(FieldList),
                                       typeof(FieldNode),
                                       typeof(Sequence)
                               };

            int checkType = 0;

            foreach (ASTNode node in ienumerable)
            {
                Type type = typeNodes[checkType++];
                Assert.AreEqual(type, node.GetType());

                Console.WriteLine(node.GetType());
            }
        }

        [Test]
        public void Iterator_Test3()
        {
            var input = "WHERE Column1=1 AND (Column2=2 OR Column3=3)";

            var parser = new NParser(input);
            var caml = parser.Parse();

            IEnumerable<ASTNode> ienumerable = ASTNodeIterator.PreOrderWalk(caml);

            Type[] typeNodes = {
                                       typeof(Query),
                                       typeof(Sequence),
                                       typeof(Where),
                                       typeof(BooleanAnd),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(BooleanOr),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Expression),
                                       typeof(OpEqual),
                                       typeof(FieldNode),
                                       typeof(ValueNode),
                                       typeof(Sequence),
                                       typeof(Sequence)
                               };

            int checkType = 0;

            foreach (ASTNode node in ienumerable)
            {
                Type type = typeNodes[checkType++];
                Assert.AreEqual(type, node.GetType());

                Console.WriteLine(node.GetType());
            }
        }
    }
}