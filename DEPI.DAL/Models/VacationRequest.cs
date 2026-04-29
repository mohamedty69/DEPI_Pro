using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.DAL.Model
{
    public class VacationRequest
    {
        public int VacationRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        // navigation property for employee
        public Employee Employee { get; set; }
        public string EmployeeId { get; set; }

        // navigation property for schedule
        public List<Schedule> Schedules { get; set; }   
    }
}
