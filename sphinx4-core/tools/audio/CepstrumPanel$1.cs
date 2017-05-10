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
	[EnclosingMethod("edu.cmu.sphinx.tools.audio.CepstrumPanel", "<init>", "(Ledu.cmu.sphinx.frontend.FrontEnd;Ledu.cmu.sphinx.frontend.util.StreamDataSource;Ledu.cmu.sphinx.tools.audio.AudioData;)V")]
	[SourceFile("CepstrumPanel.java")]
	
	internal sealed class CepstrumPanel$1 : java.lang.Object, ChangeListener, EventListener
	{
		
		
		internal CepstrumPanel$1(CepstrumPanel cepstrumPanel)
		{
		}

		[LineNumberTable(new byte[]
		{
			29,
			107
		})]
		
		public void stateChanged(ChangeEvent changeEvent)
		{
			this.this$0.computeCepstrum();
		}

		
		internal CepstrumPanel this$0 = cepstrumPanel;
	}
}
