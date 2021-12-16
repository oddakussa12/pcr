using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    //this class file is used while importing excel file to database in ActionoTable.cshtml
    public class DemoResponse<T>
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        //public T Data { get; set; }
        public string Data { get; set; }
        public string Solution { get; set; }

        public static DemoResponse<T> GetResult(int code, string msg, string success, string solution)
       // public static DemoResponse<T> GetResult(int code, string msg, T data = default(T))
        {
            return new DemoResponse<T>
            {
                Code = code,
                Msg = msg,
                Data = success,
                Solution = solution
            };
        }
       
    }
}
