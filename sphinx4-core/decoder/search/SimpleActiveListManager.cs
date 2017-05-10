using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.decoder.search
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.decoder.search.ActiveListManager"
	})]
	public class SimpleActiveListManager : java.lang.Object, ActiveListManager, Configurable
	{
		
		
		internal static ActiveList[] access$000(SimpleActiveListManager simpleActiveListManager)
		{
			return simpleActiveListManager.currentActiveLists;
		}

		
		
		internal static bool access$100(SimpleActiveListManager simpleActiveListManager)
		{
			return simpleActiveListManager.checkPriorLists;
		}

		[LineNumberTable(new byte[]
		{
			60,
			108,
			108,
			98,
			100,
			132,
			114,
			238,
			58,
			230,
			72
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			127,
			127,
			31
		})]
		
		private void dumpList(ActiveList activeList)
		{
			java.lang.System.@out.println(new StringBuilder().append("Size: ").append(activeList.size()).append(" Best token: ").append(activeList.getBestToken()).toString());
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			128,
			130,
			104,
			150,
			103,
			103
		})]
		
		public SimpleActiveListManager(List activeListFactories, bool checkPriorLists)
		{
			this.logger = Logger.getLogger(Object.instancehelper_getClass(this).getName());
			this.activeListFactories = activeListFactories;
			this.checkPriorLists = checkPriorLists;
		}

		[LineNumberTable(new byte[]
		{
			15,
			134
		})]
		
		public SimpleActiveListManager()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			25,
			140,
			118,
			118
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.activeListFactories = ps.getComponentList("activeListFactories", ClassLiteral<ActiveListFactory>.Value);
			this.checkPriorLists = ps.getBoolean("checkPriorListsEmpty").booleanValue();
		}

		[LineNumberTable(new byte[]
		{
			40,
			140,
			109,
			112,
			144,
			116,
			159,
			18,
			15,
			197,
			102
		})]
		
		public virtual void setNumStateOrder(int numStateOrder)
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

		[LineNumberTable(new byte[]
		{
			78,
			104,
			99,
			112,
			159,
			0,
			103
		})]
		
		public virtual void add(Token token)
		{
			ActiveList activeList = this.findListFor(token);
			if (activeList == null)
			{
				string text = new StringBuilder().append("Cannot find ActiveList for ").append(Object.instancehelper_getClass(token.getSearchState())).toString();
				
				throw new Error(text);
			}
			activeList.add(token);
		}

		[LineNumberTable(new byte[]
		{
			104,
			113
		})]
		public virtual ActiveList getEmittingList()
		{
			return this.currentActiveLists[this.currentActiveLists.Length - 1];
		}

		[LineNumberTable(new byte[]
		{
			113,
			113,
			118
		})]
		
		public virtual void clearEmittingList()
		{
			ActiveList activeList = this.currentActiveLists[this.currentActiveLists.Length - 1];
			this.currentActiveLists[this.currentActiveLists.Length - 1] = activeList.newInstance();
		}

		
		
		
		public virtual Iterator getNonEmittingListIterator()
		{
			return new SimpleActiveListManager.NonEmittingListIterator(this);
		}

		[LineNumberTable(new byte[]
		{
			160,
			114,
			111,
			116,
			39,
			166
		})]
		
		public virtual void dump()
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

		
		[Implements(new string[]
		{
			"java.util.Iterator"
		})]
		
		[SourceFile("SimpleActiveListManager.java")]
		
		internal sealed class NonEmittingListIterator : java.lang.Object, Iterator
		{
			[LineNumberTable(new byte[]
			{
				160,
				95,
				107,
				110,
				105,
				255,
				37,
				61,
				233,
				71
			})]
			
			private void checkPriorLists()
			{
				for (int i = 0; i < this.listPtr; i++)
				{
					ActiveList activeList = SimpleActiveListManager.access$000(this.this$0)[i];
					if (activeList.size() > 0)
					{
						string text = new StringBuilder().append("At while processing state order").append(this.listPtr).append(", state order ").append(i).append(" not empty").toString();
						
						throw new Error(text);
					}
				}
			}

			[Throws(new string[]
			{
				"java.util.NoSuchElementException"
			})]
			[LineNumberTable(new byte[]
			{
				160,
				81,
				142,
				116,
				139,
				109,
				134
			})]
			
			public ActiveList next()
			{
				this.listPtr++;
				if (this.listPtr >= SimpleActiveListManager.access$000(this.this$0).Length)
				{
					
					throw new NoSuchElementException();
				}
				if (SimpleActiveListManager.access$100(this.this$0))
				{
					this.checkPriorLists();
				}
				return SimpleActiveListManager.access$000(this.this$0)[this.listPtr];
			}

			[LineNumberTable(new byte[]
			{
				160,
				70,
				111,
				103
			})]
			
			public NonEmittingListIterator(SimpleActiveListManager simpleActiveListManager)
			{
				this.listPtr = -1;
			}

			
			
			public bool hasNext()
			{
				return this.listPtr + 1 < SimpleActiveListManager.access$000(this.this$0).Length - 1;
			}

			[LineNumberTable(new byte[]
			{
				160,
				106,
				119,
				114
			})]
			
			public void remove()
			{
				SimpleActiveListManager.access$000(this.this$0)[this.listPtr] = SimpleActiveListManager.access$000(this.this$0)[this.listPtr].newInstance();
			}

			
			
			
			public object next()
			{
				return this.next();
			}

			private int listPtr;

			
			internal SimpleActiveListManager this$0 = simpleActiveListManager;
		}
	}
}
