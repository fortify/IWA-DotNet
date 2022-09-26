using System.Runtime.Serialization;
using System.ServiceModel;

namespace MicroFocus.InsecureWebApp.Models
{
    [ServiceContract()]
    public interface ICart
    {
        [OperationContract()]
        Product AddItem(Product a);

        [OperationContract()]
        [FaultContract(typeof(CartFault))]
        Product CompareItem(Product a, Product b);

        [OperationContract()]
        Order GetOrderInfo();
    }

    [DataContract]
    public class CartFault
    {
        [DataMember]
        public string operation;
        [DataMember]
        public string description;
    }

    [MessageContract()]
    public class Cart : ICart
    {
        public Product AddItem(Product a)
        {
            return a;
        }

        public Product CompareItem(Product a, Product b)
        {
            return b;
        }

        public Order GetOrderInfo()
        {
            Order order = new Order();
            order.IsAdmin = true;

            return order;
        }
    }
}
