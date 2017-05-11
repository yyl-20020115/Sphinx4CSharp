using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.feature
{
	public abstract class AbstractFeatureExtractor : BaseDataProcessor
	{
		private void reset()
		{
			this.bufferPosition = 0;
			this.currentPosition = 0;
		}
	
		private Data getNextData()
		{
			Data data = this.getPredecessor().getData();
			while (data != null && !(data is DoubleData) && !(data is DataEndSignal) && !(data is DataStartSignal) && !(data is SpeechEndSignal))
			{
				this.outputQueue.add(data);
				data = this.getPredecessor().getData();
			}
			return data;
		}
		private void addCepstrum(DoubleData doubleData)
		{
			DoubleData[] array = this.cepstraBuffer;
			int num = this.bufferPosition;
			int num2 = num;
			this.bufferPosition = num + 1;
			array[num2] = doubleData;
			int num3 = this.bufferPosition;
			int num4 = this.cepstraBufferSize;
			this.bufferPosition = ((num4 != -1) ? (num3 % num4) : 0);
		}
		
		private void computeFeatures(int num)
		{
			if (num == 1)
			{
				this.computeFeature();
			}
			else
			{
				for (int i = 0; i < num; i++)
				{
					this.computeFeature();
				}
			}
		}
		
		private int processFirstCepstrum(Data data)
		{
			if (data is DataEndSignal)
			{
				this.outputQueue.add(data);
				return 0;
			}
			if (data is DataStartSignal)
			{
				string text = "Too many UTTERANCE_START";
				
				throw new Error(text);
			}
			Arrays.fill(this.cepstraBuffer, 0, this.window + 1, data);
			this.bufferPosition = this.window + 1;
			int num = this.bufferPosition;
			int num2 = this.cepstraBufferSize;
			this.bufferPosition = ((num2 != -1) ? (num % num2) : 0);
			this.currentPosition = this.window;
			int num3 = this.currentPosition;
			int num4 = this.cepstraBufferSize;
			this.currentPosition = ((num4 != -1) ? (num3 % num4) : 0);
			int num5 = 1;
			this.pendingSignal = null;
			for (int i = 0; i < this.window; i++)
			{
				Data nextData = this.getNextData();
				if (nextData != null)
				{
					if (nextData is DoubleData)
					{
						this.addCepstrum((DoubleData)nextData);
					}
					else
					{
						if (nextData is DataEndSignal || nextData is SpeechEndSignal)
						{
							this.pendingSignal = (Signal)nextData;
							this.replicateLastCepstrum();
							num5 += i;
							break;
						}
						if (nextData is DataStartSignal)
						{
							string text2 = "Too many UTTERANCE_START";
							
							throw new Error(text2);
						}
					}
				}
			}
			return num5;
		}
		
		private int replicateLastCepstrum()
		{
			DoubleData doubleData;
			if (this.bufferPosition > 0)
			{
				doubleData = this.cepstraBuffer[this.bufferPosition - 1];
			}
			else
			{
				if (this.bufferPosition != 0)
				{
					string text = "BufferPosition < 0";
					
					throw new Error(text);
				}
				doubleData = this.cepstraBuffer[this.cepstraBuffer.Length - 1];
			}
			for (int i = 0; i < this.window; i++)
			{
				this.addCepstrum(doubleData);
			}
			return this.window;
		}
		
		private void computeFeature()
		{
			Data data = this.computeNextFeature();
			this.outputQueue.add(data);
		}

		protected internal abstract Data computeNextFeature();
		
		public AbstractFeatureExtractor(int window)
		{
			this.initLogger();
			this.window = window;
		}
		
		public AbstractFeatureExtractor()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.window = ps.getInt("windowSize");
		}
		
		public override void initialize()
		{
			base.initialize();
			this.cepstraBufferSize = 256;
			this.cepstraBuffer = new DoubleData[this.cepstraBufferSize];
			this.cepstraBufferEdge = this.cepstraBufferSize - (this.window * 2 + 2);
			this.outputQueue = new LinkedList();
			this.reset();
		}
		
		public override Data getData()
		{
			if (this.outputQueue.isEmpty())
			{
				Data nextData = this.getNextData();
				if (nextData != null)
				{
					if (nextData is DoubleData)
					{
						this.addCepstrum((DoubleData)nextData);
						this.computeFeatures(1);
					}
					else if (nextData is DataStartSignal)
					{
						this.pendingSignal = null;
						this.outputQueue.add(nextData);
						Data nextData2 = this.getNextData();
						int num = this.processFirstCepstrum(nextData2);
						this.computeFeatures(num);
						if (this.pendingSignal != null)
						{
							this.outputQueue.add(this.pendingSignal);
						}
					}
					else if (nextData is SpeechEndSignal)
					{
						int num2 = this.replicateLastCepstrum();
						this.computeFeatures(num2);
						this.outputQueue.add(nextData);
					}
					else if (nextData is DataEndSignal)
					{
						this.outputQueue.add(nextData);
					}
				}
			}
			return (!this.outputQueue.isEmpty()) ? ((Data)this.outputQueue.removeFirst()) : null;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			3
		})]
		public const string PROP_FEATURE_WINDOW = "windowSize";

		private int bufferPosition;

		private Signal pendingSignal;
		
		private LinkedList outputQueue;

		protected internal int cepstraBufferEdge;

		protected internal int window;

		protected internal int currentPosition;

		protected internal int cepstraBufferSize;

		protected internal DoubleData[] cepstraBuffer;
	}
}
