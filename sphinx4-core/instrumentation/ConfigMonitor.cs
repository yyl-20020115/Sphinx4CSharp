using edu.cmu.sphinx.util.props;
using edu.cmu.sphinx.util.props.tools;
using java.io;
using java.lang;
using java.util.logging;

namespace edu.cmu.sphinx.instrumentation
{
	public class ConfigMonitor : java.lang.Object, Configurable, Runnable, Monitor
	{
		public ConfigMonitor()
		{
			this.showHTML = true;
			this.showGDL = true;
			this.htmlPath = "config.html";
			this.gdlPath = "config.gdl";
			this.xmlPath = "config.xml";
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.logger = ps.getLogger();
			this.cm = ConfigurationManagerUtils.getPropertyManager(ps);
			this.showConfig = ps.getBoolean("showConfig").booleanValue();
			this.showHTML = ps.getBoolean("showConfigAsHTML").booleanValue();
			this.showGDL = ps.getBoolean("showConfigAsGDL").booleanValue();
			this.saveXML = ps.getBoolean("saveConfigAsXML").booleanValue();
			if (ps.getString("file") != null)
			{
				File file = new File(ps.getString("file"));
				if (file.getParentFile().isDirectory())
				{
					this.htmlPath = file.getPath();
					this.gdlPath = file.getPath();
					this.xmlPath = file.getPath();
				}
			}
		}
	
		public virtual void run()
		{
			if (this.showConfig)
			{
				ConfigurationManagerUtils.showConfig(this.cm);
			}
			if (this.showHTML)
			{
				try
				{
					HTMLDumper.showConfigAsHTML(this.cm, "foo.html");
				}
				catch (IOException ex)
				{
					this.logger.warning(new StringBuilder().append("Can't open ").append(this.htmlPath).append(' ').append(ex).toString());
				}
			}
			if (this.showGDL)
			{
				try
				{
					GDLDumper.showConfigAsGDL(this.cm, this.gdlPath);
				}
				catch (IOException ex4)
				{
					this.logger.warning(new StringBuilder().append("Can't open ").append(this.gdlPath).append(' ').append(ex4).toString());
				}
			}
			if (this.saveXML)
			{
				ConfigurationManager configurationManager = this.cm;
				ConfigurationManagerUtils.save(configurationManager, new File(this.xmlPath));
			}
		}

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_CONFIG = "showConfig";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_CONFIG_AS_HTML = "showConfigAsHTML";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SHOW_CONFIG_AS_GDL = "showConfigAsGDL";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_SAVE_CONFIG_AS_XML = "saveConfigAsXML";

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"mandatory",
			false
		})]
		public const string PROP_OUTFILE = "file";

		private bool showConfig;

		private bool showHTML;

		private bool saveXML;

		private bool showGDL;

		private Logger logger;

		private ConfigurationManager cm;

		private string htmlPath;

		private string gdlPath;

		private string xmlPath;
	}
}
