using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DeliveryAddressService
    {
        public List<DeliveryAddressEntity> GetAddresses(int customerId)
        {
            using var context = new RestrauntContext();

            return context.DeliveryAddresses
                .Where(a => a.CustomerId == customerId)
                .ToList();
        }

        public void AddAddress(int customerId, string address)
        {
            using var context = new RestrauntContext();

            context.DeliveryAddresses.Add(new DeliveryAddressEntity
            {
                CustomerId = customerId,
                Address = address
            });

            context.SaveChanges();
        }

        public void RemoveAddress(int addressId)
        {
            using var context = new RestrauntContext();

            var address = context.DeliveryAddresses.FirstOrDefault(a => a.Id == addressId);
            if (address == null) return;

            context.DeliveryAddresses.Remove(address);
            context.SaveChanges();
        }
    }
}
