using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XdPagamentosApi.Domain.Models.NotificacaoTransacao
{
	
	public class DtoDeviceInfo
	{

	
		public string Reference { get; set; }

	
		public string Bin { get; set; }

	
		public string Holder { get; set; }

	
		public string SerialNumber { get; set; }
	}
}
