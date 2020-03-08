using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.Model.Parameter
{
    public enum ResourceTypeE { Oil, Gold, Coal, Iron, Silver, RealEstate, Total, Nada, } // Nada second last - so total works for arrays
    public enum StatusTypeE { Nada, Unexplored, Explored, Developing, Producing, Suspended, Total }
    public enum TerrainTypeE { Grassland, Forest, Mountain, City, River, Slough, Sand, Total = 4 }
    public enum CityDensity { Low, Medium, High, Total }
    public enum InfrastructureType { MainRiver, Tributary, Road, Pipeline, RailRoad, Hub, Total }
    public enum Difficulty { Easy, Medium, Hard }
    public class ParameterMaster
    {        
        
    }
}
