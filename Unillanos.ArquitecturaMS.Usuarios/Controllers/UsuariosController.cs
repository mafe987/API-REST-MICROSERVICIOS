using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
namespace Unillanos.ArquitecturaMS.Usuarios.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        [HttpGet]
        [Route("LeerUsuario/{correo}")]
        public Usuarios LeerUsuario(string correo)
        {
            Servicios objServicios = new Servicios();
            return objServicios.Leer(correo);
        }

        [HttpPut]
        [Route("ActualizarUsuario")]
        public string Actualizar(Usuarios usuario)
        {
            Servicios objServicios = new Servicios();
            return objServicios.ActualizarUsuario(usuario);
        }


        [HttpPost]
        [Route("InsertarUsuario")]
        public string Insertar(Usuarios usuario)
        {
            //Lógica para leer de la BD, o recursos externos
            Servicios objServicios = new Servicios();
            return objServicios.Insertar(usuario);
        }

        [HttpDelete]
        [Route("EliminarUsuario/{correo}")]
        public string EliminarUsuario(string correo)
        {
            //Lógica para leer de la BD, o recursos externos
            Servicios objServicios = new Servicios();
            return objServicios.Eliminar(correo);
        }

        public class Servicios
        {
            public string Insertar(Usuarios usuario)
            {     
                validacion objVal = new validacion();
                string json = objVal.leerArchivo();

                List<Usuarios> listaUsuarios= new List<Usuarios>();
                listaUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(json); 

                if(listaUsuarios == null)
                {
                    listaUsuarios=new List<Usuarios>(); 
                }
                
                listaUsuarios.Add(usuario);

                json=JsonConvert.SerializeObject(listaUsuarios);
                objVal.escribirArchivo(json);
                
                return "Usuario agregado con éxito";
            }

            public Usuarios Leer(string correo)
            {
                validacion objVal = new validacion();
                string json = objVal.leerArchivo();

                List<Usuarios> listaUsuarios = new List<Usuarios>();
                listaUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(json);
                Usuarios usuarioEncontrado = new Usuarios();    

                for (int i = 0; i < listaUsuarios.Count; i++)
                {
                    if (listaUsuarios[i].Correo.Equals(correo))
                    {
                        usuarioEncontrado = listaUsuarios[i];
                        return usuarioEncontrado;
                    }
                }

                return null;
               
            }

            public string Eliminar(string correo)
            {
                validacion objVal = new validacion();
                string json = objVal.leerArchivo();

                List<Usuarios> listaUsuarios = new List<Usuarios>();
                listaUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(json);
                Boolean borrado = false;

                for (int i = 0; i < listaUsuarios.Count; i++)
                {
                    if (listaUsuarios[i].Correo.Equals(correo))
                    {
                        listaUsuarios.RemoveAt(i);
                        
                        borrado= true;  
                    }
                }

                if (borrado)
                {
                    json = JsonConvert.SerializeObject(listaUsuarios);
                    objVal.escribirArchivo(json);
                    return "Usuario eliminado con éxito";
                }
                else
                {
                    return "Usuario no encontrado";
                }
                
            }

            public string ActualizarUsuario(Usuarios usuario)
            {
                validacion objVal = new validacion();
                string json = objVal.leerArchivo();

                List<Usuarios> listaUsuarios = new List<Usuarios>();
                listaUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(json);
                Boolean actualizado = false;

                for (int i = 0; i < listaUsuarios.Count; i++)
                {
                    if (listaUsuarios[i].Correo.Equals(usuario.Correo))
                    {
                        listaUsuarios[i] = usuario;
                        
                        actualizado = true;
                    }
                }

                if (actualizado)
                {
                    json = JsonConvert.SerializeObject(listaUsuarios);
                    objVal.escribirArchivo(json);
                    return "Usuario actualizado con éxito";
                }
                else
                {
                    return "Usuario no encontrado";
                }
            }


        }

        public class validacion
        {
            public string leerArchivo()
            {
                string path = @"Archivo.json";
                if (System.IO.File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        return sr.ReadToEnd();
                    }
                }
                else
                {
                    StreamWriter sw= System.IO.File.CreateText(path);
                    using (StreamReader sr = new StreamReader(path))
                    {
                        return sr.ReadToEnd();
                    }
                }
               
            }

            public void escribirArchivo(String json)
            {
                string path = @"Archivo.json";               
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(json);
                    
                }
            }
        }


        public class Usuarios
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Sexo { get; set; }
            public string Correo { get; set; }
            public string Telefono { get; set; }
            public string Edad { get; set; }

        }

    }
}
