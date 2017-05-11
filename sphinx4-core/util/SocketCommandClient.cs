using java.io;
using java.lang;
using java.net;

namespace edu.cmu.sphinx.util
{
	public class SocketCommandClient : java.lang.Object
	{		
		public virtual bool sendCommand(string command)
		{
			for (int i = 0; i < 2; i++)
			{
				if (this.checkOpen())
				{
					this.outWriter.println(command);
					if (!this.outWriter.checkError())
					{
						return true;
					}
					this.close();
					java.lang.System.err.println(new StringBuilder().append("IO error while sending ").append(command).toString());
				}
			}
			return false;
		}
		
		public virtual bool isResponse()
		{
			int result = 0;
			if (!this.checkOpen())
			{
				return false;
			}
			try
			{
				result = (this.inReader.ready() ? 1 : 0);
			}
			catch (IOException ex)
			{
				goto IL_20;
			}
			return result != 0;
			IL_20:
			java.lang.System.err.println("IO error while checking response");
			this.close();
			return result != 0;
		}
		
		public virtual string getResponse()
		{
			string result = null;
			if (!this.checkOpen())
			{
				return null;
			}
			try
			{
				result = this.inReader.readLine();
			}
			catch (IOException ex)
			{
				goto IL_20;
			}
			return result;
			IL_20:
			java.lang.System.err.println("IO error while reading response");
			this.close();
			return result;
		}
		
		public virtual string sendCommandGetResponse(string command)
		{
			string result = null;
			if (this.sendCommand(command))
			{
				result = this.getResponse();
			}
			return result;
		}
		
		private void open()
		{
			this.open(this.host, this.port);
		}
		
		public virtual void open(string aHost, int aPort)
		{
			this.host = aHost;
			this.port = aPort;
			this.socket = new Socket(this.host, this.port);
			this.inReader = new BufferedReader(new InputStreamReader(this.socket.getInputStream()));
			this.outWriter = new PrintWriter(this.socket.getOutputStream(), true);
		}
		
		private bool checkOpen()
		{
			try
			{
				if (this.socket == null)
				{
					this.open();
				}
			}
			catch (IOException ex)
			{
				goto IL_16;
			}
			goto IL_32;
			IL_16:
			java.lang.System.err.println("SocketCommandClient.checkOpen():could not open socket");
			this.socket = null;
			IL_32:
			return this.socket != null;
		}
		
		public virtual void close()
		{
			try
			{
				if (this.socket != null)
				{
					this.socket.close();
				}
				else
				{
					java.lang.System.err.println("SocketCommandClient.close(): socket is null");
				}
			}
			catch (IOException ex)
			{
				goto IL_2C;
			}
			goto IL_41;
			IL_2C:
			java.lang.System.err.println("Trouble closing socket");
			IL_41:
			this.socket = null;
		}
		
		public SocketCommandClient(string host, int port)
		{
			this.host = host;
			this.port = port;
			this.open();
		}
		
		public SocketCommandClient()
		{
		}
		
		public virtual int getSoTimeout()
		{
			if (this.socket != null)
			{
				return this.socket.getSoTimeout();
			}
			return 0;
		}
		
		public virtual void setSoTimeout(int millisecs)
		{
			if (this.socket != null)
			{
				this.socket.setSoTimeout(millisecs);
			}
			else
			{
				java.lang.System.err.println("SocketCommandClient.setSoTimeout(): socket is null");
			}
		}
		
		public static void main(string[] args)
		{
			try
			{
				CommandInterpreter commandInterpreter = new CommandInterpreter();
				SocketCommandClient socketCommandClient = new SocketCommandClient("localhost", 7890);
				commandInterpreter.add("s", new SocketCommandClient_1(socketCommandClient));
				commandInterpreter.add("r", new SocketCommandClient_2(socketCommandClient));
				commandInterpreter.add("sr", new SocketCommandClient_3(socketCommandClient));
				commandInterpreter.setPrompt("scc-test> ");
				commandInterpreter.run();
			}
			catch (System.Exception ex)
			{
				java.lang.System.err.println("error occured.");
				Throwable.instancehelper_printStackTrace(ex);
				java.lang.System.exit(-1);
			}
			return;
		}

		private string host;

		private int port;

		private Socket socket;

		private BufferedReader inReader;

		private PrintWriter outWriter;
	}
}
