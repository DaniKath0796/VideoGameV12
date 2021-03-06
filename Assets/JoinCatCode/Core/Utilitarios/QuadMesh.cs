﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuadMesh : MonoBehaviour
{
    // Start is called before the first frame update

    public string carpeta = "Assets/EngineJoinCatCode/mallas/";
    public int texturaAncho = 2048;
    public int texturaAltura = 2048;
    public int tamAzulejo = 128;


    void Start()
    {
        int xSize = texturaAncho/tamAzulejo;
        int zSize = texturaAltura / tamAzulejo;

        for (int x = 1; x <= xSize; x++)
        {
            for (int z = 1; z <= zSize; z++)
            {
                Mesh mesh = new Mesh();
                Vector3[] vertices = new Vector3[4];
                int[] triangulos = new int[6];
                Vector2[] uv = new Vector2[4];

               
                int v = 0;
                int t = 0;
                float tamCelda = 1.0f;
               /* float posx = 0;
                float posz = 0;*/
                float verticesCentro = tamCelda * 0.5f;
                // Vector3 Tamxcelda = new Vector3(posx * tamCelda, 0, posz * tamCelda);
                vertices[v] = new Vector3(-verticesCentro, 0, -verticesCentro); 
                vertices[v + 1] = new Vector3(-verticesCentro, 0, verticesCentro);
                vertices[v + 2] = new Vector3(verticesCentro, 0, -verticesCentro);
                vertices[v + 3] = new Vector3(verticesCentro, 0, verticesCentro);

                triangulos[t] = v;
                triangulos[t + 1] = v + 1;
                triangulos[t + 2] = v + 2;
                triangulos[t + 3] = v + 2;
                triangulos[t + 4] = v + 1;
                triangulos[t + 5] = v + 3;

                uv[0] = new Vector2(0, 1);
                uv[1] = new Vector2(1, 1);
                uv[2] = new Vector2(0, 0);
                uv[3] = new Vector2(1, 0);

                uv[0] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, z * 128, texturaAncho, texturaAltura);
                uv[1] = convertPixelToUvCoordenadas(x * 128, z * 128, texturaAncho, texturaAltura);
                uv[2] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                uv[3] = convertPixelToUvCoordenadas(x * 128, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);

                mesh.Clear();
                mesh.vertices = vertices;
                mesh.triangles = triangulos;
                mesh.uv = uv;
                mesh.RecalculateNormals();

                string nombre = tamAzulejo.ToString()+"_"+x.ToString()+"_"+ z.ToString()+".asset";
                AssetDatabase.CreateAsset(mesh, carpeta+nombre);
                AssetDatabase.SaveAssets();
            }
           
        }
       
    }


    public Vector2 convertPixelToUvCoordenadas(int x, int y, int texturaAncho, int texturaAltura)
    {
        return new Vector2((float)x / texturaAncho, (float)y / texturaAltura);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
