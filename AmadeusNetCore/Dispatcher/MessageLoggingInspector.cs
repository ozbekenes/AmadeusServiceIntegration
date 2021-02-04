using System;
using System.Collections.Generic;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace AmadeusNetCore.Dispatcher
{
    public class MessageLoggingInspector : IClientMessageInspector
    {

        #region IClientMessageInspector members
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            return null;
        }
        #endregion

        #region IDispatchMessageInspector members
        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            return;
        }
        #endregion
    }
}

