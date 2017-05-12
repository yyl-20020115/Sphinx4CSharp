using edu.cmu.sphinx.jsgf.rule;
using java.lang;
using java.net;
using java.util;
using java.util.logging;
using org.xml.sax;
using org.xml.sax.helpers;

namespace edu.cmu.sphinx.jsgf
{
	public class GrXMLHandler : DefaultHandler
	{
		public GrXMLHandler(URL baseURL, Map rules, Logger logger)
		{
			this.baseURL = baseURL;
			this.__topRuleMap = rules;
			this.logger = logger;
		}
		
		private void addToCurrent(JSGFRule jsgfrule, JSGFRule rule)
		{
			if (jsgfrule == null)
			{
				return;
			}
			if (this.currentRule == null)
			{
				this.currentRule = jsgfrule;
				return;
			}
			if (this.currentRule is JSGFRuleSequence)
			{
				JSGFRuleSequence jsgfruleSequence = (JSGFRuleSequence)this.currentRule;
				jsgfruleSequence.append(rule);
				jsgfrule.parent = this.currentRule;
				this.currentRule = jsgfrule;
			}
			else if (this.currentRule is JSGFRuleAlternatives)
			{
				JSGFRuleAlternatives jsgfruleAlternatives = (JSGFRuleAlternatives)this.currentRule;
				jsgfruleAlternatives.append(rule);
				jsgfrule.parent = this.currentRule;
				this.currentRule = jsgfrule;
			}
		}
		
		public override void startElement(string uri, string localName, string qName, Attributes attributes)
		{
			JSGFRule jsgfrule = null;
			JSGFRule jsgfrule2 = null;
			this.logger.fine(new StringBuilder().append("Starting element ").append(qName).toString());
			if (String.instancehelper_equals(qName, "rule"))
			{
				string value = attributes.getValue("id");
				if (value != null)
				{
					jsgfrule = new JSGFRuleSequence(new ArrayList());
					this.__topRuleMap.put(value, (JSGFRuleSequence)jsgfrule);
					jsgfrule2 = (JSGFRuleSequence)jsgfrule;
				}
			}
			if (String.instancehelper_equals(qName, "item"))
			{
				string value = attributes.getValue("repeat");
				if (value != null)
				{
					jsgfrule = new JSGFRuleSequence(new ArrayList());
					JSGFRuleCount jsgfruleCount = new JSGFRuleCount((JSGFRuleSequence)jsgfrule, 3);
					jsgfrule2 = jsgfruleCount;
				}
				else
				{
					jsgfrule = new JSGFRuleSequence(new ArrayList());
					jsgfrule2 = (JSGFRuleSequence)jsgfrule;
				}
			}
			if (String.instancehelper_equals(qName, "one-of"))
			{
				jsgfrule = new JSGFRuleAlternatives(new ArrayList());
				jsgfrule2 = (JSGFRuleAlternatives)jsgfrule;
			}
			this.addToCurrent(jsgfrule, jsgfrule2);
		}
		
		public override void characters(char[] buf, int offset, int len)
		{
			string text = String.instancehelper_trim(String.newhelper(buf, offset, len));
			if (String.instancehelper_length(text) == 0)
			{
				return;
			}
			this.logger.fine(new StringBuilder().append("Processing text ").append(text).toString());
			JSGFRuleToken jsgfruleToken = new JSGFRuleToken(text);
			this.addToCurrent(jsgfruleToken, jsgfruleToken);
			this.currentRule = jsgfruleToken.parent;
		}
		
		public override void endElement(string uri, string localName, string qName)
		{
			this.logger.fine(new StringBuilder().append("Ending element ").append(qName).toString());
			if (String.instancehelper_equals(qName, "item") || String.instancehelper_equals(qName, "one-of") || String.instancehelper_equals(qName, "rule"))
			{
				this.currentRule = this.currentRule.parent;
			}
		}

		protected internal Map topRuleMap
		{
			
			get
			{
				return this.__topRuleMap;
			}
			
			private set
			{
				this.__topRuleMap = value;
			}
		}

		internal Map __topRuleMap;

		internal Logger logger;

		internal JSGFRule currentRule;

		internal URL baseURL;
	}
}
