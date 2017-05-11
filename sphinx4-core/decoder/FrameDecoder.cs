using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.decoder
{
	public class FrameDecoder : AbstractDecoder, DataProcessor, Configurable
	{
		public virtual DataProcessor getPredecessor()
		{
			return this.predecessor;
		}
		
		public override Result decode(string referenceText)
		{
			return this.searchManager.recognize(1);
		}

		public FrameDecoder(SearchManager searchManager, bool fireNonFinalResults, bool autoAllocate, List listeners) : base(searchManager, fireNonFinalResults, autoAllocate, listeners)
		{
		}

		public FrameDecoder()
		{
		}

		public virtual Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (this.isRecognizing && (data is FloatData || data is DoubleData || data is SpeechEndSignal))
			{
				this.result = this.decode(null);
				if (this.result != null)
				{
					this.fireResultListeners(this.result);
					this.result = null;
				}
			}
			if (data is DataEndSignal)
			{
				this.searchManager.stopRecognition();
			}
			if (data is SpeechStartSignal)
			{
				this.searchManager.startRecognition();
				this.isRecognizing = true;
				this.result = null;
			}
			if (data is SpeechEndSignal)
			{
				this.searchManager.stopRecognition();
				if (this.result != null)
				{
					this.fireResultListeners(this.result);
				}
				this.isRecognizing = false;
			}
			return data;
		}

		public virtual void setPredecessor(DataProcessor predecessor)
		{
			this.predecessor = predecessor;
		}

		public virtual void initialize()
		{
		}

		private DataProcessor predecessor;

		private bool isRecognizing;

		private Result result;
	}
}
