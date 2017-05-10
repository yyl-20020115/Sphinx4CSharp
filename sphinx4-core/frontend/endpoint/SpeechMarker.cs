using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechMarker : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			65,
			103,
			103,
			103,
			111,
			111,
			111,
			107,
			107
		})]
		
		private void reset()
		{
			this.inSpeech = false;
			this.speechCount = 0;
			this.silenceCount = 0;
			this.startSpeechFrames = this.startSpeechTime / 10;
			this.endSilenceFrames = this.endSilenceTime / 10;
			this.speechLeaderFrames = this.speechLeader / 10;
			this.inputQueue = new LinkedList();
			this.outputQueue = new LinkedList();
		}

		[LineNumberTable(new byte[]
		{
			33,
			104,
			102,
			103,
			103,
			103
		})]
		
		public SpeechMarker(int startSpeechTime, int endSilenceTime, int speechLeader)
		{
			this.initLogger();
			this.startSpeechTime = startSpeechTime;
			this.speechLeader = speechLeader;
			this.endSilenceTime = endSilenceTime;
		}

		[LineNumberTable(new byte[]
		{
			40,
			102
		})]
		
		public SpeechMarker()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			45,
			135,
			113,
			113,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.startSpeechTime = ps.getInt("startSpeech");
			this.endSilenceTime = ps.getInt("endSilence");
			this.speechLeader = ps.getInt("speechLeader");
		}

		[LineNumberTable(new byte[]
		{
			57,
			102,
			102
		})]
		
		public override void initialize()
		{
			base.initialize();
			this.reset();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			85,
			112,
			140,
			99,
			133,
			104,
			102,
			109,
			165,
			104,
			104,
			145,
			109,
			165,
			107,
			135,
			104,
			110,
			137,
			103,
			174,
			104,
			143,
			109,
			122,
			205,
			124,
			103,
			127,
			8,
			115,
			63,
			4,
			139,
			139,
			118,
			103,
			183,
			165,
			109,
			114,
			109,
			108,
			135,
			149
		})]
		
		public override Data getData()
		{
			while (this.outputQueue.isEmpty())
			{
				Data data = this.getPredecessor().getData();
				if (data == null)
				{
					break;
				}
				if (data is DataStartSignal)
				{
					this.reset();
					this.outputQueue.add(data);
					break;
				}
				if (data is DataEndSignal)
				{
					if (this.inSpeech)
					{
						this.outputQueue.add(new SpeechEndSignal());
					}
					this.outputQueue.add(data);
					break;
				}
				if (data is SpeechClassifiedData)
				{
					SpeechClassifiedData speechClassifiedData = (SpeechClassifiedData)data;
					if (speechClassifiedData.isSpeech())
					{
						this.speechCount++;
						this.silenceCount = 0;
					}
					else
					{
						this.speechCount = 0;
						this.silenceCount++;
					}
					if (this.inSpeech)
					{
						this.outputQueue.add(data);
					}
					else
					{
						this.inputQueue.add(data);
						if (this.inputQueue.size() > this.startSpeechFrames + this.speechLeaderFrames)
						{
							this.inputQueue.remove(0);
						}
					}
					if (!this.inSpeech && this.speechCount == this.startSpeechFrames)
					{
						this.inSpeech = true;
						this.outputQueue.add(new SpeechStartSignal(speechClassifiedData.getCollectTime() - (long)this.speechLeader - (long)this.startSpeechFrames));
						this.outputQueue.addAll(this.inputQueue.subList(java.lang.Math.max(0, this.inputQueue.size() - this.startSpeechFrames - this.speechLeaderFrames), this.inputQueue.size()));
						this.inputQueue.clear();
					}
					if (this.inSpeech && this.silenceCount == this.endSilenceFrames)
					{
						this.inSpeech = false;
						this.outputQueue.add(new SpeechEndSignal(speechClassifiedData.getCollectTime()));
					}
				}
			}
			if (!this.outputQueue.isEmpty())
			{
				object obj = (Data)this.outputQueue.remove(0);
				if (((Data)obj) is SpeechClassifiedData)
				{
					SpeechClassifiedData speechClassifiedData = (SpeechClassifiedData)((Data)obj);
					obj = speechClassifiedData.getDoubleData();
				}
				object obj2 = obj;
				Data result;
				if (obj2 != null)
				{
					if ((result = (obj2 as Data)) == null)
					{
						throw new IncompatibleClassChangeError();
					}
				}
				else
				{
					result = null;
				}
				return result;
			}
			return null;
		}

		public virtual bool inSpeech()
		{
			return this.inSpeech;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			200
		})]
		public const string PROP_START_SPEECH = "startSpeech";

		private int startSpeechTime;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			200
		})]
		public const string PROP_END_SILENCE = "endSilence";

		private int endSilenceTime;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			50
		})]
		public const string PROP_SPEECH_LEADER = "speechLeader";

		private int speechLeader;

		
		private LinkedList inputQueue;

		
		private LinkedList outputQueue;

		private bool inSpeech;

		private int speechCount;

		private int silenceCount;

		private int startSpeechFrames;

		private int endSilenceFrames;

		private int speechLeaderFrames;
	}
}
