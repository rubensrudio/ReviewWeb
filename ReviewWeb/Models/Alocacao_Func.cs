using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReviewWeb.Models
{
    [Table("alocacao_func")]
    public class Alocacao_Func
    {
        [Key]
        public int idalocacao_func { get; set; }
        public int idfuncionarios { get; set; }
        public int idempresas { get; set; }
        public int idusuarios { get; set; }
        public string aloc_uf { get; set; }
        public int aloc_cliente { get; set; }
        public int aloc_contrato { get; set; }
        public DateTime horario { get; set; }
        public DateTime horario_fim { get; set; }
        public string situacao { get; set; }
        public string observacao { get; set; }
    }
}