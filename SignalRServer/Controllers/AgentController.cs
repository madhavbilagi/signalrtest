using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SignalRSever.Models;
using SignalRSever.Hubs;
using System.Web;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.SignalR;

namespace SignalRSever.Controllers
{
    public class AgentController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [ActionName("messge")]
        public string PostMessage([FromBody] MessageDetails msgDetails)
        {
            string response = "";
            UserDetail userDetail = new UserDetail();
            ResponseMessage resp = new ResponseMessage();
            try
            {
                if (msgDetails != null)
                {
                    if (GeneralActions.GetEncryptData(msgDetails.Id, ref userDetail, ref resp))
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<IMIchatHub>();
                       
                        List<UserDetail> toUserList    = IMIchatHub.ConnectedUsers.Where(x => x.userId == userDetail.userId).ToList();
                        foreach (var user in toUserList) //if logined into different browsers
                        {
                            context.Clients.Client(user.connectionId).sendPrivateMessage(msgDetails.Message);
                        }
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


        [HttpPost]
        // POST api/<controller>
        //public void Post([FromBody]string message)
        public void Post([FromBody]JObject value)
        {
            string Id = value["Id"].ToString();
            string message = value["Message"].ToString();


            System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
            string token = string.Empty;
            string pwd = string.Empty;
            if (headers.Contains("username"))
            {
                token = headers.GetValues("username").First();
            }
            if (headers.Contains("password"))
            {
                pwd = headers.GetValues("password").First();
            }
            //to do check and authenticate
            //code to authenticate and return some thing

            var request = Request;
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ctx = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                if (ctx != null)
                {
                    var ip = ctx.Request.UserHostAddress;

                    //to do check from config and authenticate
                }
            }
            MessageDetails msg = new MessageDetails();
            //new IMIchatHub().BoardcastToAgent(teamId, message);

            var context = GlobalHost.ConnectionManager.GetHubContext<IMIchatHub>();
            // context.Clients.All.Send("Admin", "stop the chat");
            context.Clients.All.addChatMessage(message);
            var ConnectedUsers = IMIchatHub.ConnectedUsers;
          //  string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.connectionId == Id);
           // var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null)
            {
                // send to 
                context.Clients.Client(toUser.connectionId).sendPrivateMessage(message);

                // send to caller user
               // Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserId, message);
            }
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


    }
}