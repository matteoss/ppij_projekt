using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models
{
	public class PredmetModel
	{
		public PredmetModel()
		{
			Trazilica = "";
		}
		public String Trazilica { get; set; }
		public ICollection<OpisPredmeta> Opisi { get; set; }
	}
}