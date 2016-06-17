using SignalRSever.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPMS.Classes;
using WebPMS.Config;

namespace SignalRSever
{
    public class GeneralActions
    {
        public static bool VerifyData(string data, ref UserDetail userDetail, ref ResponseMessage resp)
        {
            bool flag = false;
            try
            {
                string encytdData = AES.Decrypt(data, Tools.GetConfig("AES_KEY"));
                string[] encytdDataArray = encytdData.Split('|');
                if (encytdDataArray.Length == 4)
                {

                    userDetail.groupId = encytdDataArray[0].GetInt();
                    userDetail.teamId = encytdDataArray[1].GetInt();
                    userDetail.userId = encytdDataArray[2].GetInt();
                    userDetail.sessionId = encytdDataArray[3];
                    if (DBAction.CheckSession(userDetail.sessionId, userDetail.teamId, userDetail.userId) == 1)
                    {
                        flag = true;
                    }
                    else
                    {
                        resp.code = 403;
                        resp.desc = "unauthorised access";
                    }
                }
                else
                {
                    resp.code = 405;
                    resp.desc = "Invalid Data";
                }
            }
            catch (Exception ex) { throw ex; }
            return flag;
        }

        public static bool GetEncryptData(string data, ref UserDetail userDetail, ref ResponseMessage resp)
        {
            bool flag = false;
            try
            {
                string encytdData = AES.Decrypt(data, Tools.GetConfig("AES_KEY"));
                string[] encytdDataArray = encytdData.Split('|');
                if (encytdDataArray.Length == 4)
                {

                    userDetail.groupId = encytdDataArray[0].GetInt();
                    userDetail.teamId = encytdDataArray[1].GetInt();
                    userDetail.userId = encytdDataArray[2].GetInt();
                    userDetail.sessionId = encytdDataArray[3];
                    userDetail.connectionId = encytdDataArray[4];
                    if (DBAction.CheckSession(userDetail.sessionId, userDetail.teamId, userDetail.userId) == 1)
                    {
                        flag = true;
                    }
                    else
                    {
                        resp.code = 403;
                        resp.desc = "unauthorised access";
                    }
                }
                else
                {
                    resp.code = 405;
                    resp.desc = "Invalid Data";
                }
            }
            catch (Exception ex) { throw ex; }
            return flag;
        }
    }
}