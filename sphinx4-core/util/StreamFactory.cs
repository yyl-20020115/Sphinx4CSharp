using java.io;
using java.lang;
using java.net;
using java.util.zip;

namespace edu.cmu.sphinx.util
{
	public class StreamFactory : java.lang.Object
	{
		public static InputStream getInputStream(string location, string file)
		{
			if (location != null)
			{
				return StreamFactory.getInputStream(StreamFactory.resolve(location), location, file);
			}
			return StreamFactory.getInputStream("DIRECTORY", location, file);
		}

		public static OutputStream getOutputStream(string location, string file, bool append)
		{
			if (location != null)
			{
				return StreamFactory.getOutputStream(StreamFactory.resolve(location), location, file, append);
			}
			return StreamFactory.getOutputStream("DIRECTORY", location, file);
		}

		public static string resolve(string sourceName)
		{
			if (java.lang.String.instancehelper_endsWith(sourceName, ".jar") || java.lang.String.instancehelper_endsWith(sourceName, ".zip"))
			{
				return "ZIP_FILE";
			}
			return "DIRECTORY";
		}

		public static InputStream getInputStream(string format, string location, string file)
		{
			InputStream result = null;
			string text;
			if (location == null)
			{
				text = null;
			}
			else
			{
				URI uri = URI.create(location);
				string scheme = uri.getScheme();
				string schemeSpecificPart = uri.getSchemeSpecificPart();
				File file2 = new File(schemeSpecificPart);
				URI uri2 = file2.getAbsoluteFile().toURI();
				if (scheme == null)
				{
					text = uri2.getSchemeSpecificPart();
				}
				else
				{
					text = new StringBuilder().append(scheme).append(':').append(uri2.getSchemeSpecificPart()).toString();
				}
			}
			if (java.lang.String.instancehelper_equals(format, "ZIP_FILE"))
			{
				try
				{
					URI uri = new URI(text);
					ZipFile zipFile = new ZipFile(new File(uri));
					ZipEntry entry = zipFile.getEntry(file);
					if (entry != null)
					{
						result = zipFile.getInputStream(entry);
					}
					zipFile.close();
				}
				catch (URISyntaxException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
					throw new ZipException(new StringBuilder().append("URISyntaxException: ").append(ex.getMessage()).toString());
				}
				return result;
			}
			if (java.lang.String.instancehelper_equals(format, "DIRECTORY"))
			{
				if (text != null)
				{
					result = new FileInputStream(new StringBuilder().append(text).append(File.separator).append(file).toString());
				}
				else
				{
					result = new FileInputStream(file);
				}
			}
			return result;
		}

		public static OutputStream getOutputStream(string format, string location, string file, bool append)
		{
			OutputStream outputStream;
			if (java.lang.String.instancehelper_equals(format, "ZIP_FILE"))
			{
				try
				{
					java.lang.System.@out.println("WARNING: ZIP not yet fully supported.!");
					File file2 = new File(location);
					File file3 = new File(file2.getParent());
					if (!file3.exists())
					{
						file3.mkdirs();
					}
					FileOutputStream fileOutputStream = new FileOutputStream(new File(new URI(location)), append);
					outputStream = new ZipOutputStream(new BufferedOutputStream(fileOutputStream));
					ZipEntry zipEntry = new ZipEntry(file);
					((ZipOutputStream)((ZipOutputStream)outputStream)).putNextEntry(zipEntry);
				}
				catch (URISyntaxException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
					throw new ZipException(new StringBuilder().append("URISyntaxException: ").append(ex.getMessage()).toString());
				}
				return outputStream;
			}
			if (!java.lang.String.instancehelper_equals(format, "DIRECTORY"))
			{
				string text2 = "Format not supported for writing";
				throw new IOException(text2);
			}
			if (location != null)
			{
				File file2 = new File(new StringBuilder().append(location).append(File.separator).append(file).toString());
				File file3 = new File(file2.getParent());
				if (!file3.exists())
				{
					file3.mkdirs();
				}
				outputStream = new FileOutputStream(new StringBuilder().append(location).append(File.separator).append(file).toString());
			}
			else
			{
				File file2 = new File(file);
				File file3 = new File(file2.getParent());
				if (!file3.exists())
				{
					file3.mkdirs();
				}
				outputStream = new FileOutputStream(file);
			}
			return outputStream;
		}
		
		public static OutputStream getOutputStream(string format, string location, string file)
		{
			if (java.lang.String.instancehelper_equals(format, "ZIP_FILE"))
			{
				java.lang.System.@out.println("WARNING: overwriting ZIP or JAR file!");
				return StreamFactory.getOutputStream(StreamFactory.resolve(location), location, file, false);
			}
			if (java.lang.String.instancehelper_equals(format, "DIRECTORY"))
			{
				return StreamFactory.getOutputStream("DIRECTORY", location, file, false);
			}
			string text = "Format not supported for writing";
			throw new IOException(text);
		}
		
		public StreamFactory()
		{
		}
	
		public static OutputStream getOutputStream(string location, string file)
		{
			if (location != null)
			{
				return StreamFactory.getOutputStream(StreamFactory.resolve(location), location, file);
			}
			return StreamFactory.getOutputStream("DIRECTORY", location, file);
		}

		public const string ZIP_FILE = "ZIP_FILE";

		public const string DIRECTORY = "DIRECTORY";
	}
}
