using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.DTO
{
    public class CriteriaDto
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public double VacancyWeight { get; set; }

    public int Order { get; set; }
    // Exclude the Tasks navigation property to prevent cycles
    }
}
