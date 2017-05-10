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
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.AudioTool", "createButtonPanel", "()Ljavax.swing.JPanel;")]
	[SourceFile("AudioTool.java")]
	internal sealed class AudioTool_12 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_12()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			90,
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
