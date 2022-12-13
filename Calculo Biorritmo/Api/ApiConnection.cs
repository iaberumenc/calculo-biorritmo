using Calculo_Biorritmo.Api.Contracts;
using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Models;
using Calculo_Biorritmo.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Calculo_Biorritmo.Api
{
    class ApiConnection
    {
        public static async Task<bool> GetApiStatus()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 5);
                    client.BaseAddress = new Uri(urls.base_url());
                    var result = await client.GetAsync("health/");
                    return result.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }
        public static async Task<List<EmployeeContract>> getEmployeesAsync()
        {
            var employees = new List<EmployeeContract>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urls.base_url());
                    var result = await client.GetStringAsync("employees/");
                    employees = JsonConvert.DeserializeObject<List<EmployeeContract>>(result);
                    return employees;
                }
            }
            catch (TaskCanceledException)
            {
                return employees;
            }
            
        }

        public static async Task<List<AccidentContract>> getAccidentsAsync()
        {
            var accidents = new List<AccidentContract>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urls.base_url());
                    var result = await client.GetStringAsync("accidents/");
                    accidents = JsonConvert.DeserializeObject<List<AccidentContract>>(result);
                    return accidents;
                }
            }
            catch (TaskCanceledException)
            {
                return accidents;
            }

        }

        public static async Task<bool> RefreshEmployeesFromApiAsync()
        {
            if (!await GetApiStatus())
                return false;

            var ApiEmployees = await getEmployeesAsync();
            var DbEmployees = new List<employee>();
            using (var ctx = new EmployeeEntity())
                DbEmployees = ctx.employees.ToList();

            if (!ApiEmployees.Any())
                return false;

            foreach (var item in ApiEmployees)
            {
                if (!DbEmployees.Any(x => x.curp == item.curp))
                {
                    using (var ctx = new EmployeeEntity())
                    {
                        var employee = new employee();
                        employee.curp = item.curp;
                        employee.fecha_nacimiento = Convert.ToDateTime(item.fecha_nacimiento);
                        ctx.employees.Add(employee);
                        ctx.SaveChanges();
                    }
                }
            }
            return true;

        }

        public static async Task<bool> RefreshAccidentsFromApiAsync()
        {
            if (!await GetApiStatus())
                return false;

            var ApiAccidents = await getAccidentsAsync();
            var DbAccidents = new List<accident>();
            using (var ctx = new EmployeeEntity())
                DbAccidents = ctx.accidents.ToList();

            if (!ApiAccidents.Any())
                return false;

            foreach (var item in ApiAccidents)
            {
                if (!DbAccidents.Any(x => x.curp == item.curp))
                {
                    using (var ctx = new EmployeeEntity())
                    {
                        var accident = new accident();
                        accident.curp = item.curp;
                        accident.fecha_accidente = Convert.ToDateTime(item.fecha_accidente);
                        accident.residuo_fisico = item.residuo_fisico;
                        accident.residuo_emocional = item.residuo_emocional;
                        accident.residuo_intuicional = item.residuo_intuicional;
                        accident.residuo_intelectual = item.residuo_intelectual;
                        ctx.accidents.Add(accident);
                        ctx.SaveChanges();
                    }
                }
            }
            return true;

        }

        public static async Task<bool> RegisterAccident(accident accident)
        {
            if (await GetApiStatus())
            {
                AccidentContract accidentContract = new AccidentContract(
                    id: accident.id,
                    curp: accident.curp,
                    fecha_accidente: accident.fecha_accidente.ToString("yyyy-MM-dd"),
                    residuo_fisico: accident.residuo_fisico,
                    residuo_emocional: accident.residuo_emocional,
                    residuo_intelectual: accident.residuo_intelectual,
                    residuo_intuicional: accident.residuo_intuicional);
                try
                {
                    using (var client = new HttpClient())
                    {
                        var json = JsonConvert.SerializeObject(accidentContract);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.Timeout = new TimeSpan(0, 0, 5);
                        client.BaseAddress = new Uri(urls.base_url());
                        var result = await client.PostAsync("accidents/", stringContent);
                        var contents = await result.Content.ReadAsStringAsync();
                        return result.IsSuccessStatusCode;
                    }
                }
                catch(TaskCanceledException)
                {
                    return false;
                }
            }
            return false;

        }

        public static async Task<bool> RegisterEmployee(employee employee)
        {
            if (await GetApiStatus())
            {
                EmployeeContract employeeContract = new EmployeeContract(
                    id: employee.id,
                    curp: employee.curp,
                    fecha_nacimiento: employee.fecha_nacimiento.ToString("yyyy-MM-dd"));

                try
                {
                    using (var client = new HttpClient())
                    {
                        var json = JsonConvert.SerializeObject(employeeContract);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.Timeout = new TimeSpan(0, 0, 5);
                        client.BaseAddress = new Uri(urls.base_url());
                        var result = await client.PostAsync("employees/", stringContent);
                        var contents = await result.Content.ReadAsStringAsync();
                        return result.IsSuccessStatusCode;
                    }
                }
                catch (TaskCanceledException)
                {
                    return false;
                }
            }
            return false;
        }

        public static async Task<bool> checkPendingSync()
        {
            if (await GetApiStatus())
            {
                var pendings = new List<PendingSync>();
                using (var ctx = new EmployeeEntity())
                    pendings = ctx.pendingSyncs.ToList();

                if (!pendings.Any())
                    return false;

                var pendingEmployees = new List<PendingSync>();
                pendingEmployees = pendings.Where(x => x.modelo == "employee").ToList();

                foreach (var item in pendingEmployees.ToList())
                {
                    employee employee = new employee();
                    using (var ctx = new EmployeeEntity())
                    {
                        employee = ctx.employees.Where(x => x.id == item.id_Object).FirstOrDefault();
                        if (employee != null)
                        {
                            if(!await RegisterEmployee(employee))
                                return false;
                            ctx.pendingSyncs.Attach(item);
                            ctx.pendingSyncs.Remove(item);
                            ctx.SaveChanges();
                        }

                    }
                }

                var pendingAccidents = new List<PendingSync>();
                pendingAccidents = pendings.Where(x => x.modelo == "accident").ToList();

                foreach (var item in pendingAccidents.ToList())
                {
                    accident accident = new accident();
                    using (var ctx = new EmployeeEntity())
                    {
                        accident = ctx.accidents.Where(x => x.id == item.id_Object).FirstOrDefault();
                        if (accident != null)
                        {
                            if (!await RegisterAccident(accident))
                                return false;
                            ctx.pendingSyncs.Attach(item);
                            ctx.pendingSyncs.Remove(item);
                            ctx.SaveChanges();
                        }

                    }
                }

            }
            return true;
        }
    }
}
