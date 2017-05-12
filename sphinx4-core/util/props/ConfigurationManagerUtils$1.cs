using java.lang;

namespace edu.cmu.sphinx.util.props
{
	internal sealed class ConfigurationManagerUtils_1 : Object
	{
		static ConfigurationManagerUtils_1()
		{
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__COMPONENT_LIST.ordinal()] = 1;
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__COMPONENT.ordinal()] = 2;
			}
			catch (System.Exception ex2)
			{
				throw ex2;
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__BOOLEAN.ordinal()] = 3;
			}
			catch (System.Exception ex3)
			{
				throw ex3;

			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__DOUBLE.ordinal()] = 4;
			}
			catch (System.Exception ex4)
			{
				throw ex4;
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__INT.ordinal()] = 5;
			}
			catch (System.Exception ex5)
			{
				throw ex5;
			}
			try
			{
				ConfigurationManagerUtils_1._SwitchMap_edu_cmu_sphinx_util_props_PropertyType[PropertyType.__STRING.ordinal()] = 6;
			}
			catch (System.Exception ex6)
			{
				throw ex6;
			}
		}

		ConfigurationManagerUtils_1()
		{
			throw null;
		}
		
		internal static int[] _SwitchMap_edu_cmu_sphinx_util_props_PropertyType = new int[PropertyType.values().Length];
	}
}
