using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security;

namespace AmadeusNetCore
{
    public class SecurityTokenInspector : IClientMessageInspector
    {
        #region Attributes
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Activated { get; set; }
        private string _requestStr = string.Empty;
        private string _responseStr = string.Empty;
        private long _requestLength;

        #endregion

        #region Constructor
        public SecurityTokenInspector(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        #endregion

        #region IClientMessageInspector Members
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            var buffer = reply.CreateBufferedCopy(int.MaxValue);
            reply = buffer.CreateMessage();
            using (Stream mStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(mStream))
                {
                    buffer.CreateMessage().WriteMessage(xmlWriter);
                    xmlWriter.Flush();
                    mStream.Position = 0;

                    var xml = new byte[mStream.Length];
                    mStream.Read(xml, 0, (int)mStream.Length);
                    if (xml[0] != (byte)'<')
                        _responseStr = Encoding.UTF8.GetString(xml, 3, xml.Length - 3);
                    else
                        _responseStr = Encoding.UTF8.GetString(xml, 0, xml.Length);

                    if (mStream.Length > 32000 || _requestLength > 32000)
                    {
                        var fileNameBase = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss.fff") + "_" + Guid.NewGuid();
                        var requestFileName = "Request_" + fileNameBase + ".xml";
                        var responseFileName = "Response_" + fileNameBase + ".xml";
                    }
                };
            };
        }
        
        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            var buffer = request.CreateBufferedCopy(int.MaxValue);
            request = buffer.CreateMessage();
            using Stream mStream = new MemoryStream();
            using var xmlWriter = XmlWriter.Create(mStream);
            buffer.CreateMessage().WriteMessage(xmlWriter);
            xmlWriter.Flush();
            mStream.Position = 0;
            _requestLength = mStream.Length;
            var xml = new byte[mStream.Length];
            mStream.Read(xml, 0, (int)mStream.Length);
            if (xml[0] != (byte)'<')
                _requestStr = Encoding.UTF8.GetString(xml, 3, xml.Length - 3);
            else
                _requestStr = Encoding.UTF8.GetString(xml, 0, xml.Length);
            ;
            ;

            return null;
        }
        #endregion
    }
}