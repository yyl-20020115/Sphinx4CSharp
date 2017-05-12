using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.result;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.api
{
	public class AbstractSpeechRecognizer : Object
	{	
		protected internal AbstractSpeechRecognizer(Context context)
		{
			this.__context = context;
			this.__recognizer = (Recognizer)context.getInstance(ClassLiteral<Recognizer>.Value);
			this.__speechSourceProvider = new SpeechSourceProvider();
		}
	
		public AbstractSpeechRecognizer(Configuration configuration) : this(new Context(configuration))
		{
		}
		
		public virtual SpeechResult getResult()
		{
			Result result = this.__recognizer.recognize();
			return (null != result) ? new SpeechResult(result) : null;
		}
		public virtual Stats createStats(int numClasses)
		{
			this.clusters = new ClusteredDensityFileData(this.__context.getLoader(), numClasses);
			return new Stats(this.__context.getLoader(), this.clusters);
		}
		
		public virtual void setTransform(Transform transform)
		{
			if (this.clusters != null && transform != null)
			{
				this.__context.getLoader().update(transform, this.clusters);
			}
		}
		
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
