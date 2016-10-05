using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PontoPortaria1510
{
    public class DiaPonto
    {
        public TimeSpan Debito { get; set; } = TimeSpan.FromMinutes(0);
        public TimeSpan Credito { get; set; } = TimeSpan.FromMinutes(0);
        public TimeSpan AdicionalNoturno { get; set; } = TimeSpan.FromMinutes(0);
    }
}
