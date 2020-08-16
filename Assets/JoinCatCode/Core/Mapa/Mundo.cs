using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;

namespace JoinCatCode
{
    // Solo existe un mundo por juego
    public class Mundo
    {
        enum GridSelector
        {
            nuevo,
            antiguo
        }

        string nombre;
        MapaVoxel mapaVoxel;
        Vector3Int tamAzulejo;
        Vector3Int tamMundo;
        //
        MapaNativo<Azulejo> mapaEstructuras;
        MapaNativo<Azulejo> mapaArboles;
        MapaNativo<Entity> mapaRecursos;

        Mapa<byte>    mapaPosiciones;
        
        GameObject GridPosicion;
        public Mundo (string nombre, int ancho, int altura, int largo, int idMaterialTerreno)
        {
           
          
            //Output the Game data path to the console
            this.nombre = nombre;
            tamAzulejo = new Vector3Int(1, 1, 1);
            tamMundo.x = ancho;
            tamMundo.z = largo;
            tamMundo.y = altura;
            //Se asume que se maneja texturas de 128 x 128 un total de 16 texturas de largo 16 ancho
            mapaVoxel = new MapaVoxel(nombre, idMaterialTerreno, new Vector3Int(ancho, altura, largo), 16);
            mapaVoxel.LlenarMapaTodo(1);
            mapaPosiciones = new Mapa<byte>(nombre, new Vector3Int(ancho, altura, largo), new Vector3Int(1, 1, 1), 30, false);
            mapaRecursos = new MapaNativo<Entity>(nombre, new Vector3Int(ancho, altura, largo), new Vector3(1, 1, 1), 30,false);
        }

        public Mundo()
        { 
        
        }
        ~Mundo()
        {
           // mapaPosiciones.LiberarNativos();
        }
       
        public void AgregarRecurso(int idRecurso, Vector3Int posicion, Vector2 ini, Vector2 fin)
        {
            if (PosicionValida(posicion))
            {
                posicion.y = posicion.y + 1;               
                if (!mapaVoxel.ExisteVoxel(posicion))
                {

                    RecursosPlantilla rp = AdministradorRecursos.Instanciar().ObtenerRecurso(idRecurso);


                    bool flag = false;
                    for (int x = (int)ini.x; x <= (int)fin.x; x++)
                    {
                        for (int z = (int)ini.y; z <= (int)fin.y; z++)
                        {
                            if (!mapaPosiciones.PosicionLibre(new Vector3Int(x, posicion.y, z)))
                            {                               
                                return;
                            }
                            flag = true;
                        }
                    }
                    if (flag)
                    {     
                        Entity ent = AdministradorRecursos.Instanciar().CrearRecurso(idRecurso, new Unity.Mathematics.float3(posicion.x - 1, posicion.y, posicion.z - 1));

                        mapaRecursos.AgregarAzulejo(ent,new Vector3Int (posicion.x, posicion.y-1, posicion.z));
                        for (int x = (int)ini.x; x <= (int)fin.x; x++)
                        {
                            for (int z = (int)ini.y; z <= (int)fin.y; z++)
                            {
                                mapaPosiciones.AgregarAzulejo(1, new Vector3Int(x, posicion.y,z));
                            }
                        }

                    }
                }
            }
        }

        public void removerRecurso(Vector3Int posicion)
        {
            Entity entidad;
            if (mapaRecursos.ObtenerAzulejo(posicion, out entidad))
            {
              
                mapaRecursos.EliminarAzulejo(posicion);
                EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entityManager.DestroyEntity(entidad);
            }

            
        }
        public void AgregarAzulejo(int idAzulejo, Vector3Int posicion)
        {
            AdministradorAzulejos.Instanciar().ObtenerAzulejo(ClaseAzulejo.Arbol,idAzulejo);
            if (PosicionValida(posicion))
            {
                Azulejo azulejo = new Azulejo();
                mapaArboles.AgregarAzulejo(azulejo,posicion);
            }
        }
        public void AgregarVoxelTerreno(int idVoxel,Vector3Int posicion)
        { 
            if (PosicionValida(posicion))
            {
                mapaVoxel.AgregarAzulejoVoxel(idVoxel, posicion);
            }
            
        }
        public int VoxelTerrenoCapaLibre(Vector3Int posicion)
        {
            int capaLibre = 1;
            if (PosicionValida(posicion))
            {
                while (mapaVoxel.ExisteVoxel(posicion))
                {                    
                    posicion.y +=1;
                }
                capaLibre = posicion.y;
            }                     
            return capaLibre;
        }
        public void RemoverVoxelTerreno(Vector3Int posicion)
        {
            if (PosicionValida(posicion))
            {
                mapaVoxel.AgregarAzulejoVoxel(0, posicion);
            }
        }

        public bool PosicionValida(Vector3Int posicion)
        {
            if (posicion.x <= 0 || posicion.x > tamMundo.x || posicion.z <= 0 || posicion.z > tamMundo.z)
            {
                return false;
            }
            return true;
        }

        public void Guardar()
        {
            string m_Path = Application.dataPath;
            string ruta = m_Path + "/JoinCatCode/ArchivosGuardados/" + nombre; 
            try
            {
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);             
                }
                mapaVoxel.GuardarMapa(ruta, OdinSerializer.DataFormat.Binary);                            
            }
            catch (IOException ex)
            {
                Debug.LogError("Mundo :" + ex);
            }                        
        }
        public void Cargar()
        {
            Debug.Log("Liberado");            
            Liberar();
            Debug.Log("Cargando");
            mapaVoxel = new MapaVoxel();
            
            string m_Path = Application.dataPath;
            string ruta = m_Path + "/JoinCatCode/ArchivosGuardados/" + "Mundo1";
            mapaVoxel.CargarMapa(ruta, "Mundo1", DataFormat.Binary);
        }

        void CrearGrid(int xMax, int zMax)
        {
            var gridPrefab = AssetDatabase.LoadAssetAtPath("Assets/JoinCatCode/Core/Prefabs/GridSelector.prefab", typeof(UnityEngine.Object));
            for (int x = 0; x < xMax; x++)
            {
                for (int z = 0; z < zMax; z++)
                {
                    GameObject nuevo = GameObject.Instantiate(gridPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                    nuevo.transform.localPosition = new Vector3(x, 0, z);
                    nuevo.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    nuevo.transform.parent = GridPosicion.transform;
                }
            }
        }
        public void Liberar()
        {
            mapaVoxel.Liberar();
            mapaRecursos.LiberarNativos();
            GC.SuppressFinalize(mapaVoxel);
            GC.SuppressFinalize(mapaRecursos);
        }
        //public Entity CrearEntidad(Vector3Int posicion)
        //{
        //    EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //    EntityArchetype archetype = entityManager.CreateArchetype(
        //        typeof(Translation),               
        //        typeof(RenderMesh),
        //        typeof(RenderBounds),
        //        typeof(LocalToWorld)
        //        );

        //    Entity myEntity = entityManager.CreateEntity(archetype);
        //    entityManager.SetName(myEntity, posicion.x + "_" + posicion.z);
        //    entityManager.AddComponentData(myEntity, new Translation
        //    {
        //        Value = new float3(posicion.x, posicion.y, posicion.z)
        //    });

        //    RenderBounds redn = new RenderBounds();
        //    redn.Value = mesh.bounds.ToAABB();

        //    entityManager.AddComponentData(myEntity, redn);

        //    entityManager.AddSharedComponentData(myEntity, new RenderMesh
        //    {
        //        mesh = mesh,
        //        material = material
        //    });
        //    return myEntity;
        //}
        public Vector3Int ObtenerPosicionIsometrica(int capa)
        {
            CamaraGrid.instanciar().actualizarPosicion(1, capa);
            /*----*/
            Vector3 Posicion = new Vector3(0, 0, 0);
            Plane PlanoX = new Plane(Vector3.up, 0);
            Ray ray = CamaraMovimientoSistema.CamaraLocal.ScreenPointToRay(Input.mousePosition);
            float distancia;
            if (PlanoX.Raycast(ray, out distancia))
            {
                Posicion = ray.GetPoint(distancia);
            }

            /*--------------------------------------*/
            //Debug.Log("Posicion REal:"+Posicion);
            float r = (capa - 1) + 0.25f * (capa - 1);
            int x = Mathf.FloorToInt(Posicion.x - r / tamAzulejo.x) + 1;
            int y = capa; // Mathf.FloorToInt(Posicion.y / CamaraConfiguracionComponente.azulejoTam.y) + 1;
            int z = Mathf.FloorToInt(Posicion.z - r / tamAzulejo.z) + 1;
            if (x<0 || z<0 || x>tamMundo.x || z>tamMundo.z)
            {
                return new Vector3Int { x = -1, y = -1, z = -1 };
            }
            return new Vector3Int { x = x, y = y, z = z };
            //CamaraPosicionComponente.posicionIsometrica = new Vector3Int { x = x, y = y, z = z };
            //CamaraPosicionComponente.posicionNormal = Posicion;
            //CamaraPosicionComponente.posicionNumerica = (tamMundo.x * x) - tamMundo.z + z;

            //if (CamaraPosicionComponente.posicionIsometrica.x <= 0 || CamaraPosicionComponente.posicionIsometrica.x > ConfiguracionMapa.mapaTam.x || CamaraPosicionComponente.posicionIsometrica.z <= 0 || CamaraPosicionComponente.posicionIsometrica.z > ConfiguracionMapa.mapaTam.z)
            //{

            //    CamaraPosicionComponente.posicionIsometrica = new Vector3Int(0, 0, 0);
            //    CamaraPosicionComponente.posicionNormal = new float3(0f, 0f, 0f);
            //    CamaraPosicionComponente.posicionNumerica = 0;
            //    return;
            //}
            //else
            //{
            //    // NotificarPantalla?.Invoke(this, PosicionIsometrica);
            //}
        }


    }
}