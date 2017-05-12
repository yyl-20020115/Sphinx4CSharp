using edu.cmu.sphinx.decoder;
using edu.cmu.sphinx.instrumentation;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.recognizer
{
	public class Recognizer : Object, Configurable, ResultProducer
	{		
		public virtual Result recognize()
		{
			return this.recognize(null);
		}
		
		public virtual void allocate()
		{
			this.checkState(Recognizer.State.__DEALLOCATED);
			this.setState(Recognizer.State.__ALLOCATING);
			this.decoder.allocate();
			this.setState(Recognizer.State.__ALLOCATED);
			this.setState(Recognizer.State.__READY);
		}
		
		public virtual void deallocate()
		{
			this.checkState(Recognizer.State.__READY);
			this.setState(Recognizer.State.__DEALLOCATING);
			this.decoder.deallocate();
			this.setState(Recognizer.State.__DEALLOCATED);
		}
		
		public virtual void addResultListener(ResultListener resultListener)
		{
			this.decoder.addResultListener(resultListener);
		}
		
		public virtual void addStateListener(StateListener stateListener)
		{
			this.stateListeners.add(stateListener);
		}
		
		public virtual void removeResultListener(ResultListener resultListener)
		{
			this.decoder.removeResultListener(resultListener);
		}
		
		public virtual void removeStateListener(StateListener stateListener)
		{
			this.stateListeners.remove(stateListener);
		}
		
		private void checkState(Recognizer.State state)
		{
			if (this.currentState != state)
			{
				string text = new StringBuilder().append("Expected state ").append(state).append(" actual state ").append(this.currentState).toString();
				
				throw new IllegalStateException(text);
			}
		}
		
		private void setState(Recognizer.State state)
		{
			this.currentState = state;
			lock (this.stateListeners)
			{
				Iterator iterator = this.stateListeners.iterator();
				while (iterator.hasNext())
				{
					StateListener stateListener = (StateListener)iterator.next();
					stateListener.statusChanged(this.currentState);
				}
			}
		}
		
		public virtual Result recognize(string referenceText)
		{
			this.checkState(Recognizer.State.__READY);
			Result result;
			try
			{
				this.setState(Recognizer.State.__RECOGNIZING);
				result = this.decoder.decode(referenceText);
			}
			finally
			{
				this.setState(Recognizer.State.__READY);
			}
			return result;
		}
		
		public Recognizer(Decoder decoder, List monitors)
		{
			this.currentState = Recognizer.State.__DEALLOCATED;
			this.stateListeners = Collections.synchronizedList(new ArrayList());
			this.decoder = decoder;
			this.monitors = monitors;
			this.name = null;
		}
		
		public Recognizer()
		{
			this.currentState = Recognizer.State.__DEALLOCATED;
			this.stateListeners = Collections.synchronizedList(new ArrayList());
		}
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.decoder = (Decoder)ps.getComponent("decoder");
			this.monitors = ps.getComponentList("monitors", ClassLiteral<Monitor>.Value);
			this.name = ps.getInstanceName();
		}

		public virtual Recognizer.State getState()
		{
			return this.currentState;
		}
		
		public virtual void resetMonitors()
		{
			Iterator iterator = this.monitors.iterator();
			while (iterator.hasNext())
			{
				Monitor monitor = (Monitor)iterator.next();
				if (monitor is Resetable)
				{
					((Resetable)monitor).reset();
				}
			}
		}
		
		public override string toString()
		{
			return new StringBuilder().append("Recognizer: ").append(this.name).append(" State: ").append(this.currentState).toString();
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/Decoder, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_DECODER = "decoder";

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/instrumentation/Monitor, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_MONITORS = "monitors";

		private string name;

		private Decoder decoder;

		private Recognizer.State currentState;

		private List stateListeners;
		
		private List monitors;

		[System.Serializable]
		public sealed class State : Enum
		{			
			private State(string text, int num) : base(text, num)
			{
				System.GC.KeepAlive(this);
			}
			
			public static Recognizer.State[] values()
			{
				return (Recognizer.State[])Recognizer.State._VALUES_.Clone();
			}
			
			public static Recognizer.State valueOf(string name)
			{
				return (Recognizer.State)Enum.valueOf(ClassLiteral<Recognizer.State>.Value, name);
			}
			
			public static Recognizer.State DEALLOCATED
			{
				
				get
				{
					return Recognizer.State.__DEALLOCATED;
				}
			}

			public static Recognizer.State ALLOCATING
			{
				
				get
				{
					return Recognizer.State.__ALLOCATING;
				}
			}
			
			public static Recognizer.State ALLOCATED
			{
				
				get
				{
					return Recognizer.State.__ALLOCATED;
				}
			}

			public static Recognizer.State READY
			{
				
				get
				{
					return Recognizer.State.__READY;
				}
			}
			
			public static Recognizer.State RECOGNIZING
			{
				
				get
				{
					return Recognizer.State.__RECOGNIZING;
				}
			}
			
			public static Recognizer.State DEALLOCATING
			{
				
				get
				{
					return Recognizer.State.__DEALLOCATING;
				}
			}
		
			public static Recognizer.State ERROR
			{
				
				get
				{
					return Recognizer.State.__ERROR;
				}
			}
			
			internal static Recognizer.State __DEALLOCATED = new Recognizer.State("DEALLOCATED", 0);
			
			internal static Recognizer.State __ALLOCATING = new Recognizer.State("ALLOCATING", 1);
			
			internal static Recognizer.State __ALLOCATED = new Recognizer.State("ALLOCATED", 2);
			
			internal static Recognizer.State __READY = new Recognizer.State("READY", 3);

			internal static Recognizer.State __RECOGNIZING = new Recognizer.State("RECOGNIZING", 4);
			
			internal static Recognizer.State __DEALLOCATING = new Recognizer.State("DEALLOCATING", 5);
			
			internal static Recognizer.State __ERROR = new Recognizer.State("ERROR", 6);

			private static Recognizer.State[] _VALUES_ = new Recognizer.State[]
			{
				Recognizer.State.__DEALLOCATED,
				Recognizer.State.__ALLOCATING,
				Recognizer.State.__ALLOCATED,
				Recognizer.State.__READY,
				Recognizer.State.__RECOGNIZING,
				Recognizer.State.__DEALLOCATING,
				Recognizer.State.__ERROR
			};

			[System.Serializable]
			public enum __Enum
			{
				DEALLOCATED,
				ALLOCATING,
				ALLOCATED,
				READY,
				RECOGNIZING,
				DEALLOCATING,
				ERROR
			}
		}
	}
}
