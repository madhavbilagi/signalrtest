using System;
using WebPMS.Config;
using WebPMS.Classes;
using IMI.SqlWrapper;


namespace SignalRSever
{
    public static class DBAction
    {
        public static int CheckSession(string sessionId, int teamid, int userid)
        {
            int iRes = 2;
            string senderName = "DBActions.CheckSession";
            string procName = "SPCHAT_SIGNALR_SESSION_CHK_V1";
            string ConnDB = "DSN_CHAT";
            DateTime sTime = Tools.GetTimeZoneDateTime();
            DateTime eTime = sTime;
            try
            {

                using (DBFactory objDB = new DBFactory(ConnDB))
                {
                    objDB.AddInParam("sessionid", SqlType.VarChar, sessionId);
                    objDB.AddInParam("userid", SqlType.Int, userid);
                    objDB.AddInParam("teamid", SqlType.Int, teamid);
                    objDB.AddInParam("reqtime", SqlType.DateTime, Tools.GetTimeZoneDateTime());
                    objDB.AddOutParam("retval", SqlType.Int, 10);
                    objDB.RunProc(procName);
                    iRes = objDB.GetOutValue("retval").ToString().IsNumeric() ? Convert.ToInt32(objDB.GetOutValue("retval").ToString()) : -1;
                }
                eTime = Tools.GetTimeZoneDateTime();

                // Send Connectivity Success Trap
                SNMPMonitor.Send(SNMPCodes.DB_EXCEPTION, eTrapLevel.Clear, eSingalrTrap.DB_Connectivity.GetHashCode(), "SUCCESS");

                // Send execution Success Trap
                SNMPMonitor.Send(SNMPCodes.DB_EXCEPTION, eTrapLevel.Clear, eSingalrTrap.CheckSession.GetHashCode(),
                       TrapList.GetMessageDB(senderName, procName, "SUCCESS"));
            }
            catch (Exception ex)
            {

                if (ex.Message.ToUpper().IndexOf("DB_CONNECTIVITY_FAILURE") > -1)
                {
                    // Connectivity Issue, Send failure Trap
                    SNMPMonitor.Send(SNMPCodes.DB_EXCEPTION, eTrapLevel.Critical, eSingalrTrap.DB_Connectivity.GetHashCode(), "Failed to Connect Database " + ConnDB);
                }
                else
                {
                    // send Procedure execution failure Trap
                    SNMPMonitor.Send(SNMPCodes.DB_EXCEPTION, eTrapLevel.Critical, eSingalrTrap.CheckSession.GetHashCode(),
                         TrapList.GetMessageDB(senderName, procName, "Exception::" + ex.Message));
                }
                TextLog.DBError("", "Database", senderName + "|" + procName + "|Exception>>" + ex.ToString());
            }
            finally
            {
                #region finally
                // write log with time taking
                DBTimeTaking(string.Format("{0}|{1}|{2}|{3}|{4}",
                                    sTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                    sessionId,
                                    Math.Round(eTime.Subtract(sTime).TotalMilliseconds).ToString(),
                                    senderName,
                                    procName));
                #endregion
            }
            return iRes;
        }

        public static void DBTimeTaking(string msg)
        {
            if (ShopConfig.GetConfig("ERRORLOG_DBTIME").GetBoolean())
                TextLog.Debug("", "DBTimeTaking", msg);
        }
    }
}