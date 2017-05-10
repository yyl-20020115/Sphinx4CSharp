using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.awt;
using java.awt.@event;
using java.io;
using java.lang;
using java.net;
using java.util;
using java.util.prefs;
using javax.sound.sampled;
using javax.swing;

namespace edu.cmu.sphinx.tools.audio
{
	public class AudioTool : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			86,
			107,
			111,
			139,
			99,
			146,
			144,
			99,
			111,
			111,
			148
		})]
		
		public static void getFilename(string title, int type)
		{
			AudioTool.fileChooser.setDialogTitle(title);
			AudioTool.fileChooser.setCurrentDirectory(AudioTool.file);
			AudioTool.fileChooser.setDialogType(type);
			int num;
			if (type == 0)
			{
				num = AudioTool.fileChooser.showOpenDialog(AudioTool.jframe);
			}
			else
			{
				num = AudioTool.fileChooser.showSaveDialog(AudioTool.jframe);
			}
			if (num == 0)
			{
				AudioTool.file = AudioTool.fileChooser.getSelectedFile();
				AudioTool.filename = AudioTool.file.getAbsolutePath();
				AudioTool.prefs.put("filename", AudioTool.filename);
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			73,
			112,
			177,
			139,
			108,
			103,
			103,
			104,
			97,
			108,
			105,
			112,
			237,
			59,
			232,
			72,
			140,
			102,
			98,
			134
		})]
		
		public static void getAudioFromFile(string filename)
		{
			if (java.lang.String.instancehelper_endsWith(filename, ".align"))
			{
				BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(new FileInputStream(filename)));
				AudioTool.populateAudio(bufferedReader.readLine());
				int num = Integer.parseInt(bufferedReader.readLine());
				float[] array = new float[num];
				string[] array2 = new string[num];
				for (int i = 0; i < num; i++)
				{
					StringTokenizer stringTokenizer = new StringTokenizer(bufferedReader.readLine());
					while (stringTokenizer.hasMoreTokens())
					{
						array[i] = Float.parseFloat(stringTokenizer.nextToken());
						array2[i] = stringTokenizer.nextToken();
					}
				}
				AudioTool.audioPanel.setLabels(array, array2);
				bufferedReader.close();
			}
			else
			{
				AudioTool.populateAudio(filename);
			}
		}

		
		internal static JButton access$400()
		{
			return AudioTool.recordButton;
		}

		
		internal static JMenuItem access$000()
		{
			return AudioTool.saveMenuItem;
		}

		
		
		
		internal static short[] access$500(Microphone microphone)
		{
			return AudioTool.getRecordedAudio(microphone);
		}

		
		
		
		internal static void access$100()
		{
			AudioTool.zoomIn();
		}

		
		
		
		internal static void access$200()
		{
			AudioTool.zoomOut();
		}

		
		
		
		internal static void access$300()
		{
			AudioTool.zoomReset();
		}

		[LineNumberTable(new byte[]
		{
			160,
			101,
			103,
			230,
			69,
			139,
			103,
			107,
			102,
			102,
			102,
			102,
			109,
			105,
			50,
			168,
			220,
			5,
			98,
			103,
			165,
			104,
			159,
			15,
			240,
			70
		})]
		
		private static short[] getRecordedAudio(Microphone microphone)
		{
			short[] array = new short[0];
			int num = 8000;
			while (microphone.hasMoreData())
			{
				Exception ex3;
				try
				{
					Data data = microphone.getData();
					if (data is DoubleData)
					{
						num = ((DoubleData)data).getSampleRate();
						double[] values = ((DoubleData)data).getValues();
						short[] array2 = Arrays.copyOf(array, array.Length + values.Length);
						for (int i = 0; i < values.Length; i++)
						{
							array2[array.Length + i] = (short)ByteCodeHelper.d2i(values[i]);
						}
						array = array2;
					}
				}
				catch (Exception ex)
				{
					Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
					if (ex2 == null)
					{
						throw;
					}
					ex3 = ex2;
					goto IL_8A;
				}
				continue;
				IL_8A:
				Exception ex4 = ex3;
				Throwable.instancehelper_printStackTrace(ex4);
			}
			if (num > 8000)
			{
				java.lang.System.@out.println(new StringBuilder().append("Downsampling from ").append(num).append(" to 8000.").toString());
				array = Downsampler.downsample(array, num / 1000, 16);
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			160,
			166,
			106,
			101
		})]
		
		private static void zoomReset()
		{
			AudioTool.zoom = 1f;
			AudioTool.zoomPanels();
		}

		[LineNumberTable(new byte[]
		{
			160,
			160,
			113,
			101
		})]
		
		private static void zoomOut()
		{
			AudioTool.zoom /= 2f;
			AudioTool.zoomPanels();
		}

		[LineNumberTable(new byte[]
		{
			160,
			154,
			113,
			101
		})]
		
		private static void zoomIn()
		{
			AudioTool.zoom *= 2f;
			AudioTool.zoomPanels();
		}

		[LineNumberTable(new byte[]
		{
			38,
			130,
			102,
			115,
			108,
			103,
			103,
			121,
			63,
			2,
			168,
			100,
			107,
			127,
			2,
			228,
			54,
			233,
			79,
			99,
			159,
			5
		})]
		
		private static void dumpLineInfo(string text, Line.Info[] array)
		{
			int num = 0;
			if (array != null)
			{
				int num2 = array.Length;
				for (int i = 0; i < num2; i++)
				{
					Line.Info info = array[i];
					if (info is DataLine.Info)
					{
						AudioFormat[] formats = ((DataLine.Info)info).getFormats();
						AudioFormat[] array2 = formats;
						int num3 = array2.Length;
						for (int j = 0; j < num3; j++)
						{
							AudioFormat audioFormat = array2[j];
							java.lang.System.@out.println(new StringBuilder().append(text).append(audioFormat).toString());
						}
						num++;
					}
					else if (info is Port.Info)
					{
						java.lang.System.@out.println(new StringBuilder().append(text).append(info).toString());
						num++;
					}
				}
			}
			if (num == 0)
			{
				java.lang.System.@out.println(new StringBuilder().append(text).append("none").toString());
			}
		}

		[LineNumberTable(new byte[]
		{
			105,
			103,
			99,
			135,
			101,
			251,
			74,
			2,
			161
		})]
		
		public static void populateAudio(string filename)
		{
			try
			{
				AudioData audioData = Utils.readAudioFile(filename);
				if (audioData == null)
				{
					audioData = Utils.readRawFile(filename);
				}
				AudioTool.zoomReset();
				AudioTool.audio.setAudioData(audioData.getAudioData());
			}
			catch (IOException ex)
			{
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			141,
			103,
			143,
			103,
			143,
			103,
			143
		})]
		
		private static void zoomPanels()
		{
			if (AudioTool.audioPanel != null)
			{
				AudioTool.audioPanel.zoomSet(AudioTool.zoom);
			}
			if (AudioTool.spectrogramPanel != null)
			{
				AudioTool.spectrogramPanel.zoomSet(AudioTool.zoom);
			}
			if (AudioTool.cepstrumPanel != null)
			{
				AudioTool.cepstrumPanel.zoomSet(AudioTool.zoom);
			}
		}

		[LineNumberTable(new byte[]
		{
			64,
			134,
			106,
			105,
			127,
			8,
			54,
			133,
			119,
			47,
			165,
			111,
			144,
			111,
			240,
			53,
			233,
			77
		})]
		
		private static void dumpMixers()
		{
			Mixer.Info[] mixerInfo = AudioSystem.getMixerInfo();
			for (int i = 0; i < mixerInfo.Length; i++)
			{
				Mixer mixer = AudioSystem.getMixer(mixerInfo[i]);
				java.lang.System.@out.println(new StringBuilder().append("Mixer[").append(i).append("]: \"").append(mixerInfo[i].getName()).append('"').toString());
				java.lang.System.@out.println(new StringBuilder().append("    Description: ").append(mixerInfo[i].getDescription()).toString());
				java.lang.System.@out.println("    SourceLineInfo (e.g., speakers):");
				AudioTool.dumpLineInfo("        ", mixer.getSourceLineInfo());
				java.lang.System.@out.println("    TargetLineInfo (e.g., microphones):");
				AudioTool.dumpLineInfo("        ", mixer.getTargetLineInfo());
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			172,
			102,
			135,
			107,
			136,
			107,
			112,
			235,
			77,
			136,
			111,
			116,
			107,
			239,
			76,
			140,
			107,
			112,
			235,
			78,
			136,
			107,
			112,
			235,
			69,
			168,
			107,
			136,
			107,
			112,
			235,
			69,
			136,
			107,
			112,
			235,
			69,
			168,
			107,
			136,
			107,
			109,
			235,
			69,
			136,
			107,
			109,
			235,
			69,
			136,
			107,
			109,
			235,
			69,
			168,
			107,
			136,
			107,
			112,
			235,
			70,
			136,
			234,
			83,
			107,
			112,
			107,
			104
		})]
		
		private static void createMenuBar(JFrame jframe)
		{
			JMenuBar jmenuBar = new JMenuBar();
			jframe.setJMenuBar(jmenuBar);
			JMenu jmenu = new JMenu("File");
			jmenuBar.add(jmenu);
			JMenuItem jmenuItem = new JMenuItem("Open...");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control O"));
			jmenuItem.addActionListener(new AudioTool$1());
			jmenu.add(jmenuItem);
			AudioTool.saveMenuItem = new JMenuItem("Save");
			AudioTool.saveMenuItem.setAccelerator(KeyStroke.getKeyStroke("control S"));
			AudioTool.saveMenuItem.setEnabled(false);
			AudioTool.saveMenuItem.addActionListener(new AudioTool$2());
			jmenu.add(AudioTool.saveMenuItem);
			jmenuItem = new JMenuItem("Save As...");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control V"));
			jmenuItem.addActionListener(new AudioTool$3());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Quit");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control Q"));
			jmenuItem.addActionListener(new AudioTool$4());
			jmenu.add(jmenuItem);
			jmenu = new JMenu("Edit");
			jmenuBar.add(jmenu);
			jmenuItem = new JMenuItem("Select All");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control A"));
			jmenuItem.addActionListener(new AudioTool$5());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Crop");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control X"));
			jmenuItem.addActionListener(new AudioTool$6());
			jmenu.add(jmenuItem);
			jmenu = new JMenu("View");
			jmenuBar.add(jmenu);
			jmenuItem = new JMenuItem("Zoom In");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke('>'));
			jmenuItem.addActionListener(new AudioTool$7());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Zoom Out");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke('<'));
			jmenuItem.addActionListener(new AudioTool$8());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Original Size");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke('!'));
			jmenuItem.addActionListener(new AudioTool$9());
			jmenu.add(jmenuItem);
			jmenu = new JMenu("Audio");
			jmenuBar.add(jmenu);
			jmenuItem = new JMenuItem("Play");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control P"));
			jmenuItem.addActionListener(new AudioTool$10());
			jmenu.add(jmenuItem);
			AudioTool.recordListener = new AudioTool$11();
			jmenuItem = new JMenuItem("Record Start/Stop");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control R"));
			jmenuItem.addActionListener(AudioTool.recordListener);
			jmenu.add(jmenuItem);
		}

		[LineNumberTable(new byte[]
		{
			161,
			81,
			102,
			102,
			103,
			135,
			111,
			107,
			239,
			71,
			111,
			107,
			143,
			111,
			107,
			239,
			70,
			111,
			107,
			239,
			70,
			111,
			107,
			239,
			70,
			107,
			235,
			70,
			108,
			108,
			108,
			108,
			108,
			136
		})]
		
		private static JPanel createButtonPanel()
		{
			JPanel jpanel = new JPanel();
			FlowLayout flowLayout = new FlowLayout();
			flowLayout.setAlignment(0);
			jpanel.setLayout(flowLayout);
			AudioTool.playButton = new JButton("Play");
			AudioTool.playButton.setEnabled(true);
			AudioTool.playButton.addActionListener(new AudioTool$12());
			AudioTool.recordButton = new JButton("Record");
			AudioTool.recordButton.setEnabled(true);
			AudioTool.recordButton.addActionListener(AudioTool.recordListener);
			AudioTool.zoomInButton = new JButton("Zoom In");
			AudioTool.zoomInButton.setEnabled(true);
			AudioTool.zoomInButton.addActionListener(new AudioTool$13());
			AudioTool.zoomOutButton = new JButton("Zoom Out");
			AudioTool.zoomOutButton.setEnabled(true);
			AudioTool.zoomOutButton.addActionListener(new AudioTool$14());
			AudioTool.zoomResetButton = new JButton("Reset Size");
			AudioTool.zoomResetButton.setEnabled(true);
			AudioTool.zoomResetButton.addActionListener(new AudioTool$15());
			JButton jbutton = new JButton("Exit");
			jbutton.addActionListener(new AudioTool$16());
			jpanel.add(AudioTool.recordButton);
			jpanel.add(AudioTool.playButton);
			jpanel.add(AudioTool.zoomInButton);
			jpanel.add(AudioTool.zoomOutButton);
			jpanel.add(AudioTool.zoomResetButton);
			jpanel.add(jbutton);
			return jpanel;
		}

		
		
		public AudioTool()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			146,
			116,
			121,
			148,
			116,
			101,
			230,
			69,
			101,
			136,
			101,
			154,
			144,
			135,
			117,
			106,
			138,
			113,
			113,
			114,
			178,
			109,
			142,
			108,
			106,
			231,
			69,
			103,
			117,
			191,
			3,
			118,
			152,
			103,
			111,
			109,
			111,
			109,
			111,
			109,
			143,
			137,
			113,
			113,
			138,
			116,
			138,
			138,
			104,
			105,
			103,
			113,
			191,
			2,
			2,
			98,
			135
		})]
		
		public static void main(string[] args)
		{
			AudioTool.prefs = Preferences.userRoot().node("/edu/cmu/sphinx/tools/audio/AudioTool");
			AudioTool.filename = AudioTool.prefs.get("filename", "untitled.raw");
			File.__<clinit>();
			AudioTool.file = new File(AudioTool.filename);
			if (args.Length == 1 && java.lang.String.instancehelper_equals(args[0], "-dumpMixers"))
			{
				AudioTool.dumpMixers();
				java.lang.System.exit(0);
			}
			Exception ex3;
			try
			{
				if (args.Length > 0)
				{
					AudioTool.filename = args[0];
				}
				URL url;
				if (args.Length == 2)
				{
					File.__<clinit>();
					url = new File(args[1]).toURI().toURL();
				}
				else
				{
					url = ClassLiteral<AudioTool>.Value.getResource("spectrogram.config.xml");
				}
				ConfigurationManager configurationManager = new ConfigurationManager(url);
				AudioTool.recorder = (Microphone)configurationManager.lookup("microphone");
				AudioTool.recorder.initialize();
				AudioTool.audio = new AudioData();
				FrontEnd frontEnd = (FrontEnd)configurationManager.lookup("frontEnd");
				StreamDataSource dataSource = (StreamDataSource)configurationManager.lookup("streamDataSource");
				FrontEnd frontEnd2 = (FrontEnd)configurationManager.lookup("cepstrumFrontEnd");
				StreamDataSource dataSource2 = (StreamDataSource)configurationManager.lookup("cstreamDataSource");
				PropertySheet propertySheet = configurationManager.getPropertySheet("windower");
				float @float = propertySheet.getFloat("windowShiftInMs");
				JFrame jframe = new JFrame("AudioTool");
				AudioTool.fileChooser = new JFileChooser();
				AudioTool.createMenuBar(jframe);
				float num = @float * AudioTool.audio.getAudioFormat().getSampleRate() / 1000f;
				AudioPanel.__<clinit>();
				AudioTool.audioPanel = new AudioPanel(AudioTool.audio, 1f / num, 0.004f);
				SpectrogramPanel.__<clinit>();
				AudioTool.spectrogramPanel = new SpectrogramPanel(frontEnd, dataSource, AudioTool.audio);
				CepstrumPanel.__<clinit>();
				AudioTool.cepstrumPanel = new CepstrumPanel(frontEnd2, dataSource2, AudioTool.audio);
				JPanel jpanel = new JPanel();
				jpanel.setLayout(new BoxLayout(jpanel, 3));
				jpanel.add(AudioTool.audioPanel);
				AudioTool.audioPanel.setAlignmentX(0f);
				jpanel.add(AudioTool.spectrogramPanel);
				AudioTool.spectrogramPanel.setAlignmentX(0f);
				jpanel.add(AudioTool.cepstrumPanel);
				AudioTool.cepstrumPanel.setAlignmentX(0f);
				JScrollPane jscrollPane = new JScrollPane(jpanel);
				JPanel.__<clinit>();
				JPanel jpanel2 = new JPanel(new BorderLayout());
				jpanel2.add(AudioTool.createButtonPanel(), "North");
				jpanel2.add(jscrollPane);
				AudioPlayer.__<clinit>();
				AudioTool.player = new AudioPlayer(AudioTool.audio);
				AudioTool.player.start();
				AudioTool.getAudioFromFile(AudioTool.filename);
				jframe.setDefaultCloseOperation(3);
				jframe.setContentPane(jpanel2);
				jframe.pack();
				jframe.setSize(640, 400);
				jframe.setVisible(true);
			}
			catch (Exception ex)
			{
				Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
				if (ex2 == null)
				{
					throw;
				}
				ex3 = ex2;
				goto IL_2C3;
			}
			return;
			IL_2C3:
			Exception ex4 = ex3;
			Throwable.instancehelper_printStackTrace(ex4);
		}

		static AudioTool()
		{
			// Note: this type is marked as 'beforefieldinit'.
		}

		internal const string CONTEXT = "AudioTool";

		internal const string PREFS_CONTEXT = "/edu/cmu/sphinx/tools/audio/AudioTool";

		internal const string FILENAME_PREFERENCE = "filename";

		internal const string MICROPHONE = "microphone";

		internal const string FRONT_END = "frontEnd";

		internal const string CESPTRUM_FRONT_END = "cepstrumFrontEnd";

		internal const string DATA_SOURCE = "streamDataSource";

		internal const string CEPSTRUM_DATA_SOURCE = "cstreamDataSource";

		internal const string WINDOWER = "windower";

		internal static AudioData audio;

		internal static JFrame jframe;

		internal static AudioPanel audioPanel;

		internal static SpectrogramPanel spectrogramPanel;

		internal static CepstrumPanel cepstrumPanel;

		internal static JFileChooser fileChooser;

		internal static string filename;

		internal static File file;

		internal static AudioPlayer player;

		internal static Microphone recorder;

		internal static bool recording;

		internal static Preferences prefs;

		internal static float zoom = 1f;

		private static JMenuItem saveMenuItem;

		private static JButton playButton;

		private static JButton recordButton;

		private static JButton zoomInButton;

		private static JButton zoomOutButton;

		private static JButton zoomResetButton;

		private static ActionListener recordListener;
	}
}
