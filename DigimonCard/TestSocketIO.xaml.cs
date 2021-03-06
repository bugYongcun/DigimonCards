﻿using Newtonsoft.Json.Linq;
using SocketIO4Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web;
using SocketIOClient;
using Windows.UI.Core;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DigimonCard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestSocketIO : Page
    {
        private Client socketIO;
        private CoreDispatcher SampleDispatcher;

        public TestSocketIO()
        {
            this.InitializeComponent();
            SampleDispatcher = Window.Current.CoreWindow.Dispatcher;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        void socketIO_Message(object sender, MessageEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Message.Event))
            {
                Debug.WriteLine("--> SOCKET_IO_MESSAGE:: {0}", e.Message.MessageText);
            }
            else
            {
                Debug.WriteLine("--> SOCKET_IO_MESSAGE:: {0} : {1}", e.Message.Event, e.Message.Json.ToJsonString());
                //screenTbk.Text += e.Message.Json.ToJsonString();
            }

        }

        //哭了，貌似socketIO中on事件里会发生这个闹心的问题啊
        //他的相当于另起一个线程，然后里边的东西变成了局部变量，不能改变这个class的值啊  疯了！！！
        //那怎么玩啊、、弄个static试试
        //nice啊 静态也不行。哭了哭了 这次真哭了！
        private void connectBt_Click(object sender, RoutedEventArgs e)
        {
            //socketIO = new Client(serverUrl.Text);
            //socketIO = new Client("http://168.63.151.29:3000");
            this.connectBt.IsEnabled = false;
            socketIO = new Client("http://test.twtstudio.com:3000");
            socketIO.Message += socketIO_Message;
            socketIO.SocketConnectionClosed += socketIO_SocketConnectionClosed;
            socketIO.Error += socketIO_Error;

            socketIO.On("connect", (message) => 
            {
                
                Debug.WriteLine("on connect called!!!");
                JObject jo = new JObject();
                jo["username"] = "username";
                jo["password"] = "password";
                socketIO.Emit("hConnect", jo);

            });

            socketIO.On("a", async (message) =>
            {
                Debug.WriteLine("start listening");
                Debug.WriteLine(message.Json.ToJsonString());
                await SampleDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    /*//定义一个JSON字符串 
                    string jsonText = "[{'a':'aaa','b':'bbb','c':'ccc'},{'a':'aaa2','b':'bbb2','c':'ccc2'}]";
                    //反序列化JSON字符串*/
                    //JArray ja = (JArray)JsonConvert.DeserializeObject(message.Json.ToJsonString());
                   // JArray ja = (JArray)JsonConvert.DeserializeObject(jsonText);
                    //将反序列化的JSON字符串转换成对象
                    JObject o = (JObject)JsonConvert.DeserializeObject(message.Json.ToJsonString());
                    //读取对象中的各项值
                    JArray jb = (JArray)JsonConvert.DeserializeObject(o["args"].ToString());

                    JObject ob = (JObject)jb[0];

                    JObject oc = (JObject)ob["mes"];

                    //Console.WriteLine(ja[1]["a"]);  
                    
                   // ChangedEventHandler(message.Json.ToJsonString());

                    if(oc["a"] != null )
                        ChangedEventHandler(oc["a"].ToString());
                    //{"name":"a","args":[{"mes":{"username":"username","password":"password"}}]}
                });
            });

            socketIO.ConnectAsync();

        }

        /*void con(SocketIOClient.Messages.IMessage m,  string s)
        {
            s = m.Json.ToJsonString();
            Debug.WriteLine("on connect called!!!");
            JObject jo = new JObject();
            jo["publisher"] = "username";
            jo["password"] = "password";
            socketIO.Emit("hConnect", jo);
        }*/


        /*void con( TextBlock b)
        {
            b.Text += "bb";
            Debug.WriteLine("on connect called!!!");
            JObject jo = new JObject();
            jo["publisher"] = "username";
            jo["password"] = "password";
            socketIO.Emit("hConnect", jo);
        }*/
        

        void socketIO_Error(object sender, ErrorEventArgs e)
        {
            WebErrorStatus status = WebSocketError.GetStatus(e.Exception.HResult);
            Debug.WriteLine("-->SOCKET_IO_ERROR::" + status);
        }

        void socketIO_SocketConnectionClosed(object sender, EventArgs e)
        {
            Debug.WriteLine(">>>socketIO_SocketConnectionClosed::Closed!");
        }

        private void disConnectBt_Click(object sender, RoutedEventArgs e)
        {
            //if (socketIO != null)
            //{
                socketIO.Close();
           // }
        }

        private void sendBt_Click(object sender, RoutedEventArgs e)
        {
            if (socketIO != null && socketIO.IsConnected)
            {
                Debug.WriteLine("on send connect called!!!");
                //socketIO.Emit("hConnect", JObject.Parse(sendTbx.Text));
                string s = "{\"a\":\""+this.sendTbx.Text+"\"}";
                socketIO.Emit("hConnect", JObject.Parse(s));
            
            }
        }

        private void listenBt_Click(object sender, RoutedEventArgs e)
        {
            socketIO.On("a", async (message) => 
            {
                Debug.WriteLine("start listening");
                Debug.WriteLine(message.Json.ToJsonString());
                await SampleDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ChangedEventHandler(message.Json.ToJsonString());
                });
            });
        }

        private void ChangedEventHandler(string s)
        {
            this.listenTbx.Text += s + "\n";
        }

    }
}
