using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	public class LinguistDumper : LinguistProcessor
	{
		[LineNumberTable(new byte[]
		{
			95,
			102,
			102,
			103,
			110,
			107,
			109,
			103,
			136,
			114,
			110,
			106,
			105,
			108,
			108,
			111,
			136,
			147,
			241,
			57,
			235,
			74,
			138,
			101,
			103
		})]
		
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
					for (int i = successors.Length - 1; i >= 0; i += -1)
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

		[LineNumberTable(new byte[]
		{
			159,
			182,
			233,
			60,
			231,
			160,
			139,
			107,
			235,
			159,
			121,
			103
		})]
		
		public LinguistDumper(string filename, Linguist linguist) : base(linguist)
		{
			this.depthFirst = true;
			this.eqStates = new Cache();
			this.eqSigs = new HashMap();
			this.filename = filename;
		}

		[LineNumberTable(new byte[]
		{
			159,
			186,
			232,
			56,
			231,
			160,
			139,
			107,
			235,
			159,
			126
		})]
		
		public LinguistDumper()
		{
			this.depthFirst = true;
			this.eqStates = new Cache();
			this.eqSigs = new HashMap();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			5,
			103,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.filename = ps.getString("filename");
		}

		[LineNumberTable(new byte[]
		{
			14,
			113,
			103,
			97,
			112,
			104,
			184,
			2,
			98,
			159,
			24
		})]
		
		public override void run()
		{
			FileNotFoundException ex2;
			try
			{
				FileOutputStream.__<clinit>();
				FileOutputStream fileOutputStream = new FileOutputStream(this.filename);
				PrintStream printStream = new PrintStream(fileOutputStream);
				SearchState initialState = this.getLinguist().getSearchGraph().getInitialState();
				this.dumpSearchGraph(printStream, initialState);
				printStream.close();
			}
			catch (FileNotFoundException ex)
			{
				ex2 = ByteCodeHelper.MapException<FileNotFoundException>(ex, 1);
				goto IL_46;
			}
			return;
			IL_46:
			FileNotFoundException ex3 = ex2;
			java.lang.System.@out.println(new StringBuilder().append("Can't dump to file ").append(this.filename).append(' ').append(ex3).toString());
		}

		protected internal virtual void setDepthFirst(bool depthFirst)
		{
			this.depthFirst = depthFirst;
		}

		[LineNumberTable(new byte[]
		{
			160,
			73,
			114,
			119,
			116,
			111,
			127,
			5,
			127,
			10,
			127,
			5,
			127,
			5,
			99,
			159,
			10,
			99,
			191,
			10,
			99,
			130,
			99,
			115,
			130,
			118,
			111,
			127,
			10,
			127,
			10,
			127,
			5,
			159,
			5,
			108,
			111,
			127,
			10,
			127,
			10,
			127,
			5,
			159,
			5
		})]
		
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
			if (!Object.instancehelper_equals(searchState2, searchState))
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
