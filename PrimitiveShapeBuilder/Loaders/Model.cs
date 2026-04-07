using OpenTK.Mathematics;

namespace PrimitiveShapeBuilder.Loaders
{
    internal class Model
    {
        internal float[] Data { get; private set; }
        internal uint[] Indices { get; private set; }

        internal void LoadModel(string modelPath)
        {
            List<Vector3> tempVertices = new List<Vector3>();
            List<Vector3> tempNormals = new List<Vector3>();
            List<Vector2> tempUVs = new List<Vector2>();

            List<float> tempData = new List<float>();
            List<uint> tempIndices = new List<uint>();

            Dictionary<string, uint> vertexMap = new Dictionary<string, uint>();

            foreach (string line in File.ReadAllLines(modelPath))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts[0] == "v")
                {
                    tempVertices.Add(new Vector3(
                        float.Parse(parts[1]),
                        float.Parse(parts[2]),
                        float.Parse(parts[3])));
                }
                else if (parts[0] == "vn")
                {
                    tempNormals.Add(new Vector3(
                        float.Parse(parts[1]),
                        float.Parse(parts[2]),
                        float.Parse(parts[3])));
                }
                else if (parts[0] == "vt")
                {
                    tempUVs.Add(new Vector2(
                        float.Parse(parts[1]),
                        float.Parse(parts[2])));
                }
                else if (parts[0] == "f")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string key = parts[i];

                        if (!vertexMap.TryGetValue(key, out uint index))
                        {
                            var indicesParts = key.Split('/');

                            int vIndex = int.Parse(indicesParts[0]) - 1;
                            int vtIndex = indicesParts.Length > 1 && indicesParts[1] != "" ? int.Parse(indicesParts[1]) - 1 : 0;
                            int vnIndex = indicesParts.Length > 2 ? int.Parse(indicesParts[2]) - 1 : 0;

                            var pos = tempVertices[vIndex];
                            var norm = tempNormals.Count > 0 ? tempNormals[vnIndex] : (0, 0, 0);
                            var uv = tempUVs.Count > 0 ? tempUVs[vtIndex] : (0, 0);

                            index = (uint)(tempData.Count / 8);

                            // position
                            tempData.Add(pos.X);
                            tempData.Add(pos.Y);
                            tempData.Add(pos.Z);

                            // normal
                            tempData.Add(norm.X);
                            tempData.Add(norm.Y);
                            tempData.Add(norm.Z);

                            // uv
                            tempData.Add(uv.X);
                            tempData.Add(uv.Y);

                            vertexMap[key] = index;
                        }

                        tempIndices.Add(index);
                    }
                }
            }

            // finally convert it to internal arrays.
            Data = tempData.ToArray();
            Indices = tempIndices.ToArray();
        }
    }
}
