using System;
using System.Collections.Generic;
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

namespace GoDexWPFB
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : UserControl
    {
        Nodes m_nodes = new Nodes();
        Home m_home = new Home();
        
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.slFrame.Content = m_home;
        }

        private void btnOnline_Click(object sender, RoutedEventArgs e)
        {
            this.slFrame.Content = m_nodes;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            this.Content = l;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNodeSet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.slFrame.Content = m_home;
            m_nodes.RunTest();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            try { System.Diagnostics.Process.Start("http://www.lerrick-fire.com"); }
            catch { }
        }
    }
}
