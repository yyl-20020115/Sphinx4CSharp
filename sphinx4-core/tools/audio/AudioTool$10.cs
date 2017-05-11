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
	internal sealed class AudioTool_10 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_10()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			44,
			116,
			37,
			133
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.player.play(AudioTool.audioPanel.getSelectionStart(), AudioTool.audioPanel.getSelectionEnd());
		}
	}
}
