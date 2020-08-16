using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


namespace JoinCatCode
{
    public enum TipoEdicion
    {
        Terreno,
        Recursos,
        Objetos,
        Unidades
    }
    public enum ModoEdicion
    {
        Crear,
        Eliminar,
        Seleccion
    }
    [GenerateAuthoringComponent]
    public struct EdicionModoActualData : IComponentData
    {
        public TipoEdicion tipoEdicion;
        public ModoEdicion modoEdicion;
        public int capaActual;
        public int idActual; //Aplica para recursos, terreno, o lo que sea
        public int idAnterior;
        public Vector3Int posicionTileActual;
        public int posicionValida;
    }
}
