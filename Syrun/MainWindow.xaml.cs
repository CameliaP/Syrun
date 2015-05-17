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
			var filepaths = new List<string>();
			foreach (var s in (string[])e.Data.GetData(DataFormats.FileDrop, false))
			{
				if (Directory.Exists(s))
					filepaths.AddRange(Directory.GetFiles(s));
				else
					filepaths.Add(s);
			}


			var list = from path in filepaths
				   where IsMediaFile(path)
				   select path;

			var enumerable = list as string[] ?? list.ToArray();
			var pos = Rand.Next(enumerable.Count());
			var item = enumerable.ElementAt(pos);

			Process.Start(item);
		}

		static bool IsMediaFile(string path)
		{
			var extension = Path.GetExtension(path);
			return extension != null && -1 != Array.IndexOf(MediaExtensions, extension.ToUpperInvariant());
		}
	}
}
