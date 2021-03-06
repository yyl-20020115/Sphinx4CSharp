﻿using java.lang;

namespace edu.cmu.sphinx.util.props
{
	[System.Serializable]
	internal sealed class PropertyType_2 : PropertyType
	{	
		internal PropertyType_2(string text, int num, string text2) : base(text, num, text2, null)
		{
			System.GC.KeepAlive(this);
		}
	
		protected internal override bool validateString(string text)
		{
			text = String.instancehelper_toLowerCase(text);
			return String.instancehelper_equals("true", text) || String.instancehelper_equals("false", text);
		}
	}
}
