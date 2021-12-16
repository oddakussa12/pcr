using System;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}