using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
namespace GameClientNet
{
    public class NetMsg
    {
        private static bool isStop;
        #region NetCommanApi
        public static UInt32 IPToNumber(string strIPAddress)
        {
            string[] arrayIP = strIPAddress.Split('.');
            UInt32 sip1 = UInt32.Parse(arrayIP[0]);
            UInt32 sip2 = UInt32.Parse(arrayIP[1]);
            UInt32 sip3 = UInt32.Parse(arrayIP[2]);
            UInt32 sip4 = UInt32.Parse(arrayIP[3]);
            UInt32 tmpIpNumber = (sip1 << 24) + (sip2 << 16) + (sip3 << 8) + sip4;
            return tmpIpNumber;
        }

        public static string NumberToIP(UInt32 intIPAddress)
        {
            Byte s1 = (Byte)(intIPAddress >> 24);
            Byte s2 = (Byte)(intIPAddress >> 16);
            Byte s3 = (Byte)(((UInt16)intIPAddress) >> 8);
            Byte s4 = (Byte)intIPAddress;
            string strIPAddress = s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString();
            return strIPAddress;
        }
        #endregion

        public static void Init(bool bsvr)
        {

        }

        public static bool RecvMsg()
        {
            if (isStop)
            {
                return false;
            }
            NetPacket packet = NetManager.GetInstance().RecvMsg();

            if (null != packet)
            {
                Debug.Log("RecvMsg--msgID:" + packet.Id + ",Count:" + packet.Count);
                string str = "";
                for (int i = 0; i < packet.Count; i++)
                {
                    str += (packet.datas[i] + "_");
                }
                Debug.Log("msg:" + str);
                ByteBuffer buffer = new ByteBuffer(packet.datas);
//                MainAction.GetInstance().ProcessMessage(packet.Id, buffer);

            }
            return false;
        }
        public static bool SendMsg(ushort pId, ByteBuffer buffer = null)
        {
            NetPacket packet = null;
            if (buffer == null)
            {
                packet = new NetPacket(pId, null, 0);
            }
            else
            {
                packet = new NetPacket(pId, buffer.GetData(), buffer.GetCurrent());
            }

            NetManager.GetInstance().SendMsg(packet);
            Debug.Log("SendMsg--msgID:" + packet.Id + ",Count:" + packet.Count);
            string str = "";
            for (int i = 0; i < packet.Count; i++)
            {
                str += (packet.datas[i] + "_");
            }
            Debug.Log("msg:" + str);
            return true;
        }
        public static void SetStop(bool stop)
        {
            isStop = stop;
        }
    }
}