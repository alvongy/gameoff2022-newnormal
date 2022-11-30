using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
	[SerializeField] private DamageDisplayChannelSO _event;
	[SerializeField] private Mesh _quadMesh;
	[SerializeField] private Material _material;

	private Mesh[] _digitalMesh = new Mesh[10];

	private void Awake()
	{
		for (int i = 0; i < 10; i++)
		{
			var mesh = new Mesh();
			mesh.SetVertices(_quadMesh.vertices);
			mesh.SetTangents(_quadMesh.tangents);
			mesh.SetNormals(_quadMesh.normals);
			mesh.SetUVs(0, new Vector3[] {
				new Vector3(0.0625f*i,0f,0f),
				new Vector3(0.0625f*i+0.0625f,0f,0f),
				new Vector3(0.0625f*i,1f,0f),
				new Vector3(0.0625f*i+0.0625f,1f,0f),
			});
			_digitalMesh[i] = mesh;
		}
	}
	//private void LateUpdate()
	//{
	//	CombineInstance[] combines = new CombineInstance[10];
	//	for (int i = 0; i < 10; i++)
	//	{
	//		combines[i] = new CombineInstance();
	//		combines[i].transform=
	//	}
	//	Mesh mesh = new Mesh();
	//	mesh.CombineMeshes()
	//}

}
