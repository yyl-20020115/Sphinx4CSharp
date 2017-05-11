using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer;
using edu.cmu.sphinx.util.props;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.trainer
{
	public class FlatInitializerLearner : java.lang.Object, Learner, Configurable
	{		
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
		
		private bool getFeature()
		{
			bool result;
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
				java.lang.System.@out.println(new StringBuilder().append("DataProcessingException ").append(ex).toString());
				Throwable.instancehelper_printStackTrace(ex);
				return false;
			}
			return result;
		}
		
		public FlatInitializerLearner()
		{
		}
		
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
		
		public virtual void initializeComputation(Utterance utterance, UtteranceGraph graph)
		{
			this.setUtterance(utterance);
			this.setGraph(graph);
		}
		
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
