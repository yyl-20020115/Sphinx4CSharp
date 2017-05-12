using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.decoder.search
{
	public class SimpleActiveListManager : ActiveListManagerBase
	{		
		internal static ActiveList[] access_000(SimpleActiveListManager simpleActiveListManager)
		{
			return simpleActiveListManager.currentActiveLists;
		}
		
		internal static bool access_100(SimpleActiveListManager simpleActiveListManager)
		{
			return simpleActiveListManager.checkPriorLists;
		}
		
		private void createActiveLists()
		{
			int num = this.activeListFactories.size();
			for (int i = 0; i < this.currentActiveLists.Length; i++)
			{
				int num2 = i;
				if (num2 >= num)
				{
					num2 = num - 1;
				}
				ActiveListFactory activeListFactory = (ActiveListFactory)this.activeListFactories.get(num2);
				this.currentActiveLists[i] = activeListFactory.newInstance();
			}
		}	
		
		private ActiveList findListFor(Token token)
		{
			return this.currentActiveLists[token.getSearchState().getOrder()];
		}
		
		private void dumpList(ActiveList activeList)
		{
			java.lang.System.@out.println(new StringBuilder().append("Size: ").append(activeList.size()).append(" Best token: ").append(activeList.getBestToken()).toString());
		}
		
		public SimpleActiveListManager(List activeListFactories, bool checkPriorLists)
		{
			this.logger = Logger.getLogger(java.lang.Object.instancehelper_getClass(this).getName());
			this.activeListFactories = activeListFactories;
			this.checkPriorLists = checkPriorLists;
		}
		
		public SimpleActiveListManager()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.activeListFactories = ps.getComponentList("activeListFactories", ClassLiteral<ActiveListFactory>.Value);
			this.checkPriorLists = ps.getBoolean("checkPriorListsEmpty").booleanValue();
		}
		
		public override void setNumStateOrder(int numStateOrder)
		{
			this.currentActiveLists = new ActiveList[numStateOrder];
			if (this.activeListFactories.isEmpty())
			{
				this.logger.severe("No active list factories configured");
				string text = "No active list factories configured";
				
				throw new Error(text);
			}
			if (this.activeListFactories.size() != this.currentActiveLists.Length)
			{
				this.logger.warning(new StringBuilder().append("Need ").append(this.currentActiveLists.Length).append(" active list factories, found ").append(this.activeListFactories.size()).toString());
			}
			this.createActiveLists();
		}
		
		public override void add(Token token)
		{
			ActiveList activeList = this.findListFor(token);
			if (activeList == null)
			{
				string text = new StringBuilder().append("Cannot find ActiveList for ").append(java.lang.Object.instancehelper_getClass(token.getSearchState())).toString();
				
				throw new Error(text);
			}
			activeList.add(token);
		}

		public override ActiveList getEmittingList()
		{
			return this.currentActiveLists[this.currentActiveLists.Length - 1];
		}
		
		public override void clearEmittingList()
		{
			ActiveList activeList = this.currentActiveLists[this.currentActiveLists.Length - 1];
			this.currentActiveLists[this.currentActiveLists.Length - 1] = activeList.newInstance();
		}
		
		public override Iterator getNonEmittingListIterator()
		{
			return new SimpleActiveListManager.NonEmittingListIterator(this);
		}
		
		public override void dump()
		{
			java.lang.System.@out.println("--------------------");
			ActiveList[] array = this.currentActiveLists;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				ActiveList activeList = array[i];
				this.dumpList(activeList);
			}
		}

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_CHECK_PRIOR_LISTS_EMPTY = "checkPriorListsEmpty";

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/decoder/search/ActiveListFactory, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_ACTIVE_LIST_FACTORIES = "activeListFactories";

		private Logger logger;

		private bool checkPriorLists;
		
		private List activeListFactories;

		private ActiveList[] currentActiveLists;

		internal sealed class NonEmittingListIterator : java.lang.Object, Iterator
		{			
			private void checkPriorLists()
			{
				for (int i = 0; i < this.listPtr; i++)
				{
					ActiveList activeList = SimpleActiveListManager.access_000(this.this_0)[i];
					if (activeList.size() > 0)
					{
						string text = new StringBuilder().append("At while processing state order").append(this.listPtr).append(", state order ").append(i).append(" not empty").toString();
						
						throw new Error(text);
					}
				}
			}
			
			public ActiveList next()
			{
				this.listPtr++;
				if (this.listPtr >= SimpleActiveListManager.access_000(this.this_0).Length)
				{
					
					throw new NoSuchElementException();
				}
				if (SimpleActiveListManager.access_100(this.this_0))
				{
					this.checkPriorLists();
				}
				return SimpleActiveListManager.access_000(this.this_0)[this.listPtr];
			}
			
			public NonEmittingListIterator(SimpleActiveListManager simpleActiveListManager)
			{
				this_0 = simpleActiveListManager;
				this.listPtr = -1;
			}

			public bool hasNext()
			{
				return this.listPtr + 1 < SimpleActiveListManager.access_000(this.this_0).Length - 1;
			}
			
			public void remove()
			{
				SimpleActiveListManager.access_000(this.this_0)[this.listPtr] = SimpleActiveListManager.access_000(this.this_0)[this.listPtr].newInstance();
			}

			object Iterator.next()
			{
				return this.next();
			}

			private int listPtr;
			
			internal SimpleActiveListManager this_0;
		}
	}
}
