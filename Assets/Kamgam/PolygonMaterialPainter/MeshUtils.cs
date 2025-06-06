﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kamgam.PolygonMaterialPainter
{
    public static class MeshUtils
    {
        public static void CopyMesh(Mesh source, Mesh target)
        {
			if (source == null || target == null)
				return;

            target.name = source.name;

            target.indexFormat = source.vertexCount > 65536 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
			target.vertices = source.vertices;
			target.normals = source.normals;
			target.tangents = source.tangents;
			target.colors = source.colors;
			target.uv = source.uv;
			target.uv2 = source.uv2;
			target.uv3 = source.uv3;
			target.uv4 = source.uv4;
			target.uv5 = source.uv5;
			target.uv6 = source.uv6;
			target.uv7 = source.uv7;
			target.uv8 = source.uv8;
			target.SetBoneWeights(source.GetBonesPerVertex(), source.GetAllBoneWeights());
			target.bindposes = source.bindposes;

            // Sub Meshes
			target.subMeshCount = source.subMeshCount;
			for (int m = 0; m < target.subMeshCount; m++)
			{
				target.SetTriangles(source.GetTriangles(m), m);
			}

            // Copy Blend Shape
            target.ClearBlendShapes();
            for (int s = 0; s < source.blendShapeCount; s++)
            {
                int frameCount = source.GetBlendShapeFrameCount(s);
                for (int f = 0; f < frameCount; f++)
                {
                    string name = source.GetBlendShapeName(s);
                    var weight = source.GetBlendShapeFrameWeight(s, f);

                    var deltaVertices = new Vector3[source.vertexCount];
                    var deltaNormals = new Vector3[source.vertexCount];
                    var deltaTangents = new Vector3[source.vertexCount];
                    source.GetBlendShapeFrameVertices(s, f, deltaVertices, deltaNormals, deltaTangents);
                    target.AddBlendShapeFrame(name, weight, deltaVertices, deltaNormals, deltaTangents);
                }
            }

            target.RecalculateBounds();
		}

#if UNITY_EDITOR
        public static void SmoothMesh(Mesh mesh)
		{
			SmoothMeshes(new List<Mesh>() { mesh });
		}

		public static void SmoothMeshes(List<Mesh> meshes)
		{
			// merge all meshes to one list of vertices and normals
			int vertexCount = meshes.Sum(m => m.vertexCount);
			Vector3[] vertices = new Vector3[vertexCount];
			Vector3[] normals = new Vector3[vertexCount];

			int startVertex = 0;
			for (int m = 0; m < meshes.Count; m++)
			{
			    meshes[m].vertices.CopyTo(vertices, startVertex);//
				meshes[m].normals.CopyTo(normals, startVertex);
				startVertex += meshes[m].vertexCount;
			}

			Vector3 avgNormal = Vector3.zero;
			List<int> identicalVertices = new List<int>();

			for (int a = 0; a < vertexCount; a++)
			{
				// find identical vertices (slow)
				identicalVertices.Clear();
				identicalVertices.Add(a);
				for (int b = a; b < vertexCount; b++)
				{
					if (
						   Mathf.Abs(vertices[a].x - vertices[b].x) < 0.0001f
						&& Mathf.Abs(vertices[a].y - vertices[b].y) < 0.0001f
						&& Mathf.Abs(vertices[a].z - vertices[b].z) < 0.0001f)
					{
						identicalVertices.Add(b);
					}
				}

				// calc average normal
				// N2H: ignore duplicate normals for average generation or weigh them based on the triangle areas
				avgNormal.x = 0;
				avgNormal.y = 0;
				avgNormal.z = 0;
				for (int i = 0; i < identicalVertices.Count; i++)
				{
					avgNormal += normals[identicalVertices[i]];
				}
				avgNormal /= identicalVertices.Count;
				// set new normal
				for (int i = 0; i < identicalVertices.Count; i++)
				{
					normals[identicalVertices[i]] = avgNormal.normalized;
				}
				//UtilsDebug.DrawVector(transform.TransformPoint(mesh.vertices[a]), transform.TransformVector(avgNormal), Color.green, 10.0f);
			}

			// split merged vertex list and assign to meshes
			startVertex = 0;
			for (int m = 0; m < meshes.Count; m++)
			{
				// apply new normals
				meshes[m].vertices = new ArraySegment<Vector3>(vertices, startVertex, meshes[m].vertexCount).ToArray();
				meshes[m].normals = new ArraySegment<Vector3>(normals, startVertex, meshes[m].vertexCount).ToArray();
				startVertex += meshes[m].vertexCount;
			}
		}

		[MenuItem("Tools/Polygon Material Painter/Debug/Smooth Normals", priority = 201)]
		public static void SmoothMesh()
		{
			var meshes = Selection.gameObjects.Where(g => g.GetComponent<MeshFilter>() != null).Select(g => g.GetComponent<MeshFilter>().sharedMesh).ToList();
			SmoothMeshes(meshes);
		}

		[MenuItem("Tools/Polygon Material Painter/Debug/Recalculate Normals", priority = 200)]
		public static void RecalculateNormals()
		{
			var meshes = Selection.gameObjects.Where(g => g.GetComponent<MeshFilter>() != null).Select(g => g.GetComponent<MeshFilter>().sharedMesh).ToList();
            foreach (var mesh in meshes)
            {
				mesh.RecalculateNormals();
				mesh.RecalculateTangents();
			}
		}

		[MenuItem("Tools/Polygon Material Painter/Debug/Invert Normals", priority = 203)]
		public static void InvertNormals()
		{
			var meshFilters = Selection.gameObjects.Where(g => g.GetComponent<MeshFilter>() != null).Select(g => g.GetComponent<MeshFilter>()).ToList();
			foreach (var meshFilter in meshFilters)
			{
				if (meshFilter != null)
				{
					Mesh mesh = meshFilter.sharedMesh;

					Vector3[] normals = mesh.normals;
					for (int i = 0; i < normals.Length; i++)
					{
						normals[i] = -normals[i];
					}
					mesh.normals = normals;

					for (int m = 0; m < mesh.subMeshCount; m++)
					{
						int[] triangles = mesh.GetTriangles(m);
						for (int i = 0; i < triangles.Length; i += 3)
						{
							int temp = triangles[i + 0];
							triangles[i + 0] = triangles[i + 1];
							triangles[i + 1] = temp;
						}
						mesh.SetTriangles(triangles, m);
					}
				}
			}
		}

        [MenuItem("Tools/Polygon Material Painter/Debug/Show Normals/0.01", priority = 204)]
        public static void DebugNormals0p01()
        {
            debugNormals(0.01f);
        }

        [MenuItem("Tools/Polygon Material Painter/Debug/Show Normals/0.1", priority = 204)]
		public static void DebugNormals0p1()
		{
            debugNormals(0.1f);
        }

        [MenuItem("Tools/Polygon Material Painter/Debug/Show Normals/1", priority = 205)]
        public static void DebugNormals1()
        {
            debugNormals(1f);
        }

        [MenuItem("Tools/Polygon Material Painter/Debug/Show Normals/10", priority = 206)]
        public static void DebugNormals10()
        {
            debugNormals(10f);
        }

        static void debugNormals(float length)
        {
            var meshFilters = Selection.gameObjects.Where(g => g.GetComponent<MeshFilter>() != null).Select(g => g.GetComponent<MeshFilter>()).ToList();
            foreach (var meshFilter in meshFilters)
            {
                if (meshFilter != null)
                {
                    Mesh mesh = meshFilter.sharedMesh;

                    Vector3[] normals = mesh.normals;
                    Vector3[] vertices = mesh.vertices;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        var pos = meshFilter.transform.TransformPoint(vertices[i]);
                        var normal = meshFilter.transform.TransformDirection(normals[i]);
                        Debug.DrawLine(pos, pos + normal * length, Color.blue, 10);
                    }
                }
            }

            var skinnedMeshRenderer = Selection.gameObjects.Where(g => g.GetComponent<SkinnedMeshRenderer>() != null).Select(g => g.GetComponent<SkinnedMeshRenderer>()).ToList();
            foreach (var renderer in skinnedMeshRenderer)
            {
                if (renderer != null)
                {
                    Mesh mesh = new Mesh();
                    renderer.BakeMesh(mesh);

                    Vector3[] normals = mesh.normals;
                    Vector3[] vertices = mesh.vertices;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        var pos = renderer.transform.TransformPoint(vertices[i]);
                        var normal = renderer.transform.TransformDirection(normals[i]);
                        Debug.DrawLine(pos, pos + normal * length, Color.blue, 10);
                    }
                }
            }
        }
#endif

	}
}

