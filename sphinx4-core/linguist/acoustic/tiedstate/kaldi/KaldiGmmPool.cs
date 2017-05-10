using System;

using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	
	public class KaldiGmmPool : Pool
	{
		[LineNumberTable(new byte[]
		{
			159,
			159,
			109,
			139,
			103,
			107,
			135,
			102,
			46,
			134
		})]
		
		public KaldiGmmPool(KaldiTextParser parser) : base("senones")
		{
			parser.expectToken("<DIMENSION>");
			parser.getInt();
			parser.expectToken("<NUMPDFS>");
			int @int = parser.getInt();
			for (int i = 0; i < @int; i++)
			{
				this.put(i, new DiagGmm(i, parser));
			}
		}
	}
}
