using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	public class JavaCharStream : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			161,
			4,
			110,
			118,
			115
		})]
		public virtual void backup(int amount)
		{
			this.inBuf += amount;
			int num = this.bufpos - amount;
			int num2 = num;
			this.bufpos = num;
			if (num2 < 0)
			{
				this.bufpos += this.bufsize;
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			87,
			109,
			178,
			191,
			9,
			107,
			171,
			127,
			1,
			129,
			97,
			136,
			110,
			201,
			115,
			147
		})]
		
		protected internal virtual void FillBuff()
		{
			if (this.maxNextCharInd == 4096)
			{
				int num = 0;
				int num2 = num;
				this.nextCharInd = num;
				this.maxNextCharInd = num2;
			}
			IOException ex2;
			try
			{
				int num3;
				if ((num3 = this.inputStream.read(this.nextCharBuf, this.maxNextCharInd, 4096 - this.maxNextCharInd)) == -1)
				{
					this.inputStream.close();
					
					throw new IOException();
				}
				this.maxNextCharInd += num3;
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_7A;
			}
			return;
			IL_7A:
			IOException ex3 = ex2;
			if (this.bufpos != 0)
			{
				this.bufpos--;
				this.backup(0);
			}
			else
			{
				this.bufline[this.bufpos] = this.line;
				this.bufcolumn[this.bufpos] = this.column;
			}
			throw Throwable.__<unmap>(ex3);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			141,
			137,
			142,
			123,
			135,
			238,
			69,
			123,
			134,
			159,
			6,
			135,
			195,
			123,
			198,
			159,
			3,
			135,
			140,
			118,
			159,
			2,
			162,
			104,
			241,
			74,
			226,
			57,
			161,
			101,
			138,
			163,
			103,
			235,
			70,
			108,
			144,
			118,
			110,
			110,
			143,
			249,
			70,
			226,
			60,
			129,
			223,
			42,
			101,
			162,
			106,
			227,
			69,
			103
		})]
		
		public virtual char readChar()
		{
			int num;
			if (this.inBuf > 0)
			{
				this.inBuf--;
				num = this.bufpos + 1;
				int num2 = num;
				this.bufpos = num;
				if (num2 == this.bufsize)
				{
					this.bufpos = 0;
				}
				return this.buffer[this.bufpos];
			}
			num = this.bufpos + 1;
			int num3 = num;
			this.bufpos = num;
			if (num3 == this.available)
			{
				this.AdjustBuffSize();
			}
			char[] array = this.buffer;
			int num4 = this.bufpos;
			int num5 = num = (int)this.ReadByte();
			int num6 = num4;
			char[] array2 = array;
			int num7 = num;
			array2[num6] = (char)num;
			if (num7 == 92)
			{
				this.UpdateLineColumn((char)num5);
				int num8 = 1;
				for (;;)
				{
					num = this.bufpos + 1;
					int num9 = num;
					this.bufpos = num;
					if (num9 == this.available)
					{
						this.AdjustBuffSize();
					}
					try
					{
						char[] array3 = this.buffer;
						int num10 = this.bufpos;
						num5 = (num = (int)this.ReadByte());
						num6 = num10;
						array2 = array3;
						int num11 = num;
						array2[num6] = (char)num;
						if (num11 != 92)
						{
							this.UpdateLineColumn((char)num5);
							if (num5 == 117 && (num8 & 1) == 1)
							{
								num = this.bufpos - 1;
								int num12 = num;
								this.bufpos = num;
								if (num12 < 0)
								{
									this.bufpos = this.bufsize - 1;
								}
								goto IL_11F;
							}
							break;
						}
					}
					catch (IOException ex)
					{
						goto IL_121;
					}
					this.UpdateLineColumn((char)num5);
					num8++;
				}
				try
				{
					this.backup(num8);
					num = 92;
				}
				catch (IOException ex2)
				{
					goto IL_13B;
				}
				return (char)num;
				IL_13B:
				goto IL_141;
				IL_11F:
				try
				{
					while ((num5 = (int)this.ReadByte()) == 117)
					{
						this.column++;
					}
					num5 = (int)(this.buffer[this.bufpos] = (char)(JavaCharStream.hexval((char)num5) << 12 | JavaCharStream.hexval(this.ReadByte()) << 8 | JavaCharStream.hexval(this.ReadByte()) << 4 | JavaCharStream.hexval(this.ReadByte())));
					this.column += 4;
				}
				catch (IOException ex3)
				{
					goto IL_1D9;
				}
				if (num8 == 1)
				{
					return (char)num5;
				}
				this.backup(num8 - 1);
				return '\\';
				IL_1D9:
				string text = new StringBuilder().append("Invalid escape character at line ").append(this.line).append(" column ").append(this.column).append(".").toString();
				
				throw new Error(text);
				IL_121:
				IL_141:
				if (num8 > 1)
				{
					this.backup(num8 - 1);
				}
				return '\\';
			}
			this.UpdateLineColumn((char)num5);
			return (char)num5;
		}

		[LineNumberTable(new byte[]
		{
			159,
			120,
			98,
			114,
			114,
			210,
			134,
			127,
			1,
			127,
			1,
			135,
			127,
			1,
			127,
			1,
			135,
			127,
			1,
			127,
			1,
			135,
			223,
			0,
			127,
			1,
			135,
			127,
			1,
			135,
			127,
			1,
			135,
			255,
			8,
			70,
			226,
			61,
			130,
			178,
			127,
			3,
			103
		})]
		
		protected internal virtual void ExpandBuff(bool wrapAround)
		{
			char[] array = new char[this.bufsize + 2048];
			int[] array2 = new int[this.bufsize + 2048];
			int[] array3 = new int[this.bufsize + 2048];
			Exception ex2;
			try
			{
				if (wrapAround)
				{
					ByteCodeHelper.arraycopy_primitive_2(this.buffer, this.tokenBegin, array, 0, this.bufsize - this.tokenBegin);
					ByteCodeHelper.arraycopy_primitive_2(this.buffer, 0, array, this.bufsize - this.tokenBegin, this.bufpos);
					this.buffer = array;
					ByteCodeHelper.arraycopy_primitive_4(this.bufline, this.tokenBegin, array2, 0, this.bufsize - this.tokenBegin);
					ByteCodeHelper.arraycopy_primitive_4(this.bufline, 0, array2, this.bufsize - this.tokenBegin, this.bufpos);
					this.bufline = array2;
					ByteCodeHelper.arraycopy_primitive_4(this.bufcolumn, this.tokenBegin, array3, 0, this.bufsize - this.tokenBegin);
					ByteCodeHelper.arraycopy_primitive_4(this.bufcolumn, 0, array3, this.bufsize - this.tokenBegin, this.bufpos);
					this.bufcolumn = array3;
					this.bufpos += this.bufsize - this.tokenBegin;
				}
				else
				{
					ByteCodeHelper.arraycopy_primitive_2(this.buffer, this.tokenBegin, array, 0, this.bufsize - this.tokenBegin);
					this.buffer = array;
					ByteCodeHelper.arraycopy_primitive_4(this.bufline, this.tokenBegin, array2, 0, this.bufsize - this.tokenBegin);
					this.bufline = array2;
					ByteCodeHelper.arraycopy_primitive_4(this.bufcolumn, this.tokenBegin, array3, 0, this.bufsize - this.tokenBegin);
					this.bufcolumn = array3;
					this.bufpos -= this.tokenBegin;
				}
			}
			catch (Exception ex)
			{
				ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				goto IL_1CA;
			}
			int num = this.bufsize + 2048;
			int num2 = num;
			this.bufsize = num;
			this.available = num2;
			this.tokenBegin = 0;
			return;
			IL_1CA:
			Exception ex3 = ex2;
			string text = Throwable.instancehelper_getMessage(ex3);
			
			throw new Error(text);
		}

		[LineNumberTable(new byte[]
		{
			160,
			81,
			142,
			141,
			103,
			174,
			137,
			110,
			110,
			116,
			137,
			108
		})]
		
		protected internal virtual void AdjustBuffSize()
		{
			if (this.available == this.bufsize)
			{
				if (this.tokenBegin > 2048)
				{
					this.bufpos = 0;
					this.available = this.tokenBegin;
				}
				else
				{
					this.ExpandBuff(false);
				}
			}
			else if (this.available > this.tokenBegin)
			{
				this.available = this.bufsize;
			}
			else if (this.tokenBegin - this.available < 2048)
			{
				this.ExpandBuff(true);
			}
			else
			{
				this.available = this.tokenBegin;
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			118,
			123,
			134
		})]
		
		protected internal virtual char ReadByte()
		{
			int num = this.nextCharInd + 1;
			int num2 = num;
			this.nextCharInd = num;
			if (num2 >= this.maxNextCharInd)
			{
				this.FillBuff();
			}
			return this.nextCharBuf[this.nextCharInd];
		}

		[LineNumberTable(new byte[]
		{
			159,
			89,
			162,
			142,
			136,
			103,
			155,
			136,
			103,
			133,
			169,
			185,
			191,
			0,
			103,
			130,
			103,
			130,
			110,
			127,
			11,
			226,
			69,
			115,
			115
		})]
		protected internal virtual void UpdateLineColumn(char c)
		{
			this.column++;
			if (this.prevCharIsLF)
			{
				this.prevCharIsLF = false;
				int num = this.line;
				int num2 = 1;
				int num3 = num2;
				this.column = num2;
				this.line = num + num3;
			}
			else if (this.prevCharIsCR)
			{
				this.prevCharIsCR = false;
				if (c == '\n')
				{
					this.prevCharIsLF = true;
				}
				else
				{
					int num4 = this.line;
					int num2 = 1;
					int num5 = num2;
					this.column = num2;
					this.line = num4 + num5;
				}
			}
			switch (c)
			{
			case '\t':
			{
				this.column--;
				int num6 = this.column;
				int num7 = this.tabSize;
				int num8 = this.column;
				int num9 = this.tabSize;
				this.column = num6 + (num7 - ((num9 != -1) ? (num8 % num9) : 0));
				break;
			}
			case '\n':
				this.prevCharIsLF = true;
				break;
			case '\r':
				this.prevCharIsCR = true;
				break;
			}
			this.bufline[this.bufpos] = this.line;
			this.bufcolumn[this.bufpos] = this.column;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			159,
			138,
			66,
			191,
			160,
			136,
			130,
			130,
			130,
			130,
			130,
			130,
			130,
			130,
			130,
			195,
			163,
			163,
			163,
			163,
			163,
			163
		})]
		
		internal static int hexval(char c)
		{
			switch (c)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case 'A':
			case 'a':
				return 10;
			case 'B':
			case 'b':
				return 11;
			case 'C':
			case 'c':
				return 12;
			case 'D':
			case 'd':
				return 13;
			case 'E':
			case 'e':
				return 14;
			case 'F':
			case 'f':
				return 15;
			}
			
			throw new IOException();
		}

		[LineNumberTable(new byte[]
		{
			161,
			11,
			232,
			158,
			193,
			231,
			71,
			103,
			135,
			103,
			231,
			70,
			103,
			103,
			103,
			231,
			161,
			44,
			103,
			103,
			137,
			115,
			109,
			109,
			109,
			112
		})]
		
		public JavaCharStream(Reader dstream, int startline, int startcolumn, int buffersize)
		{
			this.bufpos = -1;
			this.column = 0;
			this.line = 1;
			this.prevCharIsCR = false;
			this.prevCharIsLF = false;
			this.maxNextCharInd = 0;
			this.nextCharInd = -1;
			this.inBuf = 0;
			this.tabSize = 8;
			this.inputStream = dstream;
			this.line = startline;
			this.column = startcolumn - 1;
			this.bufsize = buffersize;
			this.available = buffersize;
			this.buffer = new char[buffersize];
			this.bufline = new int[buffersize];
			this.bufcolumn = new int[buffersize];
			this.nextCharBuf = new char[4096];
		}

		[LineNumberTable(new byte[]
		{
			161,
			37,
			103,
			103,
			137,
			147,
			115,
			109,
			109,
			109,
			144,
			114,
			125,
			114
		})]
		public virtual void ReInit(Reader dstream, int startline, int startcolumn, int buffersize)
		{
			this.inputStream = dstream;
			this.line = startline;
			this.column = startcolumn - 1;
			if (this.buffer == null || buffersize != this.buffer.Length)
			{
				this.bufsize = buffersize;
				this.available = buffersize;
				this.buffer = new char[buffersize];
				this.bufline = new int[buffersize];
				this.bufcolumn = new int[buffersize];
				this.nextCharBuf = new char[4096];
			}
			int num = 0;
			bool flag = num != 0;
			this.prevCharIsCR = (num != 0);
			this.prevCharIsLF = flag;
			num = 0;
			int num2 = num;
			this.maxNextCharInd = num;
			num = num2;
			int num3 = num;
			this.inBuf = num;
			this.tokenBegin = num3;
			num = -1;
			int num4 = num;
			this.bufpos = num;
			this.nextCharInd = num4;
		}

		[Throws(new string[]
		{
			"java.io.UnsupportedEncodingException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			68,
			125
		})]
		
		public JavaCharStream(InputStream dstream, string encoding, int startline, int startcolumn, int buffersize) : this((encoding != null) ? new InputStreamReader(dstream, encoding) : new InputStreamReader(dstream), startline, startcolumn, buffersize)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			74,
			115
		})]
		
		public JavaCharStream(InputStream dstream, int startline, int startcolumn, int buffersize) : this(new InputStreamReader(dstream), startline, startcolumn, 4096)
		{
		}

		[Throws(new string[]
		{
			"java.io.UnsupportedEncodingException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			102,
			125
		})]
		
		public virtual void ReInit(InputStream dstream, string encoding, int startline, int startcolumn, int buffersize)
		{
			this.ReInit((encoding != null) ? new InputStreamReader(dstream, encoding) : new InputStreamReader(dstream), startline, startcolumn, buffersize);
		}

		[LineNumberTable(new byte[]
		{
			161,
			108,
			112
		})]
		
		public virtual void ReInit(InputStream dstream, int startline, int startcolumn, int buffersize)
		{
			this.ReInit(new InputStreamReader(dstream), startline, startcolumn, buffersize);
		}

		protected internal virtual void setTabSize(int i)
		{
			this.tabSize = i;
		}

		protected internal virtual int getTabSize(int i)
		{
			return this.tabSize;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			126,
			137,
			142,
			123,
			135,
			108,
			174,
			103,
			135
		})]
		
		public virtual char BeginToken()
		{
			if (this.inBuf > 0)
			{
				this.inBuf--;
				int num = this.bufpos + 1;
				int num2 = num;
				this.bufpos = num;
				if (num2 == this.bufsize)
				{
					this.bufpos = 0;
				}
				this.tokenBegin = this.bufpos;
				return this.buffer[this.bufpos];
			}
			this.tokenBegin = 0;
			this.bufpos = -1;
			return this.readChar();
		}

		[Obsolete]
		
		[Deprecated(new object[]
		{
			64,
			"Ljava/lang/Deprecated;"
		})]
		public virtual int getColumn()
		{
			return this.bufcolumn[this.bufpos];
		}

		[Obsolete]
		
		[Deprecated(new object[]
		{
			64,
			"Ljava/lang/Deprecated;"
		})]
		public virtual int getLine()
		{
			return this.bufline[this.bufpos];
		}

		
		public virtual int getEndColumn()
		{
			return this.bufcolumn[this.bufpos];
		}

		
		public virtual int getEndLine()
		{
			return this.bufline[this.bufpos];
		}

		
		public virtual int getBeginColumn()
		{
			return this.bufcolumn[this.tokenBegin];
		}

		
		public virtual int getBeginLine()
		{
			return this.bufline[this.tokenBegin];
		}

		[LineNumberTable(new byte[]
		{
			161,
			26,
			110
		})]
		
		public JavaCharStream(Reader dstream, int startline, int startcolumn) : this(dstream, startline, startcolumn, 4096)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			31,
			110
		})]
		
		public JavaCharStream(Reader dstream) : this(dstream, 1, 1, 4096)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			57,
			110
		})]
		
		public virtual void ReInit(Reader dstream, int startline, int startcolumn)
		{
			this.ReInit(dstream, startline, startcolumn, 4096);
		}

		[LineNumberTable(new byte[]
		{
			161,
			62,
			110
		})]
		
		public virtual void ReInit(Reader dstream)
		{
			this.ReInit(dstream, 1, 1, 4096);
		}

		[Throws(new string[]
		{
			"java.io.UnsupportedEncodingException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			80,
			112
		})]
		
		public JavaCharStream(InputStream dstream, string encoding, int startline, int startcolumn) : this(dstream, encoding, startline, startcolumn, 4096)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			86,
			110
		})]
		
		public JavaCharStream(InputStream dstream, int startline, int startcolumn) : this(dstream, startline, startcolumn, 4096)
		{
		}

		[Throws(new string[]
		{
			"java.io.UnsupportedEncodingException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			91,
			111
		})]
		
		public JavaCharStream(InputStream dstream, string encoding) : this(dstream, encoding, 1, 1, 4096)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			96,
			110
		})]
		
		public JavaCharStream(InputStream dstream) : this(dstream, 1, 1, 4096)
		{
		}

		[Throws(new string[]
		{
			"java.io.UnsupportedEncodingException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			113,
			112
		})]
		
		public virtual void ReInit(InputStream dstream, string encoding, int startline, int startcolumn)
		{
			this.ReInit(dstream, encoding, startline, startcolumn, 4096);
		}

		[LineNumberTable(new byte[]
		{
			161,
			118,
			110
		})]
		
		public virtual void ReInit(InputStream dstream, int startline, int startcolumn)
		{
			this.ReInit(dstream, startline, startcolumn, 4096);
		}

		[Throws(new string[]
		{
			"java.io.UnsupportedEncodingException"
		})]
		[LineNumberTable(new byte[]
		{
			161,
			122,
			111
		})]
		
		public virtual void ReInit(InputStream dstream, string encoding)
		{
			this.ReInit(dstream, encoding, 1, 1, 4096);
		}

		[LineNumberTable(new byte[]
		{
			161,
			127,
			110
		})]
		
		public virtual void ReInit(InputStream dstream)
		{
			this.ReInit(dstream, 1, 1, 4096);
		}

		[LineNumberTable(new byte[]
		{
			161,
			132,
			110,
			159,
			2
		})]
		
		public virtual string GetImage()
		{
			if (this.bufpos >= this.tokenBegin)
			{
				return java.lang.String.newhelper(this.buffer, this.tokenBegin, this.bufpos - this.tokenBegin + 1);
			}
			return new StringBuilder().append(java.lang.String.newhelper(this.buffer, this.tokenBegin, this.bufsize - this.tokenBegin)).append(java.lang.String.newhelper(this.buffer, 0, this.bufpos + 1)).toString();
		}

		[LineNumberTable(new byte[]
		{
			161,
			141,
			135,
			107,
			186,
			159,
			9,
			191,
			0
		})]
		public virtual char[] GetSuffix(int len)
		{
			char[] array = new char[len];
			if (this.bufpos + 1 >= len)
			{
				ByteCodeHelper.arraycopy_primitive_2(this.buffer, this.bufpos - len + 1, array, 0, len);
			}
			else
			{
				ByteCodeHelper.arraycopy_primitive_2(this.buffer, this.bufsize - (len - this.bufpos - 1), array, 0, len - this.bufpos - 1);
				ByteCodeHelper.arraycopy_primitive_2(this.buffer, 0, array, len - this.bufpos - 1, this.bufpos + 1);
			}
			return array;
		}

		public virtual void Done()
		{
			this.nextCharBuf = null;
			this.buffer = null;
			this.bufline = null;
			this.bufcolumn = null;
		}

		[LineNumberTable(new byte[]
		{
			161,
			165,
			167,
			142,
			217,
			190,
			102,
			133,
			159,
			35,
			105,
			119,
			108,
			100,
			169,
			135,
			110,
			140,
			139,
			127,
			25,
			144,
			206,
			110,
			110
		})]
		public virtual void adjustBeginLineColumn(int newLine, int newCol)
		{
			int num = this.tokenBegin;
			int num2;
			if (this.bufpos >= this.tokenBegin)
			{
				num2 = this.bufpos - this.tokenBegin + this.inBuf + 1;
			}
			else
			{
				num2 = this.bufsize - this.tokenBegin + this.bufpos + 1 + this.inBuf;
			}
			int i = 0;
			int num3 = 0;
			int num4 = 0;
			while (i < num2)
			{
				int[] array = this.bufline;
				int num5 = num;
				int num6 = this.bufsize;
				object obj = array[num3 = ((num6 != -1) ? (num5 % num6) : 0)];
				int[] array2 = this.bufline;
				num++;
				int num7 = num;
				int num8 = this.bufsize;
				int num9;
				if (obj != array2[num9 = ((num8 != -1) ? (num7 % num8) : 0)])
				{
					break;
				}
				this.bufline[num3] = newLine;
				int num10 = num4 + this.bufcolumn[num9] - this.bufcolumn[num3];
				this.bufcolumn[num3] = newCol + num4;
				num4 = num10;
				i++;
			}
			if (i < num2)
			{
				int[] array3 = this.bufline;
				int num11 = num3;
				int num12 = newLine;
				newLine++;
				array3[num11] = num12;
				this.bufcolumn[num3] = newCol + num4;
				for (;;)
				{
					int num13 = i;
					i++;
					if (num13 >= num2)
					{
						break;
					}
					int[] array4 = this.bufline;
					int num14 = num;
					int num15 = this.bufsize;
					object obj2 = array4[num3 = ((num15 != -1) ? (num14 % num15) : 0)];
					int[] array5 = this.bufline;
					num++;
					int num16 = num;
					int num17 = this.bufsize;
					if (obj2 != array5[(num17 != -1) ? (num16 % num17) : 0])
					{
						int[] array6 = this.bufline;
						int num18 = num3;
						int num19 = newLine;
						newLine++;
						array6[num18] = num19;
					}
					else
					{
						this.bufline[num3] = newLine;
					}
				}
			}
			this.line = this.bufline[num3];
			this.column = this.bufcolumn[num3];
		}

		public const bool staticFlag = false;

		public int bufpos;

		internal int bufsize;

		internal int available;

		internal int tokenBegin;

		protected internal int[] bufline;

		protected internal int[] bufcolumn;

		protected internal int column;

		protected internal int line;

		protected internal bool prevCharIsCR;

		protected internal bool prevCharIsLF;

		protected internal Reader inputStream;

		protected internal char[] nextCharBuf;

		protected internal char[] buffer;

		protected internal int maxNextCharInd;

		protected internal int nextCharInd;

		protected internal int inBuf;

		protected internal int tabSize;
	}
}
