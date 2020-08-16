using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Escola.Models
{
    public partial class Alunos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        
        [DataType(DataType.Currency)]
        [Display(Name = "Valor da Mensalidade")]
        public decimal ValorMensalidade { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data de Vencimento")]
        public DateTime DataVencimento { get; set; }

        public int idProfessor { get; set; }

    }
}
