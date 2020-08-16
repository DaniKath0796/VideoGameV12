using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JoinCatCode
{
    public class CuadranteNativo <T>
          where T : struct
    {
        public int cuadranteX;
        public int cuadranteZ;
        public GameObject gameObject;         
        public bool generaGameObject;
        /*-------*/
        Dictionary<int, CapaNativo<T>> contenedorCapas;
        readonly MapaNativo<T> mapa;
        private MeshCombiner meshCombiner;
        public CuadranteNativo(MapaNativo<T> mapa, int cuadranteX, int cuadranteZ,bool generaGameObject)
        {
            this.generaGameObject = generaGameObject;
            this.mapa = mapa;     
            this.cuadranteX = cuadranteX;
            this.cuadranteZ = cuadranteZ;
            this.contenedorCapas = new Dictionary<int, CapaNativo<T>>();
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

        public CapaNativo<T> AgregarPieza(T azulejo, Vector3Int posicion)
        {
           
            if (contenedorCapas.ContainsKey(posicion.y))
            {
                return contenedorCapas[posicion.y].AgregarAzulejo(azulejo,  posicion);
            }
            else
            {
              
                CapaNativo<T> c = new CapaNativo<T>(mapa, this,posicion.y, generaGameObject);
                contenedorCapas.Add(posicion.y, c);
                return contenedorCapas[posicion.y].AgregarAzulejo(azulejo,  posicion);
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

        public List<CapaNativo<T>> RellenarCapas(int Cantidadcapas)
        {
            List<CapaNativo<T>> capas = new List<CapaNativo<T>>();
            for (int i = 1; i <= Cantidadcapas; i++)
            {               
                CapaNativo<T> c = new CapaNativo<T>(mapa, this, i, generaGameObject);
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

      
        public List<CapaNativo<T>> ObtenerCapas()
        {
            return contenedorCapas.Values.ToList();
        }

        public void LiberarNativos()
        {
            foreach (var item in contenedorCapas)
            {
                item.Value.LiberarContenedorNativo();
            }
        }
    }
}