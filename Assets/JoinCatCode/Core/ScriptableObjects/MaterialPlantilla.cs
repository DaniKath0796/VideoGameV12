using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoinCatCode
{
    [CreateAssetMenu(fileName = "MaterialPlantilla", menuName = "ScriptableObjects/MaterialPlantilla", order = 1)]
    public class MaterialPlantilla : ScriptableObject
    {
        public int id;
        public Material material;
    }
}