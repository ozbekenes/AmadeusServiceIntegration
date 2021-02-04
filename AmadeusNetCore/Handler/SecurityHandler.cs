using System;
using System.Collections.Generic;
using System.Text;
using Amadeus;

namespace AmadeusNetCore
{
    public class SecurityHandler : HeaderHandler
    {
        #region Attributes
        private AMA_SecurityHostedUser mHostedUser;
        private SecurityTokenInspector iSecurity;
        #endregion

        #region Constructor
        public SecurityHandler(AmadeusWebServicesPTClient client) : base(client)
        {

            this.iSecurity = new SecurityTokenInspector("userName", "password");
            SecurityBehavior behavior = new SecurityBehavior();
            behavior.inspector = this.iSecurity;
            client.Endpoint.EndpointBehaviors.Insert(0, behavior);
        }
        #endregion

        #region Members
        public void fill()
        {
            addWSSecurity();
            setHostedUser();
        }

        public void reset()
        {
            mHostedUser = null;
            removeWSSecurity();
        }

        private void removeWSSecurity()
        {
            iSecurity.Activated = false;
        }

        private void addWSSecurity()
        {
            iSecurity.Activated = true;
        }

        private void setHostedUser()
        {
            mHostedUser = new AMA_SecurityHostedUser
            {
                UserID = new AMA_SecurityHostedUserUserID()
            };
            mHostedUser.UserID.AgentDutyCode = "SU";
            mHostedUser.UserID.POS_Type = "1";
            mHostedUser.UserID.PseudoCityCode = "EZSLL28AA";
            mHostedUser.UserID.RequestorType = "U";
        }

        public AMA_SecurityHostedUser getHostedUser()
        {
            return mHostedUser;
        }
        #endregion
    }
}
