using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class PronounceableFSM : Object
	{
		public virtual bool accept(string inputString)
		{
			int num = this.transition(0, 35);
			int num2 = String.instancehelper_length(inputString) - 1;
			int num3 = (!this.scanFromFront) ? num2 : 0;
			int num4 = num3;
			while (0 <= num4 && num4 <= num2)
			{
				int num5 = (int)String.instancehelper_charAt(inputString, num4);
				int num6;
				if (num5 == 110 || num5 == 109)
				{
					num6 = 78;
				}
				else if (String.instancehelper_indexOf("aeiouy", num5) != -1)
				{
					num6 = 86;
				}
				else
				{
					num6 = num5;
				}
				num = this.transition(num, num6);
				if (num == -1)
				{
					return false;
				}
				if (num6 == 86)
				{
					return true;
				}
				if (this.scanFromFront)
				{
					num4++;
				}
				else
				{
					num4 --;
				}
			}
			return false;
		}

		private void loadText(InputStream inputStream)
		{
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				if (!String.instancehelper_startsWith(text, "***"))
				{
					if (String.instancehelper_startsWith(text, "VOCAB_SIZE"))
					{
						this.vocabularySize = this.parseLastInt(text);
					}
					else if (String.instancehelper_startsWith(text, "NUM_OF_TRANSITIONS"))
					{
						int num = this.parseLastInt(text);
						this.transitions = new int[num];
					}
					else if (String.instancehelper_startsWith(text, "TRANSITIONS"))
					{
						StringTokenizer stringTokenizer = new StringTokenizer(text);
						stringTokenizer.nextToken();
						int num2 = 0;
						while (stringTokenizer.hasMoreTokens() && num2 < this.transitions.Length)
						{
							string text2 = String.instancehelper_trim(stringTokenizer.nextToken());
							int[] array = this.transitions;
							int num3 = num2;
							num2++;
							array[num3] = Integer.parseInt(text2);
						}
					}
				}
			}
			bufferedReader.close();
		}

		private int parseLastInt(string text)
		{
			string text2 = String.instancehelper_substring(String.instancehelper_trim(text), String.instancehelper_lastIndexOf(text, " "));
			return Integer.parseInt(String.instancehelper_trim(text2));
		}
		private int transition(int num, int num2)
		{
			for (int i = num; i < this.transitions.Length; i++)
			{
				int num3 = this.transitions[i];
				int num4 = this.vocabularySize;
				if (((num4 != -1) ? (num3 % num4) : 0) == num2)
				{
					int num5 = this.transitions[i];
					int num6 = this.vocabularySize;
					return (num6 != -1) ? (num5 / num6) : (-num5);
				}
			}
			return -1;
		}

		public PronounceableFSM(URL url, bool scanFromFront)
		{
			this.scanFromFront = scanFromFront;
			InputStream inputStream = url.openStream();
			this.loadText(inputStream);
			inputStream.close();
		}

		public PronounceableFSM(int vocabularySize, int[] transitions, bool scanFromFront)
		{
			this.vocabularySize = vocabularySize;
			this.transitions = transitions;
			this.scanFromFront = scanFromFront;
		}

		private const string VOCAB_SIZE = "VOCAB_SIZE";

		private const string NUM_OF_TRANSITIONS = "NUM_OF_TRANSITIONS";

		private const string TRANSITIONS = "TRANSITIONS";

		protected internal int vocabularySize;

		protected internal int[] transitions;

		protected internal bool scanFromFront;
	}
}
