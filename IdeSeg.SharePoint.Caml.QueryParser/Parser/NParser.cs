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

using IdeSeg.SharePoint.Caml.QueryParser.AST;
using IdeSeg.SharePoint.Caml.QueryParser.AST.Base;
using IdeSeg.SharePoint.Caml.QueryParser.LexScanner;

namespace IdeSeg.SharePoint.Caml.QueryParser.Parser
{
    /// <summary>
    /// Parser
    /// Analyzing a sequence of tokens to determine its grammatical structure 
    /// with respect to a given formal grammar. (see rules)
    /// </summary>
    public class NParser
    {
        private readonly Scanner _scanner;
        private readonly ASTNodeFactoryBase _astFactory;
        private int _parenthesisDepth;
        private Token _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="NParser"/> class.
        /// </summary>
        /// <param name="textQuery">The parser output.</param>
        public NParser(string textQuery)
        {
            _scanner = new Scanner(textQuery);
            _astFactory = new ASTNodeFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NParser"/> class.
        /// </summary>
        /// <param name="textQuery">The text query.</param>
        /// <param name="astFactory">The ast factory.</param>
        public NParser(string textQuery, ASTNodeFactoryBase astFactory)
                : this(textQuery)
        {
            _astFactory = astFactory;
        }

        /// <summary>
        /// Parser
        /// </summary>
        /// <returns>An AST</returns>
        public Query Parse()
        {
            Where where = null;
            OrderBy orderBy = null;
            GroupBy groupBy = null;

            _token = _scanner.GetToken();

            if (_token.Ttype == TokenType.WHERE)
            {
                where = WhereRule();
            }
            if (_token.Ttype == TokenType.ORDERBY)
            {
                orderBy = OrderByRule();
            }
            if (_token.Ttype == TokenType.GROUPBY)
            {
                groupBy = GroupByRule();
            }

            Sequence sequence =
                    _astFactory.CreateSequence(where,
                    _astFactory.CreateSequence(orderBy,
                    _astFactory.CreateSequence(groupBy, null)));

            Query expression = _astFactory.CreateQuery(sequence);

            return expression;
        }

        /// <summary>
        /// WHERE expression Rule
        ///  | BooleanExpression
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ParserException">Parenthesis mismath!!!</exception>
        private Where WhereRule()
        {
            Expression expression = BooleanOperatorRule();

            if (_parenthesisDepth != 0)
            {
                throw new ParserException("Parenthesis mismath!!!");
            }

            return _astFactory.CreateWhere(expression);
        }


        /// <summary>
        /// BOOLEAN Operation expression Rule
        ///  | BooleanRule
        ///  | BooleanRule AND BooleanOperatorRule
        ///  | BooleanRule OR BooleanOperatorRule
        /// <returns></returns>
        private Expression BooleanOperatorRule()
        {
            Expression booleanExp = BooleanRule();

            _token = _scanner.GetToken();

            switch (_token.Ttype)
            {
                case TokenType.AND:
                    return _astFactory.CreateBooleanAnd(booleanExp,
                                                        BooleanOperatorRule());
                case TokenType.OR:
                    return _astFactory.CreateBooleanOr(booleanExp,
                                                       BooleanOperatorRule());
                case TokenType.RIGHT_PARENTHESIS:
                    _parenthesisDepth--;
                    break;
            }

            return booleanExp;
        }


        /// <summary>
        /// BOOLEAN expression Rule
        ///  | '(' BooleanRule ')'
        ///  | OperationRule
        /// <returns></returns>
        private Expression BooleanRule()
        {
            _token = _scanner.GetToken();

            switch (_token.Ttype)
            {
                case TokenType.LEFT_PARENTHESIS:
                    _parenthesisDepth++;
                    return BooleanOperatorRule();

                case TokenType.FIELD:
                    FieldNode fieldNode = _astFactory.CreateFieldNode(_token.Value);
                    return _astFactory.CreateExpression(ComparationRule(fieldNode));

                default:
                    throw new ParserException("Field expected.");
            }
        }


        /// <summary>
        /// COMPARATION Expression Rule
        ///  | FieldRule COMPARATOR ValueRule
        /// </summary>
        /// <returns>AST Operation Tree</returns>
        /// <exception cref="ParserException"><c>ParserException</c>.</exception>
        private Operation ComparationRule(FieldNode field)
        {
            _token = _scanner.GetToken();

            switch (_token.Ttype)
            {
                case TokenType.LESS:
                    return _astFactory.CreateComparationLess(field,
                                                             ValueRule());
                case TokenType.GREATER:
                    return _astFactory.CreateComparationGreater(field,
                                                                ValueRule());
                case TokenType.LESS_EQ:
                    return _astFactory.CreateComparationLessEqual(field,
                                                                  ValueRule());
                case TokenType.GREATER_EQ:
                    return _astFactory.CreateComparationGraterEqual(field,
                                                                    ValueRule());
                case TokenType.EQ:
                    return IsNullOrEqualRule(field);
                case TokenType.NOT_EQ:
                    return IsNotNullOrNotEqualRule(field);
                case TokenType.IS:
                    return IsNullOrNotIsNullRule(field);

                case TokenType.LIKE:
                    return _astFactory.CreateComparationContains(field, ValueRule());
                default:
                    throw new ParserException(
                            string.Format("Invalid Operation at {0}", _scanner.CurrentPosition));
            }
        }

        /// <summary>
        /// Determines the correct oepration.
        /// If the value token after an Equal, is null the correct 
        /// operation is IsNull
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The operation</returns>
        private Operation IsNullOrEqualRule(FieldNode field)
        {
            ValueNode valueNode = ValueRule();

            if (valueNode == null)
            {
                return _astFactory.CreateComparationIsNull(field);
            }

            return _astFactory.CreateComparationEqual(field, valueNode);
        }

        /// <summary>
        /// Determines the correct oepration.
        /// If the value token after a distinct operator, is null the correct 
        /// operation is IsNotNull
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The operation</returns>
        private Operation IsNotNullOrNotEqualRule(FieldNode field)
        {
            ValueNode valueNode = ValueRule();

            // TODO: Introduce a class for Null Values
            if (valueNode == null)
            {
                return _astFactory.CreateComparationIsNotNull(field);
            }

            return _astFactory.CreateComparationNotEqual(field, valueNode);
        }

        private Operation IsNullOrNotIsNullRule(FieldNode field)
        {
            _token = _scanner.GetToken();
            switch (_token.Ttype)
            {
                case TokenType.NULL:
                    return _astFactory.CreateComparationIsNull(field);
                case TokenType.NOT:
                    _token = _scanner.GetToken();
                    if (_token.Ttype == TokenType.NULL)
                    {
                        return _astFactory.CreateComparationIsNotNull(field);
                    }
                    throw new ParserException(
                            string.Format("Missing NULL at {0}",
                                          _scanner.CurrentPosition));
                default:
                    throw new ParserException(
                            string.Format("Missing NULL at {0}",
                                          _scanner.CurrentPosition));
            }
        }


        /// <summary>
        /// GROUPBY
        ///     | FieldListRule
        /// </summary>
        /// <returns>AST GroupBy Tree</returns>
        private GroupBy GroupByRule()
        {
            return _astFactory.CreateGroupBy(FieldListRule());
        }

        /// <summary>
        /// FIELDLIST
        ///     | FieldNode
        ///     | FieldNode, FIELDLIST
        /// </summary>
        /// <returns>AST FieldList Tree</returns>
        private FieldList FieldListRule()
        {
            FieldNode fieldNode = FieldRule();
            _token = _scanner.GetToken();

            if (_token.Ttype == TokenType.COMMA)
            {
                return _astFactory.CreateFieldList(fieldNode, FieldListRule());
            }

            return _astFactory.CreateFieldList(fieldNode);
        }


        /// <summary>
        /// ORDERBY
        ///  | FieldList
        /// </summary>
        /// <returns>AST Orderby Tree</returns>
        private OrderBy OrderByRule()
        {
            return _astFactory.CreateOrderBy(FieldsListOrderRule());
        }

        /// <summary>
        /// FIELDLISTORDER
        ///     | FieldNodeOrder
        ///     | FieldNodeOrder, FIELDLISTORDER
        /// </summary>
        /// <returns>AST FieldList Tree</returns>
        private FieldList FieldsListOrderRule()
        {
            FieldNode fieldNodeWithOrder = FieldNodeOrderRule();

            _token = _scanner.GetToken();

            if (_token.Ttype == TokenType.COMMA)
            {
                return _astFactory.CreateFieldList(fieldNodeWithOrder,
                                                   FieldsListOrderRule());
            }

            return _astFactory.CreateFieldList(fieldNodeWithOrder);
        }


        /// <summary>
        /// FIELDNODEORDER
        ///     | FieldNode DESC
        ///     | FieldNode ASC
        ///     | FieldNode
        /// </summary>
        /// <returns>AST FieldNode</returns>
        private FieldNode FieldNodeOrderRule()
        {
            FieldNode fieldNode = FieldRule();

            _token = _scanner.GetToken();

            switch (_token.Ttype)
            {
                case TokenType.DESC:
                    fieldNode.Ascending = false;
                    return fieldNode;
                case TokenType.ASC:
                    fieldNode.Ascending = true;
                    return fieldNode;
                default:
                    _scanner.BackToken();
                    break;
            }

            return fieldNode;
        }

        /// <summary>
        /// FIELD expression Rule
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ParserException"><c>ParserException</c>.</exception>
        private FieldNode FieldRule()
        {
            _token = _scanner.GetToken();

            if (_token.Ttype == TokenType.FIELD)
            {
                FieldNode fieldNode = _astFactory.CreateFieldNode(_token.Value);
                return fieldNode;
            }

            throw new ParserException(
                    string.Format("Field name expected at {0}", _scanner.CurrentPosition));
        }

        /// <summary>
        /// VALUE expression Rule
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ParserException"><c>ParserException</c>.</exception>
        private ValueNode ValueRule()
        {
            _token = _scanner.GetToken();

            if (_token.Ttype == TokenType.VALUE)
            {
                return _astFactory.CreateValueNode(_token.Value,
                                                   _token.ValueType.ToString());
            }

            if (_token.Ttype == TokenType.NULL)
            {
                return null;
            }

            throw new ParserException(
                    string.Format("TokenValue expected at {0}", _scanner.CurrentPosition));
        }
    }
}