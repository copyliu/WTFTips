using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Meebey.SmartIrc4net;
using Timer = System.Timers.Timer;

namespace WTFTips
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_EXSTYLE = (-20);
        
        
        Thread thread=new  Thread(() => ChatClass.irc.Listen());

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        public MainWindow()
        {
            InitializeComponent();
            thread.IsBackground = true;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ChatClass.irc.IsConnected)
            {
                ChatClass.irc.ListenOnce();
            }
        }

        private int v = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            openNewWindow(tb1.Text);
            v++;
        }

        public Size CalcTextSize(string Text, double Fontsize = 64)
        {
            TextBlock tb = new TextBlock();
            tb.FontFamily = (FontFamily) Application.Current.Resources["FontFamily"];
            tb.FontSize = Fontsize;
            tb.Text = Text;
            //tb.Width = SystemParameters.PrimaryScreenWidth/2;
            tb.TextWrapping = TextWrapping.Wrap;
            //tb.Arrange(new Rect(0, 0, SystemParameters.PrimaryScreenWidth / 2,int.MaxValue));
            tb.Measure(new Size(SystemParameters.PrimaryScreenWidth/2, int.MaxValue));
            return tb.DesiredSize;

        }

        public void openNewWindow(string Text1, string Text2)
        {
            var b = CalcTextSize(Text1,40);
            var s = CalcTextSize(Text2);

            var maxw = b.Width > s.Width ? b.Width : s.Width;





            openNewWindow(Text2, SystemParameters.PrimaryScreenWidth - maxw - 36,
                SystemParameters.PrimaryScreenHeight - s.Height - 156);
            openNewWindow2(Text1, SystemParameters.PrimaryScreenWidth - maxw - 36,
                SystemParameters.PrimaryScreenHeight - s.Height - 156 - b.Height,40);






        }

        public void openNewWindow(string Text)
        {
           openNewWindow("警告",Text);
        }
        public void openNewWindow2(string Text, double? postx, double? posty, double FontSize = 64)
        {
            Window1 w = new Window1();
            w.SourceInitialized += delegate
            {
                IntPtr hwnd = new WindowInteropHelper(w).Handle;
                uint extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            };
            w.Topmost = true;
            //            w.WindowStyle = WindowStyle.SingleBorderWindow;
            //            w.AllowsTransparency = false;
            //            
            w.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            w.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (postx.HasValue)
                w.Left = (double)postx;
            if (posty.HasValue)
                w.Top = (double)posty;


            UserControl2 u = new UserControl2();
            u.HorizontalAlignment = HorizontalAlignment.Left;
            u.VerticalAlignment = VerticalAlignment.Top;
           
            u.Background = new SolidColorBrush(new Color() { A = 255, R = 0x00, G = 0xA1, B = 0xE9, });
            u.MainText.Foreground = new SolidColorBrush(Colors.Black);
            
            u.Height = 36;
            w.Content = u;
            u.MainText.Text = Text;
            u.MainText.FontSize = FontSize;
            u.MainText.Measure(new Size(SystemParameters.PrimaryScreenWidth / 2, int.MaxValue));
            u.Width = u.MainText.DesiredSize.Width ;

            u.changeAniHei(u.MainText.DesiredSize.Height );
            w.Show();

            u.BeginStoryboard((Storyboard)u.Resources["openAni"]);

            u.CloseMe();
            w.CloseMe();

        }

        public void openNewWindow(string Text, double? postx, double? posty,double FontSize=64,bool hasbg=false)
        {
            Window1 w = new Window1();
            w.SourceInitialized += delegate
            {
                IntPtr hwnd = new WindowInteropHelper(w).Handle;
                uint extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            };
            w.Topmost = true;
//            w.WindowStyle = WindowStyle.SingleBorderWindow;
//            w.AllowsTransparency = false;
//            
            w.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            w.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (postx.HasValue)
                w.Left = (double) postx;
            if (posty.HasValue)
                w.Top = (double) posty;


            UserControl1 u = new UserControl1();
            u.HorizontalAlignment = HorizontalAlignment.Left;
            u.VerticalAlignment = VerticalAlignment.Top;
            if (hasbg)
            {
                u.Background = new SolidColorBrush(new Color() {A = 255, R = 0x00, G = 0xA1, B = 0xE9,});
                u.MainText.Foreground=new SolidColorBrush(Colors.Black);
            }
            u.Height = 36;
            w.Content = u;
            u.MainText.Text = Text;
            u.MainText.FontSize = FontSize;
            u.MainText.Measure(new Size(SystemParameters.PrimaryScreenWidth/2, int.MaxValue));
            u.Width = u.MainText.DesiredSize.Width + 36;

            u.changeAniHei(u.MainText.DesiredSize.Height + 36);
            w.Show();

            u.BeginStoryboard((Storyboard) u.Resources["openAni"]);

            u.CloseMe();
            w.CloseMe();

        }

        public void logging(string text)
        {
            this.logER.Text = this.logER.Text + text + "\n";
        }

        private void ConnBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChatClass.irc != null && ChatClass.irc.IsConnected)
            {
                logging("已連接到服務器");
                return;
            }

            ChatClass.irc = new IrcClient();
            ChatClass.irc.Encoding = System.Text.Encoding.UTF8;
            ChatClass.irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            ChatClass.irc.OnError += new ErrorEventHandler(OnError);
            ChatClass.irc.OnRawMessage += new IrcEventHandler(OnRawMessage);
            ChatClass.irc.OnChannelMessage += irc_OnChannelMessage;
            string[] serverlist;
            // the server we want to connect to, could be also a simple string
            serverlist = new string[] {ServerNameTbx.Text.Trim()};
            int port = 6667;
            string channel = ChannelNameTbx.Text.Trim();
            logging("正在连接");
            
            this.openNewWindow("正在连接");
            try
            {
                // here we try to connect to the server and exceptions get handled
                ChatClass.irc.Connect(serverlist, port);
            }
            catch (ConnectionException ex)
            {
                // something went wrong, the reason will be shown
               logging( "无法连接服务器");
                logging(ex.Message);
                this.openNewWindow("无法连接服务器");
                

            }

            try
            {
                // here we logon and register our nickname and so on
                ChatClass.irc.Login(NickNameTbx.Text.Trim(), NickNameTbx.Text.Trim());
                // join the channel
                ChatClass.irc.RfcJoin(channel);

//                for (int i = 0; i < 3; i++)
//                {
//                    // here we send just 3 different types of messages, 3 times for
//                    // testing the delay and flood protection (messagebuffer work)
////                    ChatClass.irc.SendMessage(SendType.Message, channel, "test message (" + i.ToString() + ")");
////                    ChatClass.irc.SendMessage(SendType.Action, channel, "thinks this is cool (" + i.ToString() + ")");
////                    ChatClass.irc.SendMessage(SendType.Notice, channel, "SmartIrc4net rocks (" + i.ToString() + ")");
//                }
                thread.Start();
                this.NickNameTbx.IsReadOnly = true;
                this.ChannelNameTbx.IsReadOnly = true;
            }
            catch (Exception ex)
            {
             //t.Stop();   
            }
        }

      

        void irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {

                this.openNewWindow(e.Data.Nick,e.Data.Message);

                logging(e.Data.Nick+":"+e.Data.Message);
               
            }
               ));


        }

        private void OnRawMessage(object sender, IrcEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {

            if (e.Data.Type == ReceiveType.Join)
            {
                logging(e.Data.Nick+" 成功加入聊天頻道");
                
                this.openNewWindow("通知", e.Data.Nick+" 加入聊天頻道");
            }

                //logging(e.Data.Type.ToString());
            }));


            //System.Console.WriteLine("Received: " + e.Data.RawMessage);
            //throw new NotImplementedException();
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnQueryMessage(object sender, IrcEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void SendBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChatClass.irc != null && ChatClass.irc.IsConnected)
            {
                
                ChatClass.irc.SendMessage(SendType.Message, this.ChannelNameTbx.Text.Trim(),this.tb1.Text.Trim());
                openNewWindow("發送中", this.tb1.Text.Trim());
                logging("自己: "+this.tb1.Text.Trim());
            }
            else
            {
                logging("未連接");
                openNewWindow("警告","未連接到聊天服務器");
            }
        }
    }
}
