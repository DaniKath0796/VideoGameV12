using Unity.Entities;
using UnityEngine;

namespace JoinCatCode
{
    [GenerateAuthoringComponent]
    public struct EntradaEdicionTeclasData : IComponentData
    {
        public KeyCode teclaTerreno;
        public KeyCode teclaRecursos;
        public KeyCode teclaObjetos;
        public KeyCode teclaUnidades;
        /*----------------------------*/
        public KeyCode teclaCrear;
        public KeyCode teclaEliminar;
        public KeyCode teclaModificar;
        public KeyCode teclaSeleccion;
        /*----------------------------*/
        public KeyCode teclaAvanzar;
        public KeyCode teclaRetroceder;
        public KeyCode teclaSubirGrid;
        public KeyCode teclaBajarGrid;
        public KeyCode teclaActivarGrid;
        /*------------------------------*/
        public KeyCode teclaGuardar;
        public KeyCode teclaCargar;

    }
}