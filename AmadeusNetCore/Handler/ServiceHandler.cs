using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using Amadeus;
using AmadeusNetCore.Dispatcher;
using Microsoft.Web.Services3.Design;

namespace AmadeusNetCore.Handler
{
    public class ServiceHandler
    {
        #region Attributes
        private readonly AmadeusWebServicesPTClient client;
        private readonly SessionHandler hSession;
        private readonly TransactionFlowLinkHandler hLink;
        private readonly SecurityHandler hSecurity;
        private readonly AddressingHandler hAddressing;
        private readonly LoggingBehavior _loggingEndpointBehavior;
        #endregion

        #region Constructor
        public ServiceHandler(string wSap, LoggingBehavior loggingEndpointBehavior)
        {
            client = new AmadeusWebServicesPTClient();
            hSecurity = new SecurityHandler(client);
            hSession = new SessionHandler(client, hSecurity);
            hLink = new TransactionFlowLinkHandler(client);
            hAddressing = new AddressingHandler(client, wSap);
            _loggingEndpointBehavior = loggingEndpointBehavior;

        }
        #endregion

        #region Members
        public async Task<Air_FlightInfoResponse> Air_FlightInfo(SessionHandler.TransactionStatusCode transactionStatusCode, TransactionFlowLinkHandler.TransactionFlowLinkAction linkAction)
        {
            BeforeRequest(transactionStatusCode, linkAction);
            var session = hSession.Session;
            var link = hLink.Link;

            var basicHttpBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                Name = "AmadeusWebServicesPortBinding",
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                Security = { Mode = BasicHttpSecurityMode.Transport }
            };

            var nonceText = Guid.NewGuid().ToString();
            var created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z";

            var nonceBytes = Encoding.ASCII.GetBytes(nonceText);
            var nonceTextEncoded = Convert.ToBase64String(nonceBytes);

            var shaPwd1 = new System.Security.Cryptography.SHA1Managed();
            var pwd = shaPwd1.ComputeHash(Encoding.ASCII.GetBytes("password"));

            var createdBytes = System.Text.Encoding.ASCII.GetBytes(created);
            var operand = new byte[nonceBytes.Length + createdBytes.Length + pwd.Length];
            Array.Copy(nonceBytes, operand, nonceBytes.Length);
            Array.Copy(createdBytes, 0, operand, nonceBytes.Length, createdBytes.Length);
            Array.Copy(pwd, 0, operand, nonceBytes.Length + createdBytes.Length, pwd.Length);
            var sha1 = new System.Security.Cryptography.SHA1Managed();
            var trueDigest = Convert.ToBase64String(sha1.ComputeHash(operand));

            var soapSecurityHeader = new SoapSecurityHeader("userName", trueDigest, nonceTextEncoded, created);
            var factory = ChannelFactory(basicHttpBinding);
            var serviceProxy = AmadeusWebServicesPtChannel(factory, soapSecurityHeader);

            #region air flight info method call

            var result2 = await serviceProxy.Air_FlightInfoAsync(new Air_FlightInfoRequest
            {
                Air_FlightInfo = new Air_FlightInfo
                {
                    generalFlightInfo = new Air_FlightInfoGeneralFlightInfo
                    {
                        boardPointDetails = new Air_FlightInfoGeneralFlightInfoBoardPointDetails
                        {
                            trueLocationId = "TPE"
                        },
                        offPointDetails = new Air_FlightInfoGeneralFlightInfoOffPointDetails
                        {
                            trueLocationId = "LGW"
                        },
                        companyDetails = new Air_FlightInfoGeneralFlightInfoCompanyDetails { marketingCompany = "CI" },
                        flightDate = new Air_FlightInfoGeneralFlightInfoFlightDate { departureDate = "021020" },
                        flightIdentification = new Air_FlightInfoGeneralFlightInfoFlightIdentification { flightNumber = "69" }
                    }
                },
                AMA_SecurityHostedUser = hSecurity.getHostedUser()
            });
            #endregion
            factory.Close();
            ((ICommunicationObject)serviceProxy).Close();


            return null;
        }

        private static AmadeusWebServicesPTChannel AmadeusWebServicesPtChannel(ChannelFactory<AmadeusWebServicesPTChannel> factory,
            SoapSecurityHeader soapSecurityHeader)
        {
            var serviceProxy = factory.CreateChannel();
            serviceProxy.Open();
            var opContext = new OperationContext(serviceProxy);
            opContext.OutgoingMessageHeaders.Add(soapSecurityHeader);
            var prevOpContext = OperationContext.Current;
            OperationContext.Current = opContext;

            var usernameTokenHeader =
                MessageHeader.CreateHeader("MessageID", "http://www.w3.org/2005/08/addressing", Guid.NewGuid().ToString());
            OperationContext.Current.OutgoingMessageHeaders.Add(usernameTokenHeader);

            var passwordTextHeader = MessageHeader.CreateHeader("To", "http://www.w3.org/2005/08/addressing",
                "https://nodeD3.test.webservices.amadeus.com/1ASIWHARHRT");
            OperationContext.Current.OutgoingMessageHeaders.Add(passwordTextHeader);

            var action = MessageHeader.CreateHeader("Action", "http://www.w3.org/2005/08/addressing",
                "http://webservices.amadeus.com/FLIREQ_07_1_1A");
            OperationContext.Current.OutgoingMessageHeaders.Add(action);
            return serviceProxy;
        }

        private ChannelFactory<AmadeusWebServicesPTChannel> ChannelFactory(BasicHttpBinding basicHttpBinding)
        {
            var factory = new ChannelFactory<AmadeusWebServicesPTChannel>(basicHttpBinding,
                new EndpointAddress("https://nodeD3.test.webservices.amadeus.com/1ASIWHARHRT"));
            factory.Credentials.UserName.UserName = "userName";
            factory.Credentials.UserName.Password = "password";
            factory.Endpoint.EndpointBehaviors.Add(_loggingEndpointBehavior);
            return factory;
        }

        private void BeforeRequest(SessionHandler.TransactionStatusCode transactionStatusCode, TransactionFlowLinkHandler.TransactionFlowLinkAction linkAction)
        {
            hSession.HandleSession(transactionStatusCode);
            hLink.handleLinkAction(linkAction);
            hAddressing.update();
        }

        #endregion
    }
}
