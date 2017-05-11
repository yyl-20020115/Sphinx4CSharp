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
	
	.
	internal sealed class AudioTool_2 : java.lang.Object, ActionListener, EventListener
	{
		
		
		internal AudioTool_2()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			200,
			147,
			111,
			189,
			2,
			97,
			166
		})]
		
		public void actionPerformed(ActionEvent actionEvent)
		{
			if (AudioTool.filename != null && !java.lang.String.instancehelper_isEmpty(AudioTool.filename))
			{
				IOException ex2;
				try
				{
					Utils.writeRawFile(AudioTool.audio, AudioTool.filename);
					AudioTool.access_000().setEnabled(false);
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_3C;
				}
				return;
				IL_3C:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
			}
		}
	}
}
