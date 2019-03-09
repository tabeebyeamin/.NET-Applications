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
using System.Net.Sockets;

namespace ChatApplication
{
    public partial class Form1 : Form
    {
        // initialize Socket
        Socket sck;

        // set the local and remote endpoints
        EndPoint epLocal, epRemote;


        byte[] buffer;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // set up socket using InterNetwork, dgram and UDP
            sck = new Socket(
                AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sck.SetSocketOption(
                SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            // get user and remote IP, prevents you from having to retype
            // in the address everytime
            textLocalIP.Text = GetLocalIP();
            textRemoteIP.Text = GetLocalIP();

        }

        private string GetLocalIP()
        {
            // get the host name
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            // loop through the IPS in the Address List
            foreach(IPAddress IP in host.AddressList)
            {
                // if the address family is that of internetwork return the IP
                if (IP.AddressFamily == AddressFamily.InterNetwork)
                {
                    return IP.ToString();
                }
            }

            // return local host IP
            return "127.0.0.1";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // binding the socket

            // Create an enpoint with the local IP and port
            // Parse the IPAddress from the local IP box and
            // convert port to a 32 bit integer
            epLocal = new IPEndPoint(
                IPAddress.Parse(textLocalIP.Text), Convert.ToInt32(textLocalPort.Text));

            // bind the socket
            sck.Bind(epLocal);

            // set up the remote end point

            epRemote = new IPEndPoint(
                IPAddress.Parse(textRemoteIP.Text), Convert.ToInt32(textRemotePort.Text));

            // connect with the remote IP
            sck.Connect(epRemote);

            // Listening
            buffer = new byte[1500];

            // offset 0, no flags, remote IP reference
            // Asynchronous Callback fxn will be handled by MessageCallBack fxn
            sck.BeginReceiveFrom(
                buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(
                    MessageCallBack), buffer);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Convert string message to byte array
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];

            // convert the string text message to byte message to send
            sendingMessage = aEncoding.GetBytes(textMessage.Text);

            // sending the encoded message
            sck.Send(sendingMessage);

            // add to the listbox
            messageList.Items.Add("Me: " + textMessage.Text);

            // empty the msg
            textMessage.Text = "";
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                byte[] receivedData = new byte[1500];

                receivedData = (byte[])aResult.AsyncState;
                // Converting byte[] to string
                ASCIIEncoding aEncoding = new ASCIIEncoding();

                // converting receivedData to receivedMessage
                // converting ASCII to String
                string receivedMessage = aEncoding.GetString(receivedData);

                // Adding message to listbox
                messageList.Items.Add("Friend: " + receivedMessage);

                // callback again

                buffer = new byte[1500];
                sck.BeginReceiveFrom(
                    buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(
                        MessageCallBack), buffer);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }


        }
    }
}
