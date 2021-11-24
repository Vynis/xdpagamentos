using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XdPagamentosApi.Domain.Models.NotificacaoTransacao
{
	
	public class DtoCreditorFees
	{
	
		public double InstallmentFeeAmount { get; set; }

	
		public double IntermediationRateAmount { get; set; }

	
		public double IntermediationFeeAmount { get; set; }
	}

}
