using edu.cmu.sphinx.jsgf.rule;
using edu.cmu.sphinx.linguist.language.grammar;
using java.io;
using java.lang;
using java.net;
using java.util;
using javax.xml.parsers;
using org.xml.sax;

namespace edu.cmu.sphinx.jsgf
{
	public class GrXMLGrammar : JSGFGrammar
	{
		protected internal virtual void loadXML()
		{
			try
			{
				try
				{
					try
					{
						SAXParserFactory saxparserFactory = SAXParserFactory.newInstance();
						XMLReader xmlreader = saxparserFactory.newSAXParser().getXMLReader();
						this.rules = new HashMap();
						GrXMLHandler grXMLHandler = new GrXMLHandler(this.baseURL, this.rules, this.logger);
						xmlreader.setContentHandler(grXMLHandler);
						xmlreader.setErrorHandler(grXMLHandler);
						InputStream inputStream = this.baseURL.openStream();
						xmlreader.parse(new InputSource(inputStream));
						inputStream.close();
					}
					catch (SAXParseException ex)
					{
						throw new IOException(new StringBuilder().append("Error while parsing line ").append(ex.getLineNumber()).append(" of ").append(this.baseURL).append(": ").append(ex.getMessage()).toString());
					}
				}
				catch (SAXException ex3)
				{
					throw new IOException(new StringBuilder().append("Problem with XML: ").append(ex3).toString());
				}
			}
			catch (ParserConfigurationException ex5)
			{
				throw new IOException(Throwable.instancehelper_getMessage(ex5));
			}
		}

		public GrXMLGrammar()
		{
		}
		
		public override void commitChanges()
		{
			try
			{
				if (this.loadGrammar)
				{
					if (this.manager == null)
					{
						this.getGrammarManager();
					}
					this.loadXML();
					this.loadGrammar = false;
				}
				this.__ruleStack = new JSGFGrammar.RuleStack(this);
				this.newGrammar();
				this.firstNode = this.createGrammarNode("<sil>");
				GrammarNode grammarNode = this.createGrammarNode("<sil>");
				grammarNode.setFinalNode(true);
				Iterator iterator = this.rules.entrySet().iterator();
				while (iterator.hasNext())
				{
					Map.Entry entry = (Map.Entry)iterator.next();
					JSGFGrammar.GrammarGraph grammarGraph = new JSGFGrammar.GrammarGraph(this);
					this.__ruleStack.push((string)entry.getKey(), grammarGraph);
					JSGFGrammar.GrammarGraph grammarGraph2 = this.processRule((JSGFRule)entry.getValue());
					this.__ruleStack.pop();
					this.firstNode.add(grammarGraph.getStartNode(), 0f);
					grammarGraph.getEndNode().add(grammarNode, 0f);
					grammarGraph.getStartNode().add(grammarGraph2.getStartNode(), 0f);
					grammarGraph2.getEndNode().add(grammarGraph.getEndNode(), 0f);
				}
				this.postProcessGrammar();
			}
			catch (MalformedURLException ex)
			{
				throw new IOException(new StringBuilder().append("bad base grammar URL ").append(this.baseURL).append(' ').append(ex).toString());
			}
		}

		internal Map rules;
	}
}
