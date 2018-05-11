﻿#if LocalAzure

using System.Collections.Generic;

namespace Lunggo.Framework.Environment
{
    public partial class EnvVariables
    {
        private static Dictionary<string, string> PopulateDictionary()
        {
            return new Dictionary<string, string>
            {
                {"db.connectionString","Server=tcp:travorama-development-sql-server.database.windows.net,1433;Database=travorama-local;User ID=developer@travorama-development-sql-server;Password=Standar1234;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"},
                {"azureStorage.connectionString","DefaultEndpointsProtocol=https;AccountName=travoramalocal;AccountKey=t9BOHU0NktEB4qvBd7eSdXtSYabT/wDxnC2PndRtDNdQWymLUko6q0oKGICBZ0FoX7GLvGV9v4QSNYZPu98ZWw=="},
                {"azureStorage.rootUrl","https://travoramalocal.blob.core.windows.net"},
                {"general.rootUrl","http://travorama-local-cw.azurewebsites.net"},
                {"general.mobileUrl","travorama-local-cw.azurewebsites.net"},
                {"general.cloudAppUrl","http://travorama-development.cloudapp.net/"},
                {"general.environment","local"},
                {"general.corsAllowedDomains","http://localhost,https://localhost,http://localhost:23321,https://localhost:23321,http://m.localhost,https://m.localhost,http://m.localhost:23321,https://m.localhost:23321,http://localhost:1523,https://localhost:1523,http://local.travorama.com,https://local.travorama.com,http://m.local.travorama.com,https://m.local.travorama.com,http://travorama-local-cw.azurewebsites.net,https://travorama-local-cw.azurewebsites.net"},
                {"general.seqGeneratorContainerName","seqgeneratorcontainer"},
                {"general.bankTransferStartTime","0000"},
                {"general.bankTransferEndTime","2359"},
                {"cw.basicAuthenticationUser","travorama"},
                {"cw.basicAuthenticationPassword","Standar1234"},
                {"api.apiUrl","http://travorama-local-api.azurewebsites.net"},
                {"veritrans.endPoint","https://api.sandbox.midtrans.com"},
                {"veritrans.chargeEndPoint","https://api.sandbox.midtrans.com/v2/charge"},
                {"veritrans.tokenEndPoint","https://api.sandbox.midtrans.com/v2/token"},
                {"veritrans.cancelEndPoint","https://api.sandbox.midtrans.com/v2/order_id/cancel"},
                {"veritrans.approveEndPoint","https://api.sandbox.midtrans.com/v2/order_id/approve"},
                {"veritrans.clientKey","VT-client-J8i9AzRyIU49D_v3"},
                {"AzureWebJobsDashboard.connectionString",""},
                {"AzureWebJobsStorage.connectionString",""},
                {"freshdesk.apikey","tufIQO4DgOZSTfAsbCWu"},
                {"freshdesk.RestSharpClientUrl","https://travelmadezy.freshdesk.com/"},
                {"zendesk.zendeskSiteUrl","https://travelmadezy.zendesk.com/api/v2"},
                {"zendesk.zendeskEmailAccount","developer@travelmadezy.com"},
                {"zendesk.apikey","8hH5MJivQ1E3ZbWBQ4qCd0dRT4Hx7mCd57A89jCA"},
                {"mandrill.apiKey","s5KQ-w5JJH87Z3VZRqTVGQ"},
                {"redis.masterDataCacheConnectionString","travorama-development.redis.cache.windows.net,allowAdmin=true,syncTimeout=60000,ssl=true,password=16EGFGYzLMtwUP1KiNjgsi2rcgBPYnlSYWRqOK0EX5c="},
                {"redis.databaseIndex","0"},
                {"hotel.hotelSearchResultCacheTimeout","30"},
                {"hotel.defaultStayLength","1"},
                {"hotel.defaultCheckInOffset","1"},
                {"hotel.defaultResultCount","10"},
                {"hotel.defaultAdultCount","2"},
                {"hotel.defaultRoomCount","1"},
                {"flight.SearchResultCacheTimeout","30"},
                {"flight.ItineraryCacheTimeout","30"},
                {"flight.topdestinationcachekey","flighttopdestination"},
                {"flight.paymentTimeout","120"},
                {"flight.issueTimeout","60"},
                {"flight.searchTimeout","300"},
                {"domain.imageDomain","services.carsolize.com"},
                {"travolutionary.apiUserName","rama.adhitia@travelmadezy.com"},
                {"travolutionary.apiPassword","d61Md7l7"},
                {"mystifly.apiAccountNumber","MCN004085"},
                {"mystifly.apiUserName","GOAXML"},
                {"mystifly.apiPassword","GA2014_xml"},
                {"mystifly.apiTargetServer","Test"},
                {"mystifly.apiEndPoint","http://apidemo.myfarebox.com/V2/OnePoint.svc"},
                {"facebook.appId","895478350520463"},
                {"facebook.appSecret","9969ec757434f5243b35f38325b6d636"},
                {"airAsia.webUserName","IDTDEZYCGK_ADMIN"},
                {"airAsia.webPassword","Travorama123"},
                {"citilink.webUserName","Travelmadev"},
                {"citilink.webPassword","dev12345"},
                {"sriwijaya.webUserName","MLWAG02152"},
                {"sriwijaya.webPassword","Dev12345"},
                {"mailchimp.addMemberApiRootUrl","http://us11.api.mailchimp.com"},
                {"mailchimp.basicAuthUserName","travorama"},
                {"mailchimp.basicAuthPassword","ad2872c0ab96857c93f3d69fdc88026f"},
                {"mailchimp.addMemberApiPath","3.0/lists/4997f6c614/members"},
                {"mandiri.webCompanyId","TMDZ001"},
                {"mandiri.webUserName","TRANSAUTO"},
                {"mandiri.webPassword","3c15ab94c40a4577c7f0748f833457d6620910a6"},
                {"mandiri.bankAccountNumber","1020006675802"},
                {"lionAir.webUserName","trv.agent.lima"},
                {"lionAir.webPassword","Standar1234"},
                {"Garuda.webUserName","SA3ALEU"},
                {"Garuda.webPassword","Travorama1234"},
                {"notification.connectionString","Endpoint=sb://travorama-development-notificationhub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=u9K9wjHKUKMEWDGdVGChNGVO/Yz0V6NBrkSlBfCg3zU="},
                {"notification.hubName","travorama-local"},
                {"log.slack","https://hooks.slack.com/services/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"},
                {"documentDb.endpoint","https://travorama-development-docdb.documents.azure.com:443/"},
                {"documentDb.authorizationKey","64nWp0NH3XWQ2TGepOWrhqKQFBG4B2VeixUfnyfPqwpEAmLlvUx2ZyqvAwvlFluvDNnk7ofQXYQ8G6wbQPLwkw=="},
                {"documentDb.databaseName","travorama-local"},
                {"documentDb.collectionName","travorama-local"},
                {"hotel.selectCacheTimeOut","30"},
                {"hotel.standardSizeImage","https://photos.hotelbeds.com/giata/"},
                {"hotel.bigSizeImage","https://photos.hotelbeds.com/giata/bigger/"},
                {"hotel.smallSizeImage","https://photos.hotelbeds.com/giata/small/"},
                {"hotel.apiKey","p8zy585gmgtkjvvecb982azn"},
                {"hotel.apiSecret","QrwuWTNf8a"},
                {"hotel.contentUrl","https://api.test.hotelbeds.com/hotel-content-api"},
                {"hotel.apiUrl","https://api.test.hotelbeds.com/hotel-api"},
                {"sendGrid.apikey","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"},
                {"deathbycaptcha.userName","ramaadhitia_tmi"},
                {"deathbycaptcha.password","Standar1234"},
                {"nicepay.endPoint","https://dev.nicepay.co.id"},
                {"nicepay.merchantId","IONPAYTEST"},
                {"nicepay.merchantKey","33F49GnCMS1mFYlGXisbUDzVf2ATWCl9k3R++d5hDd3Frmuos/XLx8XhXpe+LDYAbpGKZYSwtlyyLOtS/8aD7A=="},
                {"e2pay.merchantCode","IF00217"},
                {"e2pay.merchantKey","CUBe2rVJd8"},
                {"e2pay.paymentEndPoint","https://sandbox.e2pay.co.id/epayment/entry.asp"},
                {"e2pay.requeryEndPoint","https://sandbox.e2pay.co.id/epayment/enquiry.asp"}
            };
        }
    }
}

#endif