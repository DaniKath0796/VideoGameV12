using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace JoinCatCode
{
	public class BloqueVoxel
	{
		[SerializeField]
		Vector3 posicion;
		public bool BloqueLleno;
		public byte[,,] mapaVoxels;

		[NonSerialized]
		public GameObject gameObject;
		[NonSerialized]
		MeshRenderer meshRenderer;
		[NonSerialized]
		MeshFilter meshFilter;
		[NonSerialized]
		MapaVoxel mapa;

		

		[NonSerialized]
		int vertexIndex = 0;
		[NonSerialized]
		List<Vector3> vertices = new List<Vector3>();
		[NonSerialized]
		List<int> triangles = new List<int>();
		[NonSerialized]
		List<Vector2> uvs = new List<Vector2>();
		[NonSerialized]
		AdministradorAzulejosVoxel admin = AdministradorAzulejosVoxel.Instanciar();
		
		//	public byte[,,] mapaVoxelsDirecciones;

		public BloqueVoxel(MapaVoxel mapa, Vector3 posicion)
		{
			this.mapa = mapa;
			mapaVoxels = new byte[(int)mapa.voxelTam.x, (int)mapa.voxelTam.y, (int)mapa.voxelTam.z];
			//	mapaVoxelsDirecciones= new byte[mapa.voxelTam.x, mapa.voxelTam.y, mapa.voxelTam.z];
			gameObject = new GameObject();
			meshFilter = gameObject.AddComponent<MeshFilter>();
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = mapa.material;
			meshRenderer.allowOcclusionWhenDynamic = false;
			this.posicion = posicion;
			gameObject.transform.localPosition = new Vector3((posicion.x * mapa.voxelTam.x) - 0.5f, -0.5f, (posicion.z * mapa.voxelTam.z) - 0.5f);
			gameObject.transform.parent = mapa.gameObject.transform;
			gameObject.name = "Voxel_" + posicion.x + "_" + posicion.z;
			BloqueLleno = false;
		}
		public bool  ExisteVoxel(Vector3Int pos)
		{
			int xCheck = Mathf.FloorToInt(pos.x - 1);
			int yCheck = Mathf.FloorToInt(pos.y - 1);
			int zCheck = Mathf.FloorToInt(pos.z - 1);

			xCheck = xCheck - (int)(posicion.x * mapa.voxelTam.x); //Mathf.FloorToInt(gameObject.transform.position.x);
			zCheck = zCheck - (int)(posicion.z * mapa.voxelTam.z);

			if (mapaVoxels[xCheck, yCheck, zCheck] ==0)
			{
				return false;
			}
			return true;
		}
		public void LlenarBloqueCompleto(int numeroCapas)
		{
			for (int y = 0; y < numeroCapas; y++)
			{
				for (int x = 0; x < mapa.voxelTam.x; x++)
				{
					for (int z = 0; z < mapa.voxelTam.z; z++)
					{
						mapaVoxels[x, y, z] = 1; // azulejo 1	
												 //	mapaVoxelsDirecciones[x, y, z] = 0;
					}
				}
			}
			BloqueLleno = false;
			ActualizarBloque();
		}
		public void LlenarBloqueCompleto()
		{
			for (int y = 0; y < mapa.voxelTam.y; y++)
			{
				for (int x = 0; x < mapa.voxelTam.x; x++)
				{
					for (int z = 0; z < mapa.voxelTam.z; z++)
					{
						mapaVoxels[x, y, z] = 1; // azulejo 1						
												 //	mapaVoxelsDirecciones[x, y, z] = 0;
					}
				}
			}
			BloqueLleno = true;
			ActualizarBloque();
		}



		void ActualizarBloque()
		{
			LimpiarDatosMalla();
			for (int y = 0; y < mapa.voxelTam.y; y++)
			{
				for (int x = 0; x < mapa.voxelTam.x; x++)
				{
					for (int z = 0; z < mapa.voxelTam.z; z++)
					{
						if (mapaVoxels[x, y, z] != 0)
						{
							ActualizarMeshData(new Vector3(x, y, z));
						}
					}
				}
			}
			CrearMalla();
		}
		void LimpiarDatosMalla()
		{
			vertexIndex = 0;
			vertices.Clear();
			triangles.Clear();
			uvs.Clear();
		}

		void CrearMalla()
		{

			Mesh mesh = new Mesh();
			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.RecalculateNormals();
			meshFilter.mesh = mesh;

		}

		public void EditarVoxel(Vector3 pos, byte newID)
		{

			int xCheck = Mathf.FloorToInt(pos.x - 1);
			int yCheck = Mathf.FloorToInt(pos.y - 1);
			int zCheck = Mathf.FloorToInt(pos.z - 1);

			xCheck = xCheck - (int)(posicion.x * mapa.voxelTam.x); //Mathf.FloorToInt(gameObject.transform.position.x);
			zCheck = zCheck - (int)(posicion.z * mapa.voxelTam.z);

			mapaVoxels[xCheck, yCheck, zCheck] = newID;
			//	mapaVoxelsDirecciones[xCheck, yCheck, zCheck] = (byte)direccion;
			UpdateSurroundingVoxels(xCheck, yCheck, zCheck);

			UpdateChunk();

		}

		public byte GetVoxelFromGlobalVector3(Vector3 pos)
		{
			int xCheck = Mathf.FloorToInt(pos.x - 1);
			int yCheck = Mathf.FloorToInt(pos.y);
			int zCheck = Mathf.FloorToInt(pos.z - 1);

			xCheck = xCheck - (int)(posicion.x * mapa.voxelTam.x); //Mathf.FloorToInt(gameObject.transform.position.x);
			zCheck = zCheck - (int)(posicion.z * mapa.voxelTam.z);

			return mapaVoxels[xCheck, yCheck, zCheck];

		}


		void UpdateChunk()
		{

			LimpiarDatosMalla();

			for (int y = 0; y < mapa.voxelTam.y; y++)
			{
				for (int x = 0; x < mapa.voxelTam.x; x++)
				{
					for (int z = 0; z < mapa.voxelTam.z; z++)
					{

						if (mapaVoxels[x, y, z] != 0)
						{
							ActualizarMeshData(new Vector3(x, y, z));
						}

					}
				}
			}

			CrearMalla();

		}

		void UpdateSurroundingVoxels(int x, int y, int z)
		{

			Vector3 thisVoxel = new Vector3(x, y, z);

			for (int p = 0; p < 6; p++)
			{

				Vector3 currentVoxel = thisVoxel + VoxelData.faceChecks[p];

				if (!IsVoxelInChunk((int)currentVoxel.x, (int)currentVoxel.y, (int)currentVoxel.z))
				{
					BloqueVoxel b = mapa.ObtenerBloqueVoxel(currentVoxel + posicion);
					if (b != null)
					{
						b.UpdateChunk();
					}

				}

			}
		}
		bool IsVoxelInChunk(int x, int y, int z)
		{
			if ((x < 0 || x > mapa.voxelTam.x - 1) || (y < 0 || y > mapa.voxelTam.y - 1) || (z < 0 || z > mapa.voxelTam.z - 1))
				return false;
			else
				return true;

		}
		void ActualizarMeshData(Vector3 pos)
		{

			for (int p = 0; p < 6; p++)
			{

				if (!CheckVoxel(pos + VoxelData.faceChecks[p]))
				{

					byte blockID = mapaVoxels[(int)pos.x, (int)pos.y, (int)pos.z];

					vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
					vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
					vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
					vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);
					/*	if (p == 2)
						{
						//	byte direcionID = mapaVoxelsDirecciones[(int)pos.x, (int)pos.y, (int)pos.z];
							AgregarTextura(admin.ObtenerAzulejo(blockID).ObtenerCara(p), direcionID);
						}
						else*/
					{
						AgregarTextura(admin.ObtenerAzulejo(blockID).ObtenerCara(p));
					}
					triangles.Add(vertexIndex);
					triangles.Add(vertexIndex + 1);
					triangles.Add(vertexIndex + 2);
					triangles.Add(vertexIndex + 2);
					triangles.Add(vertexIndex + 1);
					triangles.Add(vertexIndex + 3);
					vertexIndex += 4;

				}
			}
		}

		bool CheckVoxel(Vector3 pos)
		{
			int x = Mathf.FloorToInt(pos.x);
			int y = Mathf.FloorToInt(pos.y);
			int z = Mathf.FloorToInt(pos.z);
			if (!IsVoxelInChunk(x, y, z))
			{
				//return mapa.CheckForVoxel(pos + posicion);
				return false;
			}
			byte blockID = mapaVoxels[x, y, z];
			if (blockID == 0)
			{
				return false;
			}
			return true;

		}

		bool VerificarVoxel(Vector3 pos)
		{
			int x = Mathf.FloorToInt(pos.x);
			int y = Mathf.FloorToInt(pos.y);
			int z = Mathf.FloorToInt(pos.z);
			if (x < 0 || x > mapa.voxelTam.x - 1 || y < 0 || y > mapa.voxelTam.y - 1 || z < 0 || z > mapa.voxelTam.z - 1)
			{
				return false;
			}
			return true;

		}

		void AgregarTextura(int textureID)
		{

			float y = textureID / mapa.texturasCuadrante;
			float x = textureID - (y * mapa.texturasCuadrante);

			float texturaNormalizada = NormalizarTexturaBloque();

			x *= texturaNormalizada;
			y *= texturaNormalizada;

			y = 1f - y - texturaNormalizada;

			uvs.Add(new Vector2(x, y));
			uvs.Add(new Vector2(x, y + texturaNormalizada));
			uvs.Add(new Vector2(x + texturaNormalizada, y));
			uvs.Add(new Vector2(x + texturaNormalizada, y + texturaNormalizada));
		}

		public void Liberar()
		{
			GameObject.DestroyImmediate(this.gameObject);
		}

		void AgregarTextura(int textureID, int Orientacion)
		{

			float y = textureID / mapa.texturasCuadrante;
			float x = textureID - (y * mapa.texturasCuadrante);

			float texturaNormalizada = NormalizarTexturaBloque();

			x *= texturaNormalizada;
			y *= texturaNormalizada;

			y = 1f - y - texturaNormalizada;

			switch (Orientacion)
			{
				case 0:
					uvs.Add(new Vector2(x, y));
					uvs.Add(new Vector2(x, y + texturaNormalizada));
					uvs.Add(new Vector2(x + texturaNormalizada, y));
					uvs.Add(new Vector2(x + texturaNormalizada, y + texturaNormalizada));
					break;
				case 1:
					uvs.Add(new Vector2(x, y));
					uvs.Add(new Vector2(x + texturaNormalizada, y));

					uvs.Add(new Vector2(x, y + texturaNormalizada));
					uvs.Add(new Vector2(x + texturaNormalizada, y + texturaNormalizada));
					break;
				case 2:

					uvs.Add(new Vector2(x, y + texturaNormalizada));
					uvs.Add(new Vector2(x + texturaNormalizada, y));
					uvs.Add(new Vector2(x + texturaNormalizada, y + texturaNormalizada));
					uvs.Add(new Vector2(x, y));
					break;
				case 3:
					uvs.Add(new Vector2(x + texturaNormalizada, y + texturaNormalizada));
					uvs.Add(new Vector2(x, y + texturaNormalizada));
					uvs.Add(new Vector2(x + texturaNormalizada, y));
					uvs.Add(new Vector2(x, y));
					break;
				default:
					uvs.Add(new Vector2(x, y));
					uvs.Add(new Vector2(x, y + texturaNormalizada));
					uvs.Add(new Vector2(x + texturaNormalizada, y));
					uvs.Add(new Vector2(x + texturaNormalizada, y + texturaNormalizada));
					break;
			}

		}
		public float NormalizarTexturaBloque()
		{
			return 1f / (float)mapa.texturasCuadrante;
		}

		public void GuardarBloque(string rutaArchivo, DataFormat tipoFormato)
		{
			
			var bytes = SerializationUtility.SerializeValue(this, tipoFormato);
			if (tipoFormato == DataFormat.JSON)
			{
				string ruta = rutaArchivo + "/" + "Bloque_" + posicion.x + "_" + posicion.y + "_" + posicion.z + ".json";
				var jsonString = System.Text.Encoding.UTF8.GetString(bytes);
				File.WriteAllText(ruta, jsonString);
			}
			else
			{
				string ruta = rutaArchivo + "/" + "Bloque_" + posicion.x + "_" + posicion.y + "_" + posicion.z + ".jcc";
				File.WriteAllBytes(ruta, bytes);
			}
		}

		public void CargarBloque(string rutaArchivo,	DataFormat tipoFormato)
		{
			string ruta = rutaArchivo + "/" + "Bloque_" + posicion.x + "_" + posicion.y + "_" + posicion.z + ".jcc";
			if (!File.Exists(ruta))
			{
				return; // No state to load
			}			
			byte[] bytes = File.ReadAllBytes(ruta);
			BloqueVoxel temp = SerializationUtility.DeserializeValue<BloqueVoxel>(bytes, tipoFormato);
			this.mapaVoxels = temp.mapaVoxels;
			ActualizarBloque();
		}

	}

}


//EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


//EntityArchetype archetype = entityManager.CreateArchetype(
//	typeof(Translation),
//	//	typeof(Rotation),
//	typeof(RenderMesh),
//	typeof(RenderBounds),
//	typeof(LocalToWorld)
//	);

//Entity myEntity = entityManager.CreateEntity(archetype);
//entityManager.SetName(myEntity, posicion.x+"_" +posicion.z);
//			entityManager.AddComponentData(myEntity, new Translation
//			{
//				Value = new float3((posicion.x* mapa.voxelTam.x) - 0.5f, -0.5f, (posicion.z* mapa.voxelTam.z) - 0.5f)
//			});

//			RenderBounds redn = new RenderBounds();
//redn.Value = mesh.bounds.ToAABB();

//			entityManager.AddComponentData(myEntity, redn);
			
//			entityManager.AddSharedComponentData(myEntity, new RenderMesh
//			{
//				mesh = mesh,
//				material = material
//			});
//			return myEntity;