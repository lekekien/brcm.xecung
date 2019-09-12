using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.CRM.XeCung.InfrastructureLayer.Core.Enum
{
    public class Response
    {
        public Response()
        {
            this.Code = SystemCode.Success;
            this.Message = string.Empty;
        }
        public Response(SystemCode code, string message, dynamic data)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }

        public SystemCode Code { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
