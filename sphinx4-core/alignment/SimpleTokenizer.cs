using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	public class SimpleTokenizer : Object, TextTokenizer
	{
		public SimpleTokenizer()
		{
		}

		public virtual List expand(string text)
		{
			text = String.instancehelper_replace(text, '’', '\'');
			text = String.instancehelper_replace(text, '‘', ' ');
			text = String.instancehelper_replace(text, '”', ' ');
			text = String.instancehelper_replace(text, '“', ' ');
			text = String.instancehelper_replace(text, '"', ' ');
			text = String.instancehelper_replace(text, '»', ' ');
			text = String.instancehelper_replace(text, '«', ' ');
			text = String.instancehelper_replace(text, '–', '-');
			text = String.instancehelper_replace(text, '—', ' ');
			text = String.instancehelper_replace(text, '…', ' ');
			string text2 = text;
			object obj = " - ";
			object obj2 = " ";
			CharSequence charSequence = CharSequence.Cast(obj);
			CharSequence charSequence2 = CharSequence.Cast(obj2);

			text = String.instancehelper_replace(text2, charSequence2, charSequence);
			text = String.instancehelper_replaceAll(text, "[/_*%]", " ");
			text = String.instancehelper_toLowerCase(text);
			string[] array = String.instancehelper_split(text, "[.,?:!;()]");
			return Arrays.asList(array);
		}
	}
}
