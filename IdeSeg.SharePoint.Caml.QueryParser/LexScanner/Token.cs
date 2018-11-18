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

namespace IdeSeg.SharePoint.Caml.QueryParser.LexScanner
{
    public class Token
    {
        private string _value;
        private TokenValueType _valueType;
        private TokenType _ttype;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public TokenValueType ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        public TokenType Ttype
        {
            get { return _ttype; }
            set { _ttype = value; }
        }


        public Token(TokenType tokenType, TokenValueType tokenValueType, string tokenValue)
        {
            if ((tokenType == TokenType.FIELD
                 || tokenType == TokenType.VALUE)
                && (tokenValueType == TokenValueType.None))
            {
                throw new ArgumentException("TokenValueType None is not allowed with TokenType VALUE or FIELD.");
            }

            if ((tokenType != TokenType.FIELD
                 && tokenType != TokenType.VALUE)
                && (tokenValueType != TokenValueType.None))
            {
                throw new ArgumentException("Only TokenValueType is allowed for this TokenType.");
            }

            _ttype = tokenType;
            _valueType = tokenValueType;
            _value = tokenValue;
        }

        public Token(TokenType tokenType) : this(tokenType, TokenValueType.None, string.Empty)
        {
        }
    }
}