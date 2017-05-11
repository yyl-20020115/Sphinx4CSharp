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
	internal sealed class AudioTool_16 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_16()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			126,
			102
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			java.lang.System.exit(0);
		}
	}
}
