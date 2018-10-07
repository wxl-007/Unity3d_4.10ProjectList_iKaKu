using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Util;
namespace GameClientNet
{
    public enum EnumAgreementSize
    {
        EAG_SINGLE = 0,
        EAG_BIGARRAY,
        EAG_SMALLARRAY,
    }

    #region NetPacket

    public class NetPacket
    {
        NetPacketHeader m_header;
		private byte[] m_datas;
		
        public NetPacket(ushort id,byte[] datas,ushort length)
        {
			m_header = new NetPacketHeader();
			m_header.PacketId = id;
			m_header.BodyLength = length;
			if(datas == null || length == 0){
				m_header.BodyLength = 0;
			}else{
				m_header.BodyLength = length;
				
				m_datas = new byte[length];
				Array.ConstrainedCopy(datas, 0, m_datas, 0, length);
			}
        }
		public byte[] datas{
			get{return m_datas;}
		}
		public NetPacket(NetPacketHeader header)
        {
			m_header = header.Clone();
        }
		public NetPacketHeader GetHeader(){
			return m_header;
		}
        public UInt16 Id
        {
            get {return m_header.PacketId;}
            set { m_header.PacketId = value; }
        }
		public byte isZip
        {
            get {return m_header.IsZip;}
            set { m_header.IsZip = value; }
        }
        public int Count
        {
            get 
            {
				return m_datas == null?0:m_datas.Length;
            }
            set { }
        }

        public bool Read(Byte[] buf,int len)
        {
//            UInt16 offset = 0;
//            UInt16 nl;
			
//			string str = "";
//			Debug.Log("len:" + len);
//			for(int i = 0;i < len;i++){
//				str += (buf[i] + "_");
//			}
//			Debug.Log("NetPacket Read:" + str);
			m_datas = new byte[len];
			Array.ConstrainedCopy(buf, 0, m_datas, 0, len);
            return true;
        }

        public UInt16 Write(Byte[] buf)
        {
			UInt16 offset = NetPacketHeader.GetHeadLen();
			if(m_datas == null){
			}else{
				Array.ConstrainedCopy(m_datas, 0, buf, offset, m_datas.Length);
				offset += (UInt16)m_datas.Length;
			}
            m_header.WriteHead(buf);
            return offset;
        }
		public Byte[] UnZip(Byte[] bytes){
			return bytes;
		}
		public Byte[] Zip(Byte[] bytes){
			return bytes;
		}
    }

    #endregion //NetPacket

    #region NetPacketHeader

    public class NetPacketHeader
    {
        private UInt16 m_nPacketId;
        private UInt16 m_nBodyLen;
		private byte m_nIsZip;
        private static bool m_IsBigEndian;

        public static bool IsBigEndian
        {
            get { return m_IsBigEndian; }
            set { }
        }

        #region ntoh
		public static String NtohString(Byte[] buf, int offset,out UInt16 len)
        {
			ushort length = NetPacketHeader.NtohUint16(buf, offset);
            if (m_IsBigEndian)
            {
                Array.Reverse(buf, offset, length + 2);
            }
			len = (ushort)(length + 2);
            string str = Encoding.UTF8.GetString(buf, offset + 2, length);
            return str;
        }
		public static byte NtohUint8(Byte[] buf, int offset)
        {
            return buf[offset];
        }
        public static UInt16 NtohUint16(Byte[] buf, int offset)
        {
            if (m_IsBigEndian)
            {
                Array.Reverse(buf, offset, 2);
            }
            return BitConverter.ToUInt16(buf, offset);
        }
        public static UInt32 NtohUint32(Byte[] buf, int offset)
        {
            if (m_IsBigEndian)
            {
                Array.Reverse(buf, offset, 4);
            }
            return BitConverter.ToUInt32(buf, offset);
        }
        public static UInt64 NtohUint64(Byte[] buf, int offset)
        {
            if (m_IsBigEndian)
            {
                Array.Reverse(buf, offset, 8);
            }
            return BitConverter.ToUInt64(buf, offset);
        }
        public static float NtohFloat(Byte[] buf, int offset)
        {
            if (m_IsBigEndian)
            {
                Array.Reverse(buf, offset, 4);
            }
            return BitConverter.ToSingle(buf, offset);
        }
        public static double NtohDouble(Byte[] buf, int offset)
        {
            if (m_IsBigEndian)
            {
                Array.Reverse(buf, offset, 8);
            }
            return BitConverter.ToDouble(buf, offset);
        }
        #endregion
        
        #region hton
		
		public static UInt16 HtonString(Byte[] buf,int offset,string vl)
        {
			UInt16 nResult = 0;
            if (null != vl)
            {
				String str = vl;
				
                Byte[] strbuf = Encoding.UTF8.GetBytes(str);
				
				NetPacketHeader.HtonUInt16(buf,offset,(ushort)strbuf.Length);
                Array.ConstrainedCopy(strbuf, 0, buf, offset + 2, strbuf.Length);
                nResult = (UInt16)(strbuf.Length + 2);
            }
            return nResult;
        }
		public static UInt16 HtonUInt8(Byte[] buf,int offset,byte vl)
        {
            buf[offset] = vl;
            return 1;
        }
        public static UInt16 HtonUInt16(Byte[] buf,int offset,UInt16 vl)
        {
            Byte[] netbuf = BitConverter.GetBytes(vl);
            if (m_IsBigEndian)
            {
                Array.Reverse(netbuf);
            }
            Array.ConstrainedCopy(netbuf, 0, buf, offset, 2);
            return 2;
        }

        public static UInt16 HtonUInt32(Byte[] buf,int offset,UInt32 vl)
        {
            Byte[] netbuf = BitConverter.GetBytes(vl);
            if (m_IsBigEndian)
            {
                Array.Reverse(netbuf);
            }
            Array.ConstrainedCopy(netbuf, 0, buf, offset, 4);
            return 4;
        }
        
        public static UInt16 HtonUInt64(Byte[] buf, int offset, UInt64 vl)
        {
            Byte[] netbuf = BitConverter.GetBytes(vl);
            if (m_IsBigEndian)
            {
                Array.Reverse(netbuf);
            }
            Array.ConstrainedCopy(netbuf, 0, buf, offset, 8);
            return 8;
        }

        public static UInt16 HtonFloat(Byte[] buf, int offset, float vl)
        {
            Byte[] netbuf = BitConverter.GetBytes(vl);
            if (m_IsBigEndian)
            {
                Array.Reverse(netbuf);
            }
            Array.ConstrainedCopy(netbuf, 0, buf, offset, 4);
            return 4;
        }

        public static UInt16 HtonDouble(Byte[] buf, int offset, double vl)
        {
            Byte[] netbuf = BitConverter.GetBytes(vl);
            if (m_IsBigEndian)
            {
                Array.Reverse(netbuf);
            }
            Array.ConstrainedCopy(netbuf, 0, buf, offset, 8);
            return 8;
        }
        #endregion

        public NetPacketHeader()
        {
            UInt16 endian = 0x1200;
            Byte[] buf = BitConverter.GetBytes(endian);
            m_IsBigEndian = (buf[0] != 0);
			m_nIsZip = 0;
        }

        public UInt16 PacketId
        {
            get { return m_nPacketId; }
            set { m_nPacketId = value; }
        }
        public UInt16 BodyLength
        {
            get { return m_nBodyLen; }
            set { m_nBodyLen = value; }
        }
        public byte IsZip
        {
            get { return m_nIsZip; }
            set { m_nIsZip = value; }
        }
        public static UInt16 GetHeadLen()
        {
            return 5;
        }

        public bool ReadHead(Byte[] buf)
        {
//			string str = "";
//			for(int i = 0;i < 5;i++){
//				str += (buf[i] + "_");
//			}
//			Debug.Log("NetPacketHeader Read:" + str);
            //Byte[] netbuf = new Byte[2];
            this.m_nBodyLen = NtohUint16(buf, 0);
            this.m_nPacketId = NtohUint16(buf, 2);
			this.m_nIsZip = buf[4];
            return this.m_nPacketId > 0;
        }

        public bool WriteHead(Byte[] buf)
        {
            HtonUInt16(buf, 0, m_nBodyLen);
            HtonUInt16(buf, 2, m_nPacketId);
			buf[4] = m_nIsZip;
            return true;
        }
		public NetPacketHeader Clone(){
			NetPacketHeader header = new NetPacketHeader();
			header.m_nPacketId = m_nPacketId;
			header.m_nIsZip = m_nIsZip;
			header.m_nBodyLen = m_nBodyLen;
			return header;
		}
    }

    #endregion //NetPacketHeader
	
   
}
