using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using Amadeus;

namespace AmadeusNetCore
{
    public abstract class HeaderHandler
    {
        protected ClientBase<AmadeusWebServicesPT> mClient;

        public HeaderHandler(ClientBase<AmadeusWebServicesPT> client)
        {
            mClient = client;
        }
    }
}
