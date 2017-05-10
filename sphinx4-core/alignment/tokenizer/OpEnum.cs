using java.util;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	internal sealed class OpEnum : java.lang.Object
	{

		private OpEnum(string text)
		{
			this.name = text;
			OpEnum.map.put(text, this);
		}
		public static OpEnum getInstance(string text)
		{
			return (OpEnum)OpEnum.map.get(text);
		}

		public override string toString()
		{
			return this.name;
		}

		private static Map map = new HashMap();

		
		public static OpEnum NEXT = new OpEnum("n");

		
		public static OpEnum PREV = new OpEnum("p");

		
		public static OpEnum NEXT_NEXT = new OpEnum("nn");

		
		public static OpEnum PREV_PREV = new OpEnum("pp");

		
		public static OpEnum PARENT = new OpEnum("parent");

		
		public static OpEnum DAUGHTER = new OpEnum("daughter");

		
		public static OpEnum LAST_DAUGHTER = new OpEnum("daughtern");

		
		public static OpEnum RELATION = new OpEnum("R");

		private string name;
	}
}
