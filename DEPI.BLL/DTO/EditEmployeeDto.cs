using DEPI.DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPI.BLL.DTO
{
    public class EditEmployeeDto
    {
        public string Ssn { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public int? VacationBalance { get; set; }
        public string? DefaultRole { get; set; }
        public string? ManagerSsn { get; set; }
        public int? ShiftId { get; set; }
        public int? ProductionLineId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
