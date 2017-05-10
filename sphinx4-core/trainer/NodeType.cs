using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.trainer
{
	public class NodeType : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			10,
			99,
			146
		})]
		
		public virtual bool equals(NodeType nodeType)
		{
			return nodeType != null && java.lang.String.instancehelper_equals(this.toString(), nodeType.toString());
		}

		public override string toString()
		{
			return this.name;
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			104,
			103
		})]
		
		protected internal NodeType(string name)
		{
			this.name = name;
		}

		[LineNumberTable(new byte[]
		{
			159,
			164,
			175,
			207,
			175,
			207,
			175,
			175
		})]
		static NodeType()
		{
		}

		
		public static NodeType DUMMY
		{
			
			get
			{
				return NodeType.__DUMMY;
			}
		}

		
		public static NodeType SILENCE_WITH_LOOPBACK
		{
			
			get
			{
				return NodeType.__SILENCE_WITH_LOOPBACK;
			}
		}

		
		public static NodeType UTTERANCE_END
		{
			
			get
			{
				return NodeType.__UTTERANCE_END;
			}
		}

		
		public static NodeType UTTERANCE_BEGIN
		{
			
			get
			{
				return NodeType.__UTTERANCE_BEGIN;
			}
		}

		
		public static NodeType WORD
		{
			
			get
			{
				return NodeType.__WORD;
			}
		}

		
		public static NodeType PHONE
		{
			
			get
			{
				return NodeType.__PHONE;
			}
		}

		
		public static NodeType STATE
		{
			
			get
			{
				return NodeType.__STATE;
			}
		}

		private string name;

		internal static NodeType __DUMMY = new NodeType("DUMMY");

		internal static NodeType __SILENCE_WITH_LOOPBACK = new NodeType("SILENCE_WITH_LOOPBACK");

		internal static NodeType __UTTERANCE_END = new NodeType("UTTERANCE_END");

		internal static NodeType __UTTERANCE_BEGIN = new NodeType("UTTERANCE_BEGIN");

		internal static NodeType __WORD = new NodeType("WORD");

		internal static NodeType __PHONE = new NodeType("PHONE");

		internal static NodeType __STATE = new NodeType("STATE");
	}
}
