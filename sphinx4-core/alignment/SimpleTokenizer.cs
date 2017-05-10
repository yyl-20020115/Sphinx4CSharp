using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.alignment.TextTokenizer"
	})]
	public class SimpleTokenizer : java.lang.Object, TextTokenizer
	{
		
		
		public SimpleTokenizer()
		{
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			160,
			111,
			111,
			111,
			111,
			108,
			111,
			111,
			111,
			111,
			143,
			127,
			21,
			114,
			136,
			108
		})]
		
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
			object _ref = obj;
			CharSequence charSequence;
			charSequence.__ref = _ref;
			CharSequence charSequence2 = charSequence;
			_ref = obj2;
			charSequence.__ref = _ref;
			text = java.lang.String.instancehelper_replace(text2, charSequence2, charSequence);
			text = java.lang.String.instancehelper_replaceAll(text, "[/_*%]", " ");
			text = java.lang.String.instancehelper_toLowerCase(text);
			string[] array = java.lang.String.instancehelper_split(text, "[.,?:!;()]");
			return Arrays.asList(array);
		}
	}
}
