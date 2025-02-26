using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSessionEmpleados.Extensions;
using MvcNetCoreSessionEmpleados.Models;
using MvcNetCoreSessionEmpleados.Repositories;

namespace MvcNetCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        //voy a recibir el salario del empleado para almacenarlo
        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if (salario != null)
            {
                //necesitamos almacenar el salario del empleado y la suma total de salarios que tengamos
                int sumaSal = 0;
                //preguntamos si ya tenemos la suma almacenada en session
                if (HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    //si existe, recuperamos el valor
                    sumaSal = HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                //realizamos la suma
                sumaSal += salario.Value; //le decimos que coja el valor del numero porque puede ser null
                //almacenamos el nuevo valor de la suma salarial dentro de session
                HttpContext.Session.SetObject("SUMASALARIAL", sumaSal);
                ViewData["MENSAJE"] = "salario almacenado " + salario.Value;
            }
            //almacenamos los datos
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        //vista para mostrar lo que vamos almacenando en session
        public IActionResult SumaSalarial()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleados(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                //buscamos al empleado para poder almacenarlo
                Empleado emp = await this.repo.FindEmpleadoAsync(idEmpleado.Value);//cuando ponemos int? -> debemos poner .value
                //en session tendremos un conjunto de empleados
                List<Empleado> empleadosList;
                //preguntamos si tenemos el conjunto de empleados almacenados en session
                if (HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    //recuperamos los empleados que tenfamos en session
                    empleadosList = HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else
                {
                    //si no existe, instanciamos la coleccion
                    empleadosList = new List<Empleado>();
                }
                //almacenamos el empleado dentro de nuestra coleccion
                empleadosList.Add(emp);
                //guardamos en session la coleccion
                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);
                ViewData["MENSAJE"] = "Empleado " + emp.Apellido + " almacenado correctamente";
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult EmpleadosAlmacenadosSession()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleadosOK(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                //almacenaremos lo minimo que podamos -> int
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    //creamos la coleccion
                    idsEmpleados = new List<int>();
                }
                else
                {
                    //existe asi que recuperamos la coleccion
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                idsEmpleados.Add(idEmpleado.Value);
                //refrescamos los datos de session
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count();

            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosOK()
        {
            //recuperamos los ids de empledaos que tenfamos en session
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "NO existen empleados almacenados en session";
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public async Task<IActionResult> SessionEmpleadosNotAlmacenados(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                //ALMACENAREMOS LO MINIMO QUE PODAMOS (int)
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    //NO EXISTE Y CREAMOS LA COLECCION
                    idsEmpleados = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION
                    idsEmpleados =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                idsEmpleados.Add(idEmpleado.Value);
                //REFRESCAMOS LOS DATOS DE SESSION
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                    + idsEmpleados.Count;
            }
            //COMPROBAMOS SI TENEMOS IDS EN SESSION
            List<int> ids =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (ids == null)
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosAsync();
                return View(empleados);
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosExcluyendoIdsAsync(ids);
                return View(empleados);
            }

        }

        public async Task<IActionResult> EmpleadosNotAlmacenados()
        {
            //DEBEMOS RECUPERAR LOS IDS DE EMPLEADOS QUE TENGAMOS
            //EN SESSION
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados almacenados "
                    + " en Session.";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }

        }

        public async Task<IActionResult> SessionEmpleadosV5(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                //ALMACENAREMOS LO MINIMO QUE PODAMOS (int)
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    //NO EXISTE Y CREAMOS LA COLECCION
                    idsEmpleados = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION
                    idsEmpleados =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                idsEmpleados.Add(idEmpleado.Value);
                //REFRESCAMOS LOS DATOS DE SESSION
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                    + idsEmpleados.Count;
            }
            //COMPROBAMOS SI TENEMOS IDS EN SESSION
            List<int> ids =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (ids == null)
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosAsync();
                return View(empleados);
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosAsync();
                return View(empleados);
            }
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV5(int? idEliminar)
        {
            // Recuperamos los IDs de empleados en sesión
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados almacenados en Session.";
                return View();
            }
            else
            {
                //PREGUNTAMOS SI HEMOS RECIBIDO ALGUN VALOR PARA ELIMINAR
                if (idEliminar != null)
                {
                    idsEmpleados.Remove(idEliminar.Value);
                    //es posible que ya no haya empleados en session
                    if(idsEmpleados.Count() == 0)
                    {
                        //eliminamos de session nuestra key
                        HttpContext.Session.Remove("IDSEMPLEADOS");
                    }else
                    {
                        //si todavia hay empleados, refrescamos session
                        HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados); //volvemos a guardar la coleccion actualizada en session
                    }
                }
                List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public IActionResult EliminarEmpleadoSession(int idEmpleado)
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados != null && idsEmpleados.Contains(idEmpleado))
            {
                idsEmpleados.Remove(idEmpleado);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
            }
            return RedirectToAction("EmpleadosAlmacenadosV5");
        }

    }
}
