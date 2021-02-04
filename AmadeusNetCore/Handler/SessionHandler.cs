using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Amadeus;

namespace AmadeusNetCore
{
    public class SessionHandler : HeaderHandler
    {
        #region Attributes
        public Session Session { get; set; }
        private SecurityHandler hSecurity;
        #endregion

        public enum TransactionStatusCode { None, Start, Continue, End };

        #region Constructor
        public SessionHandler(AmadeusWebServicesPTClient client, SecurityHandler h)
            : base(client)
        {
            this.Session = null;
            hSecurity = h;
        }
        #endregion

        #region Members
        public void HandleSession(TransactionStatusCode transactionStatusCode)
        {
            switch (transactionStatusCode)
            {
                case TransactionStatusCode.Start:
                    StartSession();
                    break;
                case TransactionStatusCode.Continue:
                    ContinueSession();
                    break;
                case TransactionStatusCode.End:
                    EndSession();
                    break;
                case TransactionStatusCode.None:
                    hSecurity.fill();
                    ResetSession();
                    break;
                default:
                    break;
            }
        }

        private void StartSession()
        {
            hSecurity.fill();
            this.Session = new Session();
            this.Session.TransactionStatusCode = "Start";
        }

        private void ContinueSession()
        {
            hSecurity.reset();
            int sequenceNumber = int.Parse(this.Session.SequenceNumber) + 1;
            this.Session.SequenceNumber = sequenceNumber.ToString();
        }

        private void EndSession()
        {
            ContinueSession();
            this.Session.TransactionStatusCode = "End";
        }

        private void ResetSession()
        {
            this.Session = null;
        }
        #endregion
    }
}
