using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class ExtendedStreamTokenizer : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			122,
			66,
			109
		})]
		
		public ExtendedStreamTokenizer(InputStream inputStream, bool eolIsSignificant) : this(new InputStreamReader(inputStream), eolIsSignificant)
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			136,
			130,
			103,
			178,
			2,
			97,
			159,
			1
		})]
		
		public virtual int getInt(string name)
		{
			int result = 0;
			try
			{
				string @string = this.getString();
				result = Integer.parseInt(@string);
			}
			catch (NumberFormatException ex)
			{
				goto IL_18;
			}
			return result;
			IL_18:
			this.corrupt(new StringBuilder().append("while parsing int ").append(name).toString());
			return result;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			101,
			103,
			105,
			191,
			24
		})]
		
		public virtual void expectString(string expecting)
		{
			string @string = this.getString();
			if (!java.lang.String.instancehelper_equals(@string, expecting))
			{
				this.corrupt(new StringBuilder().append("error matching expected string '").append(expecting).append("' in line: '").append(@string).append('\'').toString());
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			59,
			107
		})]
		
		public virtual void close()
		{
			this.reader.close();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			182,
			134,
			103,
			109,
			136,
			210,
			2,
			97,
			159,
			1
		})]
		
		public virtual float getFloat(string name)
		{
			float result = 0f;
			try
			{
				string @string = this.getString();
				if (java.lang.String.instancehelper_equals(@string, "inf"))
				{
					result = float.PositiveInfinity;
				}
				else
				{
					result = Float.parseFloat(@string);
				}
			}
			catch (NumberFormatException ex)
			{
				goto IL_31;
			}
			return result;
			IL_31:
			this.corrupt(new StringBuilder().append("while parsing float ").append(name).toString());
			return result;
		}

		[LineNumberTable(new byte[]
		{
			159,
			126,
			130,
			111,
			103
		})]
		
		public ExtendedStreamTokenizer(InputStream inputStream, int commentChar, bool eolIsSignificant) : this(new InputStreamReader(inputStream), eolIsSignificant)
		{
			this.commentChar(commentChar);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			93,
			109,
			158,
			108,
			110,
			135,
			191,
			13,
			139,
			157,
			130
		})]
		
		public virtual string getString()
		{
			if (!this.putbackList.isEmpty())
			{
				return (string)this.putbackList.remove(this.putbackList.size() - 1);
			}
			this.st.nextToken();
			if (this.st.ttype == -1)
			{
				this.atEOF = true;
			}
			if (this.st.ttype != -3 && this.st.ttype != 10 && this.st.ttype != -1)
			{
				this.corrupt("word expected but not found");
			}
			if (this.st.ttype == 10 || this.st.ttype == -1)
			{
				return null;
			}
			return this.st.sval;
		}

		public virtual bool isEOF()
		{
			return this.atEOF;
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			129,
			66,
			111,
			103
		})]
		
		public ExtendedStreamTokenizer(string path, bool eolIsSignificant) : this(new FileReader(path), eolIsSignificant)
		{
			this.path = path;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			231,
			130,
			104,
			106,
			103,
			162
		})]
		
		public virtual void skipwhite()
		{
			while (!this.isEOF())
			{
				string @string;
				if ((@string = this.getString()) != null)
				{
					this.unget(@string);
					break;
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			120,
			162,
			104,
			140,
			108,
			107,
			110,
			114,
			108,
			107
		})]
		
		public ExtendedStreamTokenizer(Reader reader, bool eolIsSignificant)
		{
			this.reader = new BufferedReader(reader);
			this.st = new StreamTokenizer(reader);
			this.st.resetSyntax();
			this.st.whitespaceChars(0, 32);
			this.st.wordChars(33, 255);
			this.st.eolIsSignificant(eolIsSignificant);
			this.putbackList = new ArrayList();
		}

		[LineNumberTable(new byte[]
		{
			81,
			108
		})]
		
		public virtual void commentChar(int ch)
		{
			this.st.commentChar(ch);
		}

		[Throws(new string[]
		{
			"java.io.StreamCorruptedException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			77,
			123
		})]
		
		private void corrupt(string text)
		{
			string text2 = new StringBuilder().append(text).append(" at line ").append(this.st.lineno()).append(" in file ").append(this.path).toString();
			
			throw new StreamCorruptedException(text2);
		}

		[LineNumberTable(new byte[]
		{
			121,
			109
		})]
		
		public virtual void unget(string @string)
		{
			this.putbackList.add(@string);
		}

		[Throws(new string[]
		{
			"java.io.FileNotFoundException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			180,
			104
		})]
		
		public ExtendedStreamTokenizer(string path) : this(path, false)
		{
		}

		[LineNumberTable(new byte[]
		{
			70,
			109
		})]
		
		public virtual void whitespaceChars(int low, int hi)
		{
			this.st.whitespaceChars(low, hi);
		}

		
		
		public virtual int getLineNumber()
		{
			return this.st.lineno();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			119,
			104,
			100,
			159,
			1
		})]
		
		public virtual void expectInt(string name, int expecting)
		{
			int @int = this.getInt(name);
			if (@int != expecting)
			{
				this.corrupt(new StringBuilder().append("Expecting integer ").append(expecting).toString());
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			157,
			134,
			103,
			109,
			140,
			211,
			2,
			97,
			159,
			1
		})]
		
		public virtual double getDouble(string name)
		{
			double result = (double)0f;
			try
			{
				string @string = this.getString();
				if (java.lang.String.instancehelper_equals(@string, "inf"))
				{
					result = double.PositiveInfinity;
				}
				else
				{
					result = Double.parseDouble(@string);
				}
			}
			catch (NumberFormatException ex)
			{
				goto IL_36;
			}
			return result;
			IL_36:
			this.corrupt(new StringBuilder().append("while parsing double ").append(name).toString());
			return result;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			208,
			134,
			103,
			99,
			101,
			109,
			136,
			210,
			2,
			97,
			159,
			1
		})]
		
		public virtual float getFloat(string name, float defaultValue)
		{
			float result = 0f;
			try
			{
				string @string = this.getString();
				if (@string == null)
				{
					result = defaultValue;
				}
				else if (java.lang.String.instancehelper_equals(@string, "inf"))
				{
					result = float.PositiveInfinity;
				}
				else
				{
					result = Float.parseFloat(@string);
				}
			}
			catch (NumberFormatException ex)
			{
				goto IL_39;
			}
			return result;
			IL_39:
			this.corrupt(new StringBuilder().append("while parsing float ").append(name).toString());
			return result;
		}

		private string path;

		
		private StreamTokenizer st;

		
		private Reader reader;

		private bool atEOF;

		
		
		private List putbackList;
	}
}
