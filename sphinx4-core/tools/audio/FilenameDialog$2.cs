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
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.FilenameDialog", "createFilenamePanel", "()V")]
	[SourceFile("FilenameDialog.java")]
	
	internal sealed class FilenameDialog_2 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal FilenameDialog_2(FilenameDialog filenameDialog)
		{
		}

		[LineNumberTable(new byte[]
		{
			42,
			108
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			this.this_0.setVisible(false);
		}

		
		internal FilenameDialog this_0 = filenameDialog;
	}
}
