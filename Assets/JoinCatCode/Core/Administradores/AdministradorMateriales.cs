using JoinCatCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministradorMateriales 
{
    static AdministradorMateriales instancia;

    public Dictionary<int, MaterialPlantilla> contenedor;

    AdministradorMateriales()
    {
        contenedor = new Dictionary<int, MaterialPlantilla>();
    }
    public static AdministradorMateriales Instanciar()
    {
        if (instancia == default)
        {
            instancia = new AdministradorMateriales();
        }
        return instancia;
    }

    public void AgregarMaterial(int id, MaterialPlantilla material)
    {
        if (!contenedor.ContainsKey(id))
        {
            contenedor.Add(id, material);
        }
    }

    public MaterialPlantilla ObtenerMaterial(int id)
    {
        if (contenedor.ContainsKey(id))
        {
            return contenedor[id];
        }
        return default;
    }

}
