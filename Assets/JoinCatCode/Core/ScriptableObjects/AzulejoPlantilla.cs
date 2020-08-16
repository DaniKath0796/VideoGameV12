using UnityEngine;

namespace JoinCatCode
{
    [CreateAssetMenu(fileName = "AzulejoPlantilla", menuName = "ScriptableObjects/AzulejoPlantilla", order = 1)]
    public class AzulejoPlantilla : ScriptableObject
    {
        public int id;
        public TipoAzulejo tipo = TipoAzulejo.Simple;
        public ClaseAzulejo ClaseAzulejo;
        public GameObject prefab;        
    }
}