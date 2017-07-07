using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Windows.Forms.DataVisualization.Charting;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace iothook
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();

        }

        int i = 0, grafik = 1;

        Thread verigetir, grafikthread,verigonderthread;


        private void Form1_Load(object sender, EventArgs e)
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            channelnumber.Visible = false;
            channelnumberlabel.Visible = false;
            apigizle.Visible = false;
            apikey.PasswordChar = '*';

            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");
            chart1.Series.Add("");


            chart1.Series[0].ChartType = SeriesChartType.Spline;
            chart1.Series[1].ChartType = SeriesChartType.Spline;
            chart1.Series[2].ChartType = SeriesChartType.Spline;
            chart1.Series[3].ChartType = SeriesChartType.Spline;
            chart1.Series[4].ChartType = SeriesChartType.Spline;
            chart1.Series[5].ChartType = SeriesChartType.Spline;
            chart1.Series[6].ChartType = SeriesChartType.Spline;
            chart1.Series[7].ChartType = SeriesChartType.Spline;
            chart1.Series[8].ChartType = SeriesChartType.Spline;
            chart1.Series[9].ChartType = SeriesChartType.Spline;


            chart1.Series[0].Enabled = false;
            chart1.Series[1].Enabled = false;
            chart1.Series[2].Enabled = false;
            chart1.Series[3].Enabled = false;
            chart1.Series[4].Enabled = false;
            chart1.Series[5].Enabled = false;
            chart1.Series[6].Enabled = false;
            chart1.Series[7].Enabled = false;
            chart1.Series[8].Enabled = false;
            chart1.Series[9].Enabled = false;




        }


        private void changed(object sender, EventArgs e)
        {

            channelnumber.Visible = true;
            channelnumberlabel.Visible = true;
            timer1.Stop();
            verilerial.Text = "Verileri Al";
            channelnumber.ReadOnly = false;

        }

        private void unchanged(object sender, EventArgs e)
        {
            channelnumber.Visible = false;
            channelnumberlabel.Visible = false;
            timer1.Stop();
            verilerial.Text = "Verileri Al";
            channelnumber.ReadOnly = false;

        }

        private void apigoster_Click(object sender, EventArgs e)
        {
            apigizle.Visible = true;
            apigoster.Visible = false;
            apikey.PasswordChar = '\0';

        }

        private void apigizle_Click(object sender, EventArgs e)
        {
            apigizle.Visible = false;
            apigoster.Visible = true;
            apikey.PasswordChar = '*';
        }

        private void degergirildi(object sender, KeyEventArgs e)
        {

            MetroFramework.Controls.MetroTextBox[] values = { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10 };
            MetroFramework.Controls.MetroLabel[] valueslabel = { vl1, vl2, vl3, vl4, vl5, vl6, vl7, vl8, vl9, vl10 };

            if (e.KeyCode == Keys.Enter && i < 10)
            {
                i++;
                values[i].Visible = true;
                valueslabel[i].Visible = true;


            }
        }



        private void verilerial_Click(object sender, EventArgs e)
        {


            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
                series.LegendText = "";
                series.Enabled = false;
            }


            if (kuladi.Text != "" && kulsifre.Text != "")
            {

                if (timer1.Enabled)
                {
                    timer1.Stop();
                    verilerial.Text = "Verileri Al";
                    Thread.Sleep(1);
                    channelnumber.ReadOnly = false;
                }
                else
                {
                    timer1.Start();
                    verilerial.Text = "Verileri Durdur";
                    channelnumber.ReadOnly = true;
                }
            }
            else
                MessageBox.Show("Kullanıcı adı ve şifre alanı boş bırakılamaz.");



        }




        public void vericek(int channel)
        {
          
            try
            {
                String url;
                if (all.Checked)
                    url = "https://iothook.com/api/v1.2/datas/?data=1";


                else if (first.Checked)
                    url = "https://iothook.com/api/v1.2/datas/?data=first&channel=" + channel;


                else
                    url = "https://iothook.com/api/v1.2/datas/?data=last&channel=" + channel;

                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:28.0) Gecko/20100101 Firefox/28.0";
                webRequest.ContentLength = 0; // added per comment
                string autorization = kuladi.Text + ":" + kulsifre.Text;
                byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);
                autorization = Convert.ToBase64String(binaryAuthorization);
                autorization = "Basic " + autorization;
                webRequest.Headers.Add("AUTHORIZATION", autorization);               
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK) MessageBox.Show(webResponse.Headers.ToString());
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {

                    string s = reader.ReadToEnd();

                    s = s.Trim('[');
                    s = s.Trim(']');

                    dynamic stuff = JObject.Parse(s);
                    reader.Close();

                    grafik++;
                    if (stuff.value_1 != null)
                    {
                        chart1.Series[0].Points.AddXY(grafik, Convert.ToDouble(stuff.value_1));
                        chart1.Series[0].LegendText = stuff.element_1;
                        chart1.Series[0].Enabled = true;
                    }

                    if (stuff.value_2 != null)
                    {
                        chart1.Series[1].Points.AddXY(grafik, Convert.ToDouble(stuff.value_2));
                        chart1.Series[1].LegendText = stuff.element_2;
                        chart1.Series[1].Enabled = true;
                    }

                    if (stuff.value_3 != null)
                    {
                        chart1.Series[2].Points.AddXY(grafik, Convert.ToDouble(stuff.value_3));
                        chart1.Series[2].LegendText = stuff.element_3;
                        chart1.Series[2].Enabled = true;
                    }

                    if (stuff.value_4 != null)
                    {
                        chart1.Series[3].Points.AddXY(grafik, Convert.ToDouble(stuff.value_4));
                        chart1.Series[3].LegendText = stuff.element_4;
                        chart1.Series[3].Enabled = true;
                    }
                    if (stuff.value_5 != null)
                    {
                        chart1.Series[4].Points.AddXY(grafik++, Convert.ToDouble(stuff.value_5));
                        chart1.Series[4].LegendText = stuff.element_5;
                        chart1.Series[4].Enabled = true;
                    }

                    if (stuff.value_6 != null)
                    {
                        chart1.Series[5].Points.AddXY(grafik, Convert.ToDouble(stuff.value_5));
                        chart1.Series[5].LegendText = stuff.element_6;
                        chart1.Series[5].Enabled = true;
                    }

                    if (stuff.value_7 != null)
                    {
                        chart1.Series[6].Points.AddXY(grafik, Convert.ToDouble(stuff.value_5));
                        chart1.Series[6].LegendText = stuff.element_7;
                        chart1.Series[6].Enabled = true;
                    }

                    if (stuff.value_8 != null)
                    {
                        chart1.Series[7].Points.AddXY(grafik, Convert.ToDouble(stuff.value_5));
                        chart1.Series[7].LegendText = stuff.element_8;
                        chart1.Series[7].Enabled = true;
                    }

                    if (stuff.value_9 != null)
                    {
                        chart1.Series[8].Points.AddXY(grafik, Convert.ToDouble(stuff.value_5));
                        chart1.Series[8].LegendText = stuff.element_9;
                        chart1.Series[8].Enabled = true;
                    }

                    if (stuff.value_10 != null)
                    {
                        chart1.Series[9].Points.AddXY(grafik, Convert.ToDouble(stuff.value_5));
                        chart1.Series[9].LegendText = stuff.element_10;
                        chart1.Series[9].Enabled = true;
                    }


                }
            }

            catch (Exception e)
            {

                timer1.Stop();
                verilerial.Text = "Veri Al";
                MessageBox.Show("Kullanıcı adı , şifreniz yanlış veya bağlantınızda sorun olabilir" + e);
                
                channelnumber.ReadOnly = false;


            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (first.Checked || last.Checked)
            {
                if (channelnumber.Text != "")
                {
              verigetir = new Thread(() => vericek(Convert.ToInt32(channelnumber.Text)));
              verigetir.IsBackground = true;
              verigetir.Start();
                   
                }
                else
                {
                    timer1.Stop();
                    verilerial.Text = "Verileri Al";
                    channelnumber.ReadOnly = false;
                    MessageBox.Show("Kanal numarası boş bırakılamaz");

                }
                    
            }
            else
            {
             verigetir = new Thread(() => vericek(0));
             verigetir.IsBackground = true;
             verigetir.Start();
            }

        }

        private void verigonder_Click(object sender, EventArgs e)
        {
           verigonderthread = new Thread(verigondermethod);
           verigonderthread.IsBackground = true;
            verigonderthread.Start();
        }


        public void verigondermethod()
        {

            if (kuladi.Text != "" && kulsifre.Text != "")
            {

                try
                {



                    String url = "https://iothook.com/api/v1.2/datas/";



                    CookieContainer cookies = new CookieContainer();
                    var webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Method = "POST";

                    webRequest.CookieContainer = cookies;
                    webRequest.ContentType = "application/json";
                    webRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
                    webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    string autorization = kuladi.Text + ":" + kulsifre.Text;
                    byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);
                    autorization = Convert.ToBase64String(binaryAuthorization);
                    autorization = "Basic " + autorization;
                    webRequest.Headers.Add("AUTHORIZATION", autorization);
                    webRequest.SendChunked = true;
                 
                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        MessageBox.Show("burada");
                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(new
                        {
                            api_key = apikey.Text,
                            value_1 = v1.Text,
                            value_2 = v2.Text,
                            value_3 = v3.Text,
                            value_4 = v4.Text,
                            value_5 = v5.Text,
                           
                        });
                       
                        streamWriter.Write(json);
                        
                        MessageBox.Show("Değerler başarılı bir şekilde yüklendi.");
                        streamWriter.Flush();
                        streamWriter.Close();

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("kullanıcı adı , şifre , api key yanlış veya bağlantınızda sorun olabilir.");
                }

            }
            else
                MessageBox.Show("Kullanıcı adı ve şifre alanı boş bırakılamaz.");

            Thread.Sleep(0);


        }

        private void grafikkaydet_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            verilerial.Text = "Verileri Al";
            saveFileDialog1.InitialDirectory = "C:";
            saveFileDialog1.Title = "Excel Dosyasını Buraya Kaydet";
            saveFileDialog1.FileName = DateTime.Now.ToString("MM/dd/yyyy HH.mm");
            saveFileDialog1.Filter = "Resim dosyası|*.png";

            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {

                chart1.SaveImage(saveFileDialog1.FileName.ToString(), ChartImageFormat.Png);
            }

        }
       

        private void rakamgirisi(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }


    }
}
