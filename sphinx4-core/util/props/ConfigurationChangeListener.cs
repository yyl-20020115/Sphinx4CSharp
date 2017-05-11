namespace edu.cmu.sphinx.util.props
{
	public interface ConfigurationChangeListener
	{
		void configurationChanged(string str1, string str2, ConfigurationManager cm);

		void componentAdded(ConfigurationManager cm, PropertySheet ps);

		void componentRemoved(ConfigurationManager cm, PropertySheet ps);

		void componentRenamed(ConfigurationManager cm, PropertySheet ps, string str);
	}
}
