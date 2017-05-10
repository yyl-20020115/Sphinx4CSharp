using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.net;

namespace edu.cmu.sphinx.util
{
	public class SocketCommandClient : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			76,
			130,
			102,
			104,
			130,
			108,
			109,
			102,
			159,
			7,
			226,
			55,
			230,
			76
		})]
		
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

		[LineNumberTable(new byte[]
		{
			124,
			130,
			104,
			194,
			215,
			226,
			61,
			97,
			111,
			134
		})]
		
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

		[LineNumberTable(new byte[]
		{
			101,
			130,
			104,
			194,
			215,
			226,
			61,
			97,
			111,
			134
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			84,
			98,
			105,
			135
		})]
		
		public virtual string sendCommandGetResponse(string command)
		{
			string result = null;
			if (this.sendCommand(command))
			{
				result = this.getResponse();
			}
			return result;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			33,
			114
		})]
		
		private void open()
		{
			this.open(this.host, this.port);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			16,
			103,
			231,
			71,
			151,
			103,
			116,
			119
		})]
		
		public virtual void open(string aHost, int aPort)
		{
			this.host = aHost;
			this.port = aPort;
			this.socket = new Socket(this.host, this.port);
			this.inReader = new BufferedReader(new InputStreamReader(this.socket.getInputStream()));
			this.outWriter = new PrintWriter(this.socket.getOutputStream(), true);
		}

		[LineNumberTable(new byte[]
		{
			160,
			111,
			104,
			241,
			70,
			226,
			60,
			97,
			143,
			135
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			95,
			104,
			141,
			250,
			69,
			2,
			97,
			143,
			103
		})]
		
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

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			187,
			104,
			103,
			103,
			102
		})]
		
		public SocketCommandClient(string host, int port)
		{
			this.host = host;
			this.port = port;
			this.open();
		}

		[LineNumberTable(new byte[]
		{
			3,
			102
		})]
		
		public SocketCommandClient()
		{
		}

		[Throws(new string[]
		{
			"java.net.SocketException"
		})]
		[LineNumberTable(new byte[]
		{
			43,
			104,
			140
		})]
		
		public virtual int getSoTimeout()
		{
			if (this.socket != null)
			{
				return this.socket.getSoTimeout();
			}
			return 0;
		}

		[Throws(new string[]
		{
			"java.net.SocketException"
		})]
		[LineNumberTable(new byte[]
		{
			59,
			104,
			142,
			175
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			124,
			102,
			144,
			241,
			80,
			241,
			78,
			241,
			83,
			107,
			253,
			70,
			226,
			60,
			97,
			111,
			102,
			134
		})]
		
		public static void main(string[] args)
		{
			Exception ex3;
			try
			{
				CommandInterpreter commandInterpreter = new CommandInterpreter();
				SocketCommandClient socketCommandClient = new SocketCommandClient("localhost", 7890);
				commandInterpreter.add("s", new SocketCommandClient$1(socketCommandClient));
				commandInterpreter.add("r", new SocketCommandClient$2(socketCommandClient));
				commandInterpreter.add("sr", new SocketCommandClient$3(socketCommandClient));
				commandInterpreter.setPrompt("scc-test> ");
				commandInterpreter.run();
			}
			catch (Exception ex)
			{
				Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_6E;
			}
			return;
			IL_6E:
			Exception ex4 = ex3;
			java.lang.System.err.println("error occured.");
			Throwable.instancehelper_printStackTrace(ex4);
			java.lang.System.exit(-1);
		}

		private string host;

		private int port;

		private Socket socket;

		private BufferedReader inReader;

		private PrintWriter outWriter;
	}
}
