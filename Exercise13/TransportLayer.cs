using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportlaget;

namespace Exercise13
{
    public enum TransSize
    {
        CHKSUMSIZE = 2,
        ACKSIZE = 4
    };

    public enum TransCHKSUM
    {
        CHKSUMHIGH = 0,
        CHKSUMLOW = 1,
        SEQNO = 2,
        TYPE = 3
    };

    public enum TransType
    {
        DATA = 0,
        ACK = 1
    };

    public class TransportLayer
    {
        private LinkLayer link;
        private Checksum checksum = new Checksum();
        private byte[] buffer;
        private int payloadsize = 1000;
        private byte seqNo;
        private byte old_seqNo;
        private int errorCount;
        private const int DEFAULT_SEQNO = 2;
        
        public void Send(byte[] payloadBuffer, int size)
        {
            buffer = new byte[size+4];
            link = new LinkLayer(buffer.Length);
            payloadBuffer.CopyTo(buffer, 4);
            buffer[(int)TransCHKSUM.SEQNO] = seqNo;
            buffer[(int) TransCHKSUM.TYPE] = (int)TransType.DATA;
            checksum.calcChecksum(ref buffer, buffer.Length);

            do
            {
                link.Send(buffer, buffer.Length);
            } while (!ReceiveAck());
        }

        public int Receive(ref byte[] payloadBuffer)
        {
            int size;
            while (true)
            {
                buffer = new byte[payloadsize+4];
                link = new LinkLayer(1000);
                size = link.Receive(ref buffer);
                if (checksum.checkChecksum(buffer, size))
                {
                    Array.Copy(buffer,4,payloadBuffer,0,buffer.Length-4);
                    SendAck(true);
                    break;
                }
                SendAck(false);
            }
            return size;
        }
        
        private void SendAck(bool ackType)
        {
            byte[] ackBuf = new byte[(int)TransSize.ACKSIZE];
            ackBuf[(int)TransCHKSUM.SEQNO] = (byte)
                    (ackType ? (byte)(buffer[(int)TransCHKSUM.SEQNO] + 1) % 2 : (byte)buffer[(int)TransCHKSUM.SEQNO]);
            ackBuf[(int)TransCHKSUM.TYPE] = (byte)(int)TransType.ACK;
            checksum.calcChecksum(ref ackBuf, (int)TransSize.ACKSIZE);

            link.Send(ackBuf, (int)TransSize.ACKSIZE);
        }

        private bool ReceiveAck()
        {
            byte[] buf = new byte[(int)TransSize.ACKSIZE];
            int size = link.Receive(ref buf);
            if (size != (int)TransSize.ACKSIZE) return false;
            if (!checksum.checkChecksum(buf, (int)TransSize.ACKSIZE) ||
                    buf[(int)TransCHKSUM.SEQNO] == seqNo ||
                    buf[(int)TransCHKSUM.TYPE] != (int)TransType.ACK)
                return false;

            seqNo = buf[(int)TransCHKSUM.SEQNO];

            return true;
        }

      }
}
