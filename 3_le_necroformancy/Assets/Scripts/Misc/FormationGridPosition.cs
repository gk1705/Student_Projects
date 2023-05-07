
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a grid position in a formation grid.
/// </summary>
[ Serializable ]
public struct FormationGridPosition
{
    /// <summary>
    /// Represents an invalid formation grid position.
    /// </summary>
    public static readonly FormationGridPosition Invalid = new FormationGridPosition( int.MinValue, int.MinValue );

    /// <summary>
    /// The grid x position.
    /// </summary>
    public int X;

    /// <summary>
    /// The grid z position.
    /// </summary>
    public int Z;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="a_x">
    /// The grid x position.
    /// </param>
    /// <param name="a_z">
    /// The grid z position.
    /// </param>
    public FormationGridPosition( int a_x, int a_z )
    {
        X = a_x;
        Z = a_z;
    }
}

public class FormationGridPositionComparer
    : IComparer< FormationGridPosition >
{
    public int Compare( FormationGridPosition a_x, FormationGridPosition a_y )
    {
        if ( a_x.Z < a_y.Z ) { return -1; }
        if ( a_x.Z > a_y.Z ) { return  1; }
        if ( a_x.X < a_y.X ) { return -1; }
        if ( a_x.X > a_y.X ) { return  1; }
        
        return 0;
    }
}
