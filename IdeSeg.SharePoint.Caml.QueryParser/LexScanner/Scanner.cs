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

using System;
using System.Collections.Generic;

namespace IdeSeg.SharePoint.Caml.QueryParser.LexScanner
{
    public class Scanner : ScannerBase
    {
        private const string STOP_CHARS = "@(),!=><&|";

        private readonly Dictionary<string, TokenType> _operators =
            new Dictionary<string, TokenType>
                {
                    {
                        "||", TokenType.OR
                        },
                    {
                        "&&", TokenType.AND
                        },
                    {
                        "<", TokenType.LESS
                        },
                    {
                        "<=", TokenType.LESS_EQ
                        },
                    {
                        ">", TokenType.GREATER
                        },
                    {
                        ">=", TokenType.GREATER_EQ
                        },
                    {
                        "=", TokenType.EQ
                        },
                    {
                        "==", TokenType.EQ
                        },
                    {
                        "<>", TokenType.NOT_EQ
                        },
                    {
                        "!=", TokenType.NOT_EQ
                        },
                    {
                        ",", TokenType.COMMA
                        },
                    {
                        "(", TokenType.LEFT_PARENTHESIS
                        },
                    {
                        ")", TokenType.RIGHT_PARENTHESIS
                        }
                };

        private readonly Dictionary<string, TokenType> _reservedWords =
            new Dictionary<string, TokenType>
                {
                    {
                        "FALSE", TokenType.FALSE
                        },
                    {
                        "GROUPBY", TokenType.GROUPBY
                        },
                    {
                        "IS", TokenType.IS
                        },
                    {
                        "NOT", TokenType.NOT
                        },
                    {
                        "NULL", TokenType.NULL
                        },
                    {
                        "TRUE", TokenType.TRUE
                        },
                    {
                        "ORDERBY", TokenType.ORDERBY
                        },
                    {
                        "WHERE", TokenType.WHERE
                        },
                    {
                        "AND", TokenType.AND
                        },
                    {
                        "OR", TokenType.OR
                        },
                    {
                        "ASC", TokenType.ASC
                        },
                    {
                        "DESC", TokenType.DESC
                        },
                    {
                        "LIKE", TokenType.LIKE
                        },
                };

        private Token _token;

        public Scanner(string input) : base(input)
        {
        }

        /// <summary>
        /// Checks the correct brackets and quotes.
        /// </summary>
        /// <exception cref="ScannerException"><c>LexerException</c>.</exception>
        protected override void CheckCorrectBracketsAndQuotes()
        {
            if (_inQuotes)
            {
                throw new ScannerException(
                    string.Format("Quote expected at {0}", CurrentPosition));
            }
            if (_inBracket)
            {
                throw new ScannerException(
                    string.Format("Bracket expected at {0}", CurrentPosition));
            }
            if (_inSharp)
            {
                throw new ScannerException(
                    string.Format("Sharp expected at {0}", CurrentPosition));
            }
        }

        public void BackToken()
        {
            RestoreScannerState();
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <returns>The token Type</returns>
        /// <exception cref="ScannerException"><c>LexerException</c>.</exception>
        public override Token GetToken()
        {
            SaveScannerState();

            EatSpaces();

            if (EndOfLine())
            {
                return new Token(TokenType.EOL);
            }

            MarkStartOfToken();

            if (char.IsLetter(CurrentChar))
            {
                _token = ScanReservedWordOrSymbol();
            }
            else if (Char.IsDigit(CurrentChar))
            {
                _token = ScanNumber();
            }
            else if (CurrentChar == '[')
            {
                _token = ScanBracketSymbol();
            }
            else if (CurrentChar == '"')
            {
                _token = ScanString();
            }
            else if (CurrentChar == '#')
            {
                _token = ScanDate();
            }
            else if (STOP_CHARS.IndexOf(CurrentChar) != -1 && !_inQuotes)
            {
                _token = ScanOperator();
            }

            if (_token == null)
            {
                throw new ScannerException(
                    string.Format("Unknow character at {0}", CurrentPosition));
            }

            SetStateToken();

            return _token;
        }

        /// <summary>
        /// Scans the reserved word or symbol.
        /// </summary>
        /// <returns>TokenType</returns>
        private Token ScanReservedWordOrSymbol()
        {
            string word = string.Empty;

            while (!EndOfLine() && char.IsLetterOrDigit(CurrentChar))
            {
                word += GetCharMoveNext();
            }

            if (_reservedWords.ContainsKey(word.ToUpper()))
            {
                return new Token(_reservedWords[word.ToUpper()]);
            }

            return new Token(TokenType.FIELD, TokenValueType.Identifier, word);
        }

        /// <summary>
        /// Scans the bracket symbol.
        /// </summary>
        /// <returns>TokenType.FIELD</returns>
        private Token ScanBracketSymbol()
        {
            string words = string.Empty;

            _inBracket = true;

            SkipChar();

            while (!EndOfLine() && CurrentChar != ']')
            {
                words += GetCharMoveNext();
            }

            SkipChar();

            _inBracket = false;

            return new Token(TokenType.FIELD, TokenValueType.Identifier, words);
        }

        /// <summary>
        /// Scans a number.
        /// </summary>
        /// <returns>TokenType.VALUE</returns>
        private Token ScanNumber()
        {
            string number = string.Empty;

            while (!EndOfLine()
                   && (char.IsDigit(CurrentChar)
                       || CurrentChar == '.'
                       || CurrentChar == ','))
            {
                number += GetCharMoveNext();
            }

            return new Token(TokenType.VALUE, TokenValueType.Numeric, number);
        }

        /// <summary>
        /// Scans a string.
        /// </summary>
        /// <returns>TokenType.VALUE</returns>
        private Token ScanString()
        {
            string words = string.Empty;

            _inQuotes = true;

            SkipChar();

            while (!EndOfLine() && CurrentChar != '"')
            {
                words += GetCharMoveNext();
            }

            SkipChar();

            _inQuotes = false;

            return new Token(TokenType.VALUE, TokenValueType.Text, words);
        }

        /// <summary>
        /// Scans a Date.
        /// </summary>
        /// <returns>TokenType.VALUE</returns>
        private Token ScanDate()
        {
            string date = string.Empty;

            _inSharp = true;

            SkipChar();

            while (!EndOfLine() && CurrentChar != '#')
            {
                date += GetCharMoveNext();
            }

            SkipChar();

            _inSharp = false;

            return new Token(TokenType.VALUE, TokenValueType.DateTime, date);
        }

        /// <summary>
        /// Scans an operator.
        /// </summary>
        /// <returns>TokenType with the operator value</returns>
        private Token ScanOperator()
        {
            string oper = string.Empty;

            while (!EndOfLine()
                   && STOP_CHARS.IndexOf(CurrentChar) != -1)
            {
                if (CurrentChar == '(' || CurrentChar == ')')
                {
                    oper += GetCharMoveNext();
                    break;
                }

                oper += GetCharMoveNext();
            }

            if (!_operators.ContainsKey(oper))
            {
                throw new ScannerException(
                    string.Format("Illegal operator {0}", oper));
            }

            return new Token(_operators[oper]);
        }

        private void SaveScannerState()
        {
            _prevoiusState = new ScannerState(_currentState);
        }

        private void RestoreScannerState()
        {
            _currentState = _prevoiusState;
        }

        private void SetStateToken()
        {
            _currentState.ReadToken = _token;
        }
    }
}