using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Net;

namespace CSRcon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            rcon cs = new rcon();
            this.txtResponse.Text = "";
         
           string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, this.txtCommand.Text).Replace("\n","\r\n");
            this.txtResponse.Text = serverResponse.Remove(0, 5);
            this.txtCommand.Text = "";       
        }

        private void saveServersBtn_Click(object sender, EventArgs e)
        {
            string serverInfo = "IP: " + this.txtServerIP.Text + " Port: " + this.txtServerPort.Text + " Rcon: " + this.txtPassword.Text + " End";

            listServerinfoBox.Items.Insert(0,serverInfo.ToString());

            using (StreamWriter w = File.AppendText("savedServers.txt"))
            {
                foreach (string serverinfo in listServerinfoBox.Items)
                {
                    w.WriteLine(serverinfo);
                }
            }        

                MessageBox.Show("Server Info Saved");
        }

        public void showLoggedtxt_Click(object sender, EventArgs e)
        {


            Process.Start("savedServers.txt");
        }



        private void button1_Click(object sender, EventArgs e)
        {
            rcon cs = new rcon();
            this.txtResponse.Text = "";

            string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "users").Replace("\n", "\r\n");
            this.txtResponse.Text = serverResponse.Remove(0, 5);
            this.txtCommand.Text = "";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            rcon cs = new rcon();
            this.lblHostname.Text = "";
            this.lblMaps.Text = "";
            this.lblPlayers.Text = "";
            this.lblVersion.Text = "";


            string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "status").Replace("\n", "\r\n");

            this.lblHostname.Text = getBetween(serverResponse, "hostname: ", "version");
            this.lblMaps.Text = getBetween(serverResponse, "map     :  ", " at:");
            this.lblPlayers.Text = (getBetween(serverResponse, "players : ", "active") +"/"+ (getBetween(serverResponse, "active (", " max)")));
            this.lblVersion.Text = getBetween(serverResponse, "version : ", "tcp/ip  :  ");
            string mapName = getBetween(serverResponse, "map     :  ", " at:");

            this.mapImgBox.SizeMode = PictureBoxSizeMode.StretchImage;
            if(!File.Exists(@"maps\" + mapName + ".jpg"))
            {
                this.mapImgBox.Image = Image.FromFile(@"maps\noimage.jpg");
            }
            else
            this.mapImgBox.Image = Image.FromFile(@"maps\"+mapName+".jpg");
          

        }

        private void listServerinfoBox_DoubleClick(object sender, EventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null)
            {   
               string savedSrvInfo = ((ListBox)sender).SelectedItem.ToString();

                this.txtServerIP.Text = getBetween(savedSrvInfo, "IP: ", " Port");
                this.txtServerPort.Text = getBetween(savedSrvInfo, "Port: ", " Rcon");
                this.txtPassword.Text = getBetween(savedSrvInfo, "Rcon: ", "End");

            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            string line;
            var file = new System.IO.StreamReader("savedServers.txt");

            if (listServerinfoBox.Items.Count <= 0)
            {


                while ((line = file.ReadLine()) != null)
                {
                    listServerinfoBox.Items.Add(line);
                }
            }
            else
            {
                MessageBox.Show("Check your savedServers.txt File!");
            }
        }


        private void button16_Click(object sender, EventArgs e)
        {
            string liveOn3 = "sv_restart \"1\" ;";

            rcon cs = new rcon();
                this.txtResponse.Text = "";

                string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, liveOn3).Replace("\n", "\r\n");
                this.txtResponse.Text = "Server: sv_restart 1";

        }

        private void button17_Click(object sender, EventArgs e)
        {
            string knife= "say \"================================== [Knife Round] \"";

            rcon cs = new rcon();
            this.txtResponse.Text = "";

            string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, knife).Replace("\n", "\r\n");
            this.txtResponse.Text = "SERVER: ================================== [Knife Round]";
        }

        private void button18_Click(object sender, EventArgs e)
        {
           
            

            rcon cs = new rcon();
            this.txtResponse.Text = "";

            cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "mp_startmoney \"16000\"").Replace("\n", "\r\n");
            Thread.Sleep(1000);
            cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "mp_roundtime \"10\"").Replace("\n", "\r\n");
            Thread.Sleep(1000);
            cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "mp_freezetime  \"0\"").Replace("\n", "\r\n");
            Thread.Sleep(1000);
            cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "sv_restart   \"1\"").Replace("\n", "\r\n");

            

        
            this.txtResponse.Text = "SERVER: Startmoney = 16000\r\nSERVER: Roundtime = 10\r\nSERVER: Freezetime = 0";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form2 m = new Form2();
            m.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rcon cs = new rcon();
            this.txtResponse.Text = "";

            string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "kick #"+ this.txtKickID.Text).Replace("\n", "\r\n");
            this.txtResponse.Text = serverResponse.Remove(0, 5);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rcon cs = new rcon();
            this.txtResponse.Text = "";

            string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "pausable 1").Replace("\n", "\r\n");
            Thread.Sleep(1000);
            serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "pause").Replace("\n", "\r\n");
            this.txtResponse.Text = serverResponse.Remove(0, 5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rcon cs = new rcon();
            this.txtResponse.Text = "";

            string serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "pausable 0").Replace("\n", "\r\n");
            Thread.Sleep(1000);
            serverResponse = cs.sendRCON(this.txtServerIP.Text, int.Parse(this.txtServerPort.Text), this.txtPassword.Text, "unpause").Replace("\n", "\r\n");
            this.txtResponse.Text = serverResponse.Remove(0, 5);
        }

        private void txtCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSend_Click(null, null);
            }
        }
    }
}