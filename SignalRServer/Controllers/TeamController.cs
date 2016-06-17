using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SignalRSever.Models;
using SignalRSever.Hubs;
using Microsoft.AspNet.SignalR;

namespace SignalRSever.Controllers
{
    public class TeamController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [Route("queue")]
        public string Post(HttpRequestMessage request)
        {
            string response = "";
            //string s = Request.Content.ReadAsStringAsync().Result;

            var jsonString =  request.Content.ReadAsStringAsync().Result ;
            MessageDetails msgDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageDetails>(jsonString);
            UserDetail userDetail = new UserDetail();
            ResponseMessage resp = new ResponseMessage();
            try
            {
                if (msgDetails != null ||msgDetails.Id!=null)
                {
                    if (GeneralActions.GetEncryptData(msgDetails.Id, ref userDetail, ref resp))
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<IMIchatHub>();
                        // context.Clients.All.Send("Admin", "stop the chat");
                        context.Clients.Group(userDetail.teamId.ToString()).chatMessage(msgDetails.Message);
                        resp.code = 200;
                        resp.desc = "Success";
                    }
                }
                else
                {
                    resp.code = 405;
                    resp.desc = "Invalid Data";
                }
            }
            catch
            {
                resp.code = 500;
                resp.desc = "Error";
            }
            finally
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject(resp);
            }
            return response;
        }

        //// POST api/<controller>
        //[HttpPost]
        ////public void Post(MessageDetails msg)
        //public void Post1([FromBody]Newtonsoft.Json.Linq.JObject value)
        //{
        //    string Id = value["Id"].ToString();
        //    string message = value["Message"].ToString();

        //   // new IMIchatHub().BoardcastToTeam(Id, message);
        //    var context = GlobalHost.ConnectionManager.GetHubContext<IMIchatHub>();
        //    // context.Clients.All.Send("Admin", "stop the chat");
        //    context.Clients.All.addChatMessage(message);
        //}
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}