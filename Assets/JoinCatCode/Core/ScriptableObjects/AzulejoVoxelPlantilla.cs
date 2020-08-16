using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JoinCatCode
{
    [CreateAssetMenu(fileName = "AzulejoVoxelPlantilla", menuName = "ScriptableObjects/AzulejoVoxelPlantilla", order = 1)]
    public class AzulejoVoxelPlantilla : ScriptableObject
    {
        public int idAzulejoVoxel;
        /*-------------------*/
        public int idCaraAtras;
        public int idCaraDelante;
        public int idCaraArriba;
        public int idCaraAbajo;
        public int idCaraIzquierda;
        public int idCaraDerecha;

        public int ObtenerCara(int cara)
        {
            switch (cara)
            {
                case 0:
                    return idCaraAtras;
                case 1:
                    return idCaraDelante;
                case 2:
                    return idCaraArriba;
                case 3:
                    return idCaraAbajo;
                case 4:
                    return idCaraIzquierda;
                case 5:
                    return idCaraDerecha;
                default:
                    return 0;
            }
        }
    }
}