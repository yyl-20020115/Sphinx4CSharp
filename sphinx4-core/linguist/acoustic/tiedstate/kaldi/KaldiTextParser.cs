using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public class KaldiTextParser : Object
	{		
		public KaldiTextParser(string path)
		{
			File file = new File(path, "final.mdl");
			InputStream inputStream = new URL(file.getPath()).openStream();
			File file2 = new File(path, "tree");
			InputStream inputStream2 = new URL(file2.getPath()).openStream();
			SequenceInputStream sequenceInputStream = new SequenceInputStream(inputStream, inputStream2);
			this.scanner = new Scanner(sequenceInputStream);
		}

		public virtual void expectToken(string expected)
		{
			string actual = this.scanner.next();
			this.assertToken(expected, actual);
		}

		public virtual int getInt()
		{
			return this.scanner.nextInt();
		}
		
		public virtual string getToken()
		{
			return this.scanner.next();
		}

		public virtual int[] getIntArray()
		{
			ArrayList arrayList = new ArrayList();
			Iterator iterator = this.getTokenList("[", "]").iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				arrayList.add(Integer.valueOf(Integer.parseInt(text)));
			}
			int[] array = new int[arrayList.size()];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((Integer)arrayList.get(i)).intValue();
			}
			return array;
		}
		
		public virtual float[] getFloatArray()
		{
			ArrayList arrayList = new ArrayList();
			Iterator iterator = this.getTokenList("[", "]").iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				arrayList.add(Float.valueOf(Float.parseFloat(text)));
			}
			float[] array = new float[arrayList.size()];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((Float)arrayList.get(i)).floatValue();
			}
			return array;
		}
		
		public virtual List getTokenList(string openToken, string closeToken)
		{
			this.expectToken(openToken);
			ArrayList arrayList = new ArrayList();
			string text;
			while (!String.instancehelper_equals(closeToken, text = this.scanner.next()))
			{
				arrayList.add(text);
			}
			return arrayList;
		}
		
		public virtual void assertToken(string expected, string actual)
		{
			if (String.instancehelper_equals(actual, expected))
			{
				return;
			}
			string text = String.format("'%s' expected, '%s' got", new object[]
			{
				expected,
				actual
			});
			string text2 = text;
			
			throw new InputMismatchException(text2);
		}
		
		public virtual float parseFloat()
		{
			return this.scanner.nextFloat();
		}
		
		private Scanner scanner;
	}
}
