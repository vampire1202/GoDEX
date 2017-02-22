using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Nodes.xaml 的交互逻辑
    /// </summary>

    public partial class Nodes : UserControl
    {
        private bool m_isTest = false;
        /// <summary>
        /// 是否监控
        /// </summary>
        public bool IsTest
        {
            get { return m_isTest; }
            set { m_isTest = value; }
        }

        CancellationTokenSource m_ctsUpdateNodes;
        Task m_taskUpdateNodes;

        public Nodes()
        {
            InitializeComponent();
        }

        void ShowNodeState()
        {
            m_ctsUpdateNodes = new CancellationTokenSource();
            m_taskUpdateNodes = TaskShowNodeState(m_ctsUpdateNodes);
        }
        List<c_node> lstNodeControls = new List<c_node>();
        List<GoDexData.Model.nodeInfo> lstNodes;
        GoDexData.BLL.nodeInfo bllNi = new GoDexData.BLL.nodeInfo();
        async Task TaskShowNodeState(CancellationTokenSource ct)
        {
            while (m_isTest)
            {
                if (ct.Token.IsCancellationRequested)
                    break;
                else
                {
                    lstNodes = bllNi.GetModelList("");
                    await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            foreach (var o in lstNodes)
                            {
                                //查找控件集里是否有该控件
                                c_node c=null;
                                string name = "node" + o.machineNo.ToString();
                                c_node cfind = lstNodeControls.Find(cc => cc.Name == name);
                                if (cfind != null)
                                    c = cfind;
                                else
                                { 
                                    c = new c_node();
                                    lstNodeControls.Add(c);
                                }
                                c.nodeID.Text = o.machineNo.ToString();
                                c.Name = "node" + o.machineNo.ToString();
                                c.online.Text = o.softversion;
                                c.Uid = "node" + o.machineNo.ToString();
                            
                                if (o.softversion == "在线")//#FF0000FF
                                    c.online.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0xFF));//#FF008000绿色
                                else
                                    c.online.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));//#FF008000绿色 
                                //if (this.nodePanel.Children.Contains(c))
                                //    c = (c_node)this.nodePanel.Children[this.nodePanel.Children.IndexOf(c)];
                                //else
                                //    this.nodePanel.Children.Add(c);
                                //this.nodePanel.UpdateDefaultStyle();
                                //火警信息
                                switch (o.fireAlarmChl)
                                {
                                    case 1:
                                        switch (o.fireAlarmGrade)
                                        {//<!--一级火警Pink0xFF, 0xFF, 0xC0, 0xCB)
                                            //二级火警LightCoral  #FFF08080
                                            //三级火警Red FFFF0000 
                                            //四级火警Purple #FF800080
                                            //正常LightCyan #FF E0 FF FF  -->
                                            case 0: c.fire1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.fire1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC0, 0xCB)); break;
                                            case 2: c.fire1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x80, 0x80)); break;
                                            case 3: c.fire1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00)); break;
                                            case 4: c.fire1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0x00, 0x80)); break;
                                        }
                                        break;
                                    case 2:
                                        switch (o.fireAlarmGrade)
                                        {
                                            case 0: c.fire2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.fire2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC0, 0xCB)); break;
                                            case 2: c.fire2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x80, 0x80)); break;
                                            case 3: c.fire2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00)); break;
                                            case 4: c.fire2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0x00, 0x80)); break;
                                        }
                                        break;
                                    case 3:
                                        switch (o.fireAlarmGrade)
                                        {
                                            case 0: c.fire3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.fire3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC0, 0xCB)); break;
                                            case 2: c.fire3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x80, 0x80)); break;
                                            case 3: c.fire3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00)); break;
                                            case 4: c.fire3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0x00, 0x80)); break;
                                        }
                                        break;
                                    case 4:
                                        switch (o.fireAlarmGrade)
                                        {
                                            case 0: c.fire4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.fire4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC0, 0xCB)); break;
                                            case 2: c.fire4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x80, 0x80)); break;
                                            case 3: c.fire4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00)); break;
                                            case 4: c.fire4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0x00, 0x80)); break;
                                        }
                                        break;
                                }
                                //气流报警
                                //气流低 yellow #FFFFFF00
                                //气流高 gold #FFFFD700
                                //正常LightCyan #FF E0 FF FF 
                                switch (o.airAlarmChl)
                                {
                                    case 1:
                                        switch (o.airAlarmGrade)
                                        {
                                            case -1: c.air1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00)); break;
                                            case 0: c.air1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.air1.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD7, 0x00)); break;
                                        }
                                        break;
                                    case 2:
                                        switch (o.fireAlarmGrade)
                                        {
                                            case -1: c.air2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00)); break;
                                            case 0: c.air2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.air2.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD7, 0x00)); break;
                                        }
                                        break;
                                    case 3:
                                        switch (o.fireAlarmGrade)
                                        {
                                            case -1: c.air3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00)); break;
                                            case 0: c.air3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.air3.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD7, 0x00)); break;
                                        }
                                        break;
                                    case 4:
                                        switch (o.fireAlarmGrade)
                                        {
                                            case -1: c.air4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00)); break;
                                            case 0: c.air4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF)); break;
                                            case 1: c.air4.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD7, 0x00)); break;
                                        }
                                        break;
                                }
                                switch (o.machineType)
                                {
                                    case 1:
                                        c.machineType.Text = "单管单区";
                                        break;
                                    case 2:
                                        c.machineType.Text = "四管单区";
                                        break;
                                    case 3:
                                        c.machineType.Text = "四管四区";
                                        break;
                                    default:
                                        c.machineType.Text = "未知";
                                        break;
                                } 
                            }
                            //实现控件集绑定
                            this.itemsControl.ItemsSource = lstNodeControls;
                        }));
                }
            }

        }

        public void RunTest()
        {
            m_isTest = true;
            ShowNodeState();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ShowNodeState(lstNi);

        }
    }
}
