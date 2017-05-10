using System;

using IKVM.Attributes;
using java.lang;
using java.util;
using javax.sound.sampled;
using javax.swing.@event;

namespace edu.cmu.sphinx.tools.audio
{
	public class AudioData : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			81,
			103,
			127,
			1,
			105
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			175,
			232,
			59,
			107,
			103,
			199,
			245,
			69,
			108
		})]
		
		public AudioData()
		{
			this.__listeners = new ArrayList();
			this.selectionStart = -1;
			this.selectionEnd = -1;
			this.format = new AudioFormat(8000f, 16, 1, true, false);
			this.shorts = new short[0];
		}

		[LineNumberTable(new byte[]
		{
			0,
			232,
			42,
			107,
			103,
			231,
			85,
			103,
			242,
			69
		})]
		
		public AudioData(short[] data, float sampleRate)
		{
			this.__listeners = new ArrayList();
			this.selectionStart = -1;
			this.selectionEnd = -1;
			this.shorts = data;
			this.format = new AudioFormat(sampleRate, 16, 1, true, false);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			17,
			232,
			25,
			107,
			103,
			231,
			102,
			108,
			251,
			69
		})]
		
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

		[LineNumberTable(new byte[]
		{
			44,
			103,
			102
		})]
		
		public virtual void setAudioData(short[] data)
		{
			this.shorts = data;
			this.fireStateChanged();
		}

		public virtual AudioFormat getAudioFormat()
		{
			return this.format;
		}

		[LineNumberTable(new byte[]
		{
			65,
			109
		})]
		
		public virtual void addChangeListener(ChangeListener listener)
		{
			this.__listeners.add(listener);
		}

		[LineNumberTable(new byte[]
		{
			75,
			109
		})]
		
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
