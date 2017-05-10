using System;

using IKVM.Attributes;
using IKVM.Runtime;
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
	internal sealed class AudioTool_1 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_1()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			182,
			107,
			115,
			161,
			191,
			2,
			2,
			97,
			134
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			AudioTool.getFilename("Open...", 0);
			if (AudioTool.filename == null || java.lang.String.instancehelper_isEmpty(AudioTool.filename))
			{
				return;
			}
			Exception ex3;
			try
			{
				AudioTool.getAudioFromFile(AudioTool.filename);
			}
			catch (Exception ex)
			{
				Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_3D;
			}
			return;
			IL_3D:
			Exception ex4 = ex3;
			Throwable.instancehelper_printStackTrace(ex4);
		}
	}
}
