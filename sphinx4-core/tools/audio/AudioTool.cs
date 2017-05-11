using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.frontend.util;
using edu.cmu.sphinx.util.props;
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

		internal static JButton access_400()
		{
			return AudioTool.recordButton;
		}
		
		internal static JMenuItem access_000()
		{
			return AudioTool.saveMenuItem;
		}				
		
		internal static short[] access_500(Microphone microphone)
		{
			return AudioTool.getRecordedAudio(microphone);
		}

		internal static void access_100()
		{
			AudioTool.zoomIn();
		}
		
		internal static void access_200()
		{
			AudioTool.zoomOut();
		}

		internal static void access_300()
		{
			AudioTool.zoomReset();
		}
		
		private static short[] getRecordedAudio(Microphone microphone)
		{
			short[] array = new short[0];
			int num = 8000;
			while (microphone.hasMoreData())
			{
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
				catch (System.Exception ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
				continue;
			}
			if (num > 8000)
			{
				java.lang.System.@out.println(new StringBuilder().append("Downsampling from ").append(num).append(" to 8000.").toString());
				array = Downsampler.downsample(array, num / 1000, 16);
			}
			return array;
		}
		
		private static void zoomReset()
		{
			AudioTool.zoom = 1f;
			AudioTool.zoomPanels();
		}
		
		private static void zoomOut()
		{
			AudioTool.zoom /= 2f;
			AudioTool.zoomPanels();
		}
		
		private static void zoomIn()
		{
			AudioTool.zoom *= 2f;
			AudioTool.zoomPanels();
		}
		
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
		
		private static void createMenuBar(JFrame jframe)
		{
			JMenuBar jmenuBar = new JMenuBar();
			jframe.setJMenuBar(jmenuBar);
			JMenu jmenu = new JMenu("File");
			jmenuBar.add(jmenu);
			JMenuItem jmenuItem = new JMenuItem("Open...");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control O"));
			jmenuItem.addActionListener(new AudioTool_1());
			jmenu.add(jmenuItem);
			AudioTool.saveMenuItem = new JMenuItem("Save");
			AudioTool.saveMenuItem.setAccelerator(KeyStroke.getKeyStroke("control S"));
			AudioTool.saveMenuItem.setEnabled(false);
			AudioTool.saveMenuItem.addActionListener(new AudioTool_2());
			jmenu.add(AudioTool.saveMenuItem);
			jmenuItem = new JMenuItem("Save As...");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control V"));
			jmenuItem.addActionListener(new AudioTool_3());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Quit");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control Q"));
			jmenuItem.addActionListener(new AudioTool_4());
			jmenu.add(jmenuItem);
			jmenu = new JMenu("Edit");
			jmenuBar.add(jmenu);
			jmenuItem = new JMenuItem("Select All");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control A"));
			jmenuItem.addActionListener(new AudioTool_5());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Crop");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control X"));
			jmenuItem.addActionListener(new AudioTool_6());
			jmenu.add(jmenuItem);
			jmenu = new JMenu("View");
			jmenuBar.add(jmenu);
			jmenuItem = new JMenuItem("Zoom In");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke('>'));
			jmenuItem.addActionListener(new AudioTool_7());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Zoom Out");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke('<'));
			jmenuItem.addActionListener(new AudioTool_8());
			jmenu.add(jmenuItem);
			jmenuItem = new JMenuItem("Original Size");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke('!'));
			jmenuItem.addActionListener(new AudioTool_9());
			jmenu.add(jmenuItem);
			jmenu = new JMenu("Audio");
			jmenuBar.add(jmenu);
			jmenuItem = new JMenuItem("Play");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control P"));
			jmenuItem.addActionListener(new AudioTool_10());
			jmenu.add(jmenuItem);
			AudioTool.recordListener = new AudioTool_11();
			jmenuItem = new JMenuItem("Record Start/Stop");
			jmenuItem.setAccelerator(KeyStroke.getKeyStroke("control R"));
			jmenuItem.addActionListener(AudioTool.recordListener);
			jmenu.add(jmenuItem);
		}
		
		private static JPanel createButtonPanel()
		{
			JPanel jpanel = new JPanel();
			FlowLayout flowLayout = new FlowLayout();
			flowLayout.setAlignment(0);
			jpanel.setLayout(flowLayout);
			AudioTool.playButton = new JButton("Play");
			AudioTool.playButton.setEnabled(true);
			AudioTool.playButton.addActionListener(new AudioTool_12());
			AudioTool.recordButton = new JButton("Record");
			AudioTool.recordButton.setEnabled(true);
			AudioTool.recordButton.addActionListener(AudioTool.recordListener);
			AudioTool.zoomInButton = new JButton("Zoom In");
			AudioTool.zoomInButton.setEnabled(true);
			AudioTool.zoomInButton.addActionListener(new AudioTool_13());
			AudioTool.zoomOutButton = new JButton("Zoom Out");
			AudioTool.zoomOutButton.setEnabled(true);
			AudioTool.zoomOutButton.addActionListener(new AudioTool_14());
			AudioTool.zoomResetButton = new JButton("Reset Size");
			AudioTool.zoomResetButton.setEnabled(true);
			AudioTool.zoomResetButton.addActionListener(new AudioTool_15());
			JButton jbutton = new JButton("Exit");
			jbutton.addActionListener(new AudioTool_16());
			jpanel.add(AudioTool.recordButton);
			jpanel.add(AudioTool.playButton);
			jpanel.add(AudioTool.zoomInButton);
			jpanel.add(AudioTool.zoomOutButton);
			jpanel.add(AudioTool.zoomResetButton);
			jpanel.add(jbutton);
			return jpanel;
		}
		
		public static void main(string[] args)
		{
			AudioTool.prefs = Preferences.userRoot().node("/edu/cmu/sphinx/tools/audio/AudioTool");
			AudioTool.filename = AudioTool.prefs.get("filename", "untitled.raw");
			AudioTool.file = new File(AudioTool.filename);
			if (args.Length == 1 && java.lang.String.instancehelper_equals(args[0], "-dumpMixers"))
			{
				AudioTool.dumpMixers();
				java.lang.System.exit(0);
			}
			try
			{
				if (args.Length > 0)
				{
					AudioTool.filename = args[0];
				}
				URL url;
				if (args.Length == 2)
				{
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
				AudioTool.audioPanel = new AudioPanel(AudioTool.audio, 1f / num, 0.004f);
				AudioTool.spectrogramPanel = new SpectrogramPanel(frontEnd, dataSource, AudioTool.audio);
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
				JPanel jpanel2 = new JPanel(new BorderLayout());
				jpanel2.add(AudioTool.createButtonPanel(), "North");
				jpanel2.add(jscrollPane);
				AudioTool.player = new AudioPlayer(AudioTool.audio);
				AudioTool.player.start();
				AudioTool.getAudioFromFile(AudioTool.filename);
				jframe.setDefaultCloseOperation(3);
				jframe.setContentPane(jpanel2);
				jframe.pack();
				jframe.setSize(640, 400);
				jframe.setVisible(true);
			}
			catch (System.Exception ex)
			{
				Throwable.instancehelper_printStackTrace(ex);
			}
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
