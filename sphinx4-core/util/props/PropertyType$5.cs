﻿using System;
using java.lang;
using java.net;

namespace edu.cmu.sphinx.util.props
{
	[Serializable]
	internal sealed class PropertyType_5 : PropertyType
	{
		internal PropertyType_5(string text, int num, string text2) : base(text, num, text2, null)
		{
			GC.KeepAlive(this);
		}
	
		protected internal override bool validateString(string text)
		{
			if (java.lang.String.instancehelper_startsWith(java.lang.String.instancehelper_toLowerCase(text), "resource:/"))
			{
				return true;
			}
			if (java.lang.String.instancehelper_indexOf(text, 58) == -1)
			{
				text = new StringBuilder().append("file:").append(text).toString();
			}
			new URL(text);
			return true;
		}
	}
}
