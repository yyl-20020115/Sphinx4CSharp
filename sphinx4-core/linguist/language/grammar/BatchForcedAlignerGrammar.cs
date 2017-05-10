using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.grammar.GrammarInterface"
	})]
	public class BatchForcedAlignerGrammar : ForcedAlignerGrammar, GrammarInterface
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			133,
			170,
			239,
			59,
			107,
			235,
			69,
			103
		})]
		
		public BatchForcedAlignerGrammar(string refFile, bool showGrammar, bool optimizeGrammar, bool addSilenceWords, bool addFillerWords, Dictionary dictionary) : base(showGrammar, optimizeGrammar, addSilenceWords, addFillerWords, dictionary)
		{
			this.__grammars = new HashMap();
			this.currentUttName = "";
			this.refFile = refFile;
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			232,
			55,
			107,
			235,
			73
		})]
		
		public BatchForcedAlignerGrammar()
		{
			this.__grammars = new HashMap();
			this.currentUttName = "";
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			3,
			135,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.refFile = ps.getString("refFile");
		}

		[LineNumberTable(new byte[]
		{
			18,
			103,
			141,
			177,
			135,
			110,
			133,
			107,
			137,
			104,
			130,
			106,
			145,
			105,
			133,
			113,
			117,
			116,
			104,
			101,
			255,
			9,
			69,
			226,
			60,
			98,
			109,
			98,
			141
		})]
		
		protected internal override GrammarNode createGrammar()
		{
			this.initialNode = null;
			this.finalNode = this.createGrammarNode(true);
			FileNotFoundException ex2;
			IOException ex4;
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
						if (java.lang.String.instancehelper_isEmpty(text))
						{
							break;
						}
						int num = java.lang.String.instancehelper_indexOf(text, 40) + 1;
						int num2 = java.lang.String.instancehelper_indexOf(text, 41);
						if (num >= 0)
						{
							if (num <= num2)
							{
								string text2 = java.lang.String.instancehelper_substring(text, num, num2);
								string text3 = java.lang.String.instancehelper_trim(java.lang.String.instancehelper_substring(text, 0, num - 1));
								if (!java.lang.String.instancehelper_isEmpty(text3))
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
					ex2 = ByteCodeHelper.MapException<FileNotFoundException>(ex, 1);
					goto IL_ED;
				}
			}
			catch (IOException ex3)
			{
				ex4 = ByteCodeHelper.MapException<IOException>(ex3, 1);
				goto IL_F1;
			}
			return this.initialNode;
			IL_ED:
			FileNotFoundException ex5 = ex2;
			Exception ex6 = ex5;
			
			throw new Error(ex6);
			IL_F1:
			IOException ex7 = ex4;
			Exception ex8 = ex7;
			
			throw new Error(ex8);
		}

		public override GrammarNode getInitialNode()
		{
			return this.initialNode;
		}

		[LineNumberTable(new byte[]
		{
			63,
			119,
			122
		})]
		
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
			ForcedAlignerGrammar.__<clinit>();
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
