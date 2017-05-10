using System;

using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.api
{
	public class AbstractSpeechRecognizer : java.lang.Object
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			189,
			104,
			103,
			118,
			107
		})]
		
		protected internal AbstractSpeechRecognizer(Context context)
		{
			this.__context = context;
			this.__recognizer = (Recognizer)context.getInstance(ClassLiteral<Recognizer>.Value);
			this.__speechSourceProvider = new SpeechSourceProvider();
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			186,
			108
		})]
		
		public AbstractSpeechRecognizer(Configuration configuration) : this(new Context(configuration))
		{
		}

		[LineNumberTable(new byte[]
		{
			10,
			108
		})]
		
		public virtual SpeechResult getResult()
		{
			Result result = this.__recognizer.recognize();
			return (null != result) ? new SpeechResult(result) : null;
		}

		[LineNumberTable(new byte[]
		{
			15,
			119
		})]
		
		public virtual Stats createStats(int numClasses)
		{
			this.clusters = new ClusteredDensityFileData(this.__context.getLoader(), numClasses);
			return new Stats(this.__context.getLoader(), this.clusters);
		}

		[LineNumberTable(new byte[]
		{
			20,
			107,
			151
		})]
		
		public virtual void setTransform(Transform transform)
		{
			if (this.clusters != null && transform != null)
			{
				this.__context.getLoader().update(transform, this.clusters);
			}
		}

		[Throws(new string[]
		{
			"java.lang.Exception"
		})]
		[LineNumberTable(new byte[]
		{
			26,
			119,
			124,
			103,
			119
		})]
		
		public virtual void loadTransform(string path, int numClass)
		{
			this.clusters = new ClusteredDensityFileData(this.__context.getLoader(), numClass);
 			Transform transform = new Transform((Sphinx3Loader)this.__context.getLoader(), numClass);
			transform.load(path);
			this.__context.getLoader().update(transform, this.clusters);
		}

		
		protected internal Context context
		{
			
			get
			{
				return this.__context;
			}
			
			private set
			{
				this.__context = value;
			}
		}

		
		protected internal Recognizer recognizer
		{
			
			get
			{
				return this.__recognizer;
			}
			
			private set
			{
				this.__recognizer = value;
			}
		}

		
		protected internal SpeechSourceProvider speechSourceProvider
		{
			
			get
			{
				return this.__speechSourceProvider;
			}
			
			private set
			{
				this.__speechSourceProvider = value;
			}
		}

		internal Context __context;

		internal Recognizer __recognizer;

		protected internal ClusteredDensityFileData clusters;

		internal SpeechSourceProvider __speechSourceProvider;
	}
}
