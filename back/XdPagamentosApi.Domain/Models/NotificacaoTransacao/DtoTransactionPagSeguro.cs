using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XdPagamentosApi.Domain.Models.NotificacaoTransacao
{
	
	public class DtoTransactionPagSeguro
	{

	
		public DateTime Date { get; set; }

	
		public string Code { get; set; }

	
		public int Type { get; set; }


		public int Status { get; set; }


		public DateTime LastEventDate { get; set; }


		public DtoPaymentMethod PaymentMethod { get; set; }


		public double GrossAmount { get; set; }


		public double DiscountAmount { get; set; }


		public DtoCreditorFees CreditorFees { get; set; }


		public double NetAmount { get; set; }


		public double ExtraAmount { get; set; }


		public DateTime EscrowEndDate { get; set; }


		public int InstallmentCount { get; set; }


		public int ItemCount { get; set; }


		public DtoItems Items { get; set; }


		public DtoPrimaryReceiver PrimaryReceiver { get; set; }

		
		public DtoDeviceInfo DeviceInfo { get; set; }

		public string Xml { get; set; }
	}
}
