
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;


namespace JoinCatCode
{

    public class AdministradorRecursos
    {
        private Dictionary<int, RecursosPlantilla> contenedorRecursos;
        static AdministradorRecursos instancia;

        AdministradorRecursos()
        {
            contenedorRecursos = new Dictionary<int, RecursosPlantilla>();
        }
        public static AdministradorRecursos Instanciar()
        {
            if (instancia == null)
            {
                instancia = new AdministradorRecursos();
            }
            return instancia;
        }

        public RecursosPlantilla ObtenerRecurso(int id)
        {
            if (contenedorRecursos.ContainsKey(id))
            {
                return contenedorRecursos[id];
            }
          
            return null;

        }
        public bool AgregarRecurso(RecursosPlantilla recursoPlantilla)
        {
            if (!contenedorRecursos.ContainsKey(recursoPlantilla.idRecurso))
            {
                contenedorRecursos.Add(recursoPlantilla.idRecurso, recursoPlantilla);
                return true;
            }
      
            return false;
        }
        public Entity CrearRecurso(int idRecurso,float3 posicion)
        {
            if (contenedorRecursos.ContainsKey(idRecurso))
            {
                AdministradorMateriales adminMaterial = AdministradorMateriales.Instanciar();
                RecursosPlantilla recursoPlantilla = contenedorRecursos[idRecurso];



                LocalToWorld localToWorld = new LocalToWorld();
                localToWorld.Value = float4x4.TRS(posicion, Quaternion.Euler(0, recursoPlantilla.angulo, 0), new float3(1, 1, 1));

                Translation translation = new Translation { Value = posicion };
                RenderMesh renderMesh = new RenderMesh {
                    mesh = recursoPlantilla.mallaMaestra,
                    material = adminMaterial.ObtenerMaterial(recursoPlantilla.idMaterial).material,
                    layer = 1
                    
                };
                RenderBounds redn = new RenderBounds();
                //redn.Value = recursoPlantilla.mallaMaestra.bounds.ToAABB().Extents;
             /*   redn.Value = new AABB
                {
                    Center = posicion
                    ,Extents = recursoPlantilla.mallaMaestra.bounds.ToAABB().Extents

            };*/

                Recurso recurso = new Recurso { 
                    idRecurso = recursoPlantilla.idRecurso, 
                    cantidadRecurso = recursoPlantilla.cantidadRecurso 
                };
                EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                EntityArchetype archetype = entityManager.CreateArchetype(
                    typeof(RenderMesh),
                    typeof(RenderBounds),
                    typeof(LocalToWorld),                  
                    typeof(Recurso)                   
                    );
                Entity myEntity = entityManager.CreateEntity(archetype);

                entityManager.AddComponentData(myEntity, localToWorld);
               // entityManager.AddComponentData(myEntity, translation);
             //    entityManager.AddComponentData(myEntity, redn);
                entityManager.AddSharedComponentData(myEntity,renderMesh);
                entityManager.AddComponentData(myEntity,recurso);
                return myEntity;
            }

            return default;
        }

    }

    public struct Recurso : IComponentData
    {
       public int idRecurso;
       public int cantidadRecurso;
    }
}
