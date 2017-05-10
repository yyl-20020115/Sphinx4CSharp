using System;

using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
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

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			72,
			108,
			127,
			4,
			109,
			174
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			73,
			123,
			124
		})]
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

		[LineNumberTable(new byte[]
		{
			160,
			105,
			100,
			168,
			102,
			38,
			198
		})]
		
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

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			92,
			104,
			109,
			98,
			104,
			240,
			70,
			117,
			110,
			124,
			108,
			124,
			98,
			103,
			110,
			103,
			102,
			136,
			110,
			144,
			108,
			103,
			100,
			98,
			104,
			240,
			51,
			233,
			81
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			85,
			105,
			114,
			104,
			147,
			144,
			107,
			39,
			166
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			118,
			103,
			109
		})]
		
		private void computeFeature()
		{
			Data data = this.computeNextFeature();
			this.outputQueue.add(data);
		}

		protected internal abstract Data computeNextFeature();

		[LineNumberTable(new byte[]
		{
			159,
			183,
			104,
			102,
			103
		})]
		
		public AbstractFeatureExtractor(int window)
		{
			this.initLogger();
			this.window = window;
		}

		[LineNumberTable(new byte[]
		{
			159,
			188,
			102
		})]
		
		public AbstractFeatureExtractor()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			6,
			103,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.window = ps.getInt("windowSize");
		}

		[LineNumberTable(new byte[]
		{
			18,
			102,
			107,
			113,
			119,
			107,
			102
		})]
		
		public override void initialize()
		{
			base.initialize();
			this.cepstraBufferSize = 256;
			this.cepstraBuffer = new DoubleData[this.cepstraBufferSize];
			this.cepstraBufferEdge = this.cepstraBufferSize - (this.window * 2 + 2);
			this.outputQueue = new LinkedList();
			this.reset();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			42,
			112,
			103,
			102,
			104,
			108,
			108,
			104,
			103,
			109,
			103,
			104,
			103,
			104,
			146,
			138,
			103,
			103,
			109,
			106,
			205
		})]
		
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
