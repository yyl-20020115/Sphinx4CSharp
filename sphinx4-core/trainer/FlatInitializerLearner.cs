using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.trainer.Learner"
	})]
	public class FlatInitializerLearner : java.lang.Object, Learner, Configurable
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			6,
			103,
			135,
			109
		})]
		
		public virtual void setUtterance(Utterance utterance)
		{
			string text = java.lang.Object.instancehelper_toString(utterance);
			FileInputStream @is = new FileInputStream(text);
			this.dataSource.setInputStream(@is, false);
		}

		
		
		public virtual void setGraph(UtteranceGraph graph)
		{
			string text = "Flat initializer does not use a graph!";
			
			throw new Error(text);
		}

		[LineNumberTable(new byte[]
		{
			21,
			145,
			104,
			167,
			109,
			113,
			104,
			196,
			109,
			164,
			109,
			255,
			7,
			71,
			226,
			60,
			97,
			127,
			5,
			102,
			162
		})]
		
		private bool getFeature()
		{
			bool result;
			DataProcessingException ex2;
			try
			{
				this.curFeature = this.frontEnd.getData();
				if (this.curFeature == null)
				{
					result = false;
				}
				else
				{
					if (this.curFeature is DataStartSignal)
					{
						this.curFeature = this.frontEnd.getData();
						if (this.curFeature == null)
						{
							return false;
						}
					}
					if (this.curFeature is DataEndSignal)
					{
						return false;
					}
					if (this.curFeature is Signal)
					{
						string text = "Can't score non-content feature";
						
						throw new Error(text);
					}
					return true;
				}
			}
			catch (DataProcessingException ex)
			{
				ex2 = ByteCodeHelper.MapException<DataProcessingException>(ex, 1);
				goto IL_8B;
			}
			return result;
			IL_8B:
			DataProcessingException ex3 = ex2;
			java.lang.System.@out.println(new StringBuilder().append("DataProcessingException ").append(ex3).toString());
			Throwable.instancehelper_printStackTrace(ex3);
			return false;
		}

		
		
		public FlatInitializerLearner()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			184,
			150,
			118,
			113
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.dataSource = (StreamCepstrumSource)ps.getComponent("source");
			this.frontEnd = (FrontEnd)ps.getComponent("frontend");
			this.frontEnd.setDataSource(this.dataSource);
		}

		public virtual void start()
		{
		}

		public virtual void stop()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			71,
			103,
			103
		})]
		
		public virtual void initializeComputation(Utterance utterance, UtteranceGraph graph)
		{
			this.setUtterance(utterance);
			this.setGraph(graph);
		}

		[LineNumberTable(new byte[]
		{
			94,
			168,
			103,
			148,
			130
		})]
		
		public virtual TrainerScore[] getScore()
		{
			if (this.getFeature())
			{
				return new TrainerScore[]
				{
					new TrainerScore(this.curFeature, 0f, -1)
				};
			}
			return null;
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/FrontEnd, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string FRONT_END = "frontend";

		private FrontEnd frontEnd;

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/util/StreamCepstrumSource, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string DATA_SOURCE = "source";

		private StreamCepstrumSource dataSource;

		private Data curFeature;
	}
}
