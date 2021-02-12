using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCSSApiServer.Models
{
    public class Server
    {
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
    }
}
