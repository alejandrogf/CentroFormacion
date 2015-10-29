using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroFormacion.model;

namespace CentroFormacion
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public static void ConsultaSimple()
        {
            //Using controla el cierre de las conexiones.
            //"usa esto y cieera cuando termines"
            using (var ctx=new CentroFormacionEntities())
            {
                var data = ctx.Profesor.Where(o => o.Nombre.Contains("Luis"));
                foreach (var profesor in data)
                {
                    Console.WriteLine(profesor);
                }
            }
        }

        //Consultas de agregados(sumar, restar, average, etc).
        public static void Suma()
        {
            using (var ctx = new CentroFormacionEntities())
            {
                var data = ctx.Curso.Sum(o => o.Duracion);
                Console.WriteLine(data);
            }
        }
        //Con .Select se crea un objeto dinámico segón lo que especifiquemos, por ejemplo
        //para así recuperar los campos que queremos, ej 2 campos de 7
        public static void ObjetoDinamico()
        {
            using (var ctx = new CentroFormacionEntities())
            {//select new se usa cuando se quieren seleccionar varios campos.
                var data = ctx.Profesor.Where(o => o.Nombre.Contains("Luis"))
                    .Select(o=>new {Denominacion=o.Nombre, Antiguedad=o.Edad});

                foreach(var profesor in data)
                {
                    Console.WriteLine(profesor);
                }

                //Nomenclatura opcional:
                var data2 = from o in ctx.Profesor
                    where o.Nombre.Contains("Luis")
                    select new
                    {
                        Denominacion = o.Nombre,
                        Antiguedad = o.Edad,
                    };

                foreach (var profesor2 in data2)
                {
                    Console.WriteLine(profesor2);
                }
                
            }

        }

        //Encadenar busquedas.
        public static void BusquedaEnlazada()
        {
            using (var ctx = new CentroFormacionEntities())
            {//select, sin new, se puede usar cuando solo se quiere obtener un campo.
                var cursosProfesor = ctx.ProfesorCurso.Where(o => o.idProfesor == 1)
                    .Select(o => o.Curso);
                Console.WriteLine(cursosProfesor);
            }
        }

        //Para gestionar, por ejemplo, una página con una vista con varios filtros, entra en accion
        //el lazyloading, que hace que las consultas que hacemos realmente lo que tienen son las
        //instrucciones para llegar a los datos, no los datos en si mismos.
        //Únicamente cuando se incluye .ToList() o se recorren con for/foreach, se obtienen los datos
        //Para gestionar el lazyloading se puede hacer a nivel local, en un método concreto, o en general
        //para la app.
        public static void SinLazy()
        {
            using (var ctx = new CentroFormacionEntities())
            {//Desactivado en local.
                ctx.Configuration.LazyLoadingEnabled = false;
                // La instruccion .include sirve para que se incluyan datos de tablas
                //relacionadas (que en el diseño están relacionadas con primary/foreign key
                //Ya que no se han cargado porque estamos con lazyloading
                var alu = ctx.Alumno.Include("Curso").Where(o => o.DNI.Contains("A"));
            }

            //Para desactivar a nivel global, dejando la opción de activarlo según el método:
            //Crear una clase partial que incluya el constructor del model.
            //Luego en la llamada para crear el ctx, la llamada al entitie y se le pasa un valor
            //ya que se ha definido que reciba un valor el constructor.

            using (var ctx = new CentroFormacionEntities(false))
            {
            }


        }


        //Subselects.
        public static void Subselect()
        {
            using (var ctx = new CentroFormacionEntities())
            {   //Busca el alumno con ID (el dni), obten los cursos, de estos cursos obten los profesores que imparten
                //dicho curso y de estos profesores, su nombre
                var data = ctx.Alumno.Find("12345678A").Curso.Select(o => o.ProfesorCurso.Select(oo => oo.Profesor.Nombre));
                Console.WriteLine(data);
            }
        }

    }
}
