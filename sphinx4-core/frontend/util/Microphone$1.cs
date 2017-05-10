using System;

using IKVM.Attributes;
using java.lang;
using java.util;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	
	[Implements(new string[]
	{
		"javax.sound.sampled.LineListener"
	})]
	[EnclosingMethod("edu.cmu.sphinx.frontend.util.Microphone", "getAudioLine", "()Ljavax.sound.sampled.TargetDataLine;")]
	[SourceFile("Microphone.java")]
	
	internal sealed class Microphone_1 : java.lang.Object, LineListener, EventListener
	{
		
		
		internal Microphone_1(Microphone microphone)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			227,
			127,
			11
		})]
		
		public void update(LineEvent lineEvent)
		{
			Microphone.access_000(this.this_0).info(new StringBuilder().append("line listener ").append(lineEvent).toString());
		}

		
		internal Microphone this_0 = microphone;
	}
}
