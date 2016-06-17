using Newtonsoft.Json.Linq;
using SignalRSever.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SignalRSever.Controllers
{
    public class ChatController : ApiController
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

        // POST api/<controller>
        [HttpPost]
        // public string Post([FromUri] MessageDetails value)
        public dynamic Post([FromBody]JObject value)
        {

            dynamic obj = Request.Content.ReadAsAsync<JObject>().Result;
           // var y = obj.var1;
            return "value";
        }

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