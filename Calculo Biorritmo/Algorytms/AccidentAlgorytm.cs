using Calculo_Biorritmo.ApplicationLayer.Constants;
using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Models;
using Calculo_Biorritmo.Utils.Data;
using Calculo_Biorritmo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculo_Biorritmo.Algorytms
{
    class AccidentAlgorytm
    {
        /*public static List<Double> startAlgorytm()
        {
            var accidentes =  new List<accident>();
            using (var ctx = new EmployeeEntity())
                foreach (var item in ctx.accidents.ToList())
                    accidentes.Add(item);

            if (!accidentes.Any())
                return null;

            var RegistrosFisicos = new List<Double>();
            var RegistrosEmocionales = new List<Double>();
            var RegistrosIntuicionales = new List<Double>();
            var RegistrosIntelectuales = new List<Double>();

            foreach (var biorritmo in accidentes){
                RegistrosFisicos.Add(biorritmo.residuo_fisico);
                RegistrosEmocionales.Add(biorritmo.residuo_emocional);
                RegistrosIntelectuales.Add(biorritmo.residuo_intelectual);
                RegistrosIntuicionales.Add(biorritmo.residuo_intuicional);
            }

            var totalFisico = RegistrosFisicos.Sum();
            var promedioFisico = totalFisico / RegistrosFisicos.Count;
            var totalEmocional = RegistrosEmocionales.Sum();
            var promedioEmocional = totalEmocional / RegistrosEmocionales.Count;
            var totalIntelectual= RegistrosIntelectuales.Sum();
            var promedioIntelectual = totalIntelectual / RegistrosIntelectuales.Count;
            var totalIntuicional = RegistrosIntuicionales.Sum();
            var promedioIntuicional = totalIntuicional / RegistrosIntuicionales.Count;

            var promedios = new List<Double>();
            promedios.Add(promedioFisico);
            promedios.Add(promedioEmocional);
            promedios.Add(promedioIntelectual);
            promedios.Add(promedioIntuicional);

            return promedios;

        }*/

        public static AvgVM startAlgorytm()
        {
            var accidentes =  new List<accident>();
            using (var ctx = new EmployeeEntity())
                foreach (var item in ctx.accidents.ToList())
                    accidentes.Add(item);

            if (!accidentes.Any())
                return null;

            
            var RegistrosFisicos = new List<Double>();
            var RegistrosEmocionales = new List<Double>();
            var RegistrosIntuicionales = new List<Double>();
            var RegistrosIntelectuales = new List<Double>();

            foreach (var biorritmo in accidentes){
                RegistrosFisicos.Add(biorritmo.residuo_fisico);
                RegistrosEmocionales.Add(biorritmo.residuo_emocional);
                RegistrosIntelectuales.Add(biorritmo.residuo_intelectual);
                RegistrosIntuicionales.Add(biorritmo.residuo_intuicional);
            }

            var modaFisico = RegistrosFisicos.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
            var modaEmocional = RegistrosEmocionales.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
            var modaIntelectual = RegistrosIntelectuales.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();
            var modaIntuicional = RegistrosIntuicionales.GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => x.Key).FirstOrDefault();

            var totalModaFisico = accidentes.Where(x => x.residuo_fisico == modaFisico).Count();
            var totalModaEmocional = accidentes.Where(x => x.residuo_emocional == modaEmocional).Count();
            var totalModaIntelectual = accidentes.Where(x => x.residuo_intelectual == modaIntelectual).Count();
            var totalModaIntuicional = accidentes.Where(x => x.residuo_intuicional == modaIntuicional).Count();

            var moda = new AvgVM();
            moda.biorritmoFisico = modaFisico;
            moda.biorritmoEmocional = modaEmocional;
            moda.biorritmoIntelectual = modaIntelectual;
            moda.biorritmoIntuicional = modaIntuicional;
            moda.totalBiorritmoFisico = totalModaFisico;
            moda.totalBiorritmoEmocional = totalModaEmocional;
            moda.totalBiorritmoIntelectual = totalModaIntelectual;
            moda.totalBiorritmoIntuicional = totalModaIntuicional;

            return moda;
        }

        public static List<int> checkCritics()
        {
            var accidentes = new List<accident>();
            using (var ctx = new EmployeeEntity())
                foreach (var item in ctx.accidents.ToList())
                    accidentes.Add(item);

            if (!accidentes.Any())
                return new List<int>();

            var accidentOnCritic = new List<accident>();
            var accidentOnPerfectCritics = new List<accident>();
            var ocurredOnFisic = new List<Double?>();
            var ocurredOnEmotional = new List<Double?>();
            var ocurredOnIntuitional = new List<Double?>();
            var ocurredOnIntelectual = new List<Double?>();

            foreach (var item in accidentes)
            {
                var date = DataCalc.getBirthDate(item.curp);
                var days = DataCalc.daysLived(date,item.fecha_accidente);
                var RegistrosFisicos = DataCalc.CalculateBiorritm(days,BiorytmDays.biorritmo_fisico);
                var wasFisicCritic =  calculateCritics(RegistrosFisicos);
                if (wasFisicCritic != null)
                    ocurredOnFisic.Add(wasFisicCritic);
                var RegistrosEmocionales = DataCalc.CalculateBiorritm(days, BiorytmDays.biorritmo_emocional);
                var wasEmotionalCritic = calculateCritics(RegistrosEmocionales);
                if (wasEmotionalCritic != null)
                    ocurredOnEmotional.Add(wasEmotionalCritic);
                var RegistrosIntuicionales = DataCalc.CalculateBiorritm(days, BiorytmDays.biorritmo_intuicional);
                var wasIntuitionalCritic = calculateCritics(RegistrosIntuicionales);
                if (wasIntuitionalCritic != null)
                    ocurredOnIntuitional.Add(wasIntuitionalCritic);
                var RegistrosIntelectuales = DataCalc.CalculateBiorritm(days, BiorytmDays.biorritmo_intelectual);
                var wasIntelectualCritic = calculateCritics(RegistrosIntelectuales);
                if (wasIntelectualCritic != null)
                    ocurredOnIntelectual.Add(wasIntelectualCritic);

                if (wasFisicCritic != null ||
                   wasEmotionalCritic != null ||
                   wasIntelectualCritic != null ||
                   wasIntuitionalCritic != null)
                {
                    accidentOnCritic.Add(item);
                    if (wasFisicCritic != null &&
                   wasEmotionalCritic != null &&
                   wasIntelectualCritic != null &&
                   wasIntuitionalCritic != null)
                        accidentOnPerfectCritics.Add(item);
                }
                    
            }

            var critics = new List<int>();

            critics.Add(accidentes.Count);
            critics.Add(ocurredOnFisic.Count);
            critics.Add(ocurredOnEmotional.Count);
            critics.Add(ocurredOnIntuitional.Count);
            critics.Add(ocurredOnIntelectual.Count);
            critics.Add(accidentOnPerfectCritics.Count);
            critics.Add(accidentOnCritic.Count);

            return critics;
        }

        /*public static List<Double> CalculateErrorMargin()
        {

            var promedios = startAlgorytm();
            if (!promedios.Any())
                return null;

            var margins = new List<Double>();
            foreach(var item in promedios)
            {
                double over;
                double under;
                if(item >= 0)
                {
                    over = item + .2;
                    under = item - .2;
                }
                else
                {
                    over = item - .2;
                    under = item + .2;
                }
                margins.Add(over);
                margins.Add(under);
            }
            return margins;
        }*/


        public static double? calculateCritics(List<Double> List)
        {
            if (List[1] == 0)
                return List[1];
            //else if (List[0] == 0)
            //    return List[1];
            //else if (List[2] == 0)
            //    return List[1];
            else if (List[0] > 0 && List[1] < 0)
                return List[1];
            else if (List[0] < 0 && List[1] > 0)
                return List[1];
            else if (List[1] < 0 && List[2] > 0)
                return List[1];
            else if (List[1] > 0 && List[2] < 0)
                return List[1];
            return null;
        }
    }
}
