using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRSever.Models;
using Microsoft.AspNet.SignalR.Hubs;
using WebPMS.Config;
using WebPMS.Classes;

namespace SignalRSever.Hubs
{
    [HubName("IMIchatHub")]
    public class IMIchatHub : Hub
    {
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        public void Hello()
        {
            Clients.All.hello();
        }

        public string Connect(string data)
        {
            string response = "";
            ResponseMessage resp = new ResponseMessage();
            try
            {

                var context = Context;
                //check if request is coming from current domain or not
                if (context.Request.Url.Host != HttpContext.Current.Request.Url.Host)
                {
                    resp.code = 405;
                    resp.desc = "Access not allowed";

                }
                else
                {
                    UserDetail userDetail = new UserDetail();
                    userDetail.connectionId = Context.ConnectionId;
                    if (GeneralActions.VerifyData(data, ref userDetail, ref resp))
                    {
                        ConnectedUsers.Add(userDetail);
                        // send to caller
                        Clients.Caller.onConnected(userDetail.connectionId, ConnectedUsers);
                        //add to group
                        Groups.Add(Context.ConnectionId, userDetail.teamId.ToString());

                        resp.code = 200;
                        resp.desc = "Success";
                    }


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




        //remove the connnection
        public string DisConnect()
        {
            ResponseMessage resp = new ResponseMessage();
            string response = "";
            try
            {
                var id = Context.ConnectionId;

                if (ConnectedUsers.Count(x => x.connectionId == id) != 0)
                {
                    var user = ConnectedUsers.SingleOrDefault(x => x.connectionId == id);
                    ConnectedUsers.Remove(user);
                    resp.code = 200;
                    resp.desc = "Success";
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


        /*Bordcast  message to Team()
        1.Moving Queue objects active status
        */
        public string BoardcastToTeam(string data, string message)
        {
            ResponseMessage resp = new ResponseMessage();
            string response = "";
            try
            {

                UserDetail userDetail = new UserDetail();
                userDetail.connectionId = Context.ConnectionId;
                if (GeneralActions.VerifyData(data, ref userDetail, ref resp))
                {
                    Clients.Group(userDetail.connectionId.ToString()).chatMessage(message);
                    resp.code = 200;
                    resp.desc = "Success";
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



        //Bordcast  message to Team()
        public void BoardcastToAgent(string teamId, string message)
        {
            Clients.All.addChatMessage(message);
            //Clients.Group(teamId).addChatMessage(message);
        }


    }
}