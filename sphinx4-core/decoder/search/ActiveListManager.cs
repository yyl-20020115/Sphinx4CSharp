﻿using edu.cmu.sphinx.util.props;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.decoder.search
{
	public interface ActiveListManager : Configurable
	{
		void add(Token t);

		Iterator getNonEmittingListIterator();

		ActiveList getEmittingList();

		void clearEmittingList();

		void dump();

		void setNumStateOrder(int i);

	}

	public abstract class ActiveListManagerBase : Object, ActiveListManager
	{
		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			2000
		})]
		public const string PROP_ABSOLUTE_WORD_BEAM_WIDTH = "absoluteWordBeamWidth";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.0
		})]
		public const string PROP_RELATIVE_WORD_BEAM_WIDTH = "relativeWordBeamWidth";

		public abstract void add(Token t);
		public abstract void clearEmittingList();
		public abstract void dump();
		public abstract ActiveList getEmittingList();
		public abstract Iterator getNonEmittingListIterator();
		public abstract void newProperties(PropertySheet ps);
		public abstract void setNumStateOrder(int i);

		public static class __Fields
		{
			public const string PROP_ABSOLUTE_WORD_BEAM_WIDTH = "absoluteWordBeamWidth";

			public const string PROP_RELATIVE_WORD_BEAM_WIDTH = "relativeWordBeamWidth";
		}
	}
}
