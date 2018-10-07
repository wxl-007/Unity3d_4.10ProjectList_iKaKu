using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace GameClientNet
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NetManager
    {
        #region data

        private Thread m_NetTcpThread = null;
        //private Thread m_SendThread = null;
        private string m_sTcpServerIp;
        private UInt16 m_nTcpServerPort = 0;
        //private Socket m_UdpSocket = null;

        public static Socket m_TcpSocket = null;
        private Byte[] m_Recvbuffer = new byte[10240];
        private Byte[] m_Sendbuffer = new byte[10240];
        private NetPacketHeader m_pNetPacketHeader = new NetPacketHeader();
        private int m_nRecvBufferOffset = 0;

        private Queue<NetPacket> m_RecvQueue = new Queue<NetPacket>();
        private Queue<NetPacket> m_SendQueue = new Queue<NetPacket>();


        private static NetManager m_pInstance = null;
        private static bool m_bThreadRunning = true;

        private NetManager()
        {
        }

        #endregion

        #region TcpSocket
        private bool isConnecting = false;
        private void ConnectTcpServer(string svrip, UInt16 port, int tBite)
        {
            isConnecting = true;
            m_sTcpServerIp = svrip;
            m_nTcpServerPort = port;
            try
            {
                m_TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint tempRemoteIP = new IPEndPoint(IPAddress.Parse(m_sTcpServerIp), m_nTcpServerPort);
                EndPoint epTemp = (EndPoint)tempRemoteIP;

                IAsyncResult result = m_TcpSocket.BeginConnect(epTemp, new AsyncCallback(connectCallback), m_TcpSocket);
                bool success = result.AsyncWaitHandle.WaitOne(5000, true);
                m_TcpSocket.Blocking = false;//
                if (!success)
                {
                    //					CloseSocket();
                    isConnecting = false;
                    Debug.Log("connect Time Out");
                }
                else
                {
                    isConnecting = false;
                    this.InitNetThread(2);
                }
            }
            catch (System.Exception ex)
            {
                string errorlog = "Error:" + ex.ToString();
                UnityEngine.Debug.Log(errorlog);
                //Console.WriteLine(errorlog);
            }
        }
        private void connectCallback(IAsyncResult asyncConnect)
        {
            if (Connected)
            {
                Debug.Log("connectSuccess");
            }
            else
            {
                CloseSocket();
                Debug.Log("connectFiled");
            }

            isConnecting = false;
        }
        public bool IsConnecting()
        {
            return isConnecting;
        }
        private bool _TcpRecv()
        {
            int recvlen = 0;
            if (m_pNetPacketHeader.PacketId > 0)
            {
                recvlen = m_pNetPacketHeader.BodyLength;
                //-m_pNetPacketHeader.GetHeadLen();
            }
            else
            {
                recvlen = NetPacketHeader.GetHeadLen();
            }

            if (m_TcpSocket.Available <= 0)
                return false;

            m_nRecvBufferOffset += m_TcpSocket.Receive(m_Recvbuffer, m_nRecvBufferOffset, recvlen - m_nRecvBufferOffset, 0);

            if (m_nRecvBufferOffset < recvlen)
            {
                Debug.Log("bao ti shu liang bu gou");
                return false;
            }

            if (m_pNetPacketHeader.PacketId > 0)
            {
                NetPacket packet = null;

                //				string str = "";
                //				Debug.Log("m_pNetPacketHeader.BodyLength:" + m_pNetPacketHeader.BodyLength);
                //				for(int i = 0;i < m_pNetPacketHeader.BodyLength;i++){
                //					str += (m_Recvbuffer[i] + "_");
                //				}
                //				Debug.Log("m_Recvbuffer:" + str);
                packet = new NetPacket(m_pNetPacketHeader);
                packet.Read(m_Recvbuffer, m_pNetPacketHeader.BodyLength);
                m_RecvQueue.Enqueue(packet);

                m_pNetPacketHeader.PacketId = 0;
                m_nRecvBufferOffset = 0;
            }
            else
            {   //read packet header

                if (m_pNetPacketHeader.ReadHead(m_Recvbuffer))
                {
                    if (m_pNetPacketHeader.BodyLength <= 0)
                    {//dose not have body data
                        NetPacket msg = new NetPacket(m_pNetPacketHeader);
                        m_RecvQueue.Enqueue(msg);
                        //						Debug.Log("m_pNetPacketHeader.PacketId:" + m_pNetPacketHeader.PacketId);
                        //						Debug.Log("msg:" + msg.Count);
                        m_pNetPacketHeader.PacketId = 0;

                    }
                    m_nRecvBufferOffset = 0;
                }
                else
                {
                    m_TcpSocket.Close(1);
                    return false;
                }
            }
            return true;
        }

        void CloseSocket()
        {
            try
            {
                m_TcpSocket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                m_TcpSocket.Close();
            }
            catch (System.Exception ex)
            {
                string errorlog = "Error:" + ex.ToString();
                Debug.Log(errorlog);
                //Console.WriteLine(errorlog);
            }
        }

        //        DateTime m_nLastSendTime = DateTime.Now;

        private bool TcpRead()
        {
            try
            {
                int n = 0;
                while (_TcpRecv())
                {
                    ++n;
                }
            }
            catch (System.Exception ex)
            {
                string errorlog = "Error:" + ex.ToString();
                Debug.Log(errorlog);
                CloseNet();
                //Console.WriteLine(errorlog);
            }
            return true;
        }

        private bool _TcpSend()
        {

            //            if (DateTime.Now.AddSeconds(-100).CompareTo(m_nLastSendTime) > 0)
            //            {
            //                UnityEngine.Debug.Log("==========Heart packet=============");
            //                NetPacket heartPacket = new NetPacket(0);
            //                heartPacket.Id = 0;
            //                SendMsg(heartPacket);
            //                m_nLastSendTime = DateTime.Now;
            //            }
            if (m_SendQueue.Count <= 0)
            {
                return false;
            }
            NetPacket msg = m_SendQueue.Dequeue();
            if (null == msg)
            {
                return false;
            }
            UInt16 nLen = msg.Write(m_Sendbuffer);
            int offset = 0;
            while (true)
            {
                //				string str = "";
                //				for(int i = 0;i < nLen;i++){
                //					str += (m_Sendbuffer[i] + "_");
                //				}
                //				Debug.Log("str" + str);
                offset += m_TcpSocket.Send(m_Sendbuffer, offset, nLen - offset, 0);
                if (offset < nLen)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    break;
                }
            }
            if (offset > 0)//We have send something
            {
                //                m_nLastSendTime = DateTime.Now;
            }
            //Thread.Sleep(10000);
            return true;
        }

        private bool TcpWrite()
        {
            try
            {
                while (_TcpSend()) ;
            }
            catch (System.Exception ex)
            {
                string errorlog = "Error:" + ex.ToString();
                //Console.WriteLine(errorlog);
                Debug.Log(errorlog);
            }
            return true;
        }

        #endregion

        #region net work thread

        private static void Threadproc()
        {
            NetManager pInstance = NetManager.GetInstance();
            while (m_bThreadRunning)
            {
                pInstance.TcpRead();
                pInstance.TcpWrite();
                Thread.Sleep(10);
            }
        }

        private void CloseNetThreads()
        {
            m_bThreadRunning = false;
        }

        private void InitNetThread(int type)
        {
            try
            {
                UnityEngine.Debug.Log("InitNetThread ============" + m_NetTcpThread);
                if (null == m_NetTcpThread)
                {
                    m_NetTcpThread = new Thread(Threadproc);
                    m_NetTcpThread.Start();
                }
            }
            catch (System.Exception ex)
            {
                string errorlog = "Error:" + ex.ToString();
                //Console.WriteLine(errorlog);
                //todo: log
                Debug.Log(errorlog);
            }
        }

        #endregion
    }
}
