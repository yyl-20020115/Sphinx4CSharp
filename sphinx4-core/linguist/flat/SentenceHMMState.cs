using edu.cmu.sphinx.linguist.dictionary;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.flat
{
	[System.Serializable]
	public abstract class SentenceHMMState : java.lang.Object, Serializable.__Interface, SearchState, System.Runtime.Serialization.ISerializable
	{		
		private void dump()
		{
			java.lang.System.@out.println(new StringBuilder().append(" ----- ").append(this.getTitle()).append(" ---- ").toString());
			for (int i = 0; i < this.getSuccessors().Length; i++)
			{
				SentenceHMMStateArc sentenceHMMStateArc = (SentenceHMMStateArc)this.getSuccessors()[i];
				java.lang.System.@out.println(new StringBuilder().append("   -> ").append(sentenceHMMStateArc.getState().toPrettyString()).toString());
			}
		}
		
		protected internal SentenceHMMState()
		{
			this.stateNumber = SentenceHMMState.globalStateNumber--;
			this.arcs = new LinkedHashMap();
		}
		
		public virtual void setWhich(int which)
		{
			if (!SentenceHMMState.assertionsDisabled && (which < 0 || which > 65535))
			{
				
				throw new AssertionError();
			}
			this.fields |= (which & 65535) << 8;
		}

		public virtual void setProcessed(bool processed)
		{
			if (processed)
			{
				this.fields |= 4;
			}
			else
			{
				this.fields &= -5;
			}
		}
		
		public virtual void setColor(SentenceHMMState.Color color)
		{
			if (color == SentenceHMMState.Color.__RED)
			{
				this.fields |= 2;
			}
			else
			{
				this.fields &= -3;
			}
		}

		public virtual SentenceHMMState getParent()
		{
			return this.parent;
		}
		
		internal static bool visitStates(SentenceHMMStateVisitor visitor, SentenceHMMState start, bool sorted)
		{
			object obj = SentenceHMMState.collectStates(start);
			if (sorted)
			{
				TreeSet treeSet = new TreeSet(new SentenceHMMState_3());
				treeSet.addAll((Set)obj);
				obj = treeSet;
			}
			object obj2 = obj;
			Set set;
			if (obj2 != null)
			{
				if ((set = (obj2 as Set)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				set = null;
			}
			Iterator iterator = set.iterator();
			while (iterator.hasNext())
			{
				SentenceHMMState sentenceHMMState = (SentenceHMMState)iterator.next();
				if (visitor.visit(sentenceHMMState))
				{
					return true;
				}
			}
			return false;
		}
		
		private void rawConnect(SentenceHMMStateArc sentenceHMMStateArc)
		{
			SentenceHMMState sentenceHMMState = (SentenceHMMState)sentenceHMMStateArc.getState();
			this.arcs.put(new StringBuilder().append(sentenceHMMState.getValueSignature()).append(sentenceHMMState.getStateNumber()).toString(), sentenceHMMStateArc);
		}
		
		public virtual string getValueSignature()
		{
			return this.getFullName();
		}

		private int getStateNumber()
		{
			return this.stateNumber;
		}
		
		public virtual string getTitle()
		{
			return new StringBuilder().append(this.getFullName()).append(':').append(this.stateNumber).toString();
		}
		
		public virtual SearchStateArc[] getSuccessors()
		{
			if (this.successorArray == null)
			{
				this.successorArray = (SentenceHMMStateArc[])this.arcs.values().toArray(new SentenceHMMStateArc[this.arcs.size()]);
			}
			return this.successorArray;
		}

		public virtual string getName()
		{
			return this.name;
		}

		public virtual bool isEmitting()
		{
			return false;
		}

		public virtual bool isFinal()
		{
			return (this.fields & 1) == 1;
		}
		
		public override string toString()
		{
			if (this.cachedName == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.isEmitting())
				{
					stringBuilder.append('*');
				}
				stringBuilder.append(this.getName());
				if (this.parent != null)
				{
					stringBuilder.append('_');
					stringBuilder.append(this.parent);
				}
				if (this.isFinal())
				{
					stringBuilder.append('!');
				}
				this.cachedName = stringBuilder.toString();
			}
			return this.cachedName;
		}
		
		public virtual string getFullName()
		{
			if (this.fullName == null)
			{
				if (this.parent == null)
				{
					this.fullName = this.getName();
				}
				else
				{
					this.fullName = new StringBuilder().append(this.getName()).append('.').append(this.parent.getFullName()).toString();
				}
			}
			return this.fullName;
		}
		
		public static Set collectStates(SentenceHMMState start)
		{
			HashSet hashSet = new HashSet();
			LinkedList linkedList = new LinkedList();
			linkedList.add(start);
			while (!linkedList.isEmpty())
			{
				SentenceHMMState sentenceHMMState = (SentenceHMMState)linkedList.remove(0);
				hashSet.add(sentenceHMMState);
				SearchStateArc[] successors = sentenceHMMState.getSuccessors();
				SearchStateArc[] array = successors;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					SearchStateArc searchStateArc = array[i];
					SentenceHMMState sentenceHMMState2 = (SentenceHMMState)searchStateArc.getState();
					if (!hashSet.contains(sentenceHMMState2) && !linkedList.contains(sentenceHMMState2))
					{
						linkedList.add(sentenceHMMState2);
					}
				}
			}
			return hashSet;
		}

		public virtual SentenceHMMState getLexState()
		{
			return this;
		}
		
		protected internal SentenceHMMState(string name, SentenceHMMState parent, int which) : this()
		{
			this.name = new StringBuilder().append(name).append(which).toString();
			this.parent = parent;
			this.setWhich(which);
			this.setProcessed(false);
			this.setColor(SentenceHMMState.Color.__RED);
		}

		public virtual bool isWordStart()
		{
			return (this.fields & 16) == 16;
		}

		public virtual void setWordStart(bool wordStart)
		{
			if (wordStart)
			{
				this.fields |= 16;
			}
			else
			{
				this.fields &= -17;
			}
		}

		public virtual bool isSharedState()
		{
			return (this.fields & 32) == 32;
		}

		public virtual void setSharedState(bool shared)
		{
			if (shared)
			{
				this.fields |= 32;
			}
			else
			{
				this.fields &= -33;
			}
		}
		
		public virtual Word getAssociatedWord()
		{
			Word result = null;
			SentenceHMMState sentenceHMMState = this;
			while (sentenceHMMState != null && !(sentenceHMMState is WordState))
			{
				sentenceHMMState = sentenceHMMState.getParent();
			}
			if (sentenceHMMState != null)
			{
				WordState wordState = (WordState)sentenceHMMState;
				result = wordState.getWord();
			}
			return result;
		}

		public virtual string getTypeLabel()
		{
			return "state";
		}

		public virtual bool isFanIn()
		{
			return (this.fields & 8) == 8;
		}

		public virtual void setFanIn(bool fanIn)
		{
			if (fanIn)
			{
				this.fields |= 8;
			}
			else
			{
				this.fields &= -9;
			}
		}

		public virtual bool isProcessed()
		{
			return (this.fields & 4) == 4;
		}
		
		public virtual void resetAllProcessed()
		{
			SentenceHMMState.visitStates(new SentenceHMMState_1(this), this, false);
		}
		
		public virtual WordSequence getWordHistory()
		{
			return WordSequence.__EMPTY;
		}
		
		public virtual int getNumSuccessors()
		{
			return this.arcs.size();
		}
		
		internal virtual void deleteSuccessor(SentenceHMMStateArc sentenceHMMStateArc)
		{
			this.arcs.values().remove(sentenceHMMStateArc);
		}
		
		public virtual void connect(SentenceHMMStateArc arc)
		{
			if (this.successorArray != null)
			{
				this.successorArray = null;
			}
			this.rawConnect(arc);
		}

		public virtual void setFinalState(bool state)
		{
			if (state)
			{
				this.fields |= 1;
			}
			else
			{
				this.fields &= -2;
			}
		}

		public virtual bool isUnit()
		{
			return false;
		}
		
		public virtual void dumpAll()
		{
			SentenceHMMState.visitStates(new SentenceHMMState_2(this), this, true);
		}

		protected internal virtual string getAnnotation()
		{
			return "";
		}

		public virtual void validateAll()
		{
		}
		
		public virtual string getPrettyName()
		{
			return this.getName();
		}		
		
		public virtual string toPrettyString()
		{
			return this.toString();
		}

		public virtual string getSignature()
		{
			return this.getFullName();
		}

		public virtual int getWhich()
		{
			return this.fields >> 8 & 65535;
		}
		
		public virtual SentenceHMMStateArc findArc(SentenceHMMState state)
		{
			return (SentenceHMMStateArc)this.arcs.get(state.getValueSignature());
		}
		
		public virtual SentenceHMMState.Color getColor()
		{
			if ((this.fields & 2) == 2)
			{
				return SentenceHMMState.Color.__RED;
			}
			return SentenceHMMState.Color.__GREEN;
		}

		public abstract int getOrder();
				
		internal static void access_000(SentenceHMMState sentenceHMMState)
		{
			sentenceHMMState.dump();
		}
		
		internal static int access_100(SentenceHMMState sentenceHMMState)
		{
			return sentenceHMMState.stateNumber;
		}

		static SentenceHMMState()
		{
			SentenceHMMState.globalStateNumber = -1000;
		}
		
		public static implicit operator Serializable(SentenceHMMState _ref)
		{
			Serializable result = Serializable.Cast(_ref);
			return result;
		}

		object SearchState.getLexState()
		{
			return this.getLexState();
		}

		[System.Security.SecurityCritical]
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.writeObject(this, serializationInfo);
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected SentenceHMMState(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}
		
		public static bool visitStates(object obj, SentenceHMMState start, bool sorted)
		{
			return SentenceHMMState.visitStates((SentenceHMMStateVisitor)obj, start, sorted);
		}

		private const int MASK_IS_FINAL = 1;

		private const int MASK_COLOR_RED = 2;

		private const int MASK_PROCESSED = 4;

		private const int MASK_FAN_IN = 8;

		private const int MASK_IS_WORD_START = 16;

		private const int MASK_IS_SHARED_STATE = 32;

		private const int MASK_WHICH = 65535;

		private const int SHIFT_WHICH = 8;

		private static int globalStateNumber;

		private int stateNumber;

		private int fields;

		private string name;
		
		private Map arcs;

		private SentenceHMMState parent;

		private string cachedName;

		private string fullName;

		private SentenceHMMStateArc[] successorArray;

		internal static bool assertionsDisabled = !ClassLiteral<SentenceHMMState>.Value.desiredAssertionStatus();

		[System.Serializable]
		public sealed class Color : java.lang.Enum
		{
			private Color(string text, int num) : base(text, num)
			{
				System.GC.KeepAlive(this);
			}

			public static SentenceHMMState.Color[] values()
			{
				return (SentenceHMMState.Color[])SentenceHMMState.Color._VALUES_.Clone();
			}
		
			public static SentenceHMMState.Color valueOf(string name)
			{
				return (SentenceHMMState.Color)java.lang.Enum.valueOf(ClassLiteral<SentenceHMMState.Color>.Value, name);
			}
			
			public static SentenceHMMState.Color RED
			{
				
				get
				{
					return SentenceHMMState.Color.__RED;
				}
			}
			
			public static SentenceHMMState.Color GREEN
			{
				
				get
				{
					return SentenceHMMState.Color.__GREEN;
				}
			}

			internal static SentenceHMMState.Color __RED = new SentenceHMMState.Color("RED", 0);

			internal static SentenceHMMState.Color __GREEN = new SentenceHMMState.Color("GREEN", 1);
			
			private static SentenceHMMState.Color[] _VALUES_ = new SentenceHMMState.Color[]
			{
				SentenceHMMState.Color.__RED,
				SentenceHMMState.Color.__GREEN
			};
			
			[System.Serializable]
			public enum __Enum
			{
				RED,
				GREEN
			}
		}
	}
}
