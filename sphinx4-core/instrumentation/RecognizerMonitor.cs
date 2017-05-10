using System;

using edu.cmu.sphinx.recognizer;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.instrumentation
{
	public class RecognizerMonitor : java.lang.Object, StateListener, EventListener, Configurable, Monitor
	{	
		private void initRecognizer(Recognizer recognizer)
		{
			if (this.recognizer == null)
			{
				this.recognizer = recognizer;
				this.recognizer.addStateListener(this);
			}
			else if (this.recognizer != recognizer)
			{
				this.recognizer.removeStateListener(this);
				this.recognizer = recognizer;
				this.recognizer.addStateListener(this);
			}
		}

		public RecognizerMonitor(Recognizer recognizer, List allocatedMonitors, List deallocatedMonitors)
		{
			this.initRecognizer(recognizer);
			this.allocatedMonitors = allocatedMonitors;
			this.deallocatedMonitors = deallocatedMonitors;
		}

		public RecognizerMonitor()
		{
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.initRecognizer((Recognizer)ps.getComponent("recognizer"));
			this.allocatedMonitors = ps.getComponentList("allocatedMonitors", ClassLiteral<Runnable>.Value);
			this.deallocatedMonitors = ps.getComponentList("deallocatedMonitors", ClassLiteral<Runnable>.Value);
		}
	
		public virtual void statusChanged(Recognizer.State status)
		{
			List list = null;
			if (status == Recognizer.State.__ALLOCATED)
			{
				list = this.allocatedMonitors;
			}
			else if (status == Recognizer.State.__DEALLOCATED)
			{
				list = this.deallocatedMonitors;
			}
			if (list != null)
			{
				Iterator iterator = list.iterator();
				while (iterator.hasNext())
				{
					Runnable runnable = (Runnable)iterator.next();
					runnable.run();
				}
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
				"Ledu/cmu/sphinx/recognizer/Recognizer, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_RECOGNIZER = "recognizer";

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/util/props/Configurable, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_ALLOCATED_MONITORS = "allocatedMonitors";

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/util/props/Configurable, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_DEALLOCATED_MONITORS = "deallocatedMonitors";

		internal Recognizer recognizer;

		
		internal List allocatedMonitors;

		
		internal List deallocatedMonitors;

		internal string name;
	}
}
