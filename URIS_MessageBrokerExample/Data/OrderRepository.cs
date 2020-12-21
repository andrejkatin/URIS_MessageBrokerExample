using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URIS_OrderService.Models;

namespace URIS_OrderService.Data
{
    public class OrderRepository : IOrderRepository
    {
        public static List<Order> Orders { get; set; } = new List<Order>();

        public OrderRepository()
        {
            FillData();
        }

        private void FillData()
        {
            Orders.Add(new Order
            {
                Id = Guid.Parse("6a411c13-a195-48f7-8dbd-67596c3974c0"),
                Code = "OR-001",
                OrderDate = "15.12.2020.",
                Quantity = 2,
                Amount = 1200.0
            });
            Orders.Add(new Order
            {
                Id = Guid.Parse("21ad52f8-0281-4241-98b0-481566d25e4f"),
                Code = "OR-002",
                OrderDate = "15.12.2020.",
                Quantity = 1,
                Amount = 100.0
            });
            Orders.Add(new Order
            {
                Id = Guid.Parse("9d8bab08-f442-4297-8ab5-ddfe08e335f3"),
                Code = "OR-003",
                OrderDate = "5.12.2020.",
                Quantity = 4,
                Amount = 2200.0
            });
        }

        public bool CreateOrder(Order order)
        {
            order.Id = Guid.NewGuid();
            Orders.Add(order);

            var testOrder = GetOrderById(order.Code);
            if (testOrder == null)
            {
                return false;
            }

            return true;
        }

        public Order GetOrderById(string orderId)
        {
            return Orders.FirstOrDefault(o => o.Code == orderId);
        }
    }
}
