//  StreamBuffer.cs
//  Nilsen
//  2015-04-05

using System;
using NetWrapper.Network.Tool;

namespace NetWrapper.Network
{
    public class RawStreamBuffer
    {
        private const byte FALSE_BYTE = 0;
        private const byte TRUE_BYTE = 1;
        private const int DEFAULT_BUFFER_SIZE = 1024; //32 * 256;
        protected byte[] m_lstBuffer;
        internal byte[] ByteBuffer
        {
            get { return this.m_lstBuffer; }
        }
        protected int m_iRead;
        internal int ReadIndex
        {
            get { return this.m_iRead; }
        }
        protected int m_iWrite;
        internal int WriteIndex
        {
            get { return this.m_iWrite; }
        }
        protected int m_iSize;

        protected int m_iCapacity;
        internal int Capacity
        {
            get { return this.m_iCapacity; }
        }

        public RawStreamBuffer()
        {
            this.m_iCapacity = DEFAULT_BUFFER_SIZE;
            byte[] capBuffer = new byte[DEFAULT_BUFFER_SIZE];
            InitBuffer(capBuffer, 0, 0, 0);
        }
        public RawStreamBuffer(int bufferSize)
        {
            this.m_iCapacity = 0;
            byte[] capBuffer = null;
            if (bufferSize > 0)
            {
                m_iCapacity = bufferSize;
                capBuffer = new byte[bufferSize];
            }

            InitBuffer(capBuffer, 0, 0, 0);
        }
        public RawStreamBuffer(byte[] buffer)
        {
            this.m_iCapacity = 0;
            if (buffer != null)
            {
                this.m_iCapacity = buffer.Length;
            }

            InitBuffer(buffer, m_iCapacity, m_iCapacity, 0);
        }

        #region WriteData重载
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private void Write(byte[] buffer, int size)
        {
            if (this.m_iWrite + size > this.m_iCapacity)
            {
                this.m_iCapacity = m_iWrite + size * 2;
                byte[] tempBuffer = new byte[m_iCapacity];
                Array.Copy(this.m_lstBuffer, 0, tempBuffer, 0, this.m_iWrite);
                this.m_lstBuffer = tempBuffer;
            }
            //Array.Reverse(buffer);//大小端转换
            Array.Copy(buffer, 0, this.m_lstBuffer, this.m_iWrite, size);
            WriteSize(size);
        }

        /// <summary>
        /// 增加已写入BUFF数据的写头索引
        /// </summary>
        /// <param name="size"></param>
        private void WriteSize(int size)
        {
            this.m_iWrite += size;
            this.m_iSize += size;

            SizeChanged();
        }

        /// <summary>
        /// 写入int数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(int data)
        {
            int tempData = NetUtil.HostToNetworkOrder(data);
            byte[] res = BitConverter.GetBytes(tempData);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入 uint 数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(uint data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入float数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(float data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入short数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(short data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入ushort数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(ushort data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入long数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(long data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入ulong数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(ulong data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入byte数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(byte data)
        {
            byte[] res = new byte[1];
            res[0] = data;
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入double数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(double data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入bool数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(bool data)
        {
            byte bData = data ? TRUE_BYTE : FALSE_BYTE;
            byte[] res = new byte[1] { bData };
            Write(res, res.Length);
        }
        public void WriteData(byte[] res)
        {
            if (res == null)
            {
                WriteData(0);
            }
            else
            {
                WriteData(res.Length);
                Write(res, res.Length);
            }
        }

        /// <summary>
        /// 写入string数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(string data)
        {
            byte[] strContents = System.Text.Encoding.UTF8.GetBytes(data);
            WriteData(strContents.Length);
            Write(strContents, strContents.Length);
        }

        /// <summary>
        /// 写入char数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(char data)
        {
            byte[] res = BitConverter.GetBytes(data);
            Write(res, res.Length);
        }

        /// <summary>
        /// 写入byte[]数据
        /// </summary>
        /// <param name="data"></param>
        public void WriteBuffer(byte[] data, int size)
        {
            Write(data, size);
        }
        public void WriteBuffer(StreamBuffer bufferData)
        {
            Write(bufferData.m_protocolHeader.GetHeaderStream(), ProtocolHeader.HeadLength);

            if (bufferData.m_protocolHeader.BodyLength > 0)
            {
                Write(bufferData.ByteBuffer, bufferData.m_protocolHeader.BodyLength);
            }
        }
        #endregion

        #region Read 方法集合
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>

        public byte[] Read(int size, int offset = 0)
        {
            int currIndex = ReadSize(size + offset);
            byte[] tmpBuffer = null;
            if (currIndex > -1)
            {
                tmpBuffer = new byte[size];

                Array.Copy(this.m_lstBuffer, currIndex + offset, tmpBuffer, 0, size);
            }

            return tmpBuffer;
        }
        private int ReadSize(int size)
        {
            int retIndex = this.m_iRead;
            if (this.m_iRead + size > this.m_iWrite)
            {
                //ERROR
                WriteFiles.WritFile.LogCallBack(LogerType.ERROR, "Buffer is not have enough content.");
                return -1;
            }
            this.m_iRead += size;
            this.m_iSize -= size;
            SizeChanged();
            return retIndex;
        }

        protected virtual void SizeChanged()
        { }

        /// <summary>
        /// 读取 int
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            int currIndex = ReadSize(sizeof(int));
            int tempData = BitConverter.ToInt32(m_lstBuffer, currIndex);
            int ret = NetUtil.NetworkToHostOrder(tempData);
            return ret;
        }

        /// <summary>
        /// 读取 uint
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            int currIndex = ReadSize(sizeof(uint));
            return BitConverter.ToUInt32(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 short
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            int currIndex = ReadSize(sizeof(short));
            return BitConverter.ToInt16(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 ushort
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            int currIndex = ReadSize(sizeof(ushort));
            return BitConverter.ToUInt16(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 long
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            int currIndex = ReadSize(sizeof(long));
            return BitConverter.ToInt64(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 ulong
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            int currIndex = ReadSize(sizeof(ulong));
            return BitConverter.ToUInt64(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 bool
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            int currIndex = ReadSize(sizeof(byte));
            return m_lstBuffer[currIndex] == TRUE_BYTE;
        }

        /// <summary>
        /// 读取 float
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            int currIndex = ReadSize(sizeof(float));
            return BitConverter.ToSingle(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 double
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            int currIndex = ReadSize(sizeof(double));
            return BitConverter.ToDouble(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 byte
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            int currIndex = ReadSize(sizeof(byte));
            return m_lstBuffer[currIndex];
        }

        /// <summary>
        /// 读取 char
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            int currIndex = ReadSize(sizeof(char));
            return BitConverter.ToChar(m_lstBuffer, currIndex);
        }

        /// <summary>
        /// 读取 byte[]
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            int byteLength = ReadInt32();
            byte[] byteContents = null;
            if (byteLength > 0)
            {
                byteContents = Read(byteLength);
            }
            return byteContents;
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string ReadStr()
        {
            int strLength = ReadInt32();
            int currIndex = ReadSize(strLength);

            return System.Text.Encoding.UTF8.GetString(m_lstBuffer, currIndex, strLength);
        }
        #endregion

        #region Other Func

        private void InitBuffer(byte[] buffer, int size, int writeIndex, int readIndex)
        {
            this.m_lstBuffer = buffer;
            this.m_iSize = size;
            this.m_iRead = readIndex;
            this.m_iWrite = writeIndex;
        }
        internal virtual int GetSendBuffeLength()
        {
            return m_iSize;
        }

        internal int GetSurplusCapacity()
        {
            return m_iCapacity - m_iWrite; ;
        }

        /// <summary>
        /// 格式化缓存
        /// </summary>
        internal void FormationBuffer()
        {
            if (this.m_iSize > 0)
            {
                Array.Copy(this.m_lstBuffer, this.m_iRead, this.m_lstBuffer, 0, this.m_iSize);
            }
            this.m_iRead = 0;
            this.m_iWrite = this.m_iSize;
            if(this.m_iCapacity > 0)
            {
                Array.Clear(this.m_lstBuffer, this.m_iWrite, this.m_iCapacity - this.m_iWrite);
            }
        }

        internal void ClearBuffer()
        {
            if(this.m_iCapacity > 0)
            {
                Array.Clear(this.m_lstBuffer, 0, m_iCapacity);
            }
            this.m_iSize = 0;
            this.m_iRead = 0;
            this.m_iWrite = 0;

            SizeChanged();
        }

        internal virtual byte[] GetCopyOfContent()
        {
            byte[] tmpBuffer = null;
            if (this.m_iSize > 0)
            {
                tmpBuffer = new byte[this.m_iSize];
                System.Array.Copy(this.m_lstBuffer, m_iRead, tmpBuffer, 0, m_iSize);
            }

            return tmpBuffer;
        }

        internal virtual int GetSendBuffer(out byte[] sendBuffer)
        {
            sendBuffer = null;
            if (m_iSize > 0)
            {
                sendBuffer = m_lstBuffer;
            }
            return m_iSize;
        }

        public void WriteRawData(byte[] buf)
        {
            if (buf != null)
            {
                Write(buf, buf.Length);
            }
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <returns></returns>
        public byte[] GetSendBuffer()
        {
            byte[] res = Read(this.m_iSize);
            return res;
        }
        #endregion
    }

    /// <summary>
    /// 缓冲流
    /// </summary>
    public class StreamBuffer : RawStreamBuffer
    {
        public ProtocolHeader m_protocolHeader;

        public StreamBuffer(int protocolID)
            : base()
        {
            InitStreamBuffer(new ProtocolHeader(protocolID));
        }
        public StreamBuffer(int protocolID, int bufferSize)
            : base(bufferSize)
        {
            InitStreamBuffer(new ProtocolHeader(protocolID));
        }
        public StreamBuffer(int protocolID, byte[] buffer)
            : base(buffer)
        {
            InitStreamBuffer(new ProtocolHeader(protocolID));
        }
        internal StreamBuffer(ProtocolHeader pHeader, byte[] buffer)
            : base(buffer)
        {
            InitStreamBuffer(pHeader);
        }

        private void InitStreamBuffer(ProtocolHeader pHeader)
        {
            m_protocolHeader = pHeader;
            m_protocolHeader.BodyLength = m_iSize;
        }

        internal override int GetSendBuffeLength()
        {
            return ProtocolHeader.HeadLength + m_protocolHeader.BodyLength;
        }

        protected override void SizeChanged()
        {
            this.m_protocolHeader.BodyLength = m_iSize;
        }

        internal override byte[] GetCopyOfContent()
        {
            byte[] tmpBuffer = null;
            if (m_iSize == 0)
            {
                tmpBuffer = new byte[ProtocolHeader.HeadLength];
                m_protocolHeader.BodyLength = 0;
                Array.Copy(m_protocolHeader.GetHeaderStream(), 0, tmpBuffer, 0, ProtocolHeader.HeadLength);
            }
            else if (m_iSize > 0)
            {
                tmpBuffer = new byte[ProtocolHeader.HeadLength + m_iSize];
                m_protocolHeader.BodyLength = m_iSize;
                Array.Copy(m_protocolHeader.GetHeaderStream(), 0, tmpBuffer, 0, ProtocolHeader.HeadLength);
                Array.Copy(m_lstBuffer, m_iRead, tmpBuffer, ProtocolHeader.HeadLength, m_iSize);
            }

            return tmpBuffer;
        }

        internal override int GetSendBuffer(out byte[] sendBuffer)
        {
            sendBuffer = GetCopyOfContent();

            return sendBuffer == null ? 0 : sendBuffer.Length;
        }
    }
}