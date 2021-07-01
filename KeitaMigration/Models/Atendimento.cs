using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeitaMigration.Models
{
    public class Atendimento
    {
        public int AtendimentoId { get; set; }
        public int Status { get; set; }

        public Usuario Fkfuncionario { get; set; }
        public Usuario Fkcliente { get; set; }

    }
}
