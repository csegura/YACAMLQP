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
//     * No Derivative Works. You may not alter, transform, or build upon this work without
//        author authorization
//     * For any reuse or distribution, you must make clear to others the license terms of this           work. The best way to do this is contact with author.
//     * Any of the above conditions can be waived if you get permission from the copyright
//        holder.
//     * Nothing in this license impairs or restricts the author's moral rights.
// 
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
// EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF  LIABILITY,  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
// (INCLUDING   NEGLIGENCE OR  OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF 
// THIS SOFTWARE, EVEN IF  ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  -----------------------------------------------------------------------------

#endregion

using IdeSeg.SharePoint.Caml.QueryParser.AST.Base;

namespace IdeSeg.SharePoint.Caml.QueryParser.AST
{
    public abstract class ASTNodeFactoryBase
    {
        #region CAML

        public virtual Query CreateQuery(Sequence sequence)
        {
            return new Query(sequence);
        }

        #endregion

        #region Sequence

        public virtual Sequence CreateSequence(Where where, Sequence sequence)
        {
            return new Sequence(where, sequence);
        }

        public virtual Sequence CreateSequence(OrderBy orderBy, Sequence sequence)
        {
            return new Sequence(orderBy, sequence);
        }

        public virtual Sequence CreateSequence(GroupBy groupBy, Sequence sequence)
        {
            return new Sequence(groupBy, sequence);
        }

        #endregion

        #region Where

        public virtual Where CreateWhere(Expression expressionExp)
        {
            return new Where(expressionExp);
        }

        #endregion

        #region GrupBy

        public virtual GroupBy CreateGroupBy(FieldList fields)
        {
            return new GroupBy(fields);
        }

        #endregion

        #region OrderBy

        public virtual OrderBy CreateOrderBy(FieldList fields)
        {
            return new OrderBy(fields);
        }

        #endregion

        #region Expressions

        public virtual BooleanAnd CreateBooleanAnd(Expression left, Expression right)
        {
            return new BooleanAnd(left, right);
        }

        public virtual BooleanOr CreateBooleanOr(Expression left, Expression right)
        {
            return new BooleanOr(left, right);
        }

        public virtual Expression CreateExpression(Operation operation)
        {
            return new Expression(operation);
        }

        #endregion

        #region FieldOperation

        public virtual Operation CreateOperation(FieldNode field, ValueNode value)
        {
            return new Operation(field, value);
        }

        #endregion

        #region Comparations

        public virtual Operation CreateComparationBegin(FieldNode field, ValueNode value)
        {
            return new OpBeginsWith(field, value);
        }

        public virtual Operation CreateComparationContains(FieldNode field, ValueNode value)
        {
            return new OpContains(field, value);
        }

        public virtual Operation CreateComparationEqual(FieldNode field, ValueNode value)
        {
            return new OpEqual(field, value);
        }

        public virtual Operation CreateComparationGreater(FieldNode field, ValueNode value)
        {
            return new OpGreater(field, value);
        }

        public virtual Operation CreateComparationGraterEqual(FieldNode field, ValueNode value)
        {
            return new OpGreaterEqual(field, value);
        }

        public virtual Operation CreateComparationLess(FieldNode field, ValueNode value)
        {
            return new OpLess(field, value);
        }

        public virtual Operation CreateComparationLessEqual(FieldNode field, ValueNode value)
        {
            return new OpLessEqual(field, value);
        }

        public virtual Operation CreateComparationNotEqual(FieldNode field, ValueNode value)
        {
            return new OpNotEqual(field, value);
        }

        public virtual Operation CreateComparationIsNull(FieldNode field)
        {
            return new OpIsNull(field);
        }

        public virtual Operation CreateComparationIsNotNull(FieldNode field)
        {
            return new OpIsNotNull(field);
        }

        #endregion

        #region Terminals

        public virtual FieldNode CreateFieldNode(string fieldName)
        {
            return new FieldNode(fieldName);
        }

        public virtual FieldNode CreateFieldNode(string fieldName, bool ascendingOrder)
        {
            return new FieldNode(fieldName, ascendingOrder);
        }

        public virtual ValueNode CreateValueNode(string value, string valueType)
        {
            return new ValueNode(value, valueType);
        }

        public virtual ValueNode CreateValueNode(string value, ValueNodeType valueType)
        {
            return new ValueNode(value, valueType);
        }

        public virtual TrueNode CreateTrueNode()
        {
            return new TrueNode();
        }

        public virtual FalseNode CreateFalseNode()
        {
            return new FalseNode();
        }

        public virtual FieldList CreateFieldList(FieldNode fieldLeft, FieldList fields)
        {
            return new FieldList(fieldLeft, fields);
        }

        public virtual FieldList CreateFieldList(FieldNode fieldLeft)
        {
            return new FieldList(fieldLeft);
        }

        #endregion
    }
}