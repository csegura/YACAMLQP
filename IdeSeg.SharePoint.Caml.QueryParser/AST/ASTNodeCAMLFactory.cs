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
using IdeSeg.SharePoint.Caml.QueryParser.AST.CAML;

namespace IdeSeg.SharePoint.Caml.QueryParser.AST
{
    // TODO: Extend with CAML.Net
    public class ASTNodeCAMLFactory : ASTNodeFactoryBase
    {
        public override Query CreateQuery(Sequence sequence)
        {
            return new CAMLQuery(sequence);
        }

        public override Where CreateWhere(Expression expressionExp)
        {
            return new CAMLWhere(expressionExp);
        }

        public override GroupBy CreateGroupBy(FieldList fields)
        {
            return new CAMLGroupBy(fields);
        }

        public override OrderBy CreateOrderBy(FieldList fields)
        {
            return new CAMLOrderBy(fields);
        }

        public override BooleanAnd CreateBooleanAnd(Expression left, Expression right)
        {
            return new CAMLBooleanAnd(left, right);
        }

        public override BooleanOr CreateBooleanOr(Expression left, Expression right)
        {
            return new CAMLBooleanOr(left, right);
        }

        public override Operation CreateComparationBegin(FieldNode field, ValueNode value)
        {
            return new CAMLOpBeginsWith(field, value);
        }

        public override Operation CreateComparationContains(FieldNode field, ValueNode value)
        {
            return new CAMLOpContains(field, value);
        }

        public override Operation CreateComparationEqual(FieldNode field, ValueNode value)
        {
            return new CAMLOpEqual(field, value);
        }

        public override Operation CreateComparationGreater(FieldNode field, ValueNode value)
        {
            return new CAMLOpGreater(field, value);
        }

        public override Operation CreateComparationGraterEqual(FieldNode field, ValueNode value)
        {
            return new CAMLOpGreaterEqual(field, value);
        }

        public override Operation CreateComparationLess(FieldNode field, ValueNode value)
        {
            return new CAMLOpLess(field, value);
        }

        public override Operation CreateComparationLessEqual(FieldNode field, ValueNode value)
        {
            return new CAMLOpLessEqual(field, value);
        }

        public override Operation CreateComparationNotEqual(FieldNode field, ValueNode value)
        {
            return new CAMLOpNotEqual(field, value);
        }

        public override Operation CreateComparationIsNull(FieldNode field)
        {
            return new CAMLOpIsNull(field);
        }

        public override Operation CreateComparationIsNotNull(FieldNode field)
        {
            return new CAMLOpIsNotNull(field);
        }

        public override FieldNode CreateFieldNode(string fieldName)
        {
            return new CAMLFieldNode(fieldName);
        }

        public override FieldNode CreateFieldNode(string fieldName, bool order)
        {
            return new CAMLFieldNode(fieldName, order);
        }

        public override ValueNode CreateValueNode(string value, string valueType)
        {
            return new CAMLValueNode(value, valueType);
        }

        public override ValueNode CreateValueNode(string value, ValueNodeType valueType)
        {
            return new CAMLValueNode(value, valueType);
        }
    }
}