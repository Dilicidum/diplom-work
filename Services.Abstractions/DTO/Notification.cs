using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.DTO
{
    public class Notification
    {
        public int TaskId { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }
    }
}
