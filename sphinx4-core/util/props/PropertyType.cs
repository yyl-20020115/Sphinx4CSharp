﻿using System;

using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util.props
{
	
	
	[Serializable]
	public class PropertyType : Enum
	{
		
		public static void __<clinit>()
		{
		}

		
		
		public static PropertyType[] values()
		{
			return (PropertyType[])PropertyType._VALUES_.Clone();
		}

		
		[LineNumberTable(new byte[]
		{
			40,
			106,
			103
		})]
		
		private PropertyType(string text, int num, string text2) : base(text, num)
		{
			this.displayName = text2;
			GC.KeepAlive(this);
		}

		[Throws(new string[]
		{
			"java.lang.Exception"
		})]
		protected internal virtual bool validateString(string obj)
		{
			return true;
		}

		
		[LineNumberTable(new byte[]
		{
			49,
			106,
			103,
			104
		})]
		
		private PropertyType(string text, int num, string text2, Class @class) : base(text, num)
		{
			this.displayName = text2;
			this.checkClass = @class;
			GC.KeepAlive(this);
		}

		
		
		public static PropertyType valueOf(string name)
		{
			return (PropertyType)Enum.valueOf(ClassLiteral<PropertyType>.Value, name);
		}

		public override string toString()
		{
			return this.displayName;
		}

		[LineNumberTable(new byte[]
		{
			78,
			104,
			109,
			136,
			127,
			6,
			97,
			162
		})]
		
		public virtual bool isValid(object obj)
		{
			if (this.checkClass != null)
			{
				return this.checkClass.isInstance(obj);
			}
			if (obj is string)
			{
				int result;
				try
				{
					result = (this.validateString((string)obj) ? 1 : 0);
				}
				catch (Exception ex)
				{
					if (ByteCodeHelper.MapException<Exception>(ex, 2) == null)
					{
						throw;
					}
					return false;
				}
				return result != 0;
			}
			return false;
		}

		
		
		
		internal PropertyType(string text, int num, string text2, PropertyType_1 propertyType_) : this(text, num, text2)
		{
			GC.KeepAlive(this);
		}

		[LineNumberTable(new byte[]
		{
			159,
			163,
			245,
			70,
			245,
			70,
			245,
			70,
			245,
			70,
			122,
			122,
			250,
			75,
			245,
			80,
			250,
			9
		})]
		static PropertyType()
		{
		}

		
		public static PropertyType INT
		{
			
			get
			{
				return PropertyType.__INT;
			}
		}

		
		public static PropertyType BOOLEAN
		{
			
			get
			{
				return PropertyType.__BOOLEAN;
			}
		}

		
		public static PropertyType FLOAT
		{
			
			get
			{
				return PropertyType.__FLOAT;
			}
		}

		
		public static PropertyType DOUBLE
		{
			
			get
			{
				return PropertyType.__DOUBLE;
			}
		}

		
		public static PropertyType COMPONENT
		{
			
			get
			{
				return PropertyType.__COMPONENT;
			}
		}

		
		public static PropertyType COMPONENT_LIST
		{
			
			get
			{
				return PropertyType.__COMPONENT_LIST;
			}
		}

		
		public static PropertyType STRING
		{
			
			get
			{
				return PropertyType.__STRING;
			}
		}

		
		public static PropertyType RESOURCE
		{
			
			get
			{
				return PropertyType.__RESOURCE;
			}
		}

		
		public static PropertyType STRING_LIST
		{
			
			get
			{
				return PropertyType.__STRING_LIST;
			}
		}

		
		internal static PropertyType __INT = new PropertyType_1("INT", 0, "int");

		
		internal static PropertyType __BOOLEAN = new PropertyType_2("BOOLEAN", 1, "boolean");

		
		internal static PropertyType __FLOAT = new PropertyType_3("FLOAT", 2, "float");

		
		internal static PropertyType __DOUBLE = new PropertyType_4("DOUBLE", 3, "double");

		
		internal static PropertyType __COMPONENT = new PropertyType("COMPONENT", 4, "Component", ClassLiteral<java.lang.String>.Value);

		
		internal static PropertyType __COMPONENT_LIST = new PropertyType("COMPONENT_LIST", 5, "ComponentList", ClassLiteral<List>.Value);

		
		internal static PropertyType __STRING = new PropertyType("STRING", 6, "java.lang.String", ClassLiteral<java.lang.String>.Value);

		
		internal static PropertyType __RESOURCE = new PropertyType_5("RESOURCE", 7, "Resource");

		
		internal static PropertyType __STRING_LIST = new PropertyType("STRING_LIST", 8, "StringList", ClassLiteral<List>.Value);

		
		private string displayName;

		
		private Class checkClass;

		
		private static PropertyType[] _VALUES_ = new PropertyType[]
		{
			PropertyType.__INT,
			PropertyType.__BOOLEAN,
			PropertyType.__FLOAT,
			PropertyType.__DOUBLE,
			PropertyType.__COMPONENT,
			PropertyType.__COMPONENT_LIST,
			PropertyType.__STRING,
			PropertyType.__RESOURCE,
			PropertyType.__STRING_LIST
		};

		
		[Serializable]
		public enum __Enum
		{
			INT,
			BOOLEAN,
			FLOAT,
			DOUBLE,
			COMPONENT,
			COMPONENT_LIST,
			STRING,
			RESOURCE,
			STRING_LIST
		}
	}
}
