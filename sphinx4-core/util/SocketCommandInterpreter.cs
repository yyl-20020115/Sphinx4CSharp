using java.io;
using java.lang;
using java.net;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class SocketCommandInterpreter : Thread
	{
		private void spawnCommandInterpreter(Socket socket)
		{
			try
			{
				BufferedReader @in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
				PrintWriter @out = new PrintWriter(socket.getOutputStream(), true);
				CommandInterpreter commandInterpreter = new CommandInterpreter(@in, @out);
				commandInterpreter.setSocket(socket);
				commandInterpreter.add(this.commandList);
				commandInterpreter.setTrace(this.trace);
				commandInterpreter.start();
			}
			catch (IOException ex)
			{
				java.lang.System.err.println(new StringBuilder().append("Could not attach CI to socket ").append(ex).toString());
			}
		}

		public SocketCommandInterpreter(int port)
		{
			this.acceptConnections = true;
			this.port = port;
			this.commandList = new HashMap();
		}

		public virtual void add(string name, CommandInterface command)
		{
			this.commandList.put(name, command);
		}

		public virtual void setTrace(bool trace)
		{
			this.trace = trace;
		}

		public sealed override void run()
		{
			ServerSocket serverSocket;
			IOException ex2 = null;
			try
			{
				serverSocket = new ServerSocket(this.port);
				java.lang.System.@out.println(new StringBuilder().append("Waiting on ").append(serverSocket).toString());
			}
			catch (IOException ex)
			{
				ex2 = ex;
				goto IL_3F;
			}
			IOException ex5;
			while (this.acceptConnections)
			{
				IOException ex4;
				try
				{
					Socket socket = serverSocket.accept();
					this.spawnCommandInterpreter(socket);
				}
				catch (IOException ex3)
				{
					ex4 = ex3;
					goto IL_9B;
				}
				continue;
				IL_9B:
				ex5 = ex4;
				java.lang.System.err.println(new StringBuilder().append("Could not accept socket ").append(ex5).toString());
				Throwable.instancehelper_printStackTrace(ex5);
				break;
			}
			IOException ex7;
			try
			{
				serverSocket.close();
			}
			catch (IOException ex6)
			{
				ex7 = ex6;
				goto IL_E4;
			}
			return;
			IL_E4:
			ex5 = ex7;
			java.lang.System.err.println(new StringBuilder().append("Could not close server socket ").append(ex5).toString());
			Throwable.instancehelper_printStackTrace(ex5);
			return;
			IL_3F:
			ex5 = ex2;
			java.lang.System.@out.println(new StringBuilder().append("Can't open socket on port ").append(this.port).toString());
			Throwable.instancehelper_printStackTrace(ex5);
		}

		public virtual void setStopAcceptConnections()
		{
			this.acceptConnections = false;
		}

		public static void main(string[] args)
		{
			SocketCommandInterpreter socketCommandInterpreter = new SocketCommandInterpreter(7890);
			socketCommandInterpreter.add("testCommand", new SocketCommandInterpreter_1());
			socketCommandInterpreter.add("time", new SocketCommandInterpreter_2());
			java.lang.System.@out.println("Welcome to SocketCommand interpreter test program");
			socketCommandInterpreter.setTrace(true);
			socketCommandInterpreter.start();
		}

		private int port;

		private Map commandList;

		private bool trace;

		private bool acceptConnections;

		internal sealed class SocketCommandInterpreter_1 : Object, CommandInterface
		{

			internal SocketCommandInterpreter_1()
			{
			}

			public string execute(CommandInterpreter commandInterpreter, string[] array)
			{
				return "this is a test";
			}

			public string getHelp()
			{
				return "a test command";
			}
		}
		internal sealed class SocketCommandInterpreter_2 : Object, CommandInterface
		{


			internal SocketCommandInterpreter_2()
			{
			}



			public string execute(CommandInterpreter commandInterpreter, string[] array)
			{
				return new StringBuilder().append("Time is ").append(new Date()).toString();
			}

			public string getHelp()
			{
				return "shows the current time";
			}
		}

	}
}
