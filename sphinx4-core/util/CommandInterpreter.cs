using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.regex;

namespace edu.cmu.sphinx.util
{
	public class CommandInterpreter : Thread
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			9,
			110
		})]
		
		public virtual void add(string name, CommandInterface command)
		{
			this.commandList.put(name, command);
		}

		[LineNumberTable(new byte[]
		{
			161,
			20,
			121
		})]
		
		public virtual void addAlias(string command, string alias)
		{
			this.commandList.put(alias, this.commandList.get(command));
		}

		[LineNumberTable(new byte[]
		{
			18,
			232,
			34,
			236,
			95,
			176,
			107,
			104
		})]
		
		public CommandInterpreter()
		{
			this.history = new CommandInterpreter.CommandHistory(this);
			BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(System.@in));
			PrintWriter printWriter = new PrintWriter(System.@out);
			this.init(bufferedReader, printWriter);
		}

		public virtual void setPrompt(string prompt)
		{
			this.prompt = prompt;
		}

		[LineNumberTable(new byte[]
		{
			161,
			141,
			139,
			102,
			103,
			117,
			133,
			104,
			111,
			159,
			5,
			103,
			104,
			248,
			71,
			229,
			61,
			97,
			111,
			162,
			102
		})]
		
		public override void run()
		{
			while (!this.done)
			{
				string text;
				try
				{
					this.printPrompt();
					text = this.getInputLine();
					if (text == null)
					{
						break;
					}
				}
				catch (IOException ex)
				{
					goto IL_27;
				}
				try
				{
					if (this.trace)
					{
						java.lang.System.@out.println("\n----------");
						java.lang.System.@out.println(new StringBuilder().append("In : ").append(text).toString());
					}
					text = java.lang.String.instancehelper_trim(text);
					if (!java.lang.String.instancehelper_isEmpty(text))
					{
						this.putResponse(this.execute(text));
					}
				}
				catch (IOException ex2)
				{
					goto IL_91;
				}
				continue;
				IL_91:
				goto IL_99;
				break;
				IL_27:
				IL_99:
				java.lang.System.@out.println("Exception: CommandInterpreter.run()");
				break;
			}
			this.onExit();
		}

		[LineNumberTable(new byte[]
		{
			161,
			41,
			107,
			108,
			107,
			104,
			191,
			5
		})]
		
		public virtual void putResponse(string response)
		{
			if (response != null && !java.lang.String.instancehelper_isEmpty(response))
			{
				this.@out.println(response);
				this.@out.flush();
				if (this.trace)
				{
					java.lang.System.@out.println(new StringBuilder().append("Out: ").append(response).toString());
				}
			}
		}

		
		
		
		internal static void access_000(CommandInterpreter commandInterpreter)
		{
			commandInterpreter.dumpCommands();
		}

		[LineNumberTable(new byte[]
		{
			161,
			66,
			198,
			133,
			116,
			99,
			139,
			166,
			142
		})]
		
		protected internal virtual string execute(string[] args)
		{
			string result = "";
			if (args.Length > 0)
			{
				CommandInterface commandInterface = (CommandInterface)this.commandList.get(args[0]);
				if (commandInterface != null)
				{
					result = commandInterface.execute(this, args);
				}
				else
				{
					result = "ERR  CMD_NOT_FOUND";
				}
				this.totalCommands++;
			}
			return result;
		}

		[LineNumberTable(new byte[]
		{
			162,
			11,
			103,
			167,
			106,
			104,
			109,
			135,
			98,
			102,
			115,
			97
		})]
		
		public virtual bool load(string filename)
		{
			int result;
			try
			{
				FileReader fileReader = new FileReader(filename);
				BufferedReader bufferedReader = new BufferedReader(fileReader);
				string cmdString;
				while ((cmdString = bufferedReader.readLine()) != null)
				{
					string text = this.execute(cmdString);
					if (!java.lang.String.instancehelper_equals(text, "OK"))
					{
						this.putResponse(text);
					}
				}
				fileReader.close();
				result = 1;
			}
			catch (IOException ex)
			{
				return false;
			}
			return result != 0;
		}

		
		
		internal static CommandInterpreter.CommandHistory access_100(CommandInterpreter commandInterpreter)
		{
			return commandInterpreter.history;
		}

		
		
		internal static int access_200(CommandInterpreter commandInterpreter)
		{
			return commandInterpreter.totalCommands;
		}

		
		
		internal static bool access_302(CommandInterpreter commandInterpreter, bool result)
		{
			commandInterpreter.done = result;
			return result;
		}


		private void dumpCommands()
		{
			TreeMap.__<clinit>();
			Iterator iterator = new TreeMap(this.commandList).entrySet().iterator();
			while (iterator.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator.next();
				this.putResponse(new StringBuilder().append((string)entry.getKey()).append(" - ").append(((CommandInterface)entry.getValue()).getHelp()).toString());
			}
		}

		private void init(BufferedReader bufferedReader, PrintWriter printWriter)
		{
			this.commandList = new HashMap();
			this.addStandardCommands();
			this.setStreams(bufferedReader, printWriter);
		}

		private void addStandardCommands()
		{
			this.add("help", new CommandInterpreter_1(this));
			this.add("history", new CommandInterpreter_2(this));
			this.add("status", new CommandInterpreter_3(this));
			this.add("echo", new CommandInterpreter_4(this));
			this.add("quit", new CommandInterpreter_5(this));
			this.add("on_exit", new CommandInterpreter_6(this));
			this.add("version", new CommandInterpreter_7(this));
			this.add("gc", new CommandInterpreter_8(this));
			this.add("memory", new CommandInterpreter_9(this));
			this.add("delay", new CommandInterpreter_10(this));
			this.add("repeat", new CommandInterpreter_11(this));
			this.add("load", new CommandInterpreter_12(this));
			this.add("chain", new CommandInterpreter_13(this));
			this.add("time", new CommandInterpreter_14(this));
		}

		public virtual void setStreams(BufferedReader @in, PrintWriter @out)
		{
			this.@in = @in;
			this.@out = @out;
		}
		public virtual string execute(string cmdString)
		{
			if (this.trace)
			{
				java.lang.System.@out.println(new StringBuilder().append("Execute: ").append(cmdString).toString());
			}
			return this.execute(this.parseMessage(cmdString));
		}	
		protected internal virtual string[] parseMessage(string message)
		{
			ArrayList arrayList = new ArrayList(20);
			StreamTokenizer streamTokenizer = new StreamTokenizer(new StringReader(message));
			streamTokenizer.resetSyntax();
			streamTokenizer.whitespaceChars(0, 32);
			streamTokenizer.wordChars(33, 255);
			streamTokenizer.quoteChar(34);
			streamTokenizer.quoteChar(34);
			streamTokenizer.commentChar(35);
			for (;;)
			{
				try
				{
					int num = streamTokenizer.nextToken();
					if (num == -3)
					{
						arrayList.add(streamTokenizer.sval);
					}
					else if (num == 39 || num == 34)
					{
						arrayList.add(streamTokenizer.sval);
					}
					else
					{
						if (num != -2)
						{
							break;
						}
						java.lang.System.@out.println("Unexpected numeric token!");
					}
				}
				catch (IOException ex)
				{
					break;
				}
			}
			return (string[])arrayList.toArray(new string[arrayList.size()]);
		}		
		private void printPrompt()
		{
			if (this.prompt != null)
			{
				this.@out.print(this.prompt);
				this.@out.flush();
			}
		}
	
		private string getInputLine()
		{
			string text = this.@in.readLine();
			if (text == null)
			{
				return null;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			Pattern pattern = CommandInterpreter.historyPush;
			object _ref = text;
			CharSequence charSequence;
			charSequence.__ref = _ref;
			Matcher matcher = pattern.matcher(charSequence);
			if (matcher.matches())
			{
				num = 1;
				num2 = 1;
				text = matcher.group(1);
			}
			if (java.lang.String.instancehelper_startsWith(text, "^"))
			{
				Pattern pattern2 = CommandInterpreter.editPattern;
				_ref = text;
				charSequence.__ref = _ref;
				matcher = pattern2.matcher(charSequence);
				if (matcher.matches())
				{
					string text2 = matcher.group(1);
					string text3 = matcher.group(2);
					PatternSyntaxException ex2;
					try
					{
						Pattern pattern3 = Pattern.compile(text2);
						Pattern pattern4 = pattern3;
						_ref = this.history.getLast(0);
						charSequence.__ref = _ref;
						Matcher matcher2 = pattern4.matcher(charSequence);
						if (matcher2.find())
						{
							text = matcher2.replaceFirst(text3);
							num2 = 1;
						}
						else
						{
							num3 = 1;
							this.putResponse(new StringBuilder().append(text).append(": substitution failed").toString());
						}
					}
					catch (PatternSyntaxException ex)
					{
						ex2 = ByteCodeHelper.MapException<PatternSyntaxException>(ex, 1);
						goto IL_114;
					}
					goto IL_144;
					IL_114:
					PatternSyntaxException ex3 = ex2;
					num3 = 1;
					this.putResponse(new StringBuilder().append("Bad regexp: ").append(ex3.getDescription()).toString());
					IL_144:;
				}
				else
				{
					num3 = 1;
					this.putResponse("bad substitution sytax, use ^old^new^");
				}
			}
			else
			{
				Pattern pattern5 = CommandInterpreter.bbPattern;
				_ref = text;
				charSequence.__ref = _ref;
				if ((matcher = pattern5.matcher(charSequence)).find())
				{
					text = matcher.replaceAll(this.history.getLast(0));
					num2 = 1;
				}
				else if (java.lang.String.instancehelper_startsWith(text, "!"))
				{
					if (java.lang.String.instancehelper_matches(text, "!\\d+"))
					{
						int num4 = Integer.parseInt(java.lang.String.instancehelper_substring(text, 1));
						text = this.history.get(num4);
					}
					else if (java.lang.String.instancehelper_matches(text, "!-\\d+"))
					{
						int num4 = Integer.parseInt(java.lang.String.instancehelper_substring(text, 2));
						text = this.history.getLast(num4 - 1);
					}
					else
					{
						text = this.history.findLast(java.lang.String.instancehelper_substring(text, 1));
					}
					num2 = 1;
				}
			}
			if (num3 != 0)
			{
				return "";
			}
			if (!java.lang.String.instancehelper_isEmpty(text))
			{
				this.history.add(text);
			}
			if (num2 != 0)
			{
				this.putResponse(text);
			}
			return (num == 0) ? text : "";
		}
		protected internal virtual void onExit()
		{
			this.execute("on_exit");
			java.lang.System.@out.println("----------\n");
		}
		
		public CommandInterpreter(BufferedReader @in, PrintWriter @out)
		{
			this.history = new CommandInterpreter.CommandHistory(this);
			this.init(@in, @out);
		}

		public virtual void setTrace(bool trace)
		{
			this.trace = trace;
		}

		public virtual Socket getSocket()
		{
			return this.socket;
		}

		public virtual void setSocket(Socket skt)
		{
			this.socket = skt;
		}

		
		[LineNumberTable(new byte[]
		{
			161,
			30,
			108
		})]
		
		public virtual void add(Map newCommands)
		{
			this.commandList.putAll(newCommands);
		}

		public virtual void close()
		{
			this.done = true;
		}

		public virtual string getPrompt()
		{
			return this.prompt;
		}

		public virtual PrintWriter getPrintWriter()
		{
			return this.@out;
		}

		[LineNumberTable(new byte[]
		{
			162,
			62,
			166,
			111,
			107,
			102,
			191,
			2,
			2,
			97,
			139
		})]
		
		public static void main(string[] args)
		{
			CommandInterpreter commandInterpreter = new CommandInterpreter();
			Exception ex2;
			try
			{
				java.lang.System.@out.println("Welcome to the Command interpreter test program");
				commandInterpreter.setPrompt("CI> ");
				commandInterpreter.run();
				java.lang.System.@out.println("Goodbye!");
			}
			catch (Exception ex)
			{
				ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				goto IL_44;
			}
			return;
			IL_44:
			Exception ex3 = ex2;
			java.lang.System.@out.println(ex3);
		}

		[LineNumberTable(new byte[]
		{
			159,
			8,
			133,
			111,
			101,
			106
		})]
		static CommandInterpreter()
		{
			Thread.__<clinit>();
			CommandInterpreter.historyPush = Pattern.compile("(.+):p");
			CommandInterpreter.editPattern = Pattern.compile("\\^(.+?)\\^(.*?)\\^?");
			CommandInterpreter.bbPattern = Pattern.compile("(!!)");
		}

		
		private Map commandList;

		private int totalCommands;

		private BufferedReader @in;

		private PrintWriter @out;

		private string prompt;

		private bool done;

		private bool trace;

		
		private CommandInterpreter.CommandHistory history;

		private Socket socket;

		
		private static Pattern historyPush;

		
		private static Pattern editPattern;

		
		private static Pattern bbPattern;

		
		.
		
		internal sealed class CommandHistory : java.lang.Object
		{
			[LineNumberTable(new byte[]
			{
				162,
				142,
				112,
				104,
				31,
				12,
				198
			})]
			
			public void dump()
			{
				for (int i = 0; i < this.history.size(); i++)
				{
					string text = this.get(i);
					this.this_0.putResponse(new StringBuilder().append(i).append(" ").append(text).toString());
				}
			}

			[LineNumberTable(new byte[]
			{
				162,
				113,
				110,
				146,
				112
			})]
			
			public string get(int num)
			{
				if (this.history.size() > num)
				{
					return (string)this.history.get(num);
				}
				this.this_0.putResponse("command not found");
				return "";
			}

			[LineNumberTable(new byte[]
			{
				162,
				75,
				143
			})]
			
			internal CommandHistory(CommandInterpreter commandInterpreter)
			{
				this.history = new ArrayList(100);
			}

			[LineNumberTable(new byte[]
			{
				162,
				86,
				109
			})]
			
			public void add(string text)
			{
				this.history.add(text);
			}

			[LineNumberTable(new byte[]
			{
				162,
				97,
				110,
				159,
				1,
				112
			})]
			
			public string getLast(int num)
			{
				if (this.history.size() > num)
				{
					return (string)this.history.get(this.history.size() - 1 - num);
				}
				this.this_0.putResponse("command not found");
				return "";
			}

			[LineNumberTable(new byte[]
			{
				162,
				129,
				114,
				104,
				105,
				226,
				61,
				230,
				70,
				112
			})]
			
			public string findLast(string text)
			{
				for (int i = this.history.size() - 1; i >= 0; i += -1)
				{
					string text2 = this.get(i);
					if (java.lang.String.instancehelper_startsWith(text2, text))
					{
						return text2;
					}
				}
				this.this_0.putResponse("command not found");
				return "";
			}

			
			
			private List history;

			
			internal CommandInterpreter this_0 = commandInterpreter;
		}
	}
}
