using System;

using IKVM.Attributes;
using java.lang;
using java.util;
using javax.swing.@event;

namespace edu.cmu.sphinx.tools.audio
{
	
	[Implements(new string[]
	{
		"javax.swing.event.ChangeListener"
	})]
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.SpectrogramPanel", "<init>", "(Ledu.cmu.sphinx.frontend.FrontEnd;Ledu.cmu.sphinx.frontend.util.StreamDataSource;Ledu.cmu.sphinx.tools.audio.AudioData;)V")]
	[SourceFile("SpectrogramPanel.java")]
	
	internal sealed class SpectrogramPanel$1 : java.lang.Object, ChangeListener, EventListener
	{
		
		
		internal SpectrogramPanel$1(SpectrogramPanel spectrogramPanel)
		{
		}

		[LineNumberTable(new byte[]
		{
			27,
			107
		})]
		
		public void stateChanged(ChangeEvent changeEvent)
		{
			this.this$0.computeSpectrogram();
		}

		
		internal SpectrogramPanel this$0 = spectrogramPanel;
	}
}
