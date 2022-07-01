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
    public class Flex_MatchingAlgo_Controller : StandardController
    {
        FlexchainService flexchainService;

        public Flex_MatchingAlgo_Controller(FlexchainService flexchainService)
        {
            this.flexchainService = flexchainService;
        }



        public async override Task<ControllerResult> Get(RequestData data)
        {
            var requestData = RequestParser.ParseRequestAndValidate<EmptyBody, EmptyQuery, EmptyPath, EmptyHeaders>(data);


            List<Flex_MatchingAlgo> flex_MatchingAlgo = new List<Flex_MatchingAlgo>();

           
            flex_MatchingAlgo = await flexchainService.Flex_MatchingAlgo_InteractDB.GetFlexibilityMatchingAlgos();
            

            HttpGetMessage responseMessage = new HttpGetMessage(flex_MatchingAlgo);

            ControllerResult result = new ControllerResult(responseMessage);

            return result;
        }



        public async override Task<ControllerResult> Post(RequestData data)
        {
            var requestData = RequestParser.ParseRequestAndValidate<EmptyBody, EmptyQuery, EmptyPath, EmptyHeaders>(data);


            List<FlexibilityRequest> flexRequest = new List<FlexibilityRequest>();


            flexRequest = await flexchainService.FlexRequestInteractDatabase.GetFlexibilityRequests();


            List<FlexibilityOffer> flexOffer = new List<FlexibilityOffer>();

            flexOffer = await flexchainService.FlexOfferInteractDatabase.GetFlexibilityOffers();




            //matching algo

            var MODE = flexRequest[0].Mode;

            Flex_MatchingAlgo flex_MatchingAlgo;

            switch (MODE)
            {
                case "fcfs":
                    FCFS fcfs = new FCFS(flexRequest, flexOffer);

                    flex_MatchingAlgo = fcfs.fcfsCalculation();
                    break;
                case "maav":
                    MaAV maav = new MaAV(flexRequest, flexOffer);

                    flex_MatchingAlgo = maav.MaAVCalculation();
                    break;
                case "miav":
                    MiAV miav = new MiAV(flexRequest, flexOffer);
                    flex_MatchingAlgo = miav.MiavCalculation();
                    break;
                case "maah":
                    MaAH maah = new MaAH(flexRequest, flexOffer);
                    flex_MatchingAlgo = maah.MaaHCalculation();
                    break;
                case "miah":
                    MiAH miah = new MiAH(flexRequest, flexOffer);
                    flex_MatchingAlgo = miah.MiahCalculation();
                    break;
                case "mapw":
                    MaPW mapw = new MaPW(flexRequest, flexOffer);
                    flex_MatchingAlgo = mapw.MaPWCalculation();
                    break;
                case "mip":
                    MiP mip = new MiP(flexRequest, flexOffer);
                    flex_MatchingAlgo = mip.MiPCalculation();
                    break;
                case "zufall":
                    Zufall zufall = new Zufall(flexRequest, flexOffer); 
                    flex_MatchingAlgo = zufall.ZufallCalculation();
                    break;
                default:
                    fcfs = new FCFS(flexRequest, flexOffer);

                    flex_MatchingAlgo = fcfs.fcfsCalculation();
                    break;

            }


            

            var final_accepted = new Dictionary<string, Flex_MatchingAlgo>();

            final_accepted["Requests"] = flex_MatchingAlgo;



            flex_MatchingAlgo =  await flexchainService.Flex_MatchingAlgo_InteractDB.CreateFlexibilityMatchingAlgo(flex_MatchingAlgo);


         
            //HttpCreateMessage responseMessage = new HttpCreateMessage(final_accepted.ToString());

            HttpCreateMessage responseMessage = new HttpCreateMessage(flex_MatchingAlgo.Id);

            ControllerResult result = new ControllerResult(responseMessage);

            return result;
        }

    }
}
