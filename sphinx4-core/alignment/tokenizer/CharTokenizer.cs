using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class CharTokenizer : Object, Iterator
	{
		public CharTokenizer()
		{
			this.whitespaceSymbols = " \t\n\r";
			this.singleCharSymbols = "(){}[]";
			this.prepunctuationSymbols = "\"'`({[";
			this.postpunctuationSymbols = "\"'`.,:;!?(){}[]";
		}

		public virtual void setWhitespaceSymbols(string symbols)
		{
			this.whitespaceSymbols = symbols;
		}

		public virtual void setSingleCharSymbols(string symbols)
		{
			this.singleCharSymbols = symbols;
		}

		public virtual void setPrepunctuationSymbols(string symbols)
		{
			this.prepunctuationSymbols = symbols;
		}

		public virtual void setPostpunctuationSymbols(string symbols)
		{
			this.postpunctuationSymbols = symbols;
		}

		public virtual void setInputText(string inputString)
		{
			this.inputText = inputString;
			this.currentPosition = 0;
			if (this.inputText != null)
			{
				this.getNextChar();
			}
		}

		public virtual void setInputReader(Reader reader)
		{
			this.reader = reader;
			this.getNextChar();
		}

		private int getNextChar()
		{
			if (this.reader != null)
			{
				try
				{
					int num = this.reader.read();
					if (num == EOF)
					{
						this.currentChar = EOF;
					}
					else
					{
						this.currentChar = (int)((ushort)num);
					}
				}
				catch (IOException ex)
				{
					this.currentChar = EOF;
					this.errorDescription = Throwable.instancehelper_getMessage(ex);
				}
			}
			else if (this.inputText != null)
			{
				if (this.currentPosition < String.instancehelper_length(this.inputText))
				{
					this.currentChar = (int)String.instancehelper_charAt(this.inputText, this.currentPosition);
				}
				else
				{
					this.currentChar = EOF;
				}
			}
			if (this.currentChar != EOF)
			{
				this.currentPosition++;
			}
			if (this.currentChar == 10)
			{
				this.lineNumber++;
			}
			return this.currentChar;
		}

		private string getTokenOfCharClass(string text)
		{
			return this.getTokenByCharClass(text, true);
		}

		private string getTokenNotOfCharClass(string text)
		{
			return this.getTokenByCharClass(text, false);
		}

		private void removeTokenPostpunctuation()
		{
			if (this.token == null)
			{
				return;
			}
			string word = this.token.getWord();
			int num = String.instancehelper_length(word);
			int num2 = num - 1;
			while (num2 > 0 && String.instancehelper_indexOf(this.postpunctuationSymbols, (int)String.instancehelper_charAt(word, num2)) != -1)
			{
				num2 --;
			}
			if (num - 1 != num2)
			{
				this.token.setPostpunctuation(String.instancehelper_substring(word, num2 + 1));
				this.token.setWord(String.instancehelper_substring(word, 0, num2 + 1));
			}
			else
			{
				this.token.setPostpunctuation("");
			}
		}

		private string getTokenByCharClass(string text, bool flag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (String.instancehelper_indexOf(text, this.currentChar) != -1 == flag && String.instancehelper_indexOf(this.singleCharSymbols, this.currentChar) == -1 && this.currentChar != -1)
			{
				stringBuilder.append((char)this.currentChar);
				this.getNextChar();
			}
			return stringBuilder.toString();
		}

		public virtual Token next()
		{
			this.lastToken = this.token;
			this.token = new Token();
			this.token.setWhitespace(this.getTokenOfCharClass(this.whitespaceSymbols));
			this.token.setPrepunctuation(this.getTokenOfCharClass(this.prepunctuationSymbols));
			if (String.instancehelper_indexOf(this.singleCharSymbols, this.currentChar) != -1)
			{
				this.token.setWord(String.valueOf((char)this.currentChar));
				this.getNextChar();
			}
			else
			{
				this.token.setWord(this.getTokenNotOfCharClass(this.whitespaceSymbols));
			}
			this.token.setPosition(this.currentPosition);
			this.token.setLineNumber(this.lineNumber);
			this.removeTokenPostpunctuation();
			return this.token;
		}

		public CharTokenizer(string @string)
		{
			this.whitespaceSymbols = " \t\n\r";
			this.singleCharSymbols = "(){}[]";
			this.prepunctuationSymbols = "\"'`({[";
			this.postpunctuationSymbols = "\"'`.,:;!?(){}[]";
			this.setInputText(@string);
		}

		public CharTokenizer(Reader file)
		{
			this.whitespaceSymbols = " \t\n\r";
			this.singleCharSymbols = "(){}[]";
			this.prepunctuationSymbols = "\"'`({[";
			this.postpunctuationSymbols = "\"'`.,:;!?(){}[]";
			this.setInputReader(file);
		}

		public virtual bool hasNext()
		{
			int num = this.currentChar;
			return num != -1;
		}

		public virtual void remove()
		{

			throw new UnsupportedOperationException();
		}

		public virtual bool hasErrors()
		{
			return this.errorDescription != null;
		}

		public virtual string getErrorDescription()
		{
			return this.errorDescription;
		}

		public virtual bool isSentenceSeparator()
		{
			string whitespace = this.token.getWhitespace();
			string text = null;
			if (this.lastToken != null)
			{
				text = this.lastToken.getPostpunctuation();
			}
			if (this.lastToken == null || this.token == null)
			{
				return false;
			}
			if (String.instancehelper_indexOf(whitespace, 10) != String.instancehelper_lastIndexOf(whitespace, 10))
			{
				return true;
			}
			if (String.instancehelper_indexOf(text, 58) != -1 || String.instancehelper_indexOf(text, 63) != -1 || String.instancehelper_indexOf(text, 33) != -1)
			{
				return true;
			}
			if (String.instancehelper_indexOf(text, 46) != -1 && String.instancehelper_length(whitespace) > 1 && Character.isUpperCase(String.instancehelper_charAt(this.token.getWord(), 0)))
			{
				return true;
			}
			string word = this.lastToken.getWord();
			int num = String.instancehelper_length(word);
			return String.instancehelper_indexOf(text, 46) != -1 && Character.isUpperCase(String.instancehelper_charAt(this.token.getWord(), 0)) && !Character.isUpperCase(String.instancehelper_charAt(word, num - 1)) && (num >= 4 || !Character.isUpperCase(String.instancehelper_charAt(word, 0)));
		}

		object Iterator.next()
		{
			return this.next();
		}

		public const int EOF = -1;

		public const string DEFAULT_WHITESPACE_SYMBOLS = " \t\n\r";

		public const string DEFAULT_SINGLE_CHAR_SYMBOLS = "(){}[]";

		public const string DEFAULT_PREPUNCTUATION_SYMBOLS = "\"'`({[";

		public const string DEFAULT_POSTPUNCTUATION_SYMBOLS = "\"'`.,:;!?(){}[]";

		private int lineNumber;

		private string inputText;

		private Reader reader;

		private int currentChar;

		private int currentPosition;

		private string whitespaceSymbols;

		private string singleCharSymbols;

		private string prepunctuationSymbols;

		private string postpunctuationSymbols;

		private string errorDescription;

		private Token token;

		private Token lastToken;
	}
}
