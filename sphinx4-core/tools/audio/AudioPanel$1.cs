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
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.AudioPanel", "<init>", "(Ledu.cmu.sphinx.tools.audio.AudioData;FF)V")]
	[SourceFile("AudioPanel.java")]
	
	internal sealed class AudioPanel$1 : java.lang.Object, ChangeListener, EventListener
	{
		
		
		internal AudioPanel$1(AudioPanel audioPanel)
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
			int num = ByteCodeHelper.f2i((float)AudioPanel.access$000(this.this$0).getAudioData().Length * AudioPanel.access$100(this.this$0));
			int num2 = ByteCodeHelper.f2i(65536f * AudioPanel.access$200(this.this$0));
			AudioPanel.access$302(this.this$0, new float[0]);
			AudioPanel.access$402(this.this$0, new string[0]);
			this.this$0.setSelectionStart(-1);
			this.this$0.setSelectionEnd(-1);
			this.this$0.setPreferredSize(new Dimension(num, num2));
			Dimension size = this.this$0.getSize();
			this.this$0.revalidate();
			this.this$0.repaint(0L, 0, 0, size.width, size.height);
		}

		
		internal AudioPanel this$0 = audioPanel;
	}
}
