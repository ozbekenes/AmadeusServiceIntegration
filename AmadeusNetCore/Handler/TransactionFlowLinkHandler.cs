using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Amadeus;

namespace AmadeusNetCore
{
    public class TransactionFlowLinkHandler : HeaderHandler
    {
        #region Attributes
        public TransactionFlowLinkType Link { get; set; }
        private String UniqueID { get; set; }
        private String ServerID { get; set; }
        #endregion

        public enum TransactionFlowLinkAction { None, New, FollowUp, Reset };

        #region Constructor
        public TransactionFlowLinkHandler(AmadeusWebServicesPTClient client)
            : base(client)
        {
            this.Link = null;
        }
        #endregion

        #region Members
        public void handleLinkAction(TransactionFlowLinkAction linkAction)
        {
            switch (linkAction)
            {
                case TransactionFlowLinkAction.New:
                    newLink();
                    break;
                case TransactionFlowLinkAction.FollowUp:
                    followUpLink();
                    break;
                case TransactionFlowLinkAction.Reset:
                    resetLink();
                    break;
                case TransactionFlowLinkAction.None:
                    newLink();
                    break;
                default:
                    break;
            }

        }

        public void AfterReply(TransactionFlowLinkAction requestedAction)
        {
            switch (requestedAction)
            {
                case TransactionFlowLinkAction.New:
                    Trace.Assert(Link.Receiver != null);
                    Trace.Assert(Link.Consumer.UniqueID.Equals(this.UniqueID));
                    this.ServerID = Link.Receiver.ServerID;
                    break;
                case TransactionFlowLinkAction.FollowUp:
                    Trace.Assert(Link.Receiver.ServerID.Equals(this.ServerID));
                    Trace.Assert(Link.Consumer.UniqueID.Equals(this.UniqueID));
                    break;
                case TransactionFlowLinkAction.Reset:
                    Trace.Assert(Link.Receiver != null);
                    Trace.Assert(!Link.Receiver.ServerID.Equals(this.ServerID));
                    Trace.Assert(Link.Consumer.UniqueID.Equals(this.UniqueID));
                    this.ServerID = Link.Receiver.ServerID;
                    break;
                case TransactionFlowLinkAction.None:
                    Trace.Assert(Link == null);
                    break;
                default:
                    break;
            }
        }

        private void newLink()
        {
            Link = new TransactionFlowLinkType();
            this.Link.Consumer = new ConsumerType();
            this.UniqueID = System.Guid.NewGuid().ToString();
            this.Link.Consumer.UniqueID = UniqueID;
        }

        private void followUpLink()
        {
            // Nothing to do
        }

        private void resetLink()
        {
            this.Link.Receiver = null;
        }

        private void noLink()
        {
            this.Link = null;
        }
        #endregion
    }
}
