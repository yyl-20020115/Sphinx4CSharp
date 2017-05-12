using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.fst.operations
{
	[System.Serializable]
	public sealed class ProjectType : Enum
	{
		private ProjectType(string text, int num) : base(text, num)
		{
			System.GC.KeepAlive(this);
		}

		public static ProjectType[] values()
		{
			return (ProjectType[])ProjectType._VALUES_.Clone();
		}

		public static ProjectType valueOf(string name)
		{
			return (ProjectType)java.lang.Enum.valueOf(ClassLiteral<ProjectType>.Value, name);
		}

		public static ProjectType INPUT
		{
			get
			{
				return ProjectType.__INPUT;
			}
		}

		public static ProjectType OUTPUT
		{
			get
			{
				return ProjectType.__OUTPUT;
			}
		}
		
		internal static ProjectType __INPUT = new ProjectType("INPUT", 0);
		
		internal static ProjectType __OUTPUT = new ProjectType("OUTPUT", 1);
		
		private static ProjectType[] _VALUES_ = new ProjectType[]
		{
			ProjectType.__INPUT,
			ProjectType.__OUTPUT
		};
		
		[System.Serializable]
		public enum __Enum
		{
			INPUT,
			OUTPUT
		}
	}
}
