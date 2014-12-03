using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise13
{
    class LinkLayer
    {
        private SerialPort _serialPort;
        private byte[] _buffer;
        private const byte Delimiter = (byte)'A';

        public LinkLayer(int bufferSize)
        {
            _serialPort = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);

            if (!_serialPort.IsOpen)
                _serialPort.Open();
        }

        public void Send(byte[] data, int size)
        {
            _buffer = new byte[(size * 2) + 2];   
            EncodeSlip(data);
            _serialPort.Write(_buffer, 0, _buffer.Length);
        }

        public int Receive(ref byte[] buf)
        {
            _buffer = new byte[(buf.Length * 2) + 2];   
            int counter = 0;

            while (_buffer[0] != Delimiter)
            {
                _buffer[0] = (byte)_serialPort.ReadByte();
            }

            do
            {
                _buffer[++counter] = (byte) _serialPort.ReadByte();
            } while (_buffer[counter] != Delimiter);

            DecodeSLIP(ref buf);
            return buf.Length;
        }

        private void EncodeSlip(byte[] original)
        {
            _buffer[0] = Delimiter;
            int bufferIndex = 0;
            foreach (var currentByte in original)
            {
                if (currentByte == Delimiter)
                {
                    _buffer[++bufferIndex] = (byte)'B';
                    _buffer[++bufferIndex] = (byte)'C';
                }
                else if (currentByte == (byte)'B')
                {
                    _buffer[++bufferIndex] = (byte)'B';
                    _buffer[++bufferIndex] = (byte)'D';
                }
                else
                {
                    _buffer[++bufferIndex] = currentByte;
                }
            }
            _buffer[++bufferIndex] = Delimiter;
        }

        private void DecodeSLIP(ref byte[] buf)
        {
            int delimiterCount = 0;
            int i = 0;
            int bufIndex = 0;

            while (delimiterCount < 2)
            {
                if (_buffer[i] == Delimiter)
                    delimiterCount++;
                else if (_buffer[i] == (byte)'B')
                {
                    if (_buffer[i + 1] == 'C')
                        buf[bufIndex++] = Delimiter;
                    else if (_buffer[i + 1] == 'D')
                        buf[bufIndex++] = (byte)'B';
                    i++;
                }
                else
                    buf[bufIndex++] = _buffer[i];
                i++;
            }

        }
    }
}







