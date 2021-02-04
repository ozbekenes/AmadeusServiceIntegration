using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text;

namespace AmadeusNetCore
{
    class SecurityBehavior : IEndpointBehavior
    {
        #region Attributes
        public SecurityTokenInspector inspector { get; set; }
        #endregion

        #region IEndpointBehavior Members
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            return;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }
        #endregion
    }
}
