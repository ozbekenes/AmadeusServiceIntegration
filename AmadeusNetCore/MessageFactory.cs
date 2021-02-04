using System;
using System.Collections.Generic;
using System.Text;
using Amadeus;

namespace AmadeusNetCore
{
    class MessageFactory
    {
        public static Air_FlightInfo buildAvailabilityRequest()
        {

            var generalInfo = new Air_FlightInfoGeneralFlightInfo();

            generalInfo.companyDetails = new Air_FlightInfoGeneralFlightInfoCompanyDetails();
            generalInfo.companyDetails.marketingCompany = "6X";

            generalInfo.flightIdentification = new Air_FlightInfoGeneralFlightInfoFlightIdentification();
            generalInfo.flightIdentification.flightNumber = "7725";

            generalInfo.flightDate = new Air_FlightInfoGeneralFlightInfoFlightDate();
            generalInfo.flightDate.departureDate = DateTime.Now.AddDays(7).ToString("ddMMyy");

            var flightRequest = new Air_FlightInfo();
            flightRequest.generalFlightInfo = generalInfo;

            return flightRequest;
        }

        public static Security_SignOut buildSignOutRequest()
        {
            return new Security_SignOut();
        }
    }
}
