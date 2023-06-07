using System;
using System.Collections.Generic;
using System.Text;

namespace movilzz.modelo
{
    public class Prestamo
    {


        public int id { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFinal { get; set; }
        public int prestado { get; set; }
        public int Estudiantes_id { get; set; }
        public string nombre_libro { get; set; }

        public string FechaInicioFormato => fechaInicio.ToString("dd/MM/yyyy");
        public string FechaFinalFormato => fechaFinal.ToString("dd/MM/yyyy");
   
    }
    

}

