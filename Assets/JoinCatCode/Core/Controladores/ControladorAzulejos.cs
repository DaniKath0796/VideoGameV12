using JoinCatCode;
using System.Collections.Generic;
using UnityEngine;

public class ControladorAzulejos : MonoBehaviour
{
    public List<AzulejoPlantilla> Azulejos;
    public List<AzulejoVoxelPlantilla> AzulejosVoxel;
    public List<MaterialPlantilla> Materiales;
    public List<RecursosPlantilla> Recursos;

    private AdministradorAzulejos admin;
    private AdministradorAzulejosVoxel adminVoxel;
    private AdministradorMateriales adminMateriales;
    private AdministradorRecursos adminRecursos;
    public void Awake()
    {
        admin = AdministradorAzulejos.Instanciar();
        adminVoxel = AdministradorAzulejosVoxel.Instanciar();
        adminMateriales = AdministradorMateriales.Instanciar();
        adminRecursos = AdministradorRecursos.Instanciar();

        foreach (AzulejoPlantilla item in Azulejos)
        {
            admin.AgregarAzulejo(ClaseAzulejo.Terreno, item);
        }
        foreach (AzulejoVoxelPlantilla item in AzulejosVoxel)
        {
            adminVoxel.AgregarAzulejo(item);
        }
        foreach (MaterialPlantilla item in Materiales)
        {
            adminMateriales.AgregarMaterial(item.id,item);
        }
        foreach (RecursosPlantilla item in Recursos)
        {
            adminRecursos.AgregarRecurso(item);
        }
    }
}