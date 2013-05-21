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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web;
using SocketIOClient;
using Windows.UI.Core;
using Newtonsoft.Json;
using Windows.System;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace DigimonCard
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class MediaPlayPage : DigimonCard.Common.LayoutAwarePage
    {
        public int readyPersonNum = 0;
        public bool hostIsReady = false;
        public bool challengerIsReady = false;
        private Client socketIO;
        private CoreDispatcher SampleDispatcher;
        public int win_width = (int)Window.Current.Bounds.Width;
        public int win_height = (int)Window.Current.Bounds.Height;

        public MediaPlayPage()
        {
            this.InitializeComponent();

            Canvas.SetTop(chatPanel, win_height - 438);
            roomNum.Text = Self.roomNum.ToString();
            vedioNum.Text = Self.roomNum.ToString();

            SampleDispatcher = Window.Current.CoreWindow.Dispatcher; //此实例是负责处理窗口消息，事件调度给客户端。

            createConnect();

        }

        public void createConnect()
        {
            //socketIO = new Client("http://168.63.151.29:3000");

            socketIO = new Client("http://test.twtstudio.com:3000/");
            socketIO.Message += socketIO_Message;
            socketIO.SocketConnectionClosed += socketIO_SocketConnectionClosed;
            socketIO.Error += socketIO_Error;

            socketIO.On("connect", (message) =>
            {
                Debug.WriteLine("on connect called!!!");
                JObject jo = new JObject();
                jo["username"] = Self.self.GetName();
                socketIO.Emit("join", jo);
            });


            socketIO.On("chat", async (message) =>
            {
                Debug.WriteLine("chat string");
                Debug.WriteLine(message.Json.ToJsonString());
                await SampleDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    JObject o = (JObject)JsonConvert.DeserializeObject(message.Json.ToJsonString());
                    JArray jb = (JArray)JsonConvert.DeserializeObject(o["args"].ToString());
                    JObject ob = (JObject)jb[0];
                    if (ob["roomNum"].ToString().Equals(Self.roomNum.ToString()))
                        ChangedEventHandler(ob["username"] + ":" + ob["chat"]);
                });
            });

            socketIO.On("num", async (message) =>
            {
                Debug.WriteLine("num changed");
                Debug.WriteLine(message.Json.ToJsonString());
                await SampleDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {

                    Debug.WriteLine(message.Json.ToJsonString());
                    JObject o = (JObject)JsonConvert.DeserializeObject(message.Json.ToJsonString());
                    JArray jb = (JArray)JsonConvert.DeserializeObject(o["args"].ToString());
                    JObject ob = (JObject)jb[0];
                    if (ob["roomNum"].ToString().Equals(Self.roomNum.ToString()))
                        ChangedNumHandler(ob["counts"].ToString());
                });
            });

            socketIO.ConnectAsync();

            string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString()  + "\"}";
            socketIO.Emit("enter_room", JObject.Parse(s));
        }

        private void ChangedNumHandler(string p)
        {
            this.onlinePersonNum.Text = p;
        }
        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="pageState">要使用可序列化状态填充的空字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
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

        void socketIO_Error(object sender, ErrorEventArgs e)
        {
            WebErrorStatus status = WebSocketError.GetStatus(e.Exception.HResult);
            Debug.WriteLine("-->SOCKET_IO_ERROR::" + status);
        }

        void socketIO_SocketConnectionClosed(object sender, EventArgs e)
        {
            Debug.WriteLine(">>>socketIO_SocketConnectionClosed::Closed!");
        }

        private void sendBtn_click(object sender, RoutedEventArgs e)
        {
            if (socketIO != null && socketIO.IsConnected && !this.sendTbx.Text.Equals(""))
            {
                Debug.WriteLine("on send connect called!!!");
                //socketIO.Emit("hConnect", JObject.Parse(sendTbx.Text));
                string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString() + "\",\"chat\":\"" + this.sendTbx.Text + "\"}";
                socketIO.Emit("client_chat", JObject.Parse(s));

            }
        }

        private void ChangedEventHandler(string s)
        {
            this.listenTbx.Text += s + "\n";
            this.sendTbx.Text = "";
        }

        private void pop_fold_Btn_Click(object sender, RoutedEventArgs e)
        {
            if ((string)pop_fold_Btn.Content == "弹出聊天框")
            {
                storyboard_chatappe.Begin();
                pop_fold_Btn.Content = "收起聊天框";
            }
            else
            {
                storyboard_chatdisa.Begin();
                pop_fold_Btn.Content = "弹出聊天框";
            }
        }

        private void keyboard_Click(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {

                if (socketIO != null && socketIO.IsConnected && !this.sendTbx.Text.Equals(""))
                {
                    Debug.WriteLine("on send connect called!!!");
                    //socketIO.Emit("hConnect", JObject.Parse(sendTbx.Text));
                    string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString() + "\",\"chat\":\"" + this.sendTbx.Text + "\"}";
                    socketIO.Emit("client_chat", JObject.Parse(s));

                }
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString() + "\"}";
            socketIO.Emit("exit_room", JObject.Parse(s));
            if (socketIO != null)
            {
                JObject jo = new JObject();
                jo["username"] = "username";
                socketIO.Emit("client_close", jo);

                socketIO.Close();
            }
            Frame.Navigate(typeof(GameLobbyPage));
        }
    }
}