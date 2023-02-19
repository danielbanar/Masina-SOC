using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
namespace MasinaPC
{
	public partial class GUI : Form
	{
		string ip = "192.168.1.67";
		HttpClient httpClient;
		public GUI()
		{
			InitializeComponent();
			this.KeyPreview = true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			httpClient = new HttpClient();
			httpClient.Timeout = new TimeSpan(0, 0, 3);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			MjpegProcessor.MjpegDecoder mjpegDecoder = new MjpegProcessor.MjpegDecoder();
			mjpegDecoder.ParseStream(new Uri($"http://{ip}:5000/stream.mjpg"));
			mjpegDecoder.FrameReady += (s, e2) =>
			{
				pictureBox1.Image = e2.Bitmap;
			};
		}
		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			httpClient.GetAsync($"http://{ip}:8000/speed/{(int)((float)trackBar1.Value * 25.5f)}");
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			ip = textBox1.Text;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
				httpClient.GetAsync($"http://{ip}:8000/led");
			else
				httpClient.GetAsync($"http://{ip}:8000/LED");
		}

		private void trackBar2_Scroll(object sender, EventArgs e)
		{
			httpClient.GetAsync($"http://{ip}:8000/r/{(int)((float)trackBar2.Value * 25.5f)}");
		}

		private void trackBar3_Scroll(object sender, EventArgs e)
		{
			httpClient.GetAsync($"http://{ip}:8000/g/{(int)((float)trackBar3.Value * 25.5f)}");
		}

		private void trackBar4_Scroll(object sender, EventArgs e)
		{
			httpClient.GetAsync($"http://{ip}:8000/b/{(int)((float)trackBar4.Value * 25.5f)}");
		}
		bool w=false, a=false, s=false, d=false;

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.W) && !w)
			{
				httpClient.GetAsync($"http://{ip}:8000/w");
				w = true;
			}
			else if (Keyboard.IsKeyUp(Key.W) && w)
			{
				httpClient.GetAsync($"http://{ip}:8000/W");
				w = false;
			}

			if (Keyboard.IsKeyDown(Key.A) && !a)
			{
				httpClient.GetAsync($"http://{ip}:8000/a");
				a = true;
			}
			else if (Keyboard.IsKeyUp(Key.A) && a)
			{
				httpClient.GetAsync($"http://{ip}:8000/A");
				a = false;
			}

			if (Keyboard.IsKeyDown(Key.S) && !s)
			{
				httpClient.GetAsync($"http://{ip}:8000/s");
				s = true;
			}
			else if (Keyboard.IsKeyUp(Key.S) && s)
			{
				httpClient.GetAsync($"http://{ip}:8000/S");
				s = false;
			}

			if (Keyboard.IsKeyDown(Key.D) && !d)
			{
				httpClient.GetAsync($"http://{ip}:8000/d");
				d = true;
			}
			else if (Keyboard.IsKeyUp(Key.D) && d)
			{
				httpClient.GetAsync($"http://{ip}:8000/D");
				d = false;
			}
		}
	}
}
