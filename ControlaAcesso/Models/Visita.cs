using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlaAcesso.Models
{
    public class Visita
    {
        public Int64 VisitaID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]        
        [DisplayName("Registro de Entrada")]
        public DateTime RegistroEntrada { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DisplayName("Registro de Saída")]
        public DateTime? RegistroSaida { get; set; }

        [Required]
        private string _CodigoCracha { get; set; }

        [DisplayName("Crachá")]
        public string CodigoCracha
        {
            get
            {
                if (string.IsNullOrEmpty(_CodigoCracha))
                {
                    return _CodigoCracha;
                }
                return _CodigoCracha.ToUpper();
            }
            set
            {
                _CodigoCracha = value;
            }
        }

        private string _Empresa { get; set; }

        public string Empresa
        {
            get
            {
                if (string.IsNullOrEmpty(_Empresa))
                {
                    return _Empresa;
                }
                return _Empresa.ToUpper();
            }
            set
            {
                _Empresa = value;
            }
        }

        [Required]
        public Int64 VisitanteID { get; set; }

        public virtual Visitante Visitante { get; set; }

        private string _Assunto;

        public string Assunto
        {
            get
            {
                if (string.IsNullOrEmpty(_Assunto))
                {
                    return _Assunto;
                }
                return _Assunto.ToUpper();
            }
            set
            {
                _Assunto = value;
            }
        }

        
    }
}