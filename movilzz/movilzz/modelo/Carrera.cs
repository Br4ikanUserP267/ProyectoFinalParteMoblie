using System;
using System.Collections.Generic;
using System.Text;

namespace movilzz.modelo
{
    public class Carrera
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public int Semestre_numero { get; set; }
        public int Carrera_id { get; set; }
        public int estudiantes_id { get; set; }
        public string nombreCarrera { get; set; }

    }
}
