//  Packing.cs
//  Nilsen
//  2015-04-09

using System;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

namespace NetWrapper.Network.Tool
{
    internal class Packing
    {
        /// <summary>
        /// 从缓存流中获取包头
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int GetPacketSize(byte[] buffer, int offset, int end)
        {
            try
            {
                if (end - offset >= GetPacketHeadSize())
                {
                    int bufferSize = BitConverter.ToInt32(buffer, offset);

                    return bufferSize;
                }
            }
            catch (Exception e)
            {
                WriteFiles.WritFile.Log(e);
            }
            return -1;
        }
        public static StreamBuffer GetPacketBuffer(ref StreamBuffer cReceiveBuffer)
        {
            try
            {
                int bufferSize = Packing.GetPacketSize(cReceiveBuffer.ByteBuffer, cReceiveBuffer.ReadIndex, cReceiveBuffer.WriteIndex);
                if (bufferSize > 0 && (bufferSize + GetPacketHeadSize()) <= (cReceiveBuffer.WriteIndex - cReceiveBuffer.ReadIndex))
                {
                    int protocolID = BitConverter.ToInt32(cReceiveBuffer.ByteBuffer, GetPacketHeadSize());
                    byte[] buffer = cReceiveBuffer.Read(bufferSize - sizeof(int), GetPacketHeadSize()+ sizeof(int));           
                    if (buffer != null)
                    {
                        StreamBuffer sb = new StreamBuffer(protocolID, buffer);
                        return sb;
                    }
                }
            }
            catch (Exception e)
            {
                WriteFiles.WritFile.Log(e);
            }

            return null;
        }
        public static ProtocolHeader GetPacketHeader(byte[] buffer, int offset, int end)
        {
            try
            {
                if (end - offset >= ProtocolHeader.HeadLength)
                {
                    ProtocolHeader pHeader = new ProtocolHeader(buffer, offset);
                    return pHeader;
                }
            }
            catch (Exception e)
            {
                WriteFiles.WritFile.Log(e);
            }

            return null;
        }
        public static StreamBuffer GetPacketBufferWithHeader(ref RawStreamBuffer cReceiveBuffer)
        {
            try
            {
                ProtocolHeader pHeader = Packing.GetPacketHeader(cReceiveBuffer.ByteBuffer, cReceiveBuffer.ReadIndex, cReceiveBuffer.WriteIndex);
                if (pHeader != null)
                {
                    if (pHeader.BodyLength > 0)
                    {
                        if((pHeader.BodyLength + ProtocolHeader.HeadLength) <= (cReceiveBuffer.WriteIndex - cReceiveBuffer.ReadIndex))
                        {
                            byte[] buffer = cReceiveBuffer.Read(pHeader.BodyLength, ProtocolHeader.HeadLength);
                            if (buffer != null)
                            {
                                StreamBuffer sb = new StreamBuffer(pHeader, buffer);
                                return sb;
                            }
                        }
                    }
                    else if (pHeader.BodyLength == 0)
                    {
                        cReceiveBuffer.Read(ProtocolHeader.HeadLength);
                        StreamBuffer sb = new StreamBuffer(pHeader.ProtocolID, 0);
                        return sb;     
                    }
                }
            }
            catch (Exception e)
            {
                WriteFiles.WritFile.Log(e);
            }

            return null;
        }

        /// <summary>
        /// 打包加密
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static byte[] PackingEncode(byte[] buffer, int len)
        {
            CEncrypt.Encoding(ref buffer, Packing.GetPacketHeadSize(), CEncrypt.CLIENT_TO_GAMESERVER_KEY, 0, len);
            return buffer;
        }
        /// <summary>
        /// 打包解密
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static byte[] PackingDecode(byte[] buffer, int len)
        {
            CEncrypt.Decoding(ref buffer, Packing.GetPacketHeadSize(), CEncrypt.GAMESERVER_TO_CLIENT_KEY, 0, len);
            return buffer;
        }

        /// <summary>
        /// 获取包头大小
        /// </summary>
        /// <returns></returns>
        public static int GetPacketHeadSize()
        {
            //return System.Runtime.InteropServices.Marshal.SizeOf(Type.GetType("PacketBase"));
            return sizeof(int);
        }
    }
}