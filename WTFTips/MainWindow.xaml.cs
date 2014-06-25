using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
        private string keys = "1";

        Thread thread;

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

 public string DecryptText(byte[] encryptedString,byte []k,byte[]iv)
	        {
	            using (var myRijndael = new RijndaelManaged())
	            {

	                myRijndael.Key = k;
	                myRijndael.IV = iv;
	                myRijndael.Mode = CipherMode.CBC;
	                myRijndael.Padding = PaddingMode.PKCS7;
	 
	                ;
                    string ourDec = DecryptStringFromBytes(encryptedString, myRijndael.Key, myRijndael.IV);
	 
	                return ourDec;
	            }

	        }

 protected string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
 {
     // Check arguments.
     if (cipherText == null || cipherText.Length <= 0)
         throw new ArgumentNullException("cipherText");
     if (Key == null || Key.Length <= 0)
         throw new ArgumentNullException("Key");
     if (IV == null || IV.Length <= 0)
         throw new ArgumentNullException("Key");

     // Declare the string used to hold
     // the decrypted text.
     string plaintext = null;

     // Create an RijndaelManaged object
     // with the specified key and IV.
     using (RijndaelManaged rijAlg = new RijndaelManaged())
     {
         rijAlg.Key = Key;
         rijAlg.IV = IV;
         rijAlg.Mode=CipherMode.CBC;
         rijAlg.Padding = PaddingMode.PKCS7;
         // Create a decrytor to perform the stream transform.
         ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

         // Create the streams used for decryption.
         using (MemoryStream msDecrypt = new MemoryStream(cipherText))
         {
             using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
             {
                 using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                 {

                     // Read the decrypted bytes from the decrypting stream
                     // and place them in a string.
                     plaintext = srDecrypt.ReadToEnd();
                 }
             }
         }

     }

     return plaintext;

 }
	 
        void Listener()
        {
            UdpClient udpServer = new UdpClient(10600);
            
            while (true)
            {
                var groupEP = new IPEndPoint(IPAddress.Any, 10600); // listen on any port
                var data = udpServer.Receive(ref groupEP);
                string s;
                if (data[0] == 'v' && data[1] == '2')
                {
                    s = Encoding.UTF8.GetString(data);
                }
                else
                {
                    var k = keys;
                    byte[] iv;
                    byte[] key=Encoding.UTF8.GetBytes(k);
                     var n = MD5.Create();
                    for (int i = 0; i < 10; i++)
                    {
                       
                        key = n.ComputeHash(key);
                    }
                    iv = n.ComputeHash(key);
                    try
                    {
                        s = DecryptStringFromBytes(data, key,iv );
                    }
                    catch (Exception ex)
                    {
                        logging(ex+"");
                        continue;
                    }
                   

                }
                s = s.Replace('\x00', ' ');
                s = s.Trim();
               logging(s);
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => parse(s)));
               

            }
        }

        void parse(string s)
        {
            var items=s.Split('/');
            if (items.Length < 6) return;
            var type = items[3];
            var data = items[4];
            var content = items[5];
            switch (type)
            {
                case "RING":
                    openNewWindow("呼叫", data+content);
                    break;
                //{RING,SMS,MMS,BATTERY,PING} 
                case "SMS":
                    break;
                case "MMS":
                    break;
                case "BATTERY":
                    break;
                case "PING":
                    openNewWindow("警告","測試");
                    break;
                default:
                    break;


            }
        }

        public MainWindow()
        {
            thread = new Thread(() => Listener());
            InitializeComponent();
            thread.IsBackground = true;
            thread.Start();
            PW.TextChanged += PW_TextChanged;
        }

        void PW_TextChanged(object sender, TextChangedEventArgs e)
        {
            keys = PW.Text.Trim();
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
            if (logER.Dispatcher.CheckAccess())
            {
                this.logER.Text = this.logER.Text + text + "\n";
            }
            else
            {
                logER.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => logging(text)));
            }
        }

      

    }
}
