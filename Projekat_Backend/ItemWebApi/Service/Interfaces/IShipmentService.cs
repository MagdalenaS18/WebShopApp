using ItemWebApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemWebApi.Services.Interfaces
{
    public interface IShipmentService
    {
        int ScheduleShipment(long orderId);
        bool CancelShipment(long orderId);
        TimeSpan GetRemainingTime(long orderId);
    }
}
