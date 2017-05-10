using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	
	[EnclosingMethod("edu.cmu.sphinx.util.props.ConfigurationManagerUtils", null, null)]
	[SourceFile("ConfigurationManagerUtils.java")]
	
	internal sealed class ConfigurationManagerUtils_1 : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		
		static ConfigurationManagerUtils_1()
		{
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__COMPONENT_LIST.ordinal()] = 1;
			}
			catch (Exception ex)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex, 2) == null)
				{
					throw;
				}
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__COMPONENT.ordinal()] = 2;
			}
			catch (Exception ex2)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex2, 2) == null)
				{
					throw;
				}
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__BOOLEAN.ordinal()] = 3;
			}
			catch (Exception ex3)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex3, 2) == null)
				{
					throw;
				}
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__DOUBLE.ordinal()] = 4;
			}
			catch (Exception ex4)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex4, 2) == null)
				{
					throw;
				}
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__INT.ordinal()] = 5;
			}
			catch (Exception ex5)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex5, 2) == null)
				{
					throw;
				}
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__STRING.ordinal()] = 6;
			}
			catch (Exception ex6)
			{
				if (ByteCodeHelper.MapException<NoSuchFieldError>(ex6, 2) == null)
				{
					throw;
				}
			}
		}

		ConfigurationManagerUtils_1()
		{
			throw null;
		}

		
		internal static int[] _SwitchMap_edu_cmu_sphinx_util_props_PropertyType = new int[PropertyType.values().Length];
	}
}
