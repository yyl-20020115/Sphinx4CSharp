using IKVM.Runtime;
using java.io;
using java.net;

namespace edu.cmu.sphinx.linguist.language.ngram.large
{
	public class BinaryStreamLoader : BinaryLoader
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
			this.modelData = byteArrayOutputStream.toByteArray();
		}
		
		public BinaryStreamLoader(URL location, string format, bool applyLanguageWeightAndWip, float languageWeight, double wip, float unigramWeight) : base(format, applyLanguageWeightAndWip, languageWeight, wip, unigramWeight)
		{
			InputStream inputStream = location.openStream();
			this.loadModelLayout(inputStream);
			inputStream = location.openStream();
			this.loadModelData(inputStream);
		}

		public override byte[] loadBuffer(long position, int size)
		{
			byte[] array = new byte[size];
			ByteCodeHelper.arraycopy_primitive_1(this.modelData, (int)position, array, 0, size);
			return array;
		}

		internal byte[] modelData;
	}
}
