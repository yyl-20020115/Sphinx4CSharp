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
	internal sealed class AudioTool_14 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_14()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			111,
			101
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.access_200();
		}
	}
}
