using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using Amadeus;

namespace AmadeusNetCore.Handler
{
    public class AddressingHandler : HeaderHandler
    {
        #region Attributes
        private String mWSAP;
        #endregion

        #region Constructor
        public AddressingHandler(AmadeusWebServicesPTClient client, String wsap)
            : base(client)
        {
            this.mWSAP = wsap;
        }
        #endregion

        #region Members
        public void setWSAP(String wsap)
        {
            mWSAP = wsap;
        }

        public String getWSAP()
        {
            return mWSAP;
        }

        public void update()
        {
            setWSAP();
        }

        private void setWSAP()
        {
            var uri = mClient.Endpoint.Address.Uri;
            var newUri = uri.Scheme + "://" + uri.Host + "/" + this.mWSAP;
            var address = new EndpointAddress(newUri);
            mClient.Endpoint.Address = address;
        }
        #endregion
    }
}