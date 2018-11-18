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
using IdeSeg.SharePoint.Caml.QueryParser.AST.CAML;
using NUnit.Framework;

namespace YACAMLQP_Tests
{

    [TestFixture]
    public class T_ASTNodeCAMLFactory
    {
        private ASTNodeFactoryBase factory;

        [SetUp]
        public void SetupFixture()
        {
            factory = new ASTNodeCAMLFactory();
        }


        [Test]
        public void CreateSequence_Where()
        {
            Sequence sequence = factory.CreateSequence(factory.CreateWhere(null), null);
            Assert.IsNotNull(sequence);
            Assert.IsInstanceOfType(typeof(CAMLWhere), sequence.LeftNode);
            Assert.IsNull(sequence.RightNode);
        }

        [Test]
        public void CreateSequence_OrderBy()
        {
            Sequence sequence = factory.CreateSequence(factory.CreateOrderBy(null), null);
            Assert.IsNotNull(sequence);
            Assert.IsInstanceOfType(typeof(CAMLOrderBy), sequence.LeftNode);
            Assert.IsNull(sequence.RightNode);
        }

        [Test]
        public void CreateSequence_GroupBy()
        {
            Sequence sequence = factory.CreateSequence(factory.CreateGroupBy(null), null);
            Assert.IsNotNull(sequence);
            Assert.IsInstanceOfType(typeof(CAMLGroupBy), sequence.LeftNode);
            Assert.IsNull(sequence.RightNode);
        }

        [Test]
        public void CreateWhere()
        {
            Where where = factory.CreateWhere(null);
            Assert.IsNotNull(where);
            Assert.IsInstanceOfType(typeof(CAMLWhere),where);
        }

        [Test]
        public void CreateGroupBy_Field()
        {
            GroupBy groupBy = factory.CreateGroupBy(
                    factory.CreateFieldList(
                            factory.CreateFieldNode("Test")));
            Assert.IsNotNull(groupBy);
            Assert.IsInstanceOfType(typeof(CAMLGroupBy), groupBy);
            Assert.IsInstanceOfType(typeof(FieldList), groupBy.LeftNode);
            Assert.IsInstanceOfType(typeof(FieldNode), groupBy.LeftNode.LeftNode);
        }

        [Test]
        public void CreateGroupBy_Fields()
        {
            GroupBy groupBy = factory.CreateGroupBy(
                    factory.CreateFieldList(
                            factory.CreateFieldNode("Test"),
                            factory.CreateFieldList(
                                    factory.CreateFieldNode("Test2"))));
            Assert.IsNotNull(groupBy);
            Assert.IsInstanceOfType(typeof(CAMLGroupBy), groupBy);
            Assert.IsInstanceOfType(typeof(FieldList), groupBy.LeftNode);
            Assert.IsInstanceOfType(typeof(FieldNode), groupBy.LeftNode.LeftNode);
            Assert.IsInstanceOfType(typeof(FieldList), groupBy.LeftNode.RightNode);
            Assert.IsInstanceOfType(typeof(FieldNode), groupBy.LeftNode.RightNode.LeftNode);
        }


        [Test]
        public void CreateOrderBy_Field()
        {
            OrderBy orderBy = factory.CreateOrderBy(
                    factory.CreateFieldList(
                            factory.CreateFieldNode("Test", true)));
            Assert.IsNotNull(orderBy);
            Assert.IsInstanceOfType(typeof(CAMLOrderBy), orderBy);
            Assert.IsInstanceOfType(typeof(FieldList), orderBy.LeftNode);
            Assert.IsInstanceOfType(typeof(FieldNode), orderBy.LeftNode.LeftNode);

            FieldNode fieldNode = orderBy.LeftNode.LeftNode as FieldNode;
            Assert.IsNotNull(fieldNode);
            Assert.AreEqual(true, fieldNode.Ascending);
        }

        [Test]
        public void CreateOrderBy_Fields()
        {
            OrderBy orderBy = factory.CreateOrderBy(
                    factory.CreateFieldList(
                            factory.CreateFieldNode("Test", true),
                            factory.CreateFieldList(
                                    factory.CreateFieldNode("Test2", false))));

            FieldNode fieldNode;

            Assert.IsNotNull(orderBy);
            Assert.IsInstanceOfType(typeof(CAMLOrderBy), orderBy);
            Assert.IsInstanceOfType(typeof(FieldList), orderBy.LeftNode);
            Assert.IsInstanceOfType(typeof(FieldNode), orderBy.LeftNode.LeftNode);

            fieldNode = orderBy.LeftNode.LeftNode as FieldNode;
            Assert.IsNotNull(fieldNode);
            Assert.AreEqual(true, fieldNode.Ascending);

            Assert.IsInstanceOfType(typeof(FieldList), orderBy.LeftNode.RightNode);
            Assert.IsInstanceOfType(typeof(FieldNode), orderBy.LeftNode.RightNode.LeftNode);

            fieldNode = orderBy.LeftNode.RightNode.LeftNode as FieldNode;
            Assert.IsNotNull(fieldNode);
            Assert.AreEqual(false, fieldNode.Ascending);
        }

        [Test]
        public void CreateBooleanAnd()
        {
            Expression expression = factory.CreateExpression(
                    factory.CreateComparationEqual(factory.CreateFieldNode("FIELD"),
                                                   factory.CreateValueNode("VALUE", ValueNodeType.Text)));


            BooleanAnd and = factory.CreateBooleanAnd(expression, expression);

            Assert.IsNotNull(and);
            Assert.IsInstanceOfType(typeof(CAMLBooleanAnd), and);
            Assert.IsInstanceOfType(typeof(Expression), and.LeftNode);
            Assert.IsInstanceOfType(typeof(Expression), and.RightNode);
        }

        [Test]
        public void CreateBooleanOr()
        {
            Expression expression = factory.CreateExpression(
                  factory.CreateComparationEqual(factory.CreateFieldNode("FIELD"),
                                                 factory.CreateValueNode("VALUE", ValueNodeType.Text)));


            BooleanOr or = factory.CreateBooleanOr(expression, expression);

            Assert.IsNotNull(or);
            Assert.IsInstanceOfType(typeof(CAMLBooleanOr), or);
            Assert.IsInstanceOfType(typeof(Expression), or.LeftNode);
            Assert.IsInstanceOfType(typeof(Expression), or.RightNode);
        }

        [Test]
        public void CreateSingleOperation()
        {
            Expression expression = factory.CreateExpression(
                  factory.CreateComparationEqual(factory.CreateFieldNode("FIELD"),
                                                 factory.CreateValueNode("VALUE", ValueNodeType.Text)));
          
            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(typeof(CAMLOpEqual), expression.LeftNode);
        }


        [Test]
        public void CreateOperationBegin()
        {
            Operation operation = factory.CreateComparationBegin(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpBeginsWith), operation.GetType());
        }

        [Test]
        public void CreateOperationContains()
        {
            Operation operation = factory.CreateComparationContains(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpContains), operation.GetType());
        }

        [Test]
        public void CreateOperationEqual()
        {
            Operation operation = factory.CreateComparationEqual(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpEqual), operation.GetType());
        }

        [Test]
        public void CreateOperationGreater()
        {
            Operation operation = factory.CreateComparationGreater(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpGreater), operation.GetType());
        }

        [Test]
        public void CreateOperationGreaterEqual()
        {
            Operation operation = factory.CreateComparationGraterEqual(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpGreaterEqual), operation.GetType());
        }

        [Test]
        public void CreateOperationLess()
        {
            Operation operation = factory.CreateComparationLess(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpLess), operation.GetType());
        }

        [Test]
        public void CreateOperationLessEqual()
        {
            Operation operation = factory.CreateComparationLessEqual(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpLessEqual), operation.GetType());
        }

        [Test]
        public void CreateOperationNotEqual()
        {
            Operation operation = factory.CreateComparationNotEqual(
                    factory.CreateFieldNode("Field"),
                    factory.CreateValueNode("Value", ValueNodeType.Text));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpNotEqual), operation.GetType());
        }

        [Test]
        public void CreateOperationIsNul()
        {
            Operation operation = factory.CreateComparationIsNull(
                    factory.CreateFieldNode("Field"));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpIsNull), operation.GetType());
        }

        [Test]
        public void CreateOperationIsNotNul()
        {
            Operation operation = factory.CreateComparationIsNotNull(
                    factory.CreateFieldNode("Field"));

            Assert.IsNotNull(operation);
            Assert.AreEqual(typeof(CAMLOpIsNotNull), operation.GetType());
        }

        [Test]
        public void CreateFieldNode()
        {
            FieldNode field = factory.CreateFieldNode("Field");
            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(typeof(CAMLFieldNode), field);
        }

        [Test]
        public void CreateFieldNode_Ascending()
        {
            FieldNode field = factory.CreateFieldNode("Field", true);
            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(typeof(CAMLFieldNode), field);
            Assert.AreEqual(true, field.Ascending);
        }

        [Test]
        public void CreateFieldNode_Desscending()
        {
            FieldNode field = factory.CreateFieldNode("Field", false);
            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(typeof(CAMLFieldNode), field);
            Assert.AreEqual(false, field.Ascending);
        }
      

        [Test]
        public void CreateValueNode()
        {
            ValueNode field = factory.CreateValueNode("Value", ValueNodeType.Text);
            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(typeof(CAMLValueNode), field);
        }

        [Test]
        public void CreateValueNode_FromString()
        {
            ValueNode field = factory.CreateValueNode("Value", "Text");
            Assert.IsNotNull(field);
            Assert.IsInstanceOfType(typeof(CAMLValueNode), field);
        }

       
    }
}