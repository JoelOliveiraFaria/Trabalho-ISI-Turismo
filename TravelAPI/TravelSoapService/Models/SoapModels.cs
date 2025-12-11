using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TravelSoapService.Models
{
    [DataContract]
    public class HotelResponse
    {
        public List<HotelInfo> Data { get; set; }
    }

    [DataContract]
    public class HotelInfo
    {
        public string Name { get; set; }
        public string HotelId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}