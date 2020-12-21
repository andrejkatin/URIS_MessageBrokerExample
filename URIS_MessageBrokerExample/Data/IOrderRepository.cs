using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URIS_OrderService.Models;

namespace URIS_OrderService.Data
{
    public interface IOrderRepository
    {
        Order GetOrderById(string orderId);
        Boolean CreateOrder(Order order);
    }
}
