using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NotificationsService:INotificationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationsService(IUnitOfWork unitOfWork,IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Notification>> GetNotifications(string userId)
        {
            var tasks = (await _unitOfWork.TasksRepository.Get(x=>x.DueDate == DateTime.Today && x.UserId == userId));

            var notifications = _mapper.Map<Notification[]>(tasks);

            return notifications;
        }


    }
}
