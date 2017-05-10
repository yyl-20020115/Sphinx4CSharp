using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.alignment.tokenizer
{
	public class PathExtractor : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		public override string toString()
		{
			return this.pathAndFeature;
		}

		[LineNumberTable(new byte[]
		{
			108,
			103,
			173,
			104,
			98,
			99,
			113,
			191,
			36,
			178,
			108,
			113,
			159,
			15
		})]
		
		public virtual object findFeature(Item item)
		{
			if (PathExtractor.INTERPRET_PATHS)
			{
				return item.findFeature(this.path);
			}
			Item item2 = this.findItem(item);
			object obj = null;
			if (item2 != null)
			{
				if (PathExtractor.LOGGER.isLoggable(Level.FINER))
				{
					PathExtractor.LOGGER.finer(new StringBuilder().append("findFeature: Item [").append(item2).append("], feature '").append(this.feature).append("'").toString());
				}
				obj = item2.getFeatures().getObject(this.feature);
			}
			obj = ((obj != null) ? obj : "0");
			if (PathExtractor.LOGGER.isLoggable(Level.FINER))
			{
				PathExtractor.LOGGER.finer(new StringBuilder().append("findFeature: ...results = '").append(obj).append("'").toString());
			}
			return obj;
		}

		[LineNumberTable(new byte[]
		{
			159,
			125,
			130,
			104,
			103,
			103,
			103,
			161,
			99,
			172,
			100,
			103,
			137,
			111,
			142,
			98,
			167,
			103,
			146
		})]
		
		public PathExtractor(string pathAndFeature, bool wantFeature)
		{
			this.pathAndFeature = pathAndFeature;
			if (PathExtractor.INTERPRET_PATHS)
			{
				this.path = pathAndFeature;
				return;
			}
			if (wantFeature)
			{
				int num = java.lang.String.instancehelper_lastIndexOf(pathAndFeature, ".");
				if (num == -1)
				{
					this.feature = pathAndFeature;
					this.path = null;
				}
				else
				{
					this.feature = java.lang.String.instancehelper_substring(pathAndFeature, num + 1);
					this.path = java.lang.String.instancehelper_substring(pathAndFeature, 0, num);
				}
			}
			else
			{
				this.path = pathAndFeature;
			}
			if (!PathExtractor.LAZY_COMPILE)
			{
				this.compiledPath = this.compile(this.path);
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			73,
			99,
			167,
			102,
			140,
			104,
			103,
			103,
			99,
			191,
			6,
			136,
			104,
			141,
			101
		})]
		
		private object[] compile(string text)
		{
			if (text == null)
			{
				return new object[0];
			}
			ArrayList arrayList = new ArrayList();
			StringTokenizer stringTokenizer = new StringTokenizer(text, ":.");
			while (stringTokenizer.hasMoreTokens())
			{
				string text2 = stringTokenizer.nextToken();
				OpEnum instance = OpEnum.getInstance(text2);
				if (instance == null)
				{
					string text3 = new StringBuilder().append("Bad path compiled ").append(text).toString();
					
					throw new Error(text3);
				}
				arrayList.add(instance);
				if (instance == OpEnum.RELATION)
				{
					arrayList.add(stringTokenizer.nextToken());
				}
			}
			return arrayList.toArray();
		}

		[LineNumberTable(new byte[]
		{
			55,
			103,
			173,
			104,
			178,
			130,
			117,
			114,
			104,
			108,
			104,
			108,
			104,
			103,
			102,
			140,
			104,
			103,
			102,
			140,
			104,
			108,
			104,
			108,
			104,
			108,
			104,
			114,
			97,
			102,
			102,
			98,
			191,
			26,
			101
		})]
		
		public virtual Item findItem(Item item)
		{
			if (PathExtractor.INTERPRET_PATHS)
			{
				return item.findItem(this.path);
			}
			if (this.compiledPath == null)
			{
				this.compiledPath = this.compile(this.path);
			}
			Item item2 = item;
			int num = 0;
			while (item2 != null && num < this.compiledPath.Length)
			{
				object[] array = this.compiledPath;
				int num2 = num;
				num++;
				OpEnum opEnum = (OpEnum)array[num2];
				if (opEnum == OpEnum.NEXT)
				{
					item2 = item2.getNext();
				}
				else if (opEnum == OpEnum.PREV)
				{
					item2 = item2.getPrevious();
				}
				else if (opEnum == OpEnum.NEXT_NEXT)
				{
					item2 = item2.getNext();
					if (item2 != null)
					{
						item2 = item2.getNext();
					}
				}
				else if (opEnum == OpEnum.PREV_PREV)
				{
					item2 = item2.getPrevious();
					if (item2 != null)
					{
						item2 = item2.getPrevious();
					}
				}
				else if (opEnum == OpEnum.PARENT)
				{
					item2 = item2.getParent();
				}
				else if (opEnum == OpEnum.DAUGHTER)
				{
					item2 = item2.getDaughter();
				}
				else if (opEnum == OpEnum.LAST_DAUGHTER)
				{
					item2 = item2.getLastDaughter();
				}
				else if (opEnum == OpEnum.RELATION)
				{
					object[] array2 = this.compiledPath;
					int num3 = num;
					num++;
					string relationName = (string)array2[num3];
					item2 = item2.getSharedContents().getItemRelation(relationName);
				}
				else
				{
					java.lang.System.@out.println(new StringBuilder().append("findItem: bad feature ").append(opEnum).append(" in ").append(this.path).toString());
				}
			}
			return item2;
		}

		[LineNumberTable(new byte[]
		{
			159,
			181,
			101,
			239,
			79,
			116,
			106,
			116,
			42
		})]
		static PathExtractor()
		{
		}

		
		private static Logger LOGGER = Logger.getLogger(ClassLiteral<PathExtractor>.Value.getName());

		public const string INTERPRET_PATHS_PROPERTY = "com.sun.speech.freetts.interpretCartPaths";

		public const string LAZY_COMPILE_PROPERTY = "com.sun.speech.freetts.lazyCartCompile";

		
		private static bool INTERPRET_PATHS = java.lang.String.instancehelper_equals(System.getProperty("com.sun.speech.freetts.interpretCartPaths", "false"), "true");

		
		private static bool LAZY_COMPILE = java.lang.String.instancehelper_equals(System.getProperty("com.sun.speech.freetts.lazyCartCompile", "true"), "true");

		private string pathAndFeature;

		private string path;

		private string feature;

		private object[] compiledPath;
	}
}
