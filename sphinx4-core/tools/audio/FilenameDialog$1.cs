using System;

using IKVM.Attributes;
using java.awt.@event;

namespace edu.cmu.sphinx.tools.audio
{
	
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.FilenameDialog", "<init>", "(Ljava.awt.Frame;ZLjava.lang.java.lang.String;)V")]
	[SourceFile("FilenameDialog.java")]
	
	internal sealed class FilenameDialog_1 : WindowAdapter
	{
		
		
		internal FilenameDialog_1(FilenameDialog filenameDialog)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			187,
			108
		})]
		
		public override void windowClosing(WindowEvent windowEvent)
		{
			this.this_0.setVisible(false);
		}

		
		internal FilenameDialog this_0 = filenameDialog;
	}
}
