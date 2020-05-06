using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.Model.ParametersForGame
{
    public enum ResourceTypeE { Oil, Gold, Coal, Iron, Silver, RealEstate, Total, Nada, } // Nada second last - so total works for arrays
    public enum StatusTypeE { Nada, Unexplored, Explored, Developing, Producing, Suspended, Total }
    public enum TerrainTypeE { Grassland, Forest, Mountain, City, River, Slough, Sand, Total }
    public enum CityDensity { Low, Medium, High, Total }
    public enum InfrastructureType { Road, Pipeline, RailRoad, Hub, Tributary, MainRiver, Total }
    public enum Difficulty { Easy, Medium, Hard }
    public class ParameterMaster { }
}
