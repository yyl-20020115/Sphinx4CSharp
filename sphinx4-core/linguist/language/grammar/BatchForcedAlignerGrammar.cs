using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class BatchForcedAlignerGrammar : ForcedAlignerGrammar, GrammarInterface
	{		
		public BatchForcedAlignerGrammar(string refFile, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, dictionary.Dictionary dictionary)
			: base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.__grammars = new HashMap();
			this.currentUttName = "";
			this.refFile = refFile;
		}
		
		public BatchForcedAlignerGrammar()
		{
			this.__grammars = new HashMap();
			this.currentUttName = "";
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.refFile = ps.getString("refFile");
		}
		
		protected internal override GrammarNode createGrammar()
		{
			this.initialNode = null;
			this.finalNode = this.createGrammarNode(true);
			try
			{
				try
				{
					LineNumberReader lineNumberReader = new LineNumberReader(new FileReader(this.refFile));
					for (;;)
					{
						string text = lineNumberReader.readLine();
						if (text == null)
						{
							break;
						}
						if (String.instancehelper_isEmpty(text))
						{
							break;
						}
						int num = String.instancehelper_indexOf(text, 40) + 1;
						int num2 = String.instancehelper_indexOf(text, 41);
						if (num >= 0)
						{
							if (num <= num2)
							{
								string text2 = String.instancehelper_substring(text, num, num2);
								string text3 = String.instancehelper_trim(String.instancehelper_substring(text, 0, num - 1));
								if (!String.instancehelper_isEmpty(text3))
								{
									this.initialNode = this.createGrammarNode("<sil>");
									this.createForcedAlignerGrammar(this.initialNode, this.finalNode, text3);
									this.__grammars.put(text2, this.initialNode);
									this.currentUttName = text2;
								}
							}
						}
					}
					lineNumberReader.close();
				}
				catch (FileNotFoundException ex)
				{
					throw new Error(ex);
				}
			}
			catch (IOException ex3)
			{
				throw new Error(ex3);
			}
			return this.initialNode;
		}

		public override GrammarNode getInitialNode()
		{
			return this.initialNode;
		}
		
		public virtual void setUtterance(string utteranceName)
		{
			this.initialNode = (GrammarNode)this.__grammars.get(utteranceName);
			if (!BatchForcedAlignerGrammar.assertionsDisabled && this.initialNode == null)
			{
				
				throw new AssertionError();
			}
		}
		
		static BatchForcedAlignerGrammar()
		{
			BatchForcedAlignerGrammar.assertionsDisabled = !ClassLiteral<BatchForcedAlignerGrammar>.Value.desiredAssertionStatus();
		}

		protected internal Map grammars
		{
			
			get
			{
				return this.__grammars;
			}
			
			private set
			{
				this.__grammars = value;
			}
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"<refFile not set>"
		})]
		public const string PROP_REF_FILE = "refFile";

		protected internal string refFile;
		
		internal Map __grammars;

		protected internal string currentUttName;

		internal static bool assertionsDisabled;
	}
}
