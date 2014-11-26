using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise13
{
    class Link
    {
        private SerialPort _serialPort;
        private byte[] _buffer;
        private const byte Delimiter = (byte)'A';

        public Link(int bufsize)
        {
            _buffer = new byte[bufsize*2+2];
            _serialPort = new SerialPort("COM3",115200, Parity.None, 8, StopBits.One);
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();    
            }
        }
        public void Send(byte[] data, int size)
        {
            Slip(data);
            _serialPort.Write(_buffer,0,_buffer.Length);
        }

        private void Slip(byte[] original)
        {
            _buffer[0] = (byte)'A';
            int bufferIndex = 0;
            foreach (var currentByte in original)
            {
                if (currentByte == (byte)'A')
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
            _buffer[++bufferIndex] = (byte) 'A';
        }
    }
}
