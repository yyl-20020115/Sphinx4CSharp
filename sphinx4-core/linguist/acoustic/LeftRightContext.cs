using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic
{
	[Serializable]
	public class LeftRightContext : Context
	{
		
		public new static void __<clinit>()
		{
		}

		
		
		public static LeftRightContext get(Unit[] leftContext, Unit[] rightContext)
		{
			return new LeftRightContext(leftContext, rightContext);
		}

		public virtual Unit[] getLeftContext()
		{
			return this.leftContext;
		}

		public virtual Unit[] getRightContext()
		{
			return this.rightContext;
		}

		[LineNumberTable(new byte[]
		{
			26,
			99,
			102,
			100,
			102,
			102,
			112,
			60,
			166,
			110
		})]
		
		public static string getContextName(Unit[] context)
		{
			if (context == null)
			{
				return "*";
			}
			if (context.Length == 0)
			{
				return "(empty)";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = context.Length;
			for (int i = 0; i < num; i++)
			{
				Unit unit = context[i];
				stringBuilder.append((unit != null) ? unit.getName() : null).append('.');
			}
			stringBuilder.setLength(stringBuilder.length() - 1);
			return stringBuilder.toString();
		}

		[LineNumberTable(new byte[]
		{
			159,
			171,
			104,
			103,
			103
		})]
		
		private LeftRightContext(Unit[] array, Unit[] array2)
		{
			this.leftContext = array;
			this.rightContext = array2;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append(LeftRightContext.getContextName(this.leftContext)).append(',').append(LeftRightContext.getContextName(this.rightContext)).toString();
		}

		[LineNumberTable(new byte[]
		{
			48,
			104,
			103,
			103,
			135,
			127,
			12,
			43,
			161
		})]
		
		public override bool isPartialMatch(Context context)
		{
			if (context is LeftRightContext)
			{
				LeftRightContext leftRightContext = (LeftRightContext)context;
				Unit[] array = leftRightContext.getLeftContext();
				Unit[] array2 = leftRightContext.getRightContext();
				return (array == null || this.leftContext == null || Unit.isContextMatch(array, this.leftContext)) && (array2 == null || this.rightContext == null || Unit.isContextMatch(array2, this.rightContext));
			}
			return context == Context.__EMPTY_CONTEXT && this.leftContext == null && this.rightContext == null;
		}

		
		static LeftRightContext()
		{
			Context.__<clinit>();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected LeftRightContext(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		internal string stringRepresentation;

		
		internal Unit[] leftContext;

		
		internal Unit[] rightContext;
	}
}
