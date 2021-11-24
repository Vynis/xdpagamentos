using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XdPagamentosApi.Domain.Models.NotificacaoTransacao
{
	
	public class DtoItem
	{

	
		public int Id { get; set; }

	
		public string Description { get; set; }

	
		public int Quantity { get; set; }

	
		public double Amount { get; set; }
	}
}
