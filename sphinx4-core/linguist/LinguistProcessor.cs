using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist
{
	public class LinguistProcessor : java.lang.Object, Configurable, Runnable
	{		
		public LinguistProcessor(Linguist linguist)
		{
			this.linguist = linguist;
		}
		
		public LinguistProcessor()
		{
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.linguist = (Linguist)ps.getComponent("linguist");
		}

		public virtual string getName()
		{
			return this.name;
		}

		public virtual void run()
		{
		}

		protected internal virtual Linguist getLinguist()
		{
			return this.linguist;
		}

		[S4Component(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Component;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/linguist/Linguist, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_LINGUIST = "linguist";

		private string name;

		private Linguist linguist;
	}
}
