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

namespace IdeSeg.SharePoint.Caml.QueryParser.LexScanner
{
    public abstract class ScannerBase
    {
        protected ScannerState _currentState;
        protected ScannerState _prevoiusState;

        protected string _input;

        protected bool _inQuotes;
        protected bool _inBracket;
        protected bool _inSharp;

        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <value>The current position.</value>
        public int CurrentPosition
        {
            get { return _currentState.CurrentPosition; }
        }

        /// <summary>
        /// Gets the start position.
        /// </summary>
        /// <value>The start position.</value>
        public int StartPosition
        {
            get { return _currentState.StartPosition; }
        }

        /// <summary>
        /// Gets the current char.
        /// </summary>
        /// <value>The current char.</value>
        protected char CurrentChar
        {
            get { return _input[CurrentPosition]; }
        }

        /// <summary>
        /// Gets the last token.
        /// </summary>
        /// <value>The last token.</value>
        public Token LastToken
        {
            get { return _currentState.ReadToken; }
        }


        protected ScannerBase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ScannerException("Input can´t be null or empty");
            }

            _input = input;
            _currentState = new ScannerState();
        }

        /// <summary>
        /// Eats spaces.
        /// </summary>
        protected void EatSpaces()
        {
            while (!EndOfLine() && CurrentChar == ' ')
            {
                SkipChar();
            }
        }

        /// <summary>
        /// Skips the char.
        /// </summary>
        /// <exception cref="ScannerException"><c>LexerException</c>.</exception>
        protected void SkipChar()
        {
            if (!EndOfLine())
            {
                _currentState.CurrentPosition++;
            }
        }

        /// <summary>
        /// Gets the char and move to the next.
        /// </summary>
        /// <returns>The current char</returns>
        protected char GetCharMoveNext()
        {
            char currentChar = _input[_currentState.CurrentPosition];
            SkipChar();
            return currentChar;
        }

        /// <summary>
        /// Check for the end of line
        /// </summary>
        /// <returns>True if EOL</returns>
        /// <exception cref="ScannerException"><c>LexerException</c>.</exception>
        protected bool EndOfLine()
        {
            if (_currentState.CurrentPosition + 1 > _input.Length)
            {
                CheckCorrectBracketsAndQuotes();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Marks the start of token.
        /// </summary>
        protected void MarkStartOfToken()
        {
            _currentState.StartPosition = _currentState.CurrentPosition;
        }

        /// <summary>
        /// Checks the correct brackets and quotes.
        /// </summary>
        /// <exception cref="ScannerException"><c>LexerException</c>.</exception>
        protected abstract void CheckCorrectBracketsAndQuotes();

        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        /// <returns>The TokenType</returns>
        public abstract Token GetToken();
    }
}