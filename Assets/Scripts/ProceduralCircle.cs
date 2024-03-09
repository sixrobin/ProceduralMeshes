namespace ProceduralMeshes
{
    using UnityEngine;

    public class ProceduralCircle : ProceduralMesh
    {
        [SerializeField]
        private float _outerRadius = 2f;
        [SerializeField]
        private float _innerRadius = 1f;
        [SerializeField, Min(3)]
        private int _circleResolution = 32;
        [SerializeField, Min(2)]
        private int _ringResolution = 32;
        
        public override void Generate()
        {
            Mesh mesh = GetNewMesh();
            _meshFilter.mesh = mesh;
            
            Vector3[] vertices = new Vector3[_circleResolution * _ringResolution + 1];
            int[] triangles = new int[((_circleResolution * _ringResolution - 1) * 6) + (_circleResolution * 3)];

            vertices[^1] = Vector3.zero; // Center vertex.

            for (int i = 0, v = 0; i < _ringResolution; ++i)
            {
                float radius = Mathf.Lerp(_innerRadius, _outerRadius, i / (float)(_ringResolution - 1));

                for (int j = 0; j < _circleResolution; ++j, ++v)
                {
                    float theta = (Mathf.PI * 2f * j) / _circleResolution;
                    vertices[v] = new Vector3(Mathf.Sin(theta) * radius, 0f, Mathf.Cos(theta) * radius);
                }
            }

            for (int t = 0; t < _ringResolution * _circleResolution - _circleResolution; ++t)
            {
                if ((t + 1) % _circleResolution > 0)
                {
                    triangles[t * 6] = t;
                    triangles[t * 6 + 1] = t + 1 + _circleResolution;
                    triangles[t * 6 + 2] = t + 1;
                    triangles[t * 6 + 3] = t;
                    triangles[t * 6 + 4] = t + _circleResolution;
                    triangles[t * 6 + 5] = t + 1 + _circleResolution;
                }
                else
                {
                    triangles[t * 6] = t;
                    triangles[t * 6 + 1] = t + 1;
                    triangles[t * 6 + 2] = t + 1 - _circleResolution;
                    triangles[t * 6 + 3] = t;
                    triangles[t * 6 + 4] = t + _circleResolution;
                    triangles[t * 6 + 5] = t + 1;
                }
            }

            for (int t = 0; t < _circleResolution; ++t)
            {
                triangles[triangles.Length - 1 - (t * 3)] = (t + 1) % _circleResolution;
                triangles[triangles.Length - 1 - (t * 3 + 1)] = t;
                triangles[triangles.Length - 1 - (t * 3 + 2)] = vertices.Length - 1;
            }
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }
}