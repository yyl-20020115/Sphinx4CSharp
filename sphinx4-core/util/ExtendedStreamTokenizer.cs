using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class ExtendedStreamTokenizer : java.lang.Object
	{		
		public ExtendedStreamTokenizer(InputStream inputStream, bool eolIsSignificant) : this(new InputStreamReader(inputStream), eolIsSignificant)
		{
		}
		
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
		
		public virtual void expectString(string expecting)
		{
			string @string = this.getString();
			if (!java.lang.String.instancehelper_equals(@string, expecting))
			{
				this.corrupt(new StringBuilder().append("error matching expected string '").append(expecting).append("' in line: '").append(@string).append('\'').toString());
			}
		}
		
		public virtual void close()
		{
			this.reader.close();
		}
		
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
		
		public ExtendedStreamTokenizer(InputStream inputStream, int commentChar, bool eolIsSignificant) : this(new InputStreamReader(inputStream), eolIsSignificant)
		{
			this.commentChar(commentChar);
		}
		
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
		
		public ExtendedStreamTokenizer(string path, bool eolIsSignificant) : this(new FileReader(path), eolIsSignificant)
		{
			this.path = path;
		}
		
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
		
		public virtual void commentChar(int ch)
		{
			this.st.commentChar(ch);
		}
		
		private void corrupt(string text)
		{
			string text2 = new StringBuilder().append(text).append(" at line ").append(this.st.lineno()).append(" in file ").append(this.path).toString();
			
			throw new StreamCorruptedException(text2);
		}
		
		public virtual void unget(string @string)
		{
			this.putbackList.add(@string);
		}
		
		public ExtendedStreamTokenizer(string path) : this(path, false)
		{
		}
		
		public virtual void whitespaceChars(int low, int hi)
		{
			this.st.whitespaceChars(low, hi);
		}
		
		public virtual int getLineNumber()
		{
			return this.st.lineno();
		}
		
		public virtual void expectInt(string name, int expecting)
		{
			int @int = this.getInt(name);
			if (@int != expecting)
			{
				this.corrupt(new StringBuilder().append("Expecting integer ").append(expecting).toString());
			}
		}
		
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
					result = java.lang.Double.parseDouble(@string);
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
