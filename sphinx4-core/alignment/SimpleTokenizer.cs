using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	public class SimpleTokenizer : java.lang.Object, TextTokenizer
	{
		public SimpleTokenizer()
		{
		}

		public virtual List expand(string text)
		{
			text = java.lang.String.instancehelper_replace(text, '’', '\'');
			text = java.lang.String.instancehelper_replace(text, '‘', ' ');
			text = java.lang.String.instancehelper_replace(text, '”', ' ');
			text = java.lang.String.instancehelper_replace(text, '“', ' ');
			text = java.lang.String.instancehelper_replace(text, '"', ' ');
			text = java.lang.String.instancehelper_replace(text, '»', ' ');
			text = java.lang.String.instancehelper_replace(text, '«', ' ');
			text = java.lang.String.instancehelper_replace(text, '–', '-');
			text = java.lang.String.instancehelper_replace(text, '—', ' ');
			text = java.lang.String.instancehelper_replace(text, '…', ' ');
			string text2 = text;
			object obj = " - ";
			object obj2 = " ";
			CharSequence charSequence = CharSequence.Cast(obj);
			CharSequence charSequence2 = CharSequence.Cast(obj2);

			text = java.lang.String.instancehelper_replace(text2, charSequence2, charSequence);
			text = java.lang.String.instancehelper_replaceAll(text, "[/_*%]", " ");
			text = java.lang.String.instancehelper_toLowerCase(text);
			string[] array = java.lang.String.instancehelper_split(text, "[.,?:!;()]");
			return Arrays.asList(array);
		}
	}
}
