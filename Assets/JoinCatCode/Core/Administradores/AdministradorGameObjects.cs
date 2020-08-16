using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoinCatCode
{
    public class AdministradorGameObjects
    {
        static AdministradorGameObjects instancia;

        public static AdministradorGameObjects Instanciar()
        {
            if (instancia == null)
            {
                instancia = new AdministradorGameObjects();
            }
            return instancia;
        }

        public bool InstanciarGameObject(GameObject gameObject)
        {
            GameObject nuevo = GameObject.Instantiate(gameObject);
            return false;
        }

        public bool InstanciarGameObject()
        {
            GameObject nuevo = new GameObject();
            return false;
        }
    }

}
