using System;
using System.ComponentModel;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	[Implements(new string[]
	{
		"java.util.Iterator"
	})]
	
	public class CharTokenizer : java.lang.Object, Iterator
	{
		[LineNumberTable(new byte[]
		{
			25,
			232,
			49,
			107,
			107,
			107,
			235,
			76
		})]
		
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

		[LineNumberTable(new byte[]
		{
			88,
			103,
			135,
			104,
			135
		})]
		
		public virtual void setInputText(string inputString)
		{
			this.inputText = inputString;
			this.currentPosition = 0;
			if (this.inputText != null)
			{
				this.getNextChar();
			}
		}

		[LineNumberTable(new byte[]
		{
			102,
			103,
			103
		})]
		
		public virtual void setInputReader(Reader reader)
		{
			this.reader = reader;
			this.getNextChar();
		}

		[LineNumberTable(new byte[]
		{
			160,
			100,
			136,
			108,
			100,
			137,
			250,
			69,
			226,
			61,
			97,
			103,
			108,
			98,
			104,
			115,
			153,
			167,
			105,
			142,
			106,
			142
		})]
		
		private int getNextChar()
		{
			if (this.reader != null)
			{
				IOException ex2;
				try
				{
					int num = this.reader.read();
					if (num == -1)
					{
						this.currentChar = -1;
					}
					else
					{
						this.currentChar = (int)((ushort)num);
					}
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_38;
				}
				goto IL_8E;
				IL_38:
				IOException ex3 = ex2;
				this.currentChar = -1;
				this.errorDescription = Throwable.instancehelper_getMessage(ex3);
			}
			else if (this.inputText != null)
			{
				if (this.currentPosition < java.lang.String.instancehelper_length(this.inputText))
				{
					this.currentChar = (int)java.lang.String.instancehelper_charAt(this.inputText, this.currentPosition);
				}
				else
				{
					this.currentChar = -1;
				}
			}
			IL_8E:
			if (this.currentChar != -1)
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

		[LineNumberTable(new byte[]
		{
			160,
			203,
			104,
			129,
			140,
			103,
			132,
			140,
			37,
			136,
			166,
			134,
			180,
			151,
			144
		})]
		
		private void removeTokenPostpunctuation()
		{
			if (this.token == null)
			{
				return;
			}
			string word = this.token.getWord();
			int num = java.lang.String.instancehelper_length(word);
			int num2 = num - 1;
			while (num2 > 0 && java.lang.String.instancehelper_indexOf(this.postpunctuationSymbols, (int)java.lang.String.instancehelper_charAt(word, num2)) != -1)
			{
				num2 += -1;
			}
			if (num - 1 != num2)
			{
				this.token.setPostpunctuation(java.lang.String.instancehelper_substring(word, num2 + 1));
				this.token.setWord(java.lang.String.instancehelper_substring(word, 0, num2 + 1));
			}
			else
			{
				this.token.setPostpunctuation("");
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			69,
			130,
			230,
			72,
			127,
			2,
			145,
			110,
			137
		})]
		
		private string getTokenByCharClass(string text, bool flag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (java.lang.String.instancehelper_indexOf(text, this.currentChar) != -1 == flag && java.lang.String.instancehelper_indexOf(this.singleCharSymbols, this.currentChar) == -1 && this.currentChar != -1)
			{
				stringBuilder.append((char)this.currentChar);
				this.getNextChar();
			}
			return stringBuilder.toString();
		}

		[LineNumberTable(new byte[]
		{
			112,
			108,
			171,
			247,
			69,
			183,
			116,
			119,
			137,
			183,
			113,
			209,
			134
		})]
		
		public virtual Token next()
		{
			this.lastToken = this.token;
			this.token = new Token();
			this.token.setWhitespace(this.getTokenOfCharClass(this.whitespaceSymbols));
			this.token.setPrepunctuation(this.getTokenOfCharClass(this.prepunctuationSymbols));
			if (java.lang.String.instancehelper_indexOf(this.singleCharSymbols, this.currentChar) != -1)
			{
				this.token.setWord(java.lang.String.valueOf((char)this.currentChar));
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

		[LineNumberTable(new byte[]
		{
			32,
			232,
			42,
			107,
			107,
			107,
			235,
			84,
			103
		})]
		
		public CharTokenizer(string @string)
		{
			this.whitespaceSymbols = " \t\n\r";
			this.singleCharSymbols = "(){}[]";
			this.prepunctuationSymbols = "\"'`({[";
			this.postpunctuationSymbols = "\"'`.,:;!?(){}[]";
			this.setInputText(@string);
		}

		[LineNumberTable(new byte[]
		{
			41,
			232,
			33,
			107,
			107,
			107,
			235,
			93,
			103
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			254,
			108,
			98,
			104,
			172,
			112,
			98,
			107,
			103,
			98,
			110,
			107,
			104,
			98,
			108,
			110,
			114,
			130,
			108,
			135,
			177,
			214,
			37,
			141,
			108,
			162
		})]
		
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
			if (java.lang.String.instancehelper_indexOf(whitespace, 10) != java.lang.String.instancehelper_lastIndexOf(whitespace, 10))
			{
				return true;
			}
			if (java.lang.String.instancehelper_indexOf(text, 58) != -1 || java.lang.String.instancehelper_indexOf(text, 63) != -1 || java.lang.String.instancehelper_indexOf(text, 33) != -1)
			{
				return true;
			}
			if (java.lang.String.instancehelper_indexOf(text, 46) != -1 && java.lang.String.instancehelper_length(whitespace) > 1 && Character.isUpperCase(java.lang.String.instancehelper_charAt(this.token.getWord(), 0)))
			{
				return true;
			}
			string word = this.lastToken.getWord();
			int num = java.lang.String.instancehelper_length(word);
			return java.lang.String.instancehelper_indexOf(text, 46) != -1 && Character.isUpperCase(java.lang.String.instancehelper_charAt(this.token.getWord(), 0)) && !Character.isUpperCase(java.lang.String.instancehelper_charAt(word, num - 1)) && (num >= 4 || !Character.isUpperCase(java.lang.String.instancehelper_charAt(word, 0)));
		}

		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public virtual object <bridge>next()
		{
			return this.next();
		}

		
		object Iterator.Object;next()
		{
			return this.<bridge>next();
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
