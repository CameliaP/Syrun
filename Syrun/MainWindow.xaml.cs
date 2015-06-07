using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Syrun
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		static readonly Random Rand = new Random();

		static readonly string[] MediaExtensions = {
		    //".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
		    //".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
		    ".AVI", ".MP4", ".DIVX", ".WMV", ".MKV" //etc
		};

		public MainWindow()
		{
			InitializeComponent();
		}

		private void UIElement_OnDrop(object sender, DragEventArgs e)
		{
			var paths = ((string[])e.Data.GetData(DataFormats.FileDrop, false))
				.SelectMany(GetFiles)
				.Where(IsMediaFile)
				.ToList();

			PlayChoice(paths);
		}

		static void PlayChoice(IList<string> choices)
		{
			if (choices.Count.Equals(0))
				return;
			var pos = Rand.Next(choices.Count);
			var item = choices.ElementAt(pos);

			if (!ShouldPlay(item))
			{
				choices.RemoveAt(pos);
				PlayChoice(choices);
			}
			Process.Start(item);
		}

		static bool ShouldPlay(string mediaItem)
		{
			var playMedia = MessageBox.Show("Play " + Path.GetFileName(mediaItem), "", MessageBoxButton.OKCancel);
			return playMedia == MessageBoxResult.OK;
		}

		static bool IsMediaFile(string path)
		{
			var extension = Path.GetExtension(path);
			return extension != null && Array.IndexOf(MediaExtensions, extension.ToUpperInvariant()) != -1;
		}

		static IEnumerable<string> GetFiles(string path)
		{
			var queue = new Queue<string>();
			queue.Enqueue(path);
			while (queue.Count > 0)
			{
				path = queue.Dequeue();
				try
				{
					foreach (var subDir in Directory.GetDirectories(path))
						queue.Enqueue(subDir);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(ex);
				}
				string[] files = null;
				try
				{
					files = Directory.GetFiles(path);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(ex);
				}
				if (files != null)
					foreach (var t in files)
						yield return t;
			}
		}
	}
}
