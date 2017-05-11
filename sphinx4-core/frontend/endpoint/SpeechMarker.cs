using edu.cmu.sphinx.util.props;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechMarker : BaseDataProcessor
	{

		private void reset()
		{
			this._inSpeech = false;
			this.speechCount = 0;
			this.silenceCount = 0;
			this.startSpeechFrames = this.startSpeechTime / 10;
			this.endSilenceFrames = this.endSilenceTime / 10;
			this.speechLeaderFrames = this.speechLeader / 10;
			this.inputQueue = new LinkedList();
			this.outputQueue = new LinkedList();
		}

		public SpeechMarker(int startSpeechTime, int endSilenceTime, int speechLeader)
		{
			this.initLogger();
			this.startSpeechTime = startSpeechTime;
			this.speechLeader = speechLeader;
			this.endSilenceTime = endSilenceTime;
		}
	
		public SpeechMarker()
		{
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.startSpeechTime = ps.getInt("startSpeech");
			this.endSilenceTime = ps.getInt("endSilence");
			this.speechLeader = ps.getInt("speechLeader");
		}
	
		public override void initialize()
		{
			base.initialize();
			this.reset();
		}

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
					if (this._inSpeech)
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
					if (this._inSpeech)
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
					if (!this._inSpeech && this.speechCount == this.startSpeechFrames)
					{
						this._inSpeech = true;
						this.outputQueue.add(new SpeechStartSignal(speechClassifiedData.getCollectTime() - (long)this.speechLeader - (long)this.startSpeechFrames));
						this.outputQueue.addAll(this.inputQueue.subList(java.lang.Math.max(0, this.inputQueue.size() - this.startSpeechFrames - this.speechLeaderFrames), this.inputQueue.size()));
						this.inputQueue.clear();
					}
					if (this._inSpeech && this.silenceCount == this.endSilenceFrames)
					{
						this._inSpeech = false;
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
			return this._inSpeech;
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

		private bool _inSpeech;

		private int speechCount;

		private int silenceCount;

		private int startSpeechFrames;

		private int endSilenceFrames;

		private int speechLeaderFrames;
	}
}
