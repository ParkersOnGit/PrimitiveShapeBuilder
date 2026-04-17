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
            List<Vector2> tempTextures = new List<Vector2>();

            List<float> tempData = new List<float>();
            List<uint> tempIndices = new List<uint>();

            Dictionary<(int v, int vt, int vn), uint> modelMap = new Dictionary<(int v, int vt, int vn), uint>();

            foreach (string line in File.ReadAllLines(modelPath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#')) continue;

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts[0] == "v")
                    tempVertices.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                else if (parts[0] == "vt")
                    tempTextures.Add(new Vector2(float.Parse(parts[1]), float.Parse(parts[2])));
                else if (parts[0] == "vn")
                    tempNormals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));

                else if (parts[0] == "f")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        int vIndex = int.Parse(parts[i].Split('/')[0]) - 1;
                        int vtIndex = int.Parse(parts[i].Split('/')[1]) - 1;
                        int vnIndex = int.Parse(parts[i].Split('/')[2]) - 1;

                        if (!modelMap.TryGetValue((vIndex, vtIndex, vnIndex), out uint index))
                        {
                            index = (uint)(tempData.Count / 8);
                            modelMap[(vIndex, vtIndex, vnIndex)] = index;

                            Vector3 pos = tempVertices[vIndex];
                            Vector2 tex = tempTextures[vtIndex];
                            Vector3 norm = tempNormals[vnIndex];

                            tempData.Add(pos.X);
                            tempData.Add(pos.Y);
                            tempData.Add(pos.Z);

                            tempData.Add(tex.X);
                            tempData.Add(tex.Y);
                        
                            tempData.Add(norm.X);
                            tempData.Add(norm.Y);
                            tempData.Add(norm.Z);
                        }

                        tempIndices.Add(index);
                    }
                }
            }

            Data = tempData.ToArray();
            Indices = tempIndices.ToArray();
        }

    }
}
