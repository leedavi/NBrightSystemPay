using System;
using System.Web;
using NBrightCore.common;
using Nevoweb.DNN.NBrightBuy.Components;

namespace Nevoweb.DNN.NBrightStore
{
    /// <summary>
    /// Summary description for XMLconnector
    /// </summary>
    public class NBrightSystemPayNotify : IHttpHandler
    {
        private String _lang = "";

        /// <summary>
        /// This function needs to process and returned message from the bank.
        /// This processing may vary widely between banks.
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            var modCtrl = new NBrightBuyController();
            var info = ProviderUtils.GetProviderSettings("NBrightSystemPaypayment");

            try
            {

                var debugMode = info.GetXmlPropertyBool("genxml/checkbox/debugmode");

                var orderid = Utils.RequestQueryStringParam(context, "vads_order_id");
                string clientlang = Utils.RequestQueryStringParam(context, "vads_order_info");

                var rtnMsg = "SECURITY WARNING";
                var sig1 = context.Request.Form.Get("signature");
                var strMacCalc = ProviderUtils.GetSignatureReturnData(info.GetXmlProperty("genxml/textbox/certificate"),context.Request);
                var sig2 = ProviderUtils.GetSignature(strMacCalc);

                var debugMsg = "START CALL notify.ashx " + DateTime.Now.ToString("s") + " </br>";
                if (debugMode)
                {
                    foreach (var f in context.Request.Form.AllKeys)
                    {
                        debugMsg += f + ": " + context.Request.Form.Get(f) + "</br>";
                    }
                    debugMsg += "NBrightSystemPay DEBUG: " + DateTime.Now.ToString("s") + " </br>";
                    debugMsg += sig1 + " </br>";
                    debugMsg += sig2 + " </br>";
                    info.SetXmlProperty("genxml/debugmsg", debugMsg);
                    modCtrl.Update(info);
                }
                else
                {
                    if (info.GetXmlProperty("genxml/debugmsg") != "")
                    {
                        info.SetXmlProperty("genxml/debugmsg", "");
                        modCtrl.Update(info);
                    }
                }

                if (sig1 == sig2)
                {

                    // ------------------------------------------------------------------------
                    rtnMsg = "";
                    int NBrightSystemPayStoreOrderID = 0;

                    if (debugMode) debugMsg += "OrderId: " + NBrightSystemPayStoreOrderID + " </br>";

                    var orderData = new OrderData(NBrightSystemPayStoreOrderID);

                    string NBrightSystemPayStatusCode = ProviderUtils.GetStatusCode(orderData, context.Request);

                    if (debugMode) debugMsg += "NBrightSystemPayStatusCode: " + NBrightSystemPayStatusCode + " </br>";

                    // Status return "00" is payment successful
                    if (NBrightSystemPayStatusCode == "00")
                    {
                        //set order status to Payed
                        orderData.PaymentOk();
                    }
                    else
                    {
                        orderData.PaymentFail();
                    }

                    if (debugMode)
                    {
                        debugMsg += "Return Message: " + rtnMsg;
                        info.SetXmlProperty("genxml/debugmsg", debugMsg);
                        modCtrl.Update(info);
                    }

                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(rtnMsg);
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.CacheControl = "no-cache";
                HttpContext.Current.Response.Expires = -1;
                HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                if (!ex.ToString().StartsWith("System.Threading.ThreadAbortException")) // we expect a thread abort from the End response.
                {
                    info.SetXmlProperty("genxml/debugmsg", "NBrightSystemPay ERROR: " + ex.ToString());
                    modCtrl.Update(info);
                }
            }


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}