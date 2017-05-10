using java.lang;

namespace edu.cmu.sphinx.alignment
{
	public class Token : java.lang.Object
	{	
		public Token()
		{
			this.token = null;
			this.whitespace = null;
			this.prepunctuation = null;
			this.postpunctuation = null;
			this.position = 0;
			this.lineNumber = 0;
		}

		public virtual string getWhitespace()
		{
			return this.whitespace;
		}

		public virtual string getPrepunctuation()
		{
			return this.prepunctuation;
		}

		public virtual string getPostpunctuation()
		{
			return this.postpunctuation;
		}

		public virtual int getPosition()
		{
			return this.position;
		}

		public virtual int getLineNumber()
		{
			return this.lineNumber;
		}

		public virtual void setWhitespace(string whitespace)
		{
			this.whitespace = whitespace;
		}

		public virtual void setPrepunctuation(string prepunctuation)
		{
			this.prepunctuation = prepunctuation;
		}

		public virtual void setPostpunctuation(string postpunctuation)
		{
			this.postpunctuation = postpunctuation;
		}

		public virtual void setPosition(int position)
		{
			this.position = position;
		}

		public virtual void setLineNumber(int lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		public virtual string getWord()
		{
			return this.token;
		}

		public virtual void setWord(string word)
		{
			this.token = word;
		}
	
		public override string toString()
		{
			StringBuffer stringBuffer = new StringBuffer();
			if (this.whitespace != null)
			{
				stringBuffer.append(this.whitespace);
			}
			if (this.prepunctuation != null)
			{
				stringBuffer.append(this.prepunctuation);
			}
			if (this.token != null)
			{
				stringBuffer.append(this.token);
			}
			if (this.postpunctuation != null)
			{
				stringBuffer.append(this.postpunctuation);
			}
			return stringBuffer.toString();
		}

		private string token;

		private string whitespace;

		private string prepunctuation;

		private string postpunctuation;

		private int position;

		private int lineNumber;
	}
}
