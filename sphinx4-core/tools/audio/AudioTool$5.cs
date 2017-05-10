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
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.AudioTool", "createMenuBar", "(Ljavax.swing.JFrame;)V")]
	[SourceFile("AudioTool.java")]
	internal sealed class AudioTool_5 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_5()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			247,
			106
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.audioPanel.selectAll();
		}
	}
}
