using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace AmadeusNetCore.Handler
{
    internal class CustomTextMessageEncoder : System.ServiceModel.Channels.MessageEncoder
    {
        private CustomTextMessageEncoderFactory _factory;
        private XmlWriterSettings _writerSettings;
        private string _contentType;

        public CustomTextMessageEncoder(CustomTextMessageEncoderFactory factory)
        {
            _factory = factory;

            _writerSettings = new XmlWriterSettings();
            _writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
            _contentType = string.Format("{0}; charset={1}",
                _factory.MediaType, _writerSettings.Encoding.WebName);
        }

        public override string ContentType
        {
            get
            {
                return _contentType;
            }
        }

        public override string MediaType
        {
            get
            {
                return _factory.MediaType;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return _factory.MessageVersion;
            }
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            var reader = XmlReader.Create(stream);
            return Message.CreateMessage(reader, maxSizeOfHeaders, MessageVersion);
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            var msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            var stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            using (var writer = XmlWriter.Create(stream, _writerSettings))
            {
                message.WriteMessage(writer);
            }
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            ArraySegment<byte> messageBuffer;
            byte[] writeBuffer = null;

            int messageLength;
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, _writerSettings))
                {
                    message.WriteMessage(writer);
                }

                // TryGetBuffer is the preferred path but requires 4.6
                //stream.TryGetBuffer(out messageBuffer);
                writeBuffer = stream.ToArray();
                messageBuffer = new ArraySegment<byte>(writeBuffer);

                messageLength = (int)stream.Position;
            }

            var totalLength = messageLength + messageOffset;
            var totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBuffer.Array, 0, totalBytes, messageOffset, messageLength);

            var byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }
    }
}
