using Services.Abstractions.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Interfaces
{
    public interface INotificationsService
    {
        public Task<IEnumerable<Notification>> GetNotifications(string userId); 
    }
}
