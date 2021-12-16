using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class SQLScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext context;

        public SQLScheduleRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Schedule> GetAllProjectSchedule(int id)
        {
            return context.Schedules.Where(ri => ri.ProjectProjectCode == id).ToList();
        }
    }
}
