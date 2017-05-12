using java.util;
using javax.sound.sampled;
using javax.swing.@event;
using java.lang;

namespace edu.cmu.sphinx.tools.audio
{
	public class AudioData : Object
	{		
		protected internal virtual void fireStateChanged()
		{
			ChangeEvent changeEvent = new ChangeEvent(this);
			Iterator iterator = this.__listeners.iterator();
			while (iterator.hasNext())
			{
				ChangeListener changeListener = (ChangeListener)iterator.next();
				changeListener.stateChanged(changeEvent);
			}
		}
		
		public AudioData()
		{
			this.__listeners = new ArrayList();
			this.selectionStart = -1;
			this.selectionEnd = -1;
			this.format = new AudioFormat(8000f, 16, 1, true, false);
			this.shorts = new short[0];
		}
		
		public AudioData(short[] data, float sampleRate)
		{
			this.__listeners = new ArrayList();
			this.selectionStart = -1;
			this.selectionEnd = -1;
			this.shorts = data;
			this.format = new AudioFormat(sampleRate, 16, 1, true, false);
		}
		
		public AudioData(AudioInputStream ais)
		{
			this.__listeners = new ArrayList();
			this.selectionStart = -1;
			this.selectionEnd = -1;
			this.shorts = Utils.toSignedPCM(ais);
			this.format = new AudioFormat(ais.getFormat().getSampleRate(), 16, 1, true, false);
		}

		public virtual short[] getAudioData()
		{
			return this.shorts;
		}
		
		public virtual void setAudioData(short[] data)
		{
			this.shorts = data;
			this.fireStateChanged();
		}

		public virtual AudioFormat getAudioFormat()
		{
			return this.format;
		}
		
		public virtual void addChangeListener(ChangeListener listener)
		{
			this.__listeners.add(listener);
		}
		
		public virtual void removeChangeListener(ChangeListener listener)
		{
			this.__listeners.remove(listener);
		}
		
		protected internal List listeners
		{
			
			get
			{
				return this.__listeners;
			}
			
			private set
			{
				this.__listeners = value;
			}
		}

		protected internal AudioFormat format;

		protected internal short[] shorts;
		
		internal List __listeners;

		protected internal int selectionStart;

		protected internal int selectionEnd;
	}
}
