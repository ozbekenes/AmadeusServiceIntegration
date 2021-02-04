using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace AmadeusNetCore.Dispatcher
{
    public class LoggingBehavior : IEndpointBehavior
    {

        private readonly SecurityTokenInspector _serviceLoggingInspector;
        public LoggingBehavior(SecurityTokenInspector serviceLoggingInspector)
        {
            _serviceLoggingInspector = serviceLoggingInspector;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(_serviceLoggingInspector);
        }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint) { }
    }
}
