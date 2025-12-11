using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TravelSoapService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string ImportHotelReservations(DateTime start, DateTime end);
    }
}