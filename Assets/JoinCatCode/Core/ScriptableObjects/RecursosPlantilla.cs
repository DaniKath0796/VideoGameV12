using UnityEngine;

namespace JoinCatCode
{
    [CreateAssetMenu(fileName = "RecursosPlantilla", menuName = "ScriptableObjects/RecursosPlantilla", order = 1)]
    public class RecursosPlantilla : ScriptableObject
    {
        public int idRecurso;
        public int idMaterial;        
        public int cantidadRecurso;
        public int angulo;
        public Vector3 escala;
        public int xMax;
        public int zMax;
        /*-----------*/
        public Mesh mallaMaestra;
        public Mesh mallaUnitaria;
    }
}