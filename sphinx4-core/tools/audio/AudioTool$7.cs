using System;

using IKVM.Attributes;
using java.awt.@event;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.audio
{
	
	[Implements(new string[]
	{
		"java.awt.event.ActionListener"
	})]
	
	.
	internal sealed class AudioTool_7 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_7()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			13,
			101
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.access_100();
		}
	}
}
