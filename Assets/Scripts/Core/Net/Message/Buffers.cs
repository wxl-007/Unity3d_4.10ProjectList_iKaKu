using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    /// <summary>
    /// 数据包格式化类.
    /// </summary>
    public static class Buffers
    {
        #region static public Byte[] MergeBytes(params Byte[][] args) 将1个2维数据包整合成以个一维数据包.
        /// <summary>
        /// 将1个2维数据包整合成以个一维数据包.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static public Byte[] MergeBytes(params Byte[][] args)
        {
            Int32 length = 0;
            foreach (byte[] tempbyte in args)
            {
                length += tempbyte.Length;  //计算数据包总长度.
            }

            Byte[] bytes = new Byte[length]; //建立新的数据包.

            Int32 tempLength = 0;

            foreach (byte[] tempByte in args)
            {
                tempByte.CopyTo(bytes, tempLength);
                tempLength += tempByte.Length;  //复制数据包到新数据包.
            }

            return bytes;

        }

        #endregion


        #region static public Byte[] GetSocketBytes(Int16 data) 将一个16位整形转换成一个BYTE[]2字节.
        /// <summary>
        /// 将一个16位整形转换成一个BYTE[]2字节.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Int16 data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(UInt16 data) 将一个16位整形转换成一个BYTE[]2字节.
        /// <summary>
        /// 将一个16位整形转换成一个BYTE[]2字节.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(UInt16 data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(Int32 data) 将一个32位整形转换成一个BYTE[]4字节..
        /// <summary>
        /// 将一个32位整形转换成一个BYTE[]4字节.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Int32 data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(UInt32 data) 将一个32位整形转换成一个BYTE[]4字节.
        /// <summary>
        /// 将一个32位整形转换成一个BYTE[]4字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(UInt32 data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(Int64 data) 将一个64位整型转换成以个BYTE[] 8字节.
        /// <summary>
        /// 将一个64位整型转换成以个BYTE[] 8字节.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Int64 data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(UInt64 data) 将一个64位整型转换成以个BYTE[] 8字节.
        /// <summary>
        /// 将一个64位整型转换成以个BYTE[] 8字节.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(UInt64 data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(float data) 将一个Float浮点数转换成以个BYTE[] 4字节.
        /// <summary>
        /// 将一个Float浮点数转换成以个BYTE[] 4字节.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(float data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(double data) 将一个Double浮点数转换成以个BYTE[] 8字节.
        /// <summary>
        /// 将一个Double浮点数转换成以个BYTE[] 8字节.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(double data)
        {
            return BitConverter.GetBytes(data);
        }

        #endregion


        #region static public Byte[] GetSocketBytes(Char data) 将一个 1位CHAR转换成1位的BYTE.
        /// <summary>
        /// 将一个 1位CHAR转换成1位的BYTE.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Char data)
        {
            Byte[] bytes = new Byte[] { (Byte)data };
            return bytes;
        }

        #endregion


        #region static public Byte[] GetSocketBytes(Byte[] data) 将一个BYTE[]数据包添加首位长度.
        /// <summary>
        /// 将一个BYTE[]数据包添加首位长度.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Byte[] data)
        {
            return MergeBytes(
                GetSocketBytes(data.Length),
                data
                );
        }

        #endregion


        #region static public Byte[] GetSocketBytesInt16(String data) 将一个字符串转换成BYTE[]，BYTE[]的首位是字符串的2字节长度.
        /// <summary>
        /// 将一个字符串转换成BYTE[]，BYTE[]的首位是字符串的2字节长度.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytesInt16(String data)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes(data);

            return MergeBytes(
                GetSocketBytes((short)bytes.Length),
                bytes
                );
        }

        #endregion


        #region static public Byte[] GetSocketBytesByInt32(String data) 将一个字符串转换成BYTE[]，BYTE[]的首位是字符串的4字节长度
        /// <summary>
        /// 将一个字符串转换成BYTE[]，BYTE[]的首位是字符串的4字节长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytesByInt32(String data)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes(data);

            return MergeBytes(
                GetSocketBytes(bytes.Length),
                bytes
                );
        }

        #endregion


        #region static public Byte[] GetSocketBytes(DateTime data) 将一个DATATIME转换成为BYTE[]数组
        /// <summary>
        /// 将一个DATATIME转换成为BYTE[]数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(DateTime data)
        {
            return GetSocketBytesByInt32(data.ToString());
        }

        #endregion
    }


    /// <summary>
    /// 读取字节
    /// </summary>
    public class ByteBuffer
    {
        /// <summary>
        /// 当前索引位置
        /// </summary>
        private ushort Current;

        /// <summary>
        /// 数据
        /// </summary>
        private byte[] Data;

        /// <summary>
        /// 数据总长度
        /// </summary>
        public int Length { get; set; }



        #region ReadBytes(Byte[] data) 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public ByteBuffer(Byte[] data)
        {
            Data = data;
            this.Length = Data.Length;
            Current = 0;
        }
        public ByteBuffer()
        {
            Data = new byte[1024];
            this.Length = Data.Length;
            Current = 0;
        }
        #endregion
        public ushort GetCurrent()
        {
            return Current;
        }

        #region void Reset() 重置读取索引
        /// <summary>
        /// 重置读取索引
        /// </summary>
        public void Reset()
        {
            Current = 0;
        }
        public byte[] GetData()
        {
            return Data;
        }
        #endregion


        #region bool IsCanReadAgainInt32() 是否可以再读出一个Int32数字
        /// <summary>
        /// 是否可以再读出一个Int32数字
        /// </summary>
        /// <returns></returns>
        public bool IsCanReadAgainInt32()
        {
            if (Data.Length - Current < 4)
                return false;

            int iResult = BitConverter.ToInt32(Data, Current);

            if (iResult > 0)
                return true;
            else
                return false;
        }

        #endregion


        #region bool ReadByte(out byte values) 读取内存流中的一位
        /// <summary>
        /// 读取内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadByte(out byte values)
        {
            if (Data.Length - Current < 1)
            {
                values = 0;
                return false;
            }

            //try
            //{
            values = (byte)Data[Current];
            Current++;
            return true;
            //}
            //catch
            //{
            //    values = 0;
            //    return false;
            //}
        }

        #endregion


        #region bool ReadInt16(out short values) 读取内存流中的头2位并转换成整型（有符号）
        /// <summary>
        /// 读取内存流中的头2位并转换成整型（有符号）
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadInt16(out short values)
        {
            if (Data.Length - Current < 2)
            {
                values = 0;
                return false;
            }

            //try
            //{
            values = (short)GameClientNet.NetPacketHeader.NtohUint16(Data, Current);
            //BitConverter.ToInt16(Data, Current);
            Current += 2;
            return true;
            //}
            //catch
            //{
            //    values = 0;
            //    return false;
            //}
        }

        #endregion


        #region bool ReadUInt16(out ushort values) 读取内存流中的头2位并转换成整型（无符号）
        /// <summary>
        /// 读取内存流中的头2位并转换成整型（无符号）
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadUInt16(out ushort values)
        {
            if (Data.Length - Current < 2)
            {
                values = 0;
                return false;
            }

            //try
            //{
            values = GameClientNet.NetPacketHeader.NtohUint16(Data, Current);
            //BitConverter.ToUInt16(Data, Current);
            Current += 2;
            return true;
            //}
            //catch
            //{
            //    values = 0;
            //    return false;
            //}
        }

        #endregion


        #region bool ReadInt32(out int values) 读取内存流中的头4位并转换成整型（有符号）
        /// <summary>
        /// 读取内存流中的头4位并转换成整型（有符号）
        /// </summary>
        /// <param name="values">内存流</param>
        /// <returns></returns>
        public bool ReadInt32(out int values)
        {
            if (Data.Length - Current < 4)
            {
                values = 0;
                return false;
            }

            //try
            //{
            values = (int)GameClientNet.NetPacketHeader.NtohUint32(Data, Current);
            //BitConverter.ToInt32(Data, Current);
            Current += 4;
            return true;
            //}
            //catch
            //{
            //    values = 0;
            //    return false;
            //}
        }

        #endregion


        #region bool ReadUInt32(out UInt32 values) 读取内存流中的头4位并转换成整型（无符号）
        /// <summary>
        /// 读取内存流中的头4位并转换成整型 （无符号）
        /// </summary>
        /// <param name="values">内存流</param>
        /// <returns></returns>
        public bool ReadUInt32(out UInt32 values)
        {
            if (Data.Length - Current < 4)
            {
                values = 0;
                return false;
            }

            //try
            //{
            values = GameClientNet.NetPacketHeader.NtohUint32(Data, Current);
            //BitConverter.ToUInt32(Data, Current);
            Current += 4;
            return true;
            //}
            //catch
            //{
            //    values = 0;
            //    return false;
            //}
        }

        #endregion


        #region bool ReadInt64(out Int64 values) 读取内存流中的头4位并转换成整型（有符号）
        /// <summary>
        /// 读取内存流中的头8位并转换成整型（有符号）
        /// </summary>
        /// <param name="values">内存流</param>
        /// <returns></returns>
        public bool ReadInt64(out Int64 values)
        {
            if (Data.Length - Current < 8)
            {
                values = 0;
                return false;
            }

            //try
            //{
            values = (Int64)GameClientNet.NetPacketHeader.NtohUint64(Data, Current);
            //BitConverter.ToInt64(Data, Current);
            Current += 8;
            return true;
            //}
            //catch
            //{
            //    values = 0;
            //    return false;
            //}
        }

        #endregion


        #region bool ReadUInt64(out UInt64 values) 读取内存流中的头8位并转换成整型（无符号）
        /// <summary>
        /// 读取内存流中的头8位并转换成整型 （无符号）
        /// </summary>
        /// <param name="values">内存流</param>
        /// <returns></returns>
        public bool ReadUInt64(out UInt64 values)
        {
            if (Data.Length - Current < 8)
            {
                values = 0;
                return false;
            }

            //try
            //{
            values = GameClientNet.NetPacketHeader.NtohUint64(Data, Current);
            //BitConverter.ToUInt64(Data, Current);
            Current += 8;
            return true;
            //}
            //catch
            //{
            //    values = 0;
            //    return false;
            //}
        }

        #endregion


        #region bool ReadFloat(out float values) 读取内存流中的Float型 4位
        /// <summary>
        /// 读取内存流中的Float型 4位
        /// </summary>
        /// <param name="values">内存流</param>
        /// <returns></returns>
        public bool ReadFloat(out float values)
        {
            if (Data.Length - Current < 4)
            {
                values = 0;
                return false;
            }

            values = GameClientNet.NetPacketHeader.NtohFloat(Data, Current);
            //BitConverter.ToSingle(Data, Current);
            Current += 4;
            return true;
        }

        #endregion


        #region bool ReadDouble(out double values) 读取内存流中的Double型 8位
        /// <summary>
        /// 读取内存流中的Double型 8位
        /// </summary>
        /// <param name="values">内存流</param>
        /// <returns></returns>
        public bool ReadDouble(out double values)
        {
            if (Data.Length - Current < 8)
            {
                values = 0;
                return false;
            }

            values = GameClientNet.NetPacketHeader.NtohDouble(Data, Current);
            //BitConverter.ToDouble(Data, Current);
            Current += 8;
            return true;
        }

        #endregion


        #region bool ReadStringByInt16(out string values) 读取内存流中一段字符串(字符前端先读取2字节数据长度)
        /// <summary>
        /// 读取内存流中一段字符串(字符前端先读取2字节数据长度)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadStringByInt16(out string values)
        {

            ushort lengt;
            values = GameClientNet.NetPacketHeader.NtohString(Data, Current, out lengt);
            Current += lengt;
            return true;
            //            try
            //            {
            //                if (ReadInt16(out lengt))
            //                {
            //
            //                    Byte[] buf = new Byte[lengt];
            //
            //                    Array.Copy(Data, Current, buf, 0, buf.Length);
            //
            //                    values = Encoding.UTF8.GetString(buf, 0, buf.Length);
            //
            //                    Current += lengt;
            //					
            //                    return true;
            //
            //                }
            //                else
            //                {
            //                    values = "";
            //                    return false;
            //                }
            //            }
            //            catch
            //            {
            //                values = "";
            //                return false;
            //            }

        }

        #endregion


        #region bool ReadStringByInt32(out string values) 读取内存流中一段字符串(字符前端先读取2字节数据长度)
        /// <summary>
        /// 读取内存流中一段字符串(字符前端先读取2字节数据长度)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        //        public bool ReadStringByInt32(out string values)
        //        {
        //            int lengt;
        //            try
        //            {
        //                if (ReadInt32(out lengt))
        //                {
        //
        //                    Byte[] buf = new Byte[lengt];
        //
        //                    Array.Copy(Data, Current, buf, 0, buf.Length);
        //
        //                    values = Encoding.UTF8.GetString(buf, 0, buf.Length);
        //
        //                    Current += lengt;
        //
        //                    return true;
        //
        //                }
        //                else
        //                {
        //                    values = "";
        //                    return false;
        //                }
        //            }
        //            catch
        //            {
        //                values = "";
        //                return false;
        //            }
        //
        //        }

        #endregion


        #region bool ReadByteArrayByInt16(out byte[] values) 读取内存流中一段数据(字节前端先读取2字节数据长度)
        /// <summary>
        /// 读取内存流中一段数据(字节前端先读取2字节数据长度)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        //        public bool ReadByteArrayByInt16(out byte[] values)
        //        {
        //            short lengt;
        //            try
        //            {
        //                if (ReadInt16(out lengt))
        //                {
        //                    values = new Byte[lengt];
        //                    Array.Copy(Data, Current, values, 0, values.Length);
        //                    Current += lengt;
        //                    return true;
        //
        //                }
        //                else
        //                {
        //                    values = null;
        //                    return false;
        //                }
        //            }
        //            catch
        //            {
        //                values = null;
        //                return false;
        //            }
        //
        //        }

        #endregion


        #region bool ReadByteArrayByInt32(out byte[] values) 读取内存流中一段数据(字节前端先读取4字节数据长度)
        /// <summary>
        /// 读取内存流中一段数据(字节前端先读取4字节数据长度)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        //        public bool ReadByteArrayByInt32(out byte[] values)
        //        {
        //            int lengt;
        //            try
        //            {
        //                if (ReadInt32(out lengt))
        //                {
        //                    values = new Byte[lengt];
        //                    Array.Copy(Data, Current, values, 0, values.Length);
        //                    Current += lengt;
        //                    return true;
        //
        //                }
        //                else
        //                {
        //                    values = null;
        //                    return false;
        //                }
        //            }
        //            catch
        //            {
        //                values = null;
        //                return false;
        //            }
        //
        //        }

        #endregion


        #region bool ReadByteArray(out byte[] values, int ReadLength) 读取内存流中一段数据(从当前位置开始，读取指定长度)
        /// <summary>
        /// 读取内存流中一段数据(从当前位置开始，读取指定长度)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadByteArray(out byte[] values, int ReadLength)
        {
            try
            {
                values = new Byte[ReadLength];
                if (GameClientNet.NetPacketHeader.IsBigEndian)
                {
                    Array.Reverse(Data, Current, ReadLength);
                }
                Array.Copy(Data, Current, values, 0, values.Length);
                Current += (ushort)ReadLength;
                return true;
            }
            catch
            {
                values = null;
                return false;
            }

        }

        #endregion


        #region bool WriteByte(byte values) 写入内存流中的一位
        /// <summary>
        /// 写入内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteByte(byte values)
        {
            if (Data.Length - Current < 1)
            {
                return false;
            }
            Data[Current] = values;
            Current++;
            return true;
        }
        #endregion

        #region bool WriteInt16(short values) 写入内存流中的一位
        /// <summary>
        /// 写入内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteInt16(short values)
        {
            if (Data.Length - Current < 1)
            {
                return false;
            }
            Current += GameClientNet.NetPacketHeader.HtonUInt16(Data, Current, (ushort)values);
            return true;
        }
        #endregion
        #region bool WriteUInt16(ushort values) 写入内存流中的一位
        /// <summary>
        /// 写入内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteUInt16(ushort values)
        {
            if (Data.Length - Current < 1)
            {
                return false;
            }
            Current += GameClientNet.NetPacketHeader.HtonUInt16(Data, Current, values);
            return true;
        }
        #endregion
        #region bool WriteInt32(int values) 写入内存流中的一位
        /// <summary>
        /// 写入内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteInt32(int values)
        {
            if (Data.Length - Current < 1)
            {
                return false;
            }
            Current += GameClientNet.NetPacketHeader.HtonUInt32(Data, Current, (uint)values);
            return true;
        }
        #endregion
        #region bool WriteUInt32(uint values) 写入内存流中的一位
        /// <summary>
        /// 写入内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteUInt32(uint values)
        {
            if (Data.Length - Current < 1)
            {
                return false;
            }
            Current += GameClientNet.NetPacketHeader.HtonUInt32(Data, Current, values);
            return true;
        }
        #endregion

        #region bool WriteInt64(long values) 写入内存流中的一位
        /// <summary>
        /// 写入内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteInt64(long values)
        {
            if (Data.Length - Current < 1)
            {
                return false;
            }
            Current += GameClientNet.NetPacketHeader.HtonUInt64(Data, Current, (ulong)values);
            return true;
        }
        #endregion
        #region bool WriteUInt64(ulong values) 写入内存流中的一位
        /// <summary>
        /// 写入内存流中的一位
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteUInt64(ulong values)
        {
            if (Data.Length - Current < 1)
            {
                return false;
            }
            Current += GameClientNet.NetPacketHeader.HtonUInt64(Data, Current, values);
            return true;
        }
        #endregion

        #region bool WriteStringByInt16(string values) 写入内存流中一段字符串(字符前端先写入2字节数据长度)
        /// <summary>
        /// 写入内存流中一段字符串(字符前端先写入2字节数据长度)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteStringByInt16(string values)
        {
            Current += GameClientNet.NetPacketHeader.HtonString(Data, Current, values);
            return true;
        }
        #endregion
    }





    /// <summary>
    /// 内存流读取类
    /// </summary>
    /*public static class PacketReads
    {
        #region static Int32 ReadInt32(MemoryStream ms) 读取内存流中的头4位并转换成整型
        /// <summary>
        /// 读取内存流中的头4位并转换成整型
        /// </summary>
        /// <param name="ms">内存流</param>
        /// <returns></returns>
        public static Int32 ReadInt32(MemoryStream ms)
        {
            Byte[] buf = new Byte[4];
            ms.Read(buf, 0, 4);
            return BitConverter.ToInt32(buf, 0);
        }

        #endregion


        #region static Int16 ReadInt16(MemoryStream ms) 读取内存流中的头2位并转换成整型
        /// <summary>
        /// 读取内存流中的头2位并转换成整型
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static Int16 ReadInt16(MemoryStream ms)
        {
            Byte[] buf = new Byte[2];
            ms.Read(buf, 0, 2);
            return BitConverter.ToInt16(buf, 0);
        }

        #endregion


        #region static Char ReadChar(MemoryStream ms) 读取内存流中的首位
        /// <summary>
        /// 读取内存流中的首位
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static Char ReadChar(MemoryStream ms)
        {
            return (Char)ms.ReadByte();
        }

        #endregion


        #region static String ReadStringByInt32(MemoryStream ms) 读取内存流中一段字符串（字节前端先读取4字节数据长度）
        /// <summary>
        /// 读取内存流中一段字符串（字节前端先读取4字节数据长度）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static String ReadStringByInt32(MemoryStream ms)
        {
            Byte[] buf = new Byte[ReadInt32(ms)];
            ms.Read(buf, 0, buf.Length);
            return Encoding.Default.GetString(buf);
        }

        #endregion


        #region static Byte[] ReadByteByInt32(MemoryStream ms) 读取内存流中一段数据（字节前端先读取4字节数据长度）
        /// <summary>
        /// 读取内存流中一段数据（字节前端先读取4字节数据长度）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static Byte[] ReadByteByInt32(MemoryStream ms)
        {
            Byte[] buf = new Byte[ReadInt32(ms)];
            ms.Read(buf, 0, buf.Length);
            return buf;
        }

        #endregion

    }*/
}
