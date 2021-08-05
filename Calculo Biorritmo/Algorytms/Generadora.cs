using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Algorytms
{
    class Generadora {
        public Generadora()
    {
        Puntos = new List<Punto>();
    }
    public List<Punto> Puntos { get; set; }
    
        public List<Punto> CalcularBiorritmo(int diasVividos, int teoria)
        {
        Puntos = new List<Punto>();
        //List<Double> values = new List<Double>();

            for (double i = 0; i < 30; i+=.1)
            {
                var dayValue = (2 * Math.PI * (diasVividos + i)) / teoria;
                var sinValue = Math.Sin(dayValue);
                var roundedValue = Math.Round(sinValue, 9, MidpointRounding.ToEven);
                //values.Add(roundedValue);
                Puntos.Add(new Punto(i, roundedValue));
            }

            return Puntos;
        }
    }
}
