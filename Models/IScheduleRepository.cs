using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> GetAllProjectSchedule(int id);//show all project Schedule in table format
        //Schedule GetSchedule(int Id);//to edit project scedule
        //Schedule Update(Schedule scheduleChanges);
        //List<Schedule> SearchSchedule(string search);
        //Schedule Delete(int Id);
        //Schedule Add(Schedule schedule);
    }
}
