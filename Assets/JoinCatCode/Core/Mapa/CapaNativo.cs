using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace JoinCatCode
{
    
    public class CapaNativo <T>
        where T : struct
    {
        
        MapaNativo<T> mapa;
        CuadranteNativo<T> cuadrante;
        int capa;
        int cantidadElementos;
        //NativeHashMap<int, T> contenedorAzulejos;
        NativeHashMap<int, T> contenedorAzulejos;
        AdministradorAzulejos adminAzulejos;
        GameObject gameObject;
        public CapaNativo(MapaNativo<T> mapa, CuadranteNativo<T> cuadrante, int capa, bool generaGameObject)
        {
            this.mapa = mapa;
            this.cuadrante = cuadrante;
            this.capa = capa;
            adminAzulejos = AdministradorAzulejos.Instanciar();            
            contenedorAzulejos = new NativeHashMap<int, T>(mapa.cuadranteTam* mapa.cuadranteTam, Allocator.Persistent);
            /*-----------*/
            if (gameObject)
            {
                gameObject = new GameObject("Capa:" + capa);
                gameObject.transform.parent = cuadrante.gameObject.transform;
                gameObject.transform.localPosition = new Vector3(0, capa - 1, 0);
            }            
        }

        public CapaNativo<T> AgregarAzulejo(T azulejo,Vector3Int posicion)
        {
        
            int p = FuncionesJCC.ObtenerPosicionUnica(mapa.mapaTam.x, mapa.mapaTam.z, posicion);

            Debug.Log("Posicion Unica:" + p);
            Debug.Log("Posicion Tile:" + posicion);
            if (!contenedorAzulejos.ContainsKey(p))
            {
                contenedorAzulejos.Add(p, azulejo);
                return this;
            }
            return null;            
        }

        public bool ObtenerAzulejo(Vector3Int posicion, out T azulejo)
        {
            azulejo = default(T);
            int p =FuncionesJCC.ObtenerPosicionUnica(mapa.mapaTam.x, mapa.mapaTam.z, posicion);

         
            if (contenedorAzulejos.ContainsKey(p))
            {
               
                azulejo = contenedorAzulejos[p];
                return true;
            }
            return false;
        }

        public bool EliminarAzulejo(Vector3Int posicion)
        {            
            int p = FuncionesJCC.ObtenerPosicionUnica(mapa.mapaTam.x, mapa.mapaTam.z, posicion);
            if (contenedorAzulejos.ContainsKey(p))
            {                
                contenedorAzulejos.Remove(p);
                return true;
            }
            return false;
        }


        public bool PosicionLibre(Vector3Int posicion)
        {
            int p = FuncionesJCC.ObtenerPosicionUnica(mapa.mapaTam.x, mapa.mapaTam.z, posicion);
            if (!contenedorAzulejos.ContainsKey(p))
            {
                return true;
            }
            return false;
        }
      


        public void LiberarContenedorNativo()
        {
            contenedorAzulejos.Dispose();
        }
      /*  public struct JobAzulejo : IJobParallelFor
        {                     
            [ReadOnly] public int capa;

            public void Execute(int index)
            {                
                Azulejo azulejo = new Azulejo { id = index, posicionNumerica = 0, tipo = tipoAzulejo.Simple, x = 0, y = capa, z = 0 };
                contenedorAzulejos[index] = azulejo;
                AdministradorGameObjects.Instanciar().InstanciarGameObject();
            }
        }*/
        
    }
}
