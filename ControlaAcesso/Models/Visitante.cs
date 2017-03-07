using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlaAcesso.Models
{
    public class Visitante
    {
        public Int64 VisitanteID { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório")]
        private string _Nome;
        public string Nome
        {
            get
            {
                if (string.IsNullOrEmpty(_Nome))
                {
                    return _Nome;
                }
                return _Nome.ToUpper();
            }
            set
            {
                _Nome = value;
            }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório")]
        [DisplayFormat(DataFormatString = "{0:000000000-00}")]
        public Int64 CPF { get; set; }

        private string _Contato;
        
        public string Contato 
        {
            get
            {
                if (string.IsNullOrEmpty(_Contato))
                {
                    return _Contato;
                }
                return _Contato.ToUpper();
            }
            set
            {
                _Contato = value;
            }
        }
    }
}