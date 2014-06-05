using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WTFTips
{
	/// <summary>
	/// UserControl2.xaml 的互動邏輯
	/// </summary>
	public partial class UserControl2 : UserControl
	{
		public UserControl2()
		{
            this.InitializeComponent();
            this.Height = 36;
            t.Elapsed += t_Elapsed;
            t.AutoReset = false;

        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Storyboard c = (Storyboard)this.Resources["closeAni"];
                c.Begin();
            }));

        }

        private Timer t = new Timer(5000);

        public void CloseMe()
        {

            t.Start();
        }

        public void changeAniHei(double hei)
        {
            Storyboard b = (Storyboard)this.Resources["openAni"];
            var kf1 = b.Children[0] as DoubleAnimationUsingKeyFrames;
            kf1.KeyFrames[2].Value = hei;


            Storyboard c = (Storyboard)this.Resources["closeAni"];
            var kf2 = c.Children[0] as DoubleAnimationUsingKeyFrames;
            kf2.KeyFrames[0].Value = hei;
        }

	}
}