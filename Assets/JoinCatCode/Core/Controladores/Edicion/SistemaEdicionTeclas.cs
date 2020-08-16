using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace JoinCatCode
{
    public class SistemaEdicionTeclas : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref EntradaEdicionTeclasData entrada, ref EdicionModoActualData modo) =>
            {
                bool teclaPresionadaTerreno = Input.GetKeyDown(entrada.teclaTerreno);
                bool teclaPresionadaRecursos = Input.GetKeyDown(entrada.teclaRecursos);

                bool teclaPresionadaCrear = Input.GetKeyDown(entrada.teclaCrear);
                bool teclaPresionadaEliminar = Input.GetKeyDown(entrada.teclaEliminar);
                bool teclaPresionadaModificar = Input.GetKeyDown(entrada.teclaModificar);
                bool teclaPresionadaSeleccion = Input.GetKeyDown(entrada.teclaSeleccion);

                bool teclaPresionadaSubirGrid = Input.GetKeyDown(entrada.teclaSubirGrid);
                bool teclaPresionadaBajarGrid = Input.GetKeyDown(entrada.teclaBajarGrid);
                bool teclaPresionadaActivarGrid = Input.GetKeyDown(entrada.teclaActivarGrid);

                bool teclaPresionadaAvanzar = Input.GetKeyDown(entrada.teclaAvanzar);
                bool teclaPresionadaRetroceder = Input.GetKeyDown(entrada.teclaRetroceder);


                bool teclaPresionadaGuardar = Input.GetKeyDown(entrada.teclaGuardar);
                bool teclaPresionadaCargar = Input.GetKeyDown(entrada.teclaCargar);

                if (teclaPresionadaGuardar)
                {
                    AdministradorMundos.MundoActual().Guardar();
                }
                if (teclaPresionadaCargar)
                {
                    AdministradorMundos.MundoActual().Cargar();
                }
                if (teclaPresionadaTerreno)
                {
                    modo.tipoEdicion = TipoEdicion.Terreno;
                }
                if (teclaPresionadaRecursos)
                {
                    modo.tipoEdicion = TipoEdicion.Recursos;
                }
                if (teclaPresionadaCrear)
                {
                    modo.modoEdicion = ModoEdicion.Crear;
                }
                if (teclaPresionadaEliminar)
                {
                    modo.modoEdicion = ModoEdicion.Eliminar;
                }
                if (teclaPresionadaSeleccion)
                {
                    modo.modoEdicion = ModoEdicion.Seleccion;
                }
                if (teclaPresionadaSubirGrid)
                {
                    if (modo.capaActual<10)
                    {
                        modo.capaActual += 1;
                    }                    
                }
                if (teclaPresionadaBajarGrid)
                {
                    if (modo.capaActual > 1)
                    {
                        modo.capaActual -= 1;
                    }
                }
                if (teclaPresionadaAvanzar)
                {

                }
                if (teclaPresionadaActivarGrid)
                {
                    CamaraGrid.instanciar().ActivarDesactivarGrid();
                }
            });
        }
    }
}