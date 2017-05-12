using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class HypothesisWord : Object
	{
		internal float getStartTime()
		{
			return this.startTime;
		}

		internal float getEndTime()
		{
			return this.endTime;
		}

		internal string getText()
		{
			return this.text;
		}
		
		internal HypothesisWord(string text, float num, float num2)
		{
			this.text = text;
			this.startTime = num;
			this.endTime = num2;
		}
		
		private string text;
		
		private float startTime;
		
		private float endTime;
	}
}
