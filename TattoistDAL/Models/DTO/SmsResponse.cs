using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TattoistDAL.Models.DTO
{
    public class SmsResponse
    {
		[XmlRoot(ElementName = "status")]
		public class Status
		{
			[XmlElement(ElementName = "code")]
			public string Code { get; set; }
			[XmlElement(ElementName = "message")]
			public string Message { get; set; }
		}

		[XmlRoot(ElementName = "order")]
		public class Order
		{
			[XmlElement(ElementName = "id")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "response")]
		public class Response
		{
			[XmlElement(ElementName = "status")]
			public Status Status { get; set; }
			[XmlElement(ElementName = "order")]
			public Order Order { get; set; }
		}
	}
}
