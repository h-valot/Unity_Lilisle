using UnityEngine;

public enum GroundType 
{
    DIRT,
    CITY,
    WATER
}

public class Ground : MonoBehaviour 
{

	[Header("References")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Mesh _cityMesh, _dirtMesh, _waterMesh;

	[Header("Debugging")]
    public GroundType type;

	public void SetMesh(GroundType type)
	{
		switch (type)
		{
			case GroundType.DIRT:
				_meshFilter.mesh = _dirtMesh;
				break;

			case GroundType.CITY:
				_meshFilter.mesh = _cityMesh;
				break;

			case GroundType.WATER:
				_meshFilter.mesh = _waterMesh;
				break;
		}
	}
}