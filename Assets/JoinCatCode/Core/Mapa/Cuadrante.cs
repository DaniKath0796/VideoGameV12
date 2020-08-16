using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JoinCatCode
{
    public class Cuadrante <T>
          where T : struct
    {
        public int cuadranteX;
        public int cuadranteZ;
        public GameObject gameObject;         
        public bool generaGameObject;
        /*-------*/
        Dictionary<int, Capa<T>> contenedorCapas;
        readonly Mapa<T> mapa;
        private MeshCombiner meshCombiner;
        public Cuadrante(Mapa<T> mapa, int cuadranteX, int cuadranteZ,bool generaGameObject)
        {
            this.generaGameObject = generaGameObject;
            this.mapa = mapa;     
            this.cuadranteX = cuadranteX;
            this.cuadranteZ = cuadranteZ;
            this.contenedorCapas = new Dictionary<int, Capa<T>>();
            /*---------------------------------------------------------*/
            if (generaGameObject)
            {
                gameObject = new GameObject("Cuadrante_" + cuadranteX + "_" + cuadranteZ);
                gameObject.transform.parent = mapa.gameObject.transform;
                gameObject.transform.localPosition = new Vector3(cuadranteX * mapa.cuadranteTam, 0, cuadranteZ * mapa.cuadranteTam);
                this.meshCombiner = this.gameObject.AddComponent<MeshCombiner>();
            }
 
        }

        public bool ObtenerAzulejo(Vector3Int posicion, out T azulejo)
        {
            azulejo = default(T);
            if (contenedorCapas.ContainsKey(posicion.y))
            {
                return contenedorCapas[posicion.y].ObtenerAzulejo(posicion, out azulejo);
            }
            return false;
        }

        public Capa<T> AgregarPieza(T dato, Vector3Int posicion)
        {
            if (contenedorCapas.ContainsKey(posicion.y))
            {
                return contenedorCapas[posicion.y].AgregarAzulejo(dato,  posicion);
            }
            else
            {
                Capa<T> c = new Capa<T>(mapa, this,posicion.y, generaGameObject);
                contenedorCapas.Add(posicion.y, c);
                return contenedorCapas[posicion.y].AgregarAzulejo(dato,  posicion);
            }
        }
        public bool EliminarAzulejo(Vector3Int posicion)
        {
            if (contenedorCapas.ContainsKey(posicion.y))
            {
                return contenedorCapas[posicion.y].EliminarAzulejo(posicion);
            }
                return false;
            
        }
        public bool PosicionLibre(Vector3Int posicion)
        {
            if (contenedorCapas.ContainsKey(posicion.y))
            {
                return contenedorCapas[posicion.y].PosicionLibre(posicion);
            }
            return true;          
        }

        public List<Capa<T>> RellenarCapas(int Cantidadcapas)
        {
            List<Capa<T>> capas = new List<Capa<T>>();
            for (int i = 1; i <= Cantidadcapas; i++)
            {               
                Capa<T> c = new Capa<T>(mapa, this, i, generaGameObject);
                capas.Add(c);
                contenedorCapas.Add(i, c);
            }
           
            return capas;
        }

        public IEnumerator CombinarMallas()
        {
            meshCombiner.CombineMeshes(true);
            Mesh mesh = this.gameObject.GetComponent<MeshFilter>().sharedMesh;
            FuncionesJCC.GuardarCombinarMallas(mesh, "JoinCatCode/Mapa/MapasCreados",mapa.nombre);
            yield break;
        }

      
        public List<Capa<T>> ObtenerCapas()
        {
            return contenedorCapas.Values.ToList();
        }

   
    }
}