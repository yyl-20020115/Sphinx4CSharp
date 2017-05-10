using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.net;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	public class BinaryStreamLoader : BinaryLoader
	{
		
		public new static void __<clinit>()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			7,
			108,
			102,
			139,
			106,
			98,
			137,
			108
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
			this.modelData = byteArrayOutputStream.toByteArray();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			133,
			162,
			179,
			103,
			135,
			103,
			103
		})]
		
		public BinaryStreamLoader(URL location, string format, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight) : base(format, applyLanguageWeightAndWip, languageWeight, wip, unigramWeight)
		{
			InputStream inputStream = location.openStream();
			this.loadModelLayout(inputStream);
			inputStream = location.openStream();
			this.loadModelData(inputStream);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			20,
			103,
			112
		})]
		public override byte[] loadBuffer(long position, int size)
		{
			byte[] array = new byte[size];
			ByteCodeHelper.arraycopy_primitive_1(this.modelData, (int)position, array, 0, size);
			return array;
		}

		
		static BinaryStreamLoader()
		{
			BinaryLoader.__<clinit>();
		}

		internal byte[] modelData;
	}
}
