using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	public class LinguistDumper : LinguistProcessor
	{		
		private void dumpSearchGraph(PrintStream @out, SearchState searchState)
		{
			LinkedList linkedList = new LinkedList();
			HashSet hashSet = new HashSet();
			this.startDump(@out);
			linkedList.add(new StateLevel(searchState, 0));
			while (!linkedList.isEmpty())
			{
				StateLevel stateLevel = (StateLevel)linkedList.remove(0);
				int level = stateLevel.getLevel();
				SearchState state = stateLevel.getState();
				if (!hashSet.contains(state.getSignature()))
				{
					hashSet.add(state.getSignature());
					this.startDumpNode(@out, state, level);
					SearchStateArc[] successors = state.getSuccessors();
					for (int i = successors.Length - 1; i >= 0; i --)
					{
						SearchState state2 = successors[i].getState();
						this.dumpArc(@out, state, successors[i], level);
						if (this.depthFirst)
						{
							linkedList.add(0, new StateLevel(state2, level + 1));
						}
						else
						{
							linkedList.add(new StateLevel(state2, level + 1));
						}
					}
					this.endDumpNode(@out, state, level);
				}
			}
			this.endDump(@out);
		}

		protected internal virtual void startDump(PrintStream @out)
		{
		}

		protected internal virtual void startDumpNode(PrintStream @out, SearchState state, int level)
		{
		}

		protected internal virtual void dumpArc(PrintStream @out, SearchState from, SearchStateArc arc, int level)
		{
		}

		protected internal virtual void endDumpNode(PrintStream @out, SearchState state, int level)
		{
		}

		protected internal virtual void endDump(PrintStream @out)
		{
		}
		
		public LinguistDumper(string filename, Linguist linguist) : base(linguist)
		{
			this.depthFirst = true;
			this.eqStates = new Cache();
			this.eqSigs = new HashMap();
			this.filename = filename;
		}
		
		public LinguistDumper()
		{
			this.depthFirst = true;
			this.eqStates = new Cache();
			this.eqSigs = new HashMap();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.filename = ps.getString("filename");
		}
		
		public override void run()
		{
			try
			{
				FileOutputStream fileOutputStream = new FileOutputStream(this.filename);
				PrintStream printStream = new PrintStream(fileOutputStream);
				SearchState initialState = this.getLinguist().getSearchGraph().getInitialState();
				this.dumpSearchGraph(printStream, initialState);
				printStream.close();
			}
			catch (FileNotFoundException ex)
			{
				java.lang.System.@out.println(new StringBuilder().append("Can't dump to file ").append(this.filename).append(' ').append(ex).toString());
			}
		}

		protected internal virtual void setDepthFirst(bool depthFirst)
		{
			this.depthFirst = depthFirst;
		}
		
		private void equalCheck(SearchState searchState)
		{
			SearchState searchState2 = (SearchState)this.eqStates.cache(searchState);
			SearchState searchState3 = (SearchState)this.eqSigs.get(searchState.getSignature());
			if (searchState2 == null ^ searchState3 == null)
			{
				java.lang.System.@out.println("Missing one: ");
				java.lang.System.@out.println(new StringBuilder().append("  state val: ").append(searchState).toString());
				java.lang.System.@out.println(new StringBuilder().append("  state sig: ").append(searchState.getSignature()).toString());
				java.lang.System.@out.println(new StringBuilder().append("  eqState val: ").append(searchState2).toString());
				java.lang.System.@out.println(new StringBuilder().append("  eqSig val: ").append(searchState3).toString());
				if (searchState2 != null)
				{
					java.lang.System.@out.println(new StringBuilder().append("   eqState sig: ").append(searchState2.getSignature()).toString());
				}
				if (searchState3 != null)
				{
					java.lang.System.@out.println(new StringBuilder().append("   eqSig sig: ").append(searchState3.getSignature()).toString());
				}
			}
			if (searchState2 == null)
			{
				searchState2 = searchState;
			}
			if (searchState3 == null)
			{
				this.eqSigs.put(searchState.getSignature(), searchState);
				searchState3 = searchState;
			}
			if (!java.lang.String.instancehelper_equals(searchState2.getSignature(), searchState.getSignature()))
			{
				java.lang.System.@out.println("Sigs mismatch for: ");
				java.lang.System.@out.println(new StringBuilder().append("  state sig: ").append(searchState.getSignature()).toString());
				java.lang.System.@out.println(new StringBuilder().append("  eqSig sig: ").append(searchState3.getSignature()).toString());
				java.lang.System.@out.println(new StringBuilder().append("  state val: ").append(searchState).toString());
				java.lang.System.@out.println(new StringBuilder().append("  eqSig val: ").append(searchState3).toString());
			}
			if (!java.lang.Object.instancehelper_equals(searchState2, searchState))
			{
				java.lang.System.@out.println("obj mismatch for: ");
				java.lang.System.@out.println(new StringBuilder().append("  state sig: ").append(searchState.getSignature()).toString());
				java.lang.System.@out.println(new StringBuilder().append("  eqSig sig: ").append(searchState3.getSignature()).toString());
				java.lang.System.@out.println(new StringBuilder().append("  state val: ").append(searchState).toString());
				java.lang.System.@out.println(new StringBuilder().append("  eqSig val: ").append(searchState3).toString());
			}
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"linguistDump.txt"
		})]
		public const string PROP_FILENAME = "filename";

		private bool depthFirst;

		private string filename;
		
		internal Cache eqStates;		
		
		internal Map eqSigs;
	}
}
