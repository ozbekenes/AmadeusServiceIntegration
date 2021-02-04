using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace AmadeusNetCore.Handler
{
    public class SoapSecurityHeader : MessageHeader
    {
        private readonly string _password, _username, _nonce;
        private readonly string _createdDate;

        public SoapSecurityHeader(string username, string password, string nonce, string createdDate)
        {
            _password = password;
            _username = username;
            _nonce = nonce;
            _createdDate = createdDate;
        }

        public override string Name
        {
            get { return "Security"; }
        }

        public override string Namespace
        {
            get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
        }

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("oas", Name, Namespace);
            writer.WriteXmlnsAttribute("oas", Namespace);
            writer.WriteXmlnsAttribute("oas1", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
        }

            
        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("oas", "UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteAttributeString("oas1", "Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", "UsernameToken-1");

            writer.WriteStartElement("oas", "Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteValue(_username);
            writer.WriteEndElement();

            writer.WriteStartElement("oas", "Password", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest");
            writer.WriteValue(_password);
            writer.WriteEndElement();

            writer.WriteStartElement("oas", "Nonce", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            writer.WriteValue(_nonce);
            writer.WriteEndElement();

            writer.WriteStartElement("oas1", "Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            writer.WriteValue(_createdDate);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
