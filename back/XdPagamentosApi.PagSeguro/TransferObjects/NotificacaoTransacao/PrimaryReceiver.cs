using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XdPagamentosApi.PagSeguro.TransferObjects.NotificacaoTransacao
{
	[XmlRoot(ElementName = "primaryReceiver")]
	public class PrimaryReceiver
	{

		[XmlElement(ElementName = "publicKey")]
		public string PublicKey { get; set; }
	}
}
