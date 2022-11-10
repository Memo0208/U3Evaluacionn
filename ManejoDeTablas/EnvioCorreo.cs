using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManejoDeTablas
{
    public class EnvioCorreo
    {
        public bool Enviado { get; set; }
        public string Correo { get; set; }
        public String Alumno { get; set; }
        public Double Calificacion { get; set; }

        public EnvioCorreo(bool Enviado, String Correo, String Alumno, Double calificacion)
        {
            this.Enviado = Enviado;
            this.Correo = Correo;
            this.Alumno = Alumno;
            this.Calificacion = calificacion;
        }
        public EnvioCorreo() { }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            EnvioCorreo other = (EnvioCorreo)obj;
            
            return other.Correo.Equals(Correo);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }

    }
}
