using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameClientNet
{
    #region NetManager
    public  partial class NetManager
    {
        /// <summary>
        /// singleton
        /// </summary>
        /// <returns></returns>
        public static NetManager GetInstance()
        {
            if (null == m_pInstance)
            {
                m_pInstance = new NetManager();
            }
            return m_pInstance;
        }

        public bool Connected
        {
            get { return (null != m_TcpSocket) && m_TcpSocket.Connected; }
        }

        /// <summary>
        /// client of net init
        /// </summary>
        /// <param name="svrip">server ip</param>
        /// <param name="port">server port</param>
        /// <param name="tBite">the max time of two send packet</param>
        public void ConnectServer(string svrip, UInt16 port, int tBite)
        {
            if (!Connected)
            {
                this.ConnectTcpServer(svrip, port, tBite);
			}
        }

        /// <summary>
        /// send message to server
        /// </summary>
        /// <param name="msg">the data of send to server</param>
        public void SendMsg(NetPacket msg)
        {
			
			
            lock (m_SendQueue)
            {
                this.m_SendQueue.Enqueue(msg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public NetPacket RecvMsg()
        {
            NetPacket msg = null;
            lock (m_RecvQueue)
            {
                if (m_RecvQueue.Count > 0)
                {
                    msg = this.m_RecvQueue.Dequeue();
                }
            }
            return msg;
        }
        /// <summary>
        /// close net,and end the net threads
        /// </summary>
        public void CloseNet()
        {	
			this.CloseSocket();
			this.CloseNetThreads();
        }

    }
    #endregion

}
