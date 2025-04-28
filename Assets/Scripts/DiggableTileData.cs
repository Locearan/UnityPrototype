using UnityEngine ;
using UnityEngine . Tilemaps ;

[ CreateAssetMenu ( fileName = "NewDiggableTileData" , menuName = "Mining Game/Diggable Tile Data" ) ]
public class DiggableTileData : ScriptableObject
{
    [ Header ( "Tile Identification" ) ]
    [ Tooltip ( "The actual Tile asset this data corresponds to." ) ]
    public TileBase tileVisual ;

    [ Header ( "Digging Properties" ) ]
    [ Tooltip ( "Is this tile actually diggable?" ) ]
    public bool isDiggable = true ;

    [ Tooltip ( "Base time in seconds to dig this tile." ) ]
    public float baseDiggingTime = 1.0f ;

    [ Header ( "Resource Drops" ) ]
    [ Tooltip ( "The type of resource this tile primarily contains." ) ]
    public ResourceType resourceType = ResourceType . None ;

    [ Tooltip ( "Amount of resource dropped when mined." ) ]
    public int resourceAmount = 1 ;
}
