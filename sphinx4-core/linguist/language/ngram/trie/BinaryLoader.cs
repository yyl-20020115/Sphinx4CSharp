using edu.cmu.sphinx.util;
using ikvm.@internal;
using java.io;
using java.lang;
using java.net;

namespace edu.cmu.sphinx.linguist.language.ngram.trie
{
	public class BinaryLoader : java.lang.Object
	{
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
		
		private int readOrder()
		{
			return (int)((sbyte)this.inStream.readByte());
		}

		private float[] readFloatArr(int num)
		{
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = Utilities.readLittleEndianFloat(this.inStream);
			}
			return array;
		}
		
		public BinaryLoader(File location)
		{
			this.inStream = new DataInputStream(new FileInputStream(location));
		}

		public BinaryLoader(URL location)
		{
			this.loadModelData(location.openStream());
		}
		
		public virtual void verifyHeader()
		{
			string text = this.readString(this.inStream, java.lang.String.instancehelper_length("Trie Language Model"));
			if (!java.lang.String.instancehelper_equals(text, "Trie Language Model"))
			{
				string text2 = new StringBuilder().append("Bad binary LM file header: ").append(text).toString();
				
				throw new Error(text2);
			}
		}
		
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
		
		public virtual void readTrieByteArr(byte[] arr)
		{
			this.inStream.read(arr);
		}
		
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
		
		public virtual void close()
		{
			this.inStream.close();
		}

		private const string TRIE_HEADER = "Trie Language Model";

		private DataInputStream inStream;
		
		internal static bool assertionsDisabled = !ClassLiteral<BinaryLoader>.Value.desiredAssertionStatus();
	}
}
