using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Scenes;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;

namespace JoinCatCode
{
    [UpdateAfter(typeof(SistemaPreview))]
    public class SistemaEdicion : ComponentSystem
    {
        
       protected override void OnStartRunning()
        {
           /*for (int i = 0; i < 600; i++)
            {
                for (int z = 0; z < 600; z++)
                {
                   AdministradorRecursos.Instanciar().CrearRecurso(1, new Unity.Mathematics.float3(i,2.0f, z));
                }               
            }     */       
        }

        protected override void OnDestroy()
        {
            AdministradorMundos.MundoActual().Liberar();
           
        }
        protected override void OnUpdate()
        {
            TipoEdicion tipoEdicion = 0;
            ModoEdicion modoEdicion = 0;
            int capa = 0;
            int idActual = 0;
            int posicionValida=0;
            Vector3Int posicionActualTile = new Vector3Int(0,0,0);
            Entities.ForEach((ref EdicionModoActualData modo) =>
            {
               
                 tipoEdicion        = modo.tipoEdicion;
                 modoEdicion        = modo.modoEdicion;
                 capa               = modo.capaActual;
                 idActual           = modo.idActual;
                 posicionValida     = modo.posicionValida;
                 posicionActualTile = modo.posicionTileActual;
                /*-----------------*/
                if (tipoEdicion == TipoEdicion.Terreno)
                {
                    ModoTerreno(modoEdicion, capa);
                }
                if (tipoEdicion == TipoEdicion.Recursos)
                {
                    ModoRecursos(modoEdicion, capa, idActual,posicionValida, posicionActualTile);
                }
            });   
        }

   
        void ModoTerreno(ModoEdicion modoEdicion, int capa)
        {
            bool mouseClickIzq = Input.GetMouseButtonDown(0);
            bool mouseClickIzqConst = Input.GetMouseButton(0);
            bool mouseClickDer = Input.GetMouseButtonDown(1);
            bool mouseClickDerConst = Input.GetMouseButton(1);
            if (modoEdicion == ModoEdicion.Crear)
            {
                Vector3Int pos = AdministradorMundos.MundoActual().ObtenerPosicionIsometrica(capa);           
                if (mouseClickIzqConst)
                {
                   /// AdministradorRecursos.Instanciar().CrearRecurso(1, new Unity.Mathematics.float3(pos.x, 1.8f, pos.z));
                    AdministradorMundos.MundoActual().AgregarVoxelTerreno(2,pos);
                }
            }
            if (modoEdicion == ModoEdicion.Eliminar)
            {
                //Debug.Log("Modo Eliminar");
            }
            if (modoEdicion == ModoEdicion.Seleccion)
            {
               // Debug.Log("Modo Seleccion");
            }
        }
        void ModoRecursos(ModoEdicion modoEdicion, int capa, int idActual, int posicionValida, Vector3Int posicionTile)
        {
            bool mouseClickIzq = Input.GetMouseButtonDown(0);
            bool mouseClickIzqConst = Input.GetMouseButton(0);
            bool mouseClickDer = Input.GetMouseButtonDown(1);
            bool mouseClickDerConst = Input.GetMouseButton(1);
            if (modoEdicion == ModoEdicion.Crear)
            {
                if (HasSingleton<AzulejoPreviewComponent>())
                {
                    AzulejoPreviewComponent pre = GetSingleton<AzulejoPreviewComponent>();                                      
                    if (mouseClickIzqConst && posicionValida == 1)
                    {
                        AdministradorMundos.MundoActual().AgregarRecurso(idActual, pre.posicionAzulejo, pre.posicionInicial, pre.posicionFinal);
                        // AdministradorRecursos.Instanciar().CrearRecurso(1, new Unity.Mathematics.float3(pos.x-1, pos.y+1.0f, pos.z-1));
                    }
                }

              
            }
            if (modoEdicion == ModoEdicion.Eliminar)
            {
                if (mouseClickIzqConst && posicionValida == 1)
                {                  
                    AdministradorMundos.MundoActual().removerRecurso(posicionTile);
                    
                }
            }
            if (modoEdicion == ModoEdicion.Seleccion)
            {
                // Debug.Log("Modo Seleccion");
            }
        }


        void GridPrevioActivar()
        { 
        
        }
        void GridPrevioDesActivar()
        {

        }
        void GridPrevio(int capa, Vector3Int posicionActual)
        { 
            
        }

    }
}