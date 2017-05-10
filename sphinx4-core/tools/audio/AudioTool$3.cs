using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.awt.@event;
using java.io;
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
	internal sealed class AudioTool$3 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool$3()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			216,
			107,
			115,
			161,
			111,
			189,
			2,
			97,
			134
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.getFilename("Save As...", 1);
			if (AudioTool.filename == null || java.lang.String.instancehelper_isEmpty(AudioTool.filename))
			{
				return;
			}
			IOException ex2;
			try
			{
				Utils.writeRawFile(AudioTool.audio, AudioTool.filename);
				AudioTool.access$000().setEnabled(false);
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_48;
			}
			return;
			IL_48:
			IOException ex3 = ex2;
			Throwable.instancehelper_printStackTrace(ex3);
		}
	}
}
