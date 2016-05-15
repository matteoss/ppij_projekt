using ppij_web_aplikacija.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ppij_web_aplikacija.Models
{
	public class KategorijaModel
	{
		public KategorijaModel()
		{
			Trazilica = "";
		}
		public String Trazilica { get; set; }
		public ICollection<OpisKategorije> Opisi { get; set; }
	}
}