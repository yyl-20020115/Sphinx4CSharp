using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util.props
{
	[System.Serializable]
	public class PropertyType : Enum
	{
		public static PropertyType[] values()
		{
			return (PropertyType[])PropertyType._VALUES_.Clone();
		}
		
		private PropertyType(string text, int num, string text2) : base(text, num)
		{
			this.displayName = text2;
			System.GC.KeepAlive(this);
		}

		protected internal virtual bool validateString(string obj)
		{
			return true;
		}
		
		private PropertyType(string text, int num, string text2, Class @class) : base(text, num)
		{
			this.displayName = text2;
			this.checkClass = @class;
			System.GC.KeepAlive(this);
		}
	
		public static PropertyType valueOf(string name)
		{
			return (PropertyType)Enum.valueOf(ClassLiteral<PropertyType>.Value, name);
		}

		public override string toString()
		{
			return this.displayName;
		}
		
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
				catch (System.Exception)
				{
					return false;
				}
				return result != 0;
			}
			return false;
		}
		
		internal PropertyType(string text, int num, string text2, PropertyType_1 propertyType_) : this(text, num, text2)
		{
			System.GC.KeepAlive(this);
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
		
		internal static PropertyType __COMPONENT = new PropertyType("COMPONENT", 4, "Component", ClassLiteral<string>.Value);
		
		internal static PropertyType __COMPONENT_LIST = new PropertyType("COMPONENT_LIST", 5, "ComponentList", ClassLiteral<List>.Value);
		
		internal static PropertyType __STRING = new PropertyType("STRING", 6, "String", ClassLiteral<string>.Value);

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

		
		[System.Serializable]
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
