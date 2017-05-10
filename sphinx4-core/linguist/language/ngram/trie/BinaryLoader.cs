using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.net;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	public class BinaryLoader : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			183,
			108,
			102,
			139,
			106,
			98,
			137,
			118
		})]
		
		private void loadModelData(InputStream inputStream)
		{
			DataInputStream dataInputStream = new DataInputStream(new BufferedInputStream(inputStream));
			ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
			byte[] array = new byte[4096];
			while (dataInputStream.read(array) >= 0)
			{
				byteArrayOutputStream.write(array);
			}
			this.inStream = new DataInputStream(new ByteArrayInputStream(byteArrayOutputStream.toByteArray()));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			90,
			102,
			103,
			104,
			102,
			43,
			166
		})]
		
		private string readString(DataInputStream dataInputStream, int num)
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = new byte[num];
			dataInputStream.read(array);
			for (int i = 0; i < num; i++)
			{
				stringBuilder.append((char)array[i]);
			}
			return stringBuilder.toString();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		
		private int readOrder()
		{
			return (int)((sbyte)this.inStream.readByte());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			73,
			103,
			102,
			46,
			134
		})]
		
		private float[] readFloatArr(int num)
		{
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = Utilities.readLittleEndianFloat(this.inStream);
			}
			return array;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			178,
			104,
			113
		})]
		
		public BinaryLoader(File location)
		{
			this.inStream = new DataInputStream(new FileInputStream(location));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			2,
			104,
			108
		})]
		
		public BinaryLoader(URL location)
		{
			this.loadModelData(location.openStream());
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			11,
			119,
			109,
			159,
			6
		})]
		
		public virtual void verifyHeader()
		{
			string text = this.readString(this.inStream, java.lang.String.instancehelper_length("Trie Language Model"));
			if (!java.lang.String.instancehelper_equals(text, "Trie Language Model"))
			{
				string text2 = new StringBuilder().append("Bad binary LM file header: ").append(text).toString();
				
				throw new Error(text2);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			23,
			103,
			103,
			103,
			46,
			166
		})]
		
		public virtual int[] readCounts()
		{
			int num = this.readOrder();
			int[] array = new int[num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Utilities.readLittleEndianInt(this.inStream);
			}
			return array;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			38,
			108,
			109,
			127,
			6,
			104,
			136,
			102,
			116,
			100,
			244,
			61,
			230,
			69
		})]
		
		public virtual NgramTrieQuant readQuant(int order)
		{
			int num = Utilities.readLittleEndianInt(this.inStream);
			if (num < 0 || num >= NgramTrieQuant.QuantType.values().Length)
			{
				string text = new StringBuilder().append("Unknown quantatization type: ").append(num).toString();
				
				throw new Error(text);
			}
			NgramTrieQuant.QuantType quantType = NgramTrieQuant.QuantType.values()[num];
			NgramTrieQuant ngramTrieQuant = new NgramTrieQuant(order, quantType);
			for (int i = 2; i <= order; i++)
			{
				ngramTrieQuant.setTable(this.readFloatArr(ngramTrieQuant.getProbTableLen()), i, true);
				if (i < order)
				{
					ngramTrieQuant.setTable(this.readFloatArr(ngramTrieQuant.getBackoffTableLen()), i, false);
				}
			}
			return ngramTrieQuant;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			59,
			105,
			104,
			104,
			115,
			115,
			243,
			60,
			230,
			70
		})]
		
		public virtual NgramTrieModel.TrieUnigram[] readUnigrams(int count)
		{
			NgramTrieModel.TrieUnigram[] array = new NgramTrieModel.TrieUnigram[count + 1];
			for (int i = 0; i < count + 1; i++)
			{
				array[i] = new NgramTrieModel.TrieUnigram();
				array[i].prob = Utilities.readLittleEndianFloat(this.inStream);
				array[i].backoff = Utilities.readLittleEndianFloat(this.inStream);
				array[i].next = Utilities.readLittleEndianInt(this.inStream);
			}
			return array;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			77,
			109
		})]
		
		public virtual void readTrieByteArr(byte[] arr)
		{
			this.inStream.read(arr);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			87,
			108,
			100,
			159,
			6,
			103,
			103,
			141,
			98,
			99,
			104,
			103,
			132,
			112,
			102,
			228,
			58,
			232,
			73,
			118
		})]
		
		public virtual string[] readWords(int unigramNum)
		{
			int num = Utilities.readLittleEndianInt(this.inStream);
			if (num <= 0)
			{
				string text = new StringBuilder().append("Bad word string size: ").append(num).toString();
				
				throw new Error(text);
			}
			string[] array = new string[unigramNum];
			byte[] array2 = new byte[num];
			this.inStream.read(array2);
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < num; i++)
			{
				if ((ushort)array2[i] == 0)
				{
					array[num2] = java.lang.String.newhelper(array2, num3, i - num3);
					num3 = i + 1;
					num2++;
				}
			}
			if (!BinaryLoader.assertionsDisabled && num2 != unigramNum)
			{
				
				throw new AssertionError();
			}
			return array;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			115,
			107
		})]
		
		public virtual void close()
		{
			this.inStream.close();
		}

		
		static BinaryLoader()
		{
		}

		private const string TRIE_HEADER = "Trie Language Model";

		private DataInputStream inStream;

		
		internal static bool assertionsDisabled = !ClassLiteral<BinaryLoader>.Value.desiredAssertionStatus();
	}
}
