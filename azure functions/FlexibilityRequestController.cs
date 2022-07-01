using AwsiAzureFramework.controller;
using AwsiAzureFramework.http_response;
using AwsiAzureFramework.request_handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexchainFunction
{
    public class FlexibilityRequestController: StandardController
    {
        FlexchainService flexchainService;

        public FlexibilityRequestController(FlexchainService flexchainService)
        {
            this.flexchainService = flexchainService;
        }

        public async override Task<ControllerResult> Post(RequestData data)
        {
            var requestData = RequestParser.ParseRequestAndValidate<FlexibilityRequest, EmptyQuery, EmptyPath, EmptyHeaders>(data);

            FlexibilityRequest flexRequest = new FlexibilityRequest(requestData.Body.RequestID, requestData.Body.Mode, requestData.Body.FullfillmentFactor, requestData.Body.IfFlexRequested,
                requestData.Body.Loc, requestData.Body.MarketType, requestData.Body.MaxPriceCtpEU,
            requestData.Body.PriceOfferCtpEU, requestData.Body.ReferencePriceCtpEU, requestData.Body.TimeSlot, requestData.Body.TotalFlexRequestedEU);

            flexRequest =  await flexchainService.FlexRequestInteractDatabase.CreateFlexibilityRequest(flexRequest);

            HttpCreateMessage responseMessage = new HttpCreateMessage(flexRequest.RequestID);

            ControllerResult result = new ControllerResult(responseMessage);

            return result;
        }


        public async override Task<ControllerResult> Get(RequestData data)
        {
            var requestData = RequestParser.ParseRequestAndValidate<EmptyBody,  EmptyQuery, GetFlexibilityRequestDto, EmptyHeaders >(data);

            string RequestID = requestData.PathParams.RequestID;

            List<FlexibilityRequest> flexRequest = new List<FlexibilityRequest>();

            if (RequestID != null)
            {
                flexRequest.Add(await flexchainService.FlexRequestInteractDatabase.GetFlexibilityRequestByID(RequestID));


            }
            else
            {
               flexRequest =  await flexchainService.FlexRequestInteractDatabase.GetFlexibilityRequests();
            }

            HttpGetMessage responseMessage = new HttpGetMessage(flexRequest);

            ControllerResult result = new ControllerResult(responseMessage);

            return result;
        }

    }
}
