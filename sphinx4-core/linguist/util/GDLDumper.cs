using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.util
{
	public class GDLDumper : LinguistDumper
	{		
		private string getColor(SearchState searchState)
		{
			string result = "lightred";
			if (searchState.isFinal())
			{
				result = "magenta";
			}
			else if (searchState is UnitSearchState)
			{
				result = "green";
			}
			else if (searchState is WordSearchState)
			{
				result = "lightblue";
			}
			else if (searchState is HMMSearchState)
			{
				result = "orange";
			}
			return result;
		}
		
		private string getUniqueName(SearchState searchState)
		{
			return searchState.getSignature();
		}
		
		private string qs(string text)
		{
			return new StringBuilder().append('"').append(text).append('"').toString();
		}
		
		private void findNextNonHMMArc(SearchStateArc searchStateArc, List list)
		{
			HashSet hashSet = new HashSet();
			ArrayList arrayList = new ArrayList();
			arrayList.add(searchStateArc);
			while (!arrayList.isEmpty())
			{
				SearchStateArc searchStateArc2 = (SearchStateArc)arrayList.remove(0);
				if (!hashSet.contains(searchStateArc2))
				{
					hashSet.add(searchStateArc2);
					if (!(searchStateArc2.getState() is HMMSearchState))
					{
						list.add(searchStateArc2);
					}
					else
					{
						arrayList.addAll(Arrays.asList(searchStateArc2.getState().getSuccessors()));
					}
				}
			}
		}

		private string getArcColor(SearchStateArc searchStateArc)
		{
			string text = null;
			if ((double)searchStateArc.getLanguageProbability() != (double)0f)
			{
				text = "green";
			}
			if ((double)searchStateArc.getInsertionProbability() != (double)0f)
			{
				if (text == null)
				{
					text = "blue";
				}
				else
				{
					text = "purple";
				}
			}
			if (text == null)
			{
				text = "black";
			}
			return text;
		}
		
		private string formatEdgeLabel(double num)
		{
			if (num == (double)1f)
			{
				return "1";
			}
			if (num == (double)0f)
			{
				return "0";
			}
			int num2 = 5;
			string text = java.lang.String.valueOf(num);
			if (java.lang.String.instancehelper_length(text) > num2)
			{
				text = Utilities.doubleToScientificString(num, 3);
			}
			return text;
		}
		
		public GDLDumper(string filename, Linguist linguist, bool verticalLayout, bool skipHMMs, bool dumpArcLabels) : base(filename, linguist)
		{
			this.verticalLayout = verticalLayout;
			this.skipHMMs = skipHMMs;
			this.dumpArcLabels = dumpArcLabels;
			this.setDepthFirst(false);
			this.logMath = LogMath.getLogMath();
		}
		
		public GDLDumper()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.verticalLayout = ps.getBoolean("verticalLayout").booleanValue();
			this.skipHMMs = ps.getBoolean("skipHMMs").booleanValue();
			this.dumpArcLabels = ps.getBoolean("dumpArcLabels").booleanValue();
			this.setDepthFirst(false);
		}

		protected internal virtual string getDefaultName()
		{
			return "linguistDump.gdl";
		}
		
		protected internal override void startDump(PrintStream @out)
		{
			@out.println("graph: {");
			@out.println("    layout_algorithm: minbackward");
			if (this.verticalLayout)
			{
				@out.println("    orientation: top_to_bottom");
				@out.println("    manhatten_edges: no");
				@out.println("    splines: yes");
			}
			else
			{
				@out.println("    orientation: left_to_right");
				@out.println("    manhatten_edges: yes");
				@out.println("    splines: no");
			}
		}
		
		protected internal override void endDump(PrintStream @out)
		{
			@out.println("}");
		}
		
		protected internal override void startDumpNode(PrintStream @out, SearchState state, int level)
		{
			if (!this.skipHMMs || !(state is HMMSearchState))
			{
				string color = this.getColor(state);
				string text = "box";
				@out.println(new StringBuilder().append("    node: {title: ").append(this.qs(this.getUniqueName(state))).append(" label: ").append(this.qs(state.toPrettyString())).append(" color: ").append(color).append(" shape: ").append(text).append(" vertical_order: ").append(level).append('}').toString());
			}
		}

		protected internal override void endDumpNode(PrintStream @out, SearchState state, int level)
		{
		}
		
		protected internal override void dumpArc(PrintStream @out, SearchState from, SearchStateArc arc, int level)
		{
			ArrayList arrayList = new ArrayList();
			if (this.skipHMMs)
			{
				if (from is HMMSearchState)
				{
					return;
				}
				if (arc.getState() is HMMSearchState)
				{
					this.findNextNonHMMArc(arc, arrayList);
				}
				else
				{
					arrayList.add(arc);
				}
			}
			else
			{
				arrayList.add(arc);
			}
			Iterator iterator = arrayList.iterator();
			while (iterator.hasNext())
			{
				SearchStateArc searchStateArc = (SearchStateArc)iterator.next();
				string text = "";
				string arcColor = this.getArcColor(searchStateArc);
				if (this.dumpArcLabels)
				{
					double num = this.logMath.logToLinear(searchStateArc.getLanguageProbability());
					double num2 = this.logMath.logToLinear(searchStateArc.getInsertionProbability());
					text = new StringBuilder().append(" label: ").append(this.qs(new StringBuilder().append('(').append(this.formatEdgeLabel(num)).append(',').append(this.formatEdgeLabel(num2)).append(')').toString())).toString();
				}
				@out.println(new StringBuilder().append("   edge: { sourcename: ").append(this.qs(this.getUniqueName(from))).append(" targetname: ").append(this.qs(this.getUniqueName(searchStateArc.getState()))).append(text).append(" color: ").append(arcColor).append('}').toString());
			}
		}

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SKIP_HMMS = "skipHMMs";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_VERTICAL_LAYOUT = "verticalLayout";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_DUMP_ARC_LABELS = "dumpArcLabels";

		private bool skipHMMs;

		private bool verticalLayout;

		private bool dumpArcLabels;

		private LogMath logMath;
	}
}
