using System;

using IKVM.Attributes;
using java.awt.@event;

namespace edu.cmu.sphinx.tools.audio
{
	
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.FilenameDialog", "<init>", "(Ljava.awt.Frame;ZLjava.lang.java.lang.String;)V")]
	[SourceFile("FilenameDialog.java")]
	
	internal sealed class FilenameDialog$1 : WindowAdapter
	{
		
		
		internal FilenameDialog$1(FilenameDialog filenameDialog)
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
			this.this$0.setVisible(false);
		}

		
		internal FilenameDialog this$0 = filenameDialog;
	}
}
