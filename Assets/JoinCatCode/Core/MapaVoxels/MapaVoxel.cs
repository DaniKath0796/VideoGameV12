
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using OdinSerializer;
using System;
using System.IO;

namespace JoinCatCode
{
    [Serializable]
    public class MapaVoxel 
    {
        [NonSerialized]
        public Entity entidad;
        [NonSerialized]
        public GameObject gameObject;
        [NonSerialized]
        public Material material;
        [NonSerialized]
        public BloqueVoxel[,] contenedorVoxels;

        public Vector3 voxelTam;
        public Vector3 mapaTam;
        public int texturasCuadrante; // Cantidad de texturas por cuadrante Longitud Ancho 
        public string nombre;               
        [SerializeField]
        int xV;
        [SerializeField]
        int zV;
        [SerializeField]
        int idMaterial;
       public MapaVoxel(string nombre,int idMaterial, Vector3Int mapaTam, int texturasCuadrante)
        {
            this.idMaterial = idMaterial;
            this.material = AdministradorMateriales.Instanciar().ObtenerMaterial(idMaterial).material;
            this.nombre = "MapaVoxel_"+nombre;
            this.mapaTam = mapaTam;            
            this.texturasCuadrante = texturasCuadrante;
            voxelTam = new Vector3Int(10, mapaTam.y, 10);
             xV = (int) (mapaTam.x /  voxelTam.x);
             zV = (int) (mapaTam.z / voxelTam.z);

            contenedorVoxels = new BloqueVoxel[xV,zV];
            this.gameObject = new GameObject(nombre);
            Debug.Log("MapaTam:"+mapaTam);
         /*   for (int x = 0; x < xV; x++)
            {
                for (int z = 0; z < zV; z++)
                {
                    BloqueVoxel nuevo = new BloqueVoxel(this, new Vector3(x, 0, z));                    
                    contenedorVoxels[x, z] = nuevo;
                }
            }*/

        }

        public MapaVoxel()
        {           

        }
        public bool ExisteVoxel(Vector3Int posicion )
        {
            int x = (int)math.floor((posicion.x - 1) / voxelTam.x);
            int z = (int)math.floor((posicion.z - 1) / voxelTam.x);
          
            if (x >= 0 && x < xV  && z >= 0 && z<zV)
            {                               
                BloqueVoxel b = contenedorVoxels[x, z];
                 return b.ExisteVoxel(posicion);
            }

            return false;
        }
        public void LlenarMapaTodo(int numeroCapas)
        {
            int xV = (int) (mapaTam.x / voxelTam.x);
            int zV = (int) (mapaTam.z / voxelTam.z);
            for (int x = 0; x < xV; x++)
            {
                for (int z = 0; z < zV; z++)
                {
                    BloqueVoxel nuevo = new BloqueVoxel(this, new Vector3(x, 0, z));
                    nuevo.LlenarBloqueCompleto(numeroCapas);
                    contenedorVoxels[x, z] = nuevo;
                }
            }
        }

        public void LlenarMapaTodo()
        {
            int xV = (int)(mapaTam.x / voxelTam.x);
            int zV = (int)(mapaTam.z / voxelTam.z);
            for (int x = 0; x < xV; x++)
            {
                for (int z= 0; z < zV; z++)
                {                    
                    BloqueVoxel nuevo = new BloqueVoxel(this, new Vector3(x,0,z));
                    nuevo.LlenarBloqueCompleto();
                    contenedorVoxels[x, z] = nuevo;
                }
            }
        }
        public void  AgregarAzulejoVoxel(int idAzuelejoVoxel, Vector3Int posicion)
        {
            if (posicion.x > 0 && posicion.x <= mapaTam.x && posicion.z > 0 && posicion.z <= mapaTam.z)
            {
                ObtenerBloqueVoxel(posicion).EditarVoxel(posicion, (byte)idAzuelejoVoxel);
            }
        }


        public BloqueVoxel ObtenerBloqueVoxel(Vector3 posicion)
        {

            int x = (int)math.floor((posicion.x - 1) / voxelTam.x);
            int z = (int)math.floor((posicion.z - 1) / voxelTam.x);
            if (posicion.x <= 0 || posicion.z <= 0)
            {
                x = 0;
                z = 0;
            }
            if (posicion.x >= 0 && posicion.z >= 0)
            {
                Debug.Log("voxel" + x + "_" + z);
                    return contenedorVoxels[x,z];                
            }
            
            return null;

        }

        bool IsChunkInWorld(Vector3 coord)
        {

            if (coord.x > 0 && coord.x < mapaTam.x - 1 && coord.z > 0 && coord.z < mapaTam.z - 1)
                return true;
            else
                return
                    false;

        }

        public void CargarMapa(string rutaArchivo,string nombre, DataFormat tipoFormato)
        {
            string ruta = rutaArchivo + "/MapaVoxel_" + nombre;
            string rutaNombre = ruta + "/MapaVoxel_" + nombre + ".jcc";
            if (!File.Exists(rutaNombre))
            {
                return; // No state to load
            }
            else
            {
                
                byte[] bytes = File.ReadAllBytes(rutaNombre);
                MapaVoxel temp = SerializationUtility.DeserializeValue<MapaVoxel>(bytes, tipoFormato);
                this.mapaTam = temp.mapaTam;
                this.nombre = temp.nombre;
                this.texturasCuadrante = temp.texturasCuadrante;
                this.voxelTam = temp.voxelTam;
                this.xV = temp.xV;
                this.zV = temp.zV;
                this.idMaterial = temp.idMaterial;
                this.material = AdministradorMateriales.Instanciar().ObtenerMaterial(temp.idMaterial).material;
                /*-----*/
                this.gameObject = new GameObject(nombre+"Cargado");
                contenedorVoxels = new BloqueVoxel[this.xV, this.zV];
                for (int x = 0; x < xV; x++)
                {
                    for (int z = 0; z < zV; z++)
                    {
                        BloqueVoxel nuevo = new BloqueVoxel(this, new Vector3(x, 0, z));

                        nuevo.CargarBloque(ruta, tipoFormato);
                        contenedorVoxels[x, z] = nuevo;
                    }
                }
            }
        }
        public void GuardarMapa(string rutaArchivo, DataFormat tipoFormato)
        {            
            string ruta = rutaArchivo+"/"+nombre;
            try
            {
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                /*-----------*/
                var bytes = SerializationUtility.SerializeValue(this, tipoFormato);
                if (tipoFormato == DataFormat.JSON)
                {
                    string rutaMapa = ruta + "/" +nombre+ ".json";
                    var jsonString = System.Text.Encoding.UTF8.GetString(bytes);
                    File.WriteAllText(rutaMapa, jsonString);
                }
                else
                {
                    string rutaMapa = ruta + "/" + nombre + ".jcc";
                    File.WriteAllBytes(rutaMapa, bytes);
                }
                /*-----------*/
                for (int x = 0; x < xV; x++)
                {
                    for (int z = 0; z < zV; z++)
                    {
                        contenedorVoxels[x, z].GuardarBloque(ruta, tipoFormato);
                    }
                }              
            }
            catch (IOException ex)
            {
                Debug.LogError("MapaVoxel:"+ex);                
            }
         
        }

        public void Liberar()
        {
            for (int x = 0; x < xV; x++)
            {
                for (int z = 0; z < zV; z++)
                {
                    contenedorVoxels[x, z].Liberar();                    
                    GC.SuppressFinalize(contenedorVoxels[x, z]);
                }
            }
            
            GameObject.DestroyImmediate(this.gameObject);
        }
       
    }
}