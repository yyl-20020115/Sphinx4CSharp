using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.awt;
using java.lang;
using java.util;
using javax.swing.@event;

namespace edu.cmu.sphinx.tools.audio
{
	
	[Implements(new string[]
	{
		"javax.swing.event.ChangeListener"
	})]
	
	.
	
	internal sealed class AudioPanel_1 : java.lang.Object, ChangeListener, EventListener
	{
		
		
		internal AudioPanel_1(AudioPanel audioPanel)
		{
		}

		[LineNumberTable(new byte[]
		{
			18,
			127,
			6,
			152,
			114,
			146,
			108,
			140,
			114,
			140,
			107,
			123
		})]
		
		public void stateChanged(ChangeEvent changeEvent)
		{
			int num = ByteCodeHelper.f2i((float)AudioPanel.access_000(this.this_0).getAudioData().Length * AudioPanel.access_100(this.this_0));
			int num2 = ByteCodeHelper.f2i(65536f * AudioPanel.access_200(this.this_0));
			AudioPanel.access_302(this.this_0, new float[0]);
			AudioPanel.access_402(this.this_0, new string[0]);
			this.this_0.setSelectionStart(-1);
			this.this_0.setSelectionEnd(-1);
			this.this_0.setPreferredSize(new Dimension(num, num2));
			Dimension size = this.this_0.getSize();
			this.this_0.revalidate();
			this.this_0.repaint(0L, 0, 0, size.width, size.height);
		}

		
		internal AudioPanel this_0 = audioPanel;
	}
}
