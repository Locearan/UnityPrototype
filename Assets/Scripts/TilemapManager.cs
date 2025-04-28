using UnityEngine ;
using UnityEngine . Tilemaps ;
using System . Collections . Generic ;

public class TilemapManager : MonoBehaviour
{
    [ Header ( "Tilemap References" ) ]
    [ SerializeField ] private Tilemap diggableTilemap ;

    [ Header ( "Tile Data Mapping" ) ]
    [ Tooltip ( "List of all ScriptableObject assets defining diggable tiles." ) ]
    [ SerializeField ] private List < DiggableTileData > tileDataList ;

    private Dictionary < TileBase , DiggableTileData > dataFromTile ;

    void Awake ( )
    {
        dataFromTile = new Dictionary < TileBase , DiggableTileData > ( ) ;
        foreach ( var tileData in tileDataList )
        {
            if ( tileData              != null
              && tileData . tileVisual != null )
            {
                if ( ! dataFromTile . ContainsKey ( tileData . tileVisual ) )
                {
                    dataFromTile . Add ( tileData . tileVisual , tileData ) ;
                }
                else
                {
                    Debug . LogWarning
                        ( $"Duplicate TileBase key found: {tileData . tileVisual . name}. Check your TileDataList." ) ;
                }
            }
            else
            {
                Debug . LogWarning ( $"Null tileData or tileVisual found in tileDataList. Skipping entry." ) ;
            }
        }

        if ( diggableTilemap == null )
        {
            Debug . LogError ( "Diggable Tilemap reference is not set in TilemapManager!" ) ;
        }
    }

    public DiggableTileData GetTileData ( Vector3 worldPosition )
    {
        if ( diggableTilemap == null ) return null ;
        Vector3Int gridPosition = diggableTilemap . WorldToCell ( worldPosition ) ;
        TileBase   tile         = diggableTilemap . GetTile ( gridPosition ) ;
        if ( tile != null
          && dataFromTile . ContainsKey ( tile ) )
        {
            return dataFromTile [ tile ] ;
        }

        return null ;
    }

    public DiggableTileData GetTileData ( Vector3Int gridPosition )
    {
        if ( diggableTilemap == null ) return null ;
        TileBase tile = diggableTilemap . GetTile ( gridPosition ) ;
        if ( tile != null
          && dataFromTile . ContainsKey ( tile ) )
        {
            return dataFromTile [ tile ] ;
        }

        return null ;
    }

    public void RemoveTile ( Vector3 worldPosition )
    {
        if ( diggableTilemap == null ) return ;
        Vector3Int gridPosition = diggableTilemap . WorldToCell ( worldPosition ) ;
        diggableTilemap . SetTile ( gridPosition , null ) ;
    }

    public void RemoveTile ( Vector3Int gridPosition )
    {
        if ( diggableTilemap == null ) return ;
        diggableTilemap . SetTile ( gridPosition , null ) ;
    }

    public Vector3Int WorldToCell ( Vector3 worldPosition )
    {
        if ( diggableTilemap == null ) return Vector3Int . zero ;
        return diggableTilemap . WorldToCell ( worldPosition ) ;
    }

    public Vector3 CellToWorld ( Vector3Int gridPosition )
    {
        if ( diggableTilemap == null ) return Vector3 . zero ;
        return diggableTilemap . GetCellCenterWorld ( gridPosition ) ;
    }

    [ ContextMenu ( "Generate Basic Resources" ) ]
    void GenerateBasicResources ( )
    {
        if ( diggableTilemap      == null
          || tileDataList . Count == 0 )
        {
            Debug . LogError ( "Cannot generate resources. Tilemap or TileDataList is not set up." ) ;
            return ;
        }

        BoundsInt        bounds   = diggableTilemap . cellBounds ;
        TileBase [ ]     allTiles = diggableTilemap . GetTilesBlock ( bounds ) ;
        DiggableTileData dirtData = tileDataList . Find ( t => t . name . Contains ( "Dirt" ) ) ;
        if ( dirtData == null )
        {
            Debug . LogError ( "Could not find 'Dirt' TileData for resource generation." ) ;
            return ;
        }

        for ( int x = bounds . xMin ; x < bounds . xMax ; x ++ )
        {
            for ( int y = bounds . yMin ; y < bounds . yMax ; y ++ )
            {
                Vector3Int pos  = new Vector3Int ( x , y , 0 ) ;
                TileBase   tile = diggableTilemap . GetTile ( pos ) ;
                if ( tile != null
                  && dataFromTile . ContainsKey ( tile ) )
                {
                    DiggableTileData currentData = dataFromTile [ tile ] ;
                    if ( currentData    == dirtData
                      && Random . value < 0.1f )
                    {
                    }
                }
            }
        }

        Debug . Log ( "Basic resource generation attempt finished. Review warnings and implement proper logic." ) ;
    }
}
