using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReviewWeb.Models
{
    public class HorasContrato
    {
        [Key]
        public virtual int idhoras { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string contrato { get; set; }
        public string descricao { get; set; }
        public DateTime horario { get; set; }
        public DateTime horario_fim { get; set; }
    }
}