using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JoinCatCode
{
    [UpdateAfter(typeof(SistemaEdicionTeclas))]
    public class SistemaPreview : ComponentSystem
    {
        GameObject gridPrevioGameObject = null;
        int capa = 0;
        int xAzul = 0;
        int zAzul = 0;
        int xMedioAzul = 0;
        int zMedioAzul = 0;
        int xInferiorAzul = 0;
        int zInferiorAzul = 0;
       
        protected override void OnStartRunning()
        {
            AdministradorMundos admin;
            admin = AdministradorMundos.Instanciar();
            admin.CrearNuevoMundo("Mundo1", 250, 10, 250, 1);
            admin.SetearMundoActual("Mundo1");
          

            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/JoinCatCode/Core/Prefabs/GridLimite.prefab", typeof(GameObject));
            gridPrevioGameObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        }
        protected override void OnUpdate()
        {
            // Assign values to local variables captured in your job here, so that it has
            // everything it needs to do its work when it runs later.
            // For example,
            //     float deltaTime = Time.DeltaTime;

            // This declares a new kind of job, which is a unit of work to do.
            // The job is declared as an Entities.ForEach with the target components as parameters,
            // meaning it will process all entities in the world that have both
            // Translation and Rotation components. Change it to process the component
            // types you want.
          

            Entities.ForEach((ref EdicionModoActualData modo) =>
            {
                ModoEdicion         modoEdicion = modo.modoEdicion;
                TipoEdicion         tipoEdicion = modo.tipoEdicion;
                int                 idAnterior  = modo.idAnterior;
                capa                            = modo.capaActual;
                int                 idAzulejo   = modo.idActual;
                modo.posicionValida             = 0;
                Vector3Int pos = AdministradorMundos.MundoActual().ObtenerPosicionIsometrica(capa);
                
                if (pos.x > 0 && pos.z > 0)
                {
                    Vector3 posPreview = new Vector3(0, capa - 0.48f, 0);
                   
                    if (modoEdicion == ModoEdicion.Crear)
                    {
                        if (tipoEdicion == TipoEdicion.Recursos && idAnterior != idAzulejo)
                        {
                            modo.idAnterior = idAzulejo;
                            Entity entidadPreview = AdministradorRecursos.Instanciar().CrearRecurso(idAzulejo, new Unity.Mathematics.float3(0,0,0));
                            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                            entityManager.SetName(entidadPreview, "AzulejoPreview");
                            Rotation rotation = new Rotation { Value = quaternion.Euler(0, 45, 0) };
                            Translation translation = new Translation { Value = new float3(0,0,0) };
                            entityManager.AddComponent<AzulejoPreviewComponent>(entidadPreview);
                            //entityManager.AddComponent<EdicionModoActualData>(entidad);
                            entityManager.AddComponentData(entidadPreview, rotation);
                            entityManager.AddComponentData(entidadPreview, translation);

                            RecursosPlantilla recurso = AdministradorRecursos.Instanciar().ObtenerRecurso(idAzulejo);
                            xAzul = recurso.xMax;
                            zAzul = recurso.zMax;
                            gridPrevioGameObject.transform.localScale = new Vector3(0.1f * recurso.xMax, 1 , 0.1f*recurso.zMax);
                         
                            gridPrevioGameObject.GetComponent<Renderer>().material.SetVector("Vector2_642EB6F1", new Vector4(recurso.xMax, recurso.zMax, 0, 0));

                        }                                                                                             
                    }
                    if (modoEdicion == ModoEdicion.Seleccion || modoEdicion == ModoEdicion.Eliminar)
                    {
                        
                        if (HasSingleton<AzulejoPreviewComponent>())
                        {
                           
                            modo.idActual = modo.idAnterior;
                            modo.idAnterior = -1;
                            Entity ent = GetSingletonEntity<AzulejoPreviewComponent>();
                            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                            entityManager.DestroyEntity(ent);
                            Debug.Log("entre seleccion");
                        }

                        xAzul = 1;
                        zAzul = 1;                       
                    }
                    modo.posicionTileActual = pos;
                    modo.posicionValida = 1;
                    actualizarPosicionGrid(posPreview,pos);

                }
            });

            Entities.ForEach((ref AzulejoPreviewComponent azulejoPreview, ref Translation translation, ref Rotation rotation, ref LocalToWorld localToWorld) =>
            {
                Vector3Int pos = AdministradorMundos.MundoActual().ObtenerPosicionIsometrica(capa);

                if (pos.x > 0 && pos.z > 0)
                {
                    azulejoPreview.posicionInicial = new Vector2(pos.x - xInferiorAzul, pos.z - zInferiorAzul);
                    azulejoPreview.posicionFinal = new Vector2(pos.x + xMedioAzul, pos.z + zMedioAzul);
                    azulejoPreview.posicionAzulejo = pos;
                    pos.x = pos.x - 1;
                    pos.z = pos.z - 1;                               
                    Vector3 medio = puntoMedio(pos, new Vector3(pos.x + xAzul, 0, pos.z + zAzul));             
                    localToWorld.Value = float4x4.TRS(new float3(medio.x-(0.5f*xAzul), pos.y + 1, medio.z-(0.5f*zAzul)), Quaternion.Euler(0, 45, 0), new float3(1, 1, 1));                                      
                    /*  translation = new Translation { Value = new float3(pos.x-1, pos.y+1, pos.z-1) };
                      rotation = new Rotation { Value = quaternion.Euler(0, 45, 0) };*/
                }
            });
        }

        void actualizarPosicionGrid(Vector3 posPreview, Vector3Int pos)
        {
            if (xAzul % 2 == 0)
            {
                posPreview.x = pos.x - 0.5f;
                xMedioAzul = xAzul / 2;
                xInferiorAzul = xMedioAzul - 1;
            }
            else
            {
                posPreview.x = pos.x - 1;
                xMedioAzul = (xAzul % 2) + 1;
                xInferiorAzul = (xAzul / 2);
                if (xAzul == 1)
                {
                    xMedioAzul = 0;
                }
            }
            if (zAzul % 2 == 0)
            {
                posPreview.z = pos.z - 0.5f;
                zMedioAzul = zAzul / 2;
                zInferiorAzul = zMedioAzul - 1;
            }
            else
            {
                posPreview.z = pos.z - 1;
                zMedioAzul = (zAzul % 2) + 1;
                zInferiorAzul = (zAzul / 2);
                if (zAzul == 1)
                {
                    zMedioAzul = 0;
                }
            }
            gridPrevioGameObject.transform.position = posPreview;
        }

        Vector3 puntoMedio( Vector3 a , Vector3 b)
        {
            float sumX = a.x + b.x;
            float sumY = a.y + b.y;
            float sumZ = a.z + b.z;
            return new Vector3((sumX / 2f), sumY / 2f, sumZ / 2f);
        }

     
    }
    public struct AzulejoPreviewComponent : IComponentData
    {
        public Vector2 posicionInicial;
        public Vector2 posicionFinal;
        public Vector3Int posicionAzulejo;
    }
}