using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace JoinCatCode
{
    public class Mapa <T>
          where T : struct
    {
        public GameObject gameObject;

        /*--------------------*/

        public string nombre;
        public Vector3Int mapaTam;
        public Vector3 azulejoTam;
        public int cuadranteTam;
        private Dictionary<int, Cuadrante<T>> contenedorCuadrantes;
        bool generaGameObject;
        /*------------*/


        public Mapa(string nombre, Vector3Int mapaTam, Vector3 azulejoTam, int cuadranteTam, bool generaGameObject)
        {
            this.generaGameObject = generaGameObject;
            this.nombre = nombre;
            this.mapaTam = mapaTam;
            this.azulejoTam = azulejoTam;
            this.cuadranteTam = cuadranteTam;
            this.contenedorCuadrantes = new Dictionary<int, Cuadrante<T>>();
            if (generaGameObject)
            {
                this.gameObject = new GameObject(nombre);
            }
            
        }

   

        public bool ObtenerAzulejo(Vector3Int posicion, out T azulejo)
        {
            azulejo = default(T);
            if (posicion.x > 0 && posicion.x <= mapaTam.x && posicion.z > 0 && posicion.z <= mapaTam.z)
            {
                int p = FuncionesJCC.ObtenerCuadrante(posicion,cuadranteTam);
                if (contenedorCuadrantes.ContainsKey(p))
                {
                    return contenedorCuadrantes[p].ObtenerAzulejo(posicion,out azulejo);
                }
            }            
            return false;
        }


        public Capa<T> AgregarAzulejo(T dato, Vector3Int posicion)
        {
            int codigoCuadrante = FuncionesJCC.ObtenerCuadrante(posicion,cuadranteTam);
            int xCuadrante = (int)math.floor((posicion.x - 1) / cuadranteTam);
            int zCuadrante = (int)math.floor((posicion.z - 1) / cuadranteTam);
            if (contenedorCuadrantes.ContainsKey(codigoCuadrante))
            {
                return contenedorCuadrantes[codigoCuadrante].AgregarPieza(dato, posicion);
            }
            else
            {
                contenedorCuadrantes.Add(codigoCuadrante, new Cuadrante<T>(this, xCuadrante, zCuadrante, generaGameObject));
                return contenedorCuadrantes[codigoCuadrante].AgregarPieza(dato, posicion);
            }
        }

        public bool EliminarAzulejo(Vector3Int posicion)
        {
            int p = FuncionesJCC.ObtenerCuadrante(posicion, cuadranteTam);
            if (contenedorCuadrantes.ContainsKey(p))
            {
                return contenedorCuadrantes[p].EliminarAzulejo(posicion);
            }
            else
            {
                return false;
            }
        }
        public bool PosicionLibre(Vector3Int posicion)
        {
            if (posicion.x > 0 && posicion.x <= mapaTam.x && posicion.z > 0 && posicion.z <= mapaTam.z)
            {
                int p = FuncionesJCC.ObtenerCuadrante(posicion, cuadranteTam);
                if (contenedorCuadrantes.ContainsKey(p))
                {
                    return contenedorCuadrantes[p].PosicionLibre(posicion);                    
                }
                else
                {
                    return true;
                }

            }
            return false;
        }

        public List<Cuadrante<T>> RellenarMapa(int numeroCapas)
        {
            List<Cuadrante<T>> cuadrantes = new List<Cuadrante<T>>();

            int xC = mapaTam.x / cuadranteTam;
            int zC = mapaTam.z / cuadranteTam;

            if (mapaTam.x % cuadranteTam !=0)
            {
                xC++;
                zC++;
            }
            
            for (int x = 0; x < xC; x++)
            {
                for (int z = 0; z < zC; z++)
                {
                    int codigoCuadrante = Int32.Parse(x.ToString() + z.ToString());
                    Cuadrante<T> c = new Cuadrante<T>(this, x, z, generaGameObject);
                    c.RellenarCapas(numeroCapas);
                    contenedorCuadrantes.Add(codigoCuadrante, c);

                    cuadrantes.Add(c);
                }
            }
            return cuadrantes;
        }
      

        public List<Cuadrante<T>> ObtenerCuadrantes()
        {
            return contenedorCuadrantes.Values.ToList();
        }
        ~Mapa()
        {
           
        }
    }
}