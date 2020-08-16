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
    
    public class Capa <T>
        where T : struct
    {
        
        Mapa<T> mapa;
        Cuadrante<T> cuadrante;
        int capa;
        int cantidadElementos; 
        Dictionary<int, T> contenedorAzulejos;
        AdministradorAzulejos adminAzulejos;
        GameObject gameObject;
        public Capa(Mapa<T> mapa, Cuadrante<T> cuadrante, int capa, bool generaGameObject)
        {
            this.mapa = mapa;
            this.cuadrante = cuadrante;
            this.capa = capa;
            adminAzulejos = AdministradorAzulejos.Instanciar();            
            contenedorAzulejos = new Dictionary<int, T>();
            /*-----------*/
            if (gameObject)
            {
                gameObject = new GameObject("Capa:" + capa);
                gameObject.transform.parent = cuadrante.gameObject.transform;
                gameObject.transform.localPosition = new Vector3(0, capa - 1, 0);
            }            
        }

        public Capa<T> AgregarAzulejo(T dato,Vector3Int posicion)
        {            
            int p = FuncionesJCC.ObtenerPosicionUnica(mapa.mapaTam.x, mapa.mapaTam.z, posicion);
            if (!contenedorAzulejos.ContainsKey(p))
            {
                contenedorAzulejos.Add(p,dato);
                return this;
            }
            return null;
        }

        public bool ObtenerAzulejo(Vector3Int posicion, out T azulejo)
        {
            azulejo = default(T);
            int p = FuncionesJCC.ObtenerPosicionUnica(mapa.mapaTam.x, mapa.mapaTam.z, posicion);
            if (contenedorAzulejos.ContainsKey(p))
            {
                azulejo= contenedorAzulejos[p];
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
        public void RellenarCapa(ClaseAzulejo claseAzulejo, int idAzulejo)
        {
            
        }

   /*     public IEnumerator CrearCapaLlena(ClaseAzulejo claseAzulejo, int idAzulejo)
        {
            
            int cantidad = mapa.cuadranteTam;
            AzulejoPlantilla azulejoPlantilla = adminAzulejos.ObtenerAzulejo(claseAzulejo, idAzulejo);
            for (int x = 1; x <= cantidad; x++)
            {
                for (int z = 1; z <= cantidad; z++)
                {
                  
                    int p = FuncionesJCC.ObtenerPosicionUnica(mapa, x+(cuadrante.cuadranteX * mapa.cuadranteTam), z+(cuadrante.cuadranteZ*mapa.cuadranteTam));

                    GameObject nuevo = GameObject.Instantiate(azulejoPlantilla.prefab);
                    nuevo.name = x + "_" + z+":"+p;
                    nuevo.transform.parent = gameObject.transform;
                    nuevo.transform.localPosition = new Vector3(x-1, mapa.azulejoTam.y / 2, z-1);

                    
                    Azulejo azulejo = new Azulejo
                    {
                        posicionNumerica = p,
                        clase = claseAzulejo,
                        id = azulejoPlantilla.id,
                        tipo = azulejoPlantilla.tipo,
                        x = x -1,
                        y = capa,
                        z = z -1
                    };
                    contenedorAzulejos.Add(p, azulejo);                
                }
            }
            yield break;
         
        }*/
  /*      public IEnumerator CrearCapaLlena(ClaseAzulejo claseAzulejo, int idAzulejo, float periodo)
        {
            int cantidad = mapa.cuadranteTam ;
            AzulejoPlantilla azulejoPlantilla = adminAzulejos.ObtenerAzulejo(claseAzulejo, idAzulejo);
            var wait = new WaitForSeconds(periodo);

            for (int x = 1; x <= cantidad; x++)
            {
                for (int z = 1; z <= cantidad; z++)
                {
                    int p = FuncionesJCC.ObtenerPosicionUnica(mapa, x + (cuadrante.cuadranteX * mapa.cuadranteTam), z + (cuadrante.cuadranteZ * mapa.cuadranteTam));

                    GameObject nuevo = GameObject.Instantiate(azulejoPlantilla.prefab);
                    nuevo.name = x + "_" + z + ":" + p;
                    nuevo.transform.parent = gameObject.transform;
                    nuevo.transform.localPosition = new Vector3(x - 1, mapa.azulejoTam.y/2, z - 1);


                    Azulejo azulejo = new Azulejo
                    {
                        posicionNumerica = p,
                        clase = claseAzulejo,
                        id = azulejoPlantilla.id,
                        tipo = azulejoPlantilla.tipo,
                        x = x - 1,
                        y = capa,
                        z = z - 1
                    };
                    contenedorAzulejos.Add(p, azulejo);
                    yield return wait;
                }
            }
                                
        }*/
        public bool PosicionLibre(Vector3Int posicion)
        {
            int p = FuncionesJCC.ObtenerPosicionUnica(mapa.mapaTam.x, mapa.mapaTam.z, posicion);
            if (!contenedorAzulejos.ContainsKey(p))
            {
                return true;
            }
            return false;
        }
        /*   public void RellenarCapa(int idAzulejo)
           {
               JobAzulejo jobAzulejo = new JobAzulejo
               {               
                   capa = 1           
               };
               JobHandle jobHandle = jobAzulejo.Schedule(cantidadElementos, 100);
               jobHandle.Complete();

               if (jobHandle.IsCompleted)
               {
                   Debug.Log("Termine la Carga");

               }   
           }*/



    
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
