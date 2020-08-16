using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;

public class CreadorQuadGameObject : MonoBehaviour
{
    public enum Orientacion
    {
        Arriba,
        Abajo,
        Derecha,
        Izquierda
    }
    public string carpeta = "Assets/EngineJoinCatCode/mallas/";
    public string nombre = "MallaNueva";
    public int texturaAncho = 2048;
    public int texturaAltura = 2048;
    public int tamAzulejo = 128;
    public float escala = 1.0f;
    public int x =1;
    public int z =1;

    public bool ejeZ = false;

    public Orientacion orientacion = Orientacion.Arriba;
    public void GenerarMalla()
    {
        Debug.Log("Genere Malla");

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        int[] triangulos = new int[6];
        Vector2[] uv = new Vector2[4];


        int v = 0;
        int t = 0;
        float tamCelda = escala;
        /* float posx = 0;
         float posz = 0;*/
        float verticesCentro = tamCelda * 0.5f;
        // Vector3 Tamxcelda = new Vector3(posx * tamCelda, 0, posz * tamCelda);
        if (ejeZ)
        {
            vertices[v] = new Vector3(-verticesCentro, 0, -verticesCentro);
            vertices[v + 1] = new Vector3(-verticesCentro, 0, verticesCentro);
            vertices[v + 2] = new Vector3(verticesCentro, 0, -verticesCentro);
            vertices[v + 3] = new Vector3(verticesCentro, 0, verticesCentro);
        }
        else
        {
            vertices[v] = new Vector3(-verticesCentro,  -verticesCentro,0);
            vertices[v + 1] = new Vector3(-verticesCentro, verticesCentro,0);
            vertices[v + 2] = new Vector3(verticesCentro, -verticesCentro,0);
            vertices[v + 3] = new Vector3(verticesCentro, verticesCentro,0);
        }

        triangulos[t] = v;
        triangulos[t + 1] = v + 1;
        triangulos[t + 2] = v + 2;
        triangulos[t + 3] = v + 2;
        triangulos[t + 4] = v + 1;
        triangulos[t + 5] = v + 3;

        //uv[0] = new Vector2(0, 0);
        //uv[1] = new Vector2(0, 1);
        //uv[2] = new Vector2(1, 1);
        //uv[3] = new Vector2(1, 0);

        //uv[0] = new Vector2(0, 1);
        //uv[1] = new Vector2(1, 1);
        //uv[2] = new Vector2(0, 0);
        //uv[3] = new Vector2(1, 0);
        //      new Vector2(0.0f, 0.0f),
        //new Vector2(0.0f, 1.0f),
        //new Vector2(1.0f, 0.0f),
        //new Vector2(1.0f, 1.0f)

        switch (orientacion)
        {
            case Orientacion.Arriba:
                uv[1] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, z * 128, texturaAncho, texturaAltura);
                uv[3] = convertPixelToUvCoordenadas(x * 128, z * 128, texturaAncho, texturaAltura);
                uv[0] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                uv[2] = convertPixelToUvCoordenadas(x * 128, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                break;
            case Orientacion.Abajo:
                uv[0] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, z * 128, texturaAncho, texturaAltura);
                uv[2] = convertPixelToUvCoordenadas(x * 128, z * 128, texturaAncho, texturaAltura);
                uv[1] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                uv[3] = convertPixelToUvCoordenadas(x * 128, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                break;
            case Orientacion.Derecha:
                uv[3] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, z * 128, texturaAncho, texturaAltura);
                uv[2] = convertPixelToUvCoordenadas(x * 128, z * 128, texturaAncho, texturaAltura);
                uv[1] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                uv[0] = convertPixelToUvCoordenadas(x * 128, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                break;
            case Orientacion.Izquierda:
                uv[0] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, z * 128, texturaAncho, texturaAltura);                
                uv[1] = convertPixelToUvCoordenadas(x * 128, z * 128, texturaAncho, texturaAltura);
                uv[2] = convertPixelToUvCoordenadas((x - 1) * tamAzulejo, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                uv[3] = convertPixelToUvCoordenadas(x * 128, (z - 1) * tamAzulejo, texturaAncho, texturaAltura);
                break;
            default:
                break;
        }
         

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangulos;
        mesh.uv = uv;
        mesh.RecalculateNormals();        
        string nombrefin = nombre+ ".asset";
        AssetDatabase.CreateAsset(mesh, carpeta + nombrefin);
        AssetDatabase.SaveAssets();

    }
    public Vector2 convertPixelToUvCoordenadas(int x, int y, int texturaAncho, int texturaAltura)
    {

        return new Vector2((float)x / texturaAncho, (float)y / texturaAltura);
    }    
}
