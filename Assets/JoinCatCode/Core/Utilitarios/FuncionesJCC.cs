using JoinCatCode;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class FuncionesJCC
{

    public static int ObtenerCuadrante(Vector3 posicion, int cuadranteTam)
    {
        int x = (int)math.floor((posicion.x - 1) / cuadranteTam);
        int z = (int)math.floor((posicion.z - 1) / cuadranteTam)*100;        
        if (posicion.x <=0 || posicion.z<=0)
        {
            x = 0;
            z = 0;
        }
        int clave = x + z;
        return clave;
    }
    public static int ObtenerCuadrante(int px, int pz, int cuadranteTam)
    {
        int x = (int)math.floor((px - 1) / cuadranteTam);
        int z = (int)math.floor((pz - 1) / cuadranteTam)*100;
        if (px <= 0 || pz <= 0)
        {
            x = 0;
            z = 0;
        }
        int clave = x + z;
        return clave;
    }

    public static int ObtenerPosicionUnica (int tamX, int tamZ, Vector3Int posicion)
    {
        return (tamX * posicion.x) - tamZ + posicion.z;
    }
    public static int ObtenerPosicionUnica(int tamX, int tamZ, int x, int z)    
    {
        return (tamX * x) - tamZ + z;
    }

    public static string GuardarCombinarMallas(Mesh mesh, string folderPath, string mapaNombre)
    {
        string carpetaMapa = "Assets/" + folderPath + "/" + mapaNombre;
        if (!Directory.Exists(carpetaMapa))
        {
            Directory.CreateDirectory(carpetaMapa);
        }

        bool meshIsSaved = AssetDatabase.Contains(mesh); // If is saved then only show it in the project view.                
        if (!meshIsSaved)
        {
            string meshPath = carpetaMapa +"/" + mesh.name + ".asset";            
            if (File.Exists(meshPath))
            {
                AssetDatabase.DeleteAsset(meshPath);
            }                                                      
            AssetDatabase.CreateAsset(mesh, meshPath);
            AssetDatabase.SaveAssets();
           // Debug.Log("<color=#ff9900><b>Mesh \"" + mesh.name + "\" was saved in the \"" + folderPath + "\" folder.</b></color>"); // Show info about saved mesh.
        }       
        return folderPath;
    }

    public static void DebugCuadranteMapa<T>(MapaNativo<T> mapa, Vector3 posicion )
    where T : struct
    {
        Vector3 lowerLeft = new Vector3(math.floor((posicion.x - 1) / mapa.cuadranteTam) * mapa.cuadranteTam, 0, math.floor((posicion.z - 1) / mapa.cuadranteTam) * mapa.cuadranteTam);
        Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+1, 0, +0) * mapa.cuadranteTam, Color.red);
        Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(+0, 0, +1) * mapa.cuadranteTam, Color.red);
        Debug.DrawLine(lowerLeft + new Vector3(+1, 0, +0) * mapa.cuadranteTam, lowerLeft + new Vector3(+1, 0, +1) * mapa.cuadranteTam, Color.red);
        Debug.DrawLine(lowerLeft + new Vector3(+0, 0, +1) * mapa.cuadranteTam, lowerLeft + new Vector3(+1, 0, +1) * mapa.cuadranteTam, Color.red);
    }
}
