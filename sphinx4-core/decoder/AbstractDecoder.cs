using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.decoder
{
	public abstract class AbstractDecoder : java.lang.Object, ResultProducer, Configurable
	{
		private void init(string text, Logger logger, SearchManager searchManager, bool flag, bool flag2, List list)
		{
			this.name = text;
			this.logger = logger;
			this.searchManager = searchManager;
			this.fireNonFinalResults = flag;
			if (flag2)
			{
				searchManager.allocate();
			}
			Iterator iterator = list.iterator();
			while (iterator.hasNext())
			{
				ResultListener resultListener = (ResultListener)iterator.next();
				this.addResultListener(resultListener);
			}
		}
		
		public virtual void addResultListener(ResultListener resultListener)
		{
			this.__resultListeners.add(resultListener);
		}	
		public AbstractDecoder()
		{
			this.__resultListeners = new ArrayList();
		}
	
		public AbstractDecoder(SearchManager searchManager, bool fireNonFinalResults, bool autoAllocate, List resultListeners)
		{
			this.__resultListeners = new ArrayList();
			string text = java.lang.Object.instancehelper_getClass(this).getName();
			this.init(text, Logger.getLogger(text), searchManager, fireNonFinalResults, autoAllocate, resultListeners);
		}

		public abstract Result decode(string str);
	
		public virtual void newProperties(PropertySheet ps)
		{
			this.init(ps.getInstanceName(), ps.getLogger(), (SearchManager)ps.getComponent("searchManager"), ps.getBoolean("fireNonFinalResults").booleanValue(), ps.getBoolean("autoAllocate").booleanValue(), ps.getComponentList("resultListeners", ClassLiteral<ResultListener>.Value));
		}

		public virtual void allocate()
		{
			this.searchManager.allocate();
		}
		
		public virtual void deallocate()
		{
			this.searchManager.deallocate();
		}
	
		public virtual void removeResultListener(ResultListener resultListener)
		{
			this.__resultListeners.remove(resultListener);
		}
	
		protected internal virtual void fireResultListeners(Result result)
		{
			if (this.fireNonFinalResults || result.isFinal())
			{
				Iterator iterator = this.__resultListeners.iterator();
				while (iterator.hasNext())
				{
					ResultListener resultListener = (ResultListener)iterator.next();
					resultListener.newResult(result);
				}
			}
			else
			{
				this.logger.finer(new StringBuilder().append("skipping non-final result ").append(result).toString());
			}
		}

		public override string toString()
		{
			return this.name;
		}

		protected internal List resultListeners
		{
			get
			{
				return this.__resultListeners;
			}
			
			private set
			{
				this.__resultListeners = value;
			}
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/search/SearchManager, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_SEARCH_MANAGER = "searchManager";

		protected internal SearchManager searchManager;

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/ResultListener, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_RESULT_LISTENERS = "resultListeners";

		internal List __resultListeners;

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string AUTO_ALLOCATE = "autoAllocate";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string FIRE_NON_FINAL_RESULTS = "fireNonFinalResults";

		private bool fireNonFinalResults;

		private string name;

		protected internal Logger logger;
	}
}
