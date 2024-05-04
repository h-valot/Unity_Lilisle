using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_TileGrid", menuName = "Data/RSO/Tile grid")]
public class RSO_TileGrid : RuntimeScriptableObject<Tile3D[,]> { }