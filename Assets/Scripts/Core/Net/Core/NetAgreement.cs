using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClientNet
{
    public enum EnumNetAgreement
    {
        ENA_VALUETYPE_INT8 =0,//int8,uint8
        ENA_VALUETYPE_INT16,  //int16,uint16
        ENA_VALUETYPE_INT32,  //int32,uint32
        ENA_VALUETYPE_INT64,  //int64,uint64
        ENA_VALUETYPE_FLOAT,  //float
        ENA_VALUETYPE_DOUBLE, //double
        ENA_VALUETYPE_STRING, //the end is 0
		ENA_VALUETYPE_OBJECT, //object
    }

    #region NetmsgAgreement
    public abstract class NetmsgAgreement
    {
        protected object m_Vlue = null;
        protected UInt16 m_nLen = 0;
        protected bool m_bBigArray = false;

        public bool IsBIgArray
        {
            set { m_bBigArray = value; }
            get { return m_bBigArray; }
        }

        public bool SetValue<T>(T vl, UInt16 len)
        {
            m_Vlue = vl;
            m_nLen = len;
            return true;
        }

        public bool GetValue<T>(out T vl)
        {
				/*if(T == typeof(short).get){
					vl = Convert.ToInt16(m_Vlue);
				}else if(T == typeof(int)){
					vl = Convert.ToInt32(m_Vlue);
				}else if(T == typeof(long)){
					vl = Convert.ToInt64(m_Vlue);
				}else{
            		vl = (T)m_Vlue;
				}*/
			vl = (T)m_Vlue;
            return true;
        }

        public abstract UInt16 Read(Byte[] buf, UInt16 offset, int len);
        public abstract UInt16 Write(Byte[] buf, UInt16 offset, int len);
        public abstract NetmsgAgreement Clone();

        protected UInt16 WriteArrayHead(Byte[] buf, UInt16 offset)
        {
            if (m_bBigArray)
            {
                NetPacketHeader.HtonUInt16(buf, offset, m_nLen);
                return 2;
            }
            else
            {
                buf[offset] = (Byte)m_nLen;
            }
            return 1;
        }
        protected UInt16 ReadArrayHead(Byte[] buf, UInt16 offset)
        {
            if (m_bBigArray)
            {
                m_nLen = NetPacketHeader.NtohUint16(buf, offset);
                return 2;
            }
            else
            {
                m_nLen = buf[offset];
            }
            return 1;
        }
    }
    #endregion
    
    #region NetmsgAgreementString

    class NetmsgAgreementString : NetmsgAgreement
    {
        public override NetmsgAgreement Clone()
        {
            return new NetmsgAgreementString();
        }

        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            //int i = offset;
            //while (buf[i] != 0)
           	//{
           	//    i++;
            //}
			ushort length = NetPacketHeader.NtohUint16(buf, offset);
            string str = Encoding.UTF8.GetString(buf, offset + 2, length);
            m_Vlue = str;
            return (UInt16)(length + 2);
        }

        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            UInt16 nResult = 0;
            if (null != m_Vlue)
            {
				String str = (string)m_Vlue;
				
                Byte[] strbuf = Encoding.UTF8.GetBytes(str);
				NetPacketHeader.HtonUInt16(buf,offset,(ushort)strbuf.Length);
                Array.ConstrainedCopy(strbuf, 0, buf, offset + 2, strbuf.Length);
                nResult = (UInt16)(strbuf.Length + 2);
            }
            return nResult;
        }
    }

    #endregion

    #region NetmsgAgreementint8
    class NetmsgAgreementValueUint8 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            if (len > offset)
            {
                m_Vlue = buf[offset];
                return 1;
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            if (len > offset)
            {
                buf[offset] = (Byte)m_Vlue;
                return 1;
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            return new NetmsgAgreementValueUint8();
        }
    }

    class NetmsgAgreemenArrayUint8 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            Byte[] bufer;
            UInt16 nResult = offset;
            offset += ReadArrayHead(buf, offset);
            if (len >= offset + m_nLen)
            {
                bufer = new Byte[m_nLen];
                Array.ConstrainedCopy(buf, offset, buf, 0, m_nLen);
                m_Vlue = bufer;
                return (UInt16)(offset - nResult + m_nLen);
            }
            return 0;  
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            Byte[] buffer;
            UInt16 nResult = offset;
            offset += WriteArrayHead(buf, offset);
            if (len > offset + m_nLen)
            {
                buffer = (Byte[])m_Vlue;
                Array.ConstrainedCopy(buffer, 0, buf, offset, m_nLen);
                return (UInt16)(offset - nResult + m_nLen);
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            NetmsgAgreement agr = new NetmsgAgreementValueUint8();
            agr.IsBIgArray = m_bBigArray;
            return agr;
        }
    }
    #endregion

    #region NetmsgAgreementint16
    class NetmsgAgreementValueUint16 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset+2)
            {
                m_Vlue = NetPacketHeader.NtohUint16(buf, offset);
                return 2;
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset+2)
            {
                NetPacketHeader.HtonUInt16(buf, offset, (UInt16)m_Vlue);
                return 2;
            }
            return 0;
        }

        public override NetmsgAgreement Clone()
        {
            return new NetmsgAgreementValueUint16();
        }
    }

    class NetmsgAgreementArrayUint16 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            UInt16[] buffer;
            UInt16 nResult = offset;
            UInt16 n = ReadArrayHead(buf, offset);
            if (len > offset + n + m_nLen*2)
            {
                offset += n;
                buffer = new UInt16[m_nLen];
                for (n = 0; n < m_nLen; ++n)
                {
                    buffer[n] = NetPacketHeader.NtohUint16(buf, offset);
                    offset += 2;
                }
                m_Vlue = buffer;
                return (UInt16)(offset - nResult);
            }
            return 0;
        }

        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            UInt16 nResult = offset;
            UInt16 n = WriteArrayHead(buf, offset);
            if (len > offset + n + m_nLen*2)
            {
                offset += n;
                for (n = 0; n < m_nLen; ++n )
                {
                    NetPacketHeader.HtonUInt16(buf, offset, (UInt16)m_Vlue);
                    offset += 2;
                }
                return (UInt16)(offset-nResult);
            }
            return 0;
        }

        public override NetmsgAgreement Clone()
        {
            NetmsgAgreementArrayUint16 p = new NetmsgAgreementArrayUint16();
            p.m_bBigArray = m_bBigArray;
            return p;
        }
    }
    #endregion

    #region NetAgreementint32
    class NetmsgAgreementValueUint32 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset+4)
            {
                m_Vlue = NetPacketHeader.NtohUint32(buf, offset);
                return 4;
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset + 4)
            {
                NetPacketHeader.HtonUInt32(buf, offset, (UInt32)m_Vlue);
                return 4;
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            return new NetmsgAgreementValueUint32();
        }
    }

    class NetmsgAgreementArrayUint32 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            UInt32[] buffer;
            UInt16 nResult = offset;
            UInt16 n = ReadArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (m_nLen*4))
            {
                buffer = new UInt32[m_nLen];
                for (n = 0; n < m_nLen; ++n )
                {
                    buffer[n] = NetPacketHeader.NtohUint32(buf, offset);
                    offset += 4;
                }
                m_Vlue = buffer;
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            UInt32[] buffer = (UInt32[])m_Vlue;
            UInt16 nResult = offset;
            UInt16 n = WriteArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (4 * m_nLen))
            {
                for (n = 0; n < m_nLen; ++n )
                {
                    NetPacketHeader.HtonUInt32(buf, offset, buffer[n]);
                    offset += 4;
                }
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            NetmsgAgreementArrayUint32 p = new NetmsgAgreementArrayUint32();
            p.IsBIgArray = m_bBigArray;
            return p;
        }
    }
    #endregion

    #region NetAgreementint64
    class NetmsgAgreementValueUint64 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset + 8)
            {
                m_Vlue = NetPacketHeader.NtohUint64(buf, offset);
                return 8;
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset + 8)
            {
                NetPacketHeader.HtonUInt64(buf, offset, (UInt64)m_Vlue);
                return 8;
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            return new NetmsgAgreementValueUint64();
        }
    }

    class NetmsgAgreementArrayUint64 : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            UInt64[] buffer;
            UInt16 nResult = offset;
            UInt16 n = ReadArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (m_nLen * 8))
            {
                buffer = new UInt64[m_nLen];
                for (n = 0; n < m_nLen; ++n)
                {
                    buffer[n] = NetPacketHeader.NtohUint64(buf, offset);
                    offset += 8;
                }
                m_Vlue = buffer;
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            UInt64[] buffer = (UInt64[])m_Vlue;
            UInt16 nResult = offset;
            UInt16 n = WriteArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (8 * m_nLen))
            {
                for (n = 0; n < m_nLen; ++n)
                {
                    NetPacketHeader.HtonUInt64(buf, offset, buffer[n]);
                    offset += 8;
                }
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            NetmsgAgreementArrayUint64 p = new NetmsgAgreementArrayUint64();
            p.IsBIgArray = m_bBigArray;
            return p;
        }
    }
    #endregion

    #region NetAgreementfloat
    class NetmsgAgreementValueFloat : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset + 4)
            {
                m_Vlue = NetPacketHeader.NtohFloat(buf, offset);
                return 4;
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset + 4)
            {
                NetPacketHeader.HtonFloat(buf, offset, (float)m_Vlue);
                return 4;
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            return new NetmsgAgreementValueFloat();
        }
    }
    class NetmsgAgreementArrayFloat : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            float[] buffer;
            UInt16 nResult = offset;
            UInt16 n = ReadArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (m_nLen * 4))
            {
                buffer = new float[m_nLen];
                for (n = 0; n < m_nLen; ++n)
                {
                    buffer[n] = NetPacketHeader.NtohFloat(buf, offset);
                    offset += 4;
                }
                m_Vlue = buffer;
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            float[] buffer = (float[])m_Vlue;
            UInt16 nResult = offset;
            UInt16 n = WriteArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (4 * m_nLen))
            {
                for (n = 0; n < m_nLen; ++n)
                {
                    NetPacketHeader.HtonFloat(buf, offset, buffer[n]);
                    offset += 4;
                }
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            NetmsgAgreementArrayFloat p = new NetmsgAgreementArrayFloat();
            p.IsBIgArray = m_bBigArray;
            return p;
        }
    }
    #endregion

    #region NetAgreementdouble
    class NetmsgAgreementValueDouble : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset + 8)
            {
                m_Vlue = NetPacketHeader.NtohDouble(buf, offset);
                return 8;
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            if (len >= offset + 8)
            {
                NetPacketHeader.HtonDouble(buf, offset, (double)m_Vlue);
                return 8;
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            return new NetmsgAgreementValueDouble();
        }
    }
    class NetmsgAgreementArrayDouble : NetmsgAgreement
    {
        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            double[] buffer;
            UInt16 nResult = offset;
            UInt16 n = ReadArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (m_nLen * 8))
            {
                buffer = new double[m_nLen];
                for (n = 0; n < m_nLen; ++n)
                {
                    buffer[n] = NetPacketHeader.NtohDouble(buf, offset);
                    offset += 8;
                }
                m_Vlue = buffer;
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            double[] buffer = (double[])m_Vlue;
            UInt16 nResult = offset;
            UInt16 n = WriteArrayHead(buf, offset);
            offset += n;
            if (len >= offset + (8 * m_nLen))
            {
                for (n = 0; n < m_nLen; ++n)
                {
                    NetPacketHeader.HtonDouble(buf, offset, buffer[n]);
                    offset += 8;
                }
                return (UInt16)(offset - nResult);
            }
            return 0;
        }
        public override NetmsgAgreement Clone()
        {
            NetmsgAgreementArrayDouble p = new NetmsgAgreementArrayDouble();
            p.IsBIgArray = m_bBigArray;
            return p;
        }
    }
    #endregion
	#region NetmsgObject
	//class NetmsgAgreementObject : NetmsgAgreement
    //{
		
	//}
	#endregion
    #region NetAgreementStruct
    
    public class NetmsgStruct
    {
        private UInt16 m_nSize;
        private NetmsgAgreement[] m_Children = null;

        public NetmsgStruct(UInt16 nsize)
        {
            m_nSize = nsize;
            m_Children = new NetmsgAgreement[nsize];
        }

		
        public object GetChildren()
        {
            return m_Children;
        }

        public void RegistChild(int no, EnumNetAgreement etype, bool bArray, bool bBigArray)
        {
            if (no >= 0 && no < m_nSize && null != m_Children)
            {
                m_Children[no] = NetmsgAgreementFactory.Create(etype, bArray, bBigArray);
            }
        }

        public bool SetData<T>(int no, T vl, UInt16 len)
        {
            if (m_Children.Length > no)
            {
                m_Children[no].SetValue<T>(vl, len);
            }
            return true;
        }

        public T GetData<T>(int no)
        {
            T vl;
            if (m_Children.Length > no)
            {
                m_Children[no].GetValue<T>(out vl);
                return vl;
            }
            object obj = null;
            return (T)obj;
        }

        public NetmsgStruct Clone()
        {
            UInt16 i;
            NetmsgStruct node = new NetmsgStruct(m_nSize);
            for (i = 0; i < m_nSize; i++)
            {
                node.m_Children[i] = m_Children[i].Clone();
            }
            return node;
        }
    }

    class NetAgreementStruct : NetmsgAgreement
    {
        private NetmsgStruct m_StructPtr;

        public NetmsgStruct MsgStructPtr
        {
            get { return m_StructPtr; }
            set {}
        }
        public NetAgreementStruct(NetmsgStruct ptr):base()
        {
            m_StructPtr = ptr;
        }

        public override UInt16 Read(Byte[] buf, UInt16 offset, int len)
        {
            NetmsgStruct child;
            UInt16 i;
            UInt16 off = ReadArrayHead(buf, offset);
			NetmsgAgreement []childlist;
            List<NetmsgStruct> mstruct = new List<NetmsgStruct>();
            for (i = 0; i < m_nLen; i++)
            {
                child = m_StructPtr.Clone();
				childlist = (NetmsgAgreement [])child.GetChildren();
                foreach (NetmsgAgreement agree in childlist)
                {
                    off += agree.Read(buf, (UInt16)(offset + off), len);
                }
                mstruct.Add(child);
            }
            m_Vlue = mstruct;
            return off;
        }

        public override UInt16 Write(Byte[] buf, UInt16 offset, int len)
        {
            UInt16 i;
            UInt16 off = WriteArrayHead(buf,offset);
            List<NetmsgStruct> list = (List<NetmsgStruct>)m_Vlue;
			NetmsgAgreement []childlist;
            for (i = 0; i < m_nLen; i++ )
            {
				childlist = (NetmsgAgreement [])list[i].GetChildren();
                foreach (NetmsgAgreement agree in childlist)
                {
                    off += agree.Write(buf, (UInt16)(offset + off), len - off);
                }
            }
            return off;
        }

        public override NetmsgAgreement Clone()
        {
            return new NetAgreementStruct(m_StructPtr);
        }
    }
    
    #endregion

    #region NetmsgAgreementFactory
    class NetmsgAgreementFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="etype"></param>
        /// <param name="type">0 is single value,1 is small array[255] ,2 is big array</param>
        /// <returns></returns>
        public static NetmsgAgreement Create(EnumNetAgreement etype, bool bArray, bool bBigArray)
        {
            switch (etype)
            {
                case EnumNetAgreement.ENA_VALUETYPE_STRING:
                    return new NetmsgAgreementString();
                case EnumNetAgreement.ENA_VALUETYPE_INT8:
                    if (bArray)
                    {
                        NetmsgAgreemenArrayUint8 a = new NetmsgAgreemenArrayUint8();
                        a.IsBIgArray = bBigArray;
                        return a;
                    }
                    else
                    {
                        return new NetmsgAgreementValueUint8();
                    }
                case EnumNetAgreement.ENA_VALUETYPE_INT16:
                    if (bArray)
                    {
                        NetmsgAgreementArrayUint16 a = new NetmsgAgreementArrayUint16();
                        a.IsBIgArray = bBigArray;
                        return a;
                    }
                    else
                    {
                        return new NetmsgAgreementValueUint16();
                    }
                case EnumNetAgreement.ENA_VALUETYPE_INT32:
                    if (bArray)
                    {
                        NetmsgAgreementArrayUint32 a = new NetmsgAgreementArrayUint32();
                        a.IsBIgArray = bBigArray;
                        return a;
                    }
                    else
                    {
                        return new NetmsgAgreementValueUint32();
                    }
                case EnumNetAgreement.ENA_VALUETYPE_INT64:
                    if (bArray)
                    {
                        NetmsgAgreementArrayUint64 a = new NetmsgAgreementArrayUint64();
                        a.IsBIgArray = bBigArray;
                        return a;
                    }
                    else
                    {
                        return new NetmsgAgreementValueUint64();
                    }
                case EnumNetAgreement.ENA_VALUETYPE_FLOAT:
                    if (bArray)
                    {
                        NetmsgAgreementArrayFloat a = new NetmsgAgreementArrayFloat();
                        a.IsBIgArray = bBigArray;
                        return a;
                    }
                    else
                    {
                        return new NetmsgAgreementValueFloat();
                    }
                case EnumNetAgreement.ENA_VALUETYPE_DOUBLE:
                    if (bArray)
                    {
                        NetmsgAgreementArrayDouble a = new NetmsgAgreementArrayDouble();
                        a.IsBIgArray = bBigArray;
                        return a;
                    }
                    else
                    {
                        return new NetmsgAgreementValueDouble();
                    }
            }
            return null;
        }
    }
    #endregion

}
