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
	
	.
	
	internal sealed class CepstrumPanel_1 : java.lang.Object, ChangeListener, EventListener
	{
		
		
		internal CepstrumPanel_1(CepstrumPanel cepstrumPanel)
		{
		}

		[LineNumberTable(new byte[]
		{
			29,
			107
		})]
		
		public void stateChanged(ChangeEvent changeEvent)
		{
			this.this_0.computeCepstrum();
		}

		
		internal CepstrumPanel this_0 = cepstrumPanel;
	}
}
