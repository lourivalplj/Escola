using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Escola.Models
{
    public partial class Professores
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Um nome deve ser especificado")]
        public string Nome { get; set; }
    }
}
