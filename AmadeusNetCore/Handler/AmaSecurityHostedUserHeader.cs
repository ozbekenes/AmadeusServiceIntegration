using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace AmadeusNetCore.Handler
{
    public class AmaSecurityHostedUserHeader 
        //: MessageHeader
    {

        //public override string Name
        //{
        //    get { return "AMA_SecurityHostedUser"; }
        //}

        //public override string Namespace
        //{
        //    get { return "http://xml.amadeus.com/2010/06/Security_v1"; }
        //}

        //protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        //{
        //    writer.WriteStartElement(Name, Namespace);
        //    writer.WriteXmlnsAttribute("", Namespace);
        //    writer.WriteAttributeString("AgentDutyCode", "SU");
        //    writer.WriteAttributeString("RequestorType", "U");
        //    writer.WriteAttributeString("PseudoCityCode", "EZSLL28AA");
        //    writer.WriteAttributeString("POS_Type", "1");
        //}

        //protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        //{
        //}
    }
}
