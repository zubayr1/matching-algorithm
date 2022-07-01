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
    public class FlexibilityOfferController : StandardController
    {
        FlexchainService flexchainService;

        public FlexibilityOfferController(FlexchainService flexchainService)
        {
            this.flexchainService = flexchainService;
        }

        public async override Task<ControllerResult> Post(RequestData data)
        {
            var offerData = RequestParser.ParseRequestAndValidate<FlexibilityOffer, EmptyQuery, EmptyPath, EmptyHeaders>(data);

            FlexibilityOffer flexOffers = new FlexibilityOffer(offerData.Body.UserId, offerData.Body.FlexOfferList);

            flexOffers = await flexchainService.FlexOfferInteractDatabase.CreateFlexibilityOffer(flexOffers);

            HttpCreateMessage responseMessage = new HttpCreateMessage(flexOffers.UserId);

            ControllerResult result = new ControllerResult(responseMessage);

            return result;
        }



        public async override Task<ControllerResult> Get(RequestData data)
        {
            var requestData = RequestParser.ParseRequestAndValidate<EmptyBody, EmptyQuery, GetFlexibilityOfferDto, EmptyHeaders>(data);

            string UserId = requestData.PathParams.UserId;

            List<FlexibilityOffer> flexOffer = new List<FlexibilityOffer>();

            if (UserId != null)
            {
                flexOffer.Add(await flexchainService.FlexOfferInteractDatabase.GetFlexibilityOfferByID(UserId));


            }
            else
            {
                flexOffer = await flexchainService.FlexOfferInteractDatabase.GetFlexibilityOffers();
            }

            HttpGetMessage responseMessage = new HttpGetMessage(flexOffer);

            ControllerResult result = new ControllerResult(responseMessage);

            return result;
        }


    }
}
