using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Presets Enum with values.
/// Use preset.Get() to return the sstring.
/// </summary>
public enum Preset
{
    Sol_System,
    Collision,
    Gas_Giant,
    Large_Outer_Body

    //public void Load(string name);
}

static class PresetFunctions
{
    
    // Preset Values
    private static readonly string[] VALUES =
    {
        "Speed=864000&0.Name=Sun&0.Type=Star&0.Material=Star_Yellow&0.Mass=1300000&0.Diameter=109.177027641461&0.Rotation=0.0005&0.Position=0,0,0&0.Velocity=Auto&1.Name=Mercury&1.Type=Planet&1.Material=Mercury&1.Mass=0.0553&1.Diameter=0.383&1.Rotation=0.0005&1.Position=0.387,0,0&1.Velocity=Auto&2.Name=Venus&2.Type=Planet&2.Material=Venus&2.Mass=0.815&2.Diameter=0.949&2.Rotation=0.0005&2.Position=0.723,0,0&2.Velocity=Auto&3.Name=Earth&3.Type=Planet&3.Material=Earth&3.Mass=1&3.Diameter=1&3.Rotation=0&3.Position=1,0,0&3.Velocity=Auto&4.Name=Mars&4.Type=Planet&4.Material=Mars&4.Mass=0.107&4.Diameter=0.532&4.Rotation=0&4.Position=1.52,0,0&4.Velocity=Auto&5.Name=Jupiter&5.Type=Planet&5.Material=Jupiter&5.Mass=317.8&5.Diameter=11.21&5.Rotation=0&5.Position=5.2,0,0&5.Velocity=Auto&6.Name=Saturn&6.Type=Planet&6.Material=Saturn&6.Mass=95.2&6.Diameter=9.45&6.Rotation=0&6.Position=9.58,0,0&6.Velocity=Auto&7.Name=Uranus&7.Type=Planet&7.Material=Uranus&7.Mass=14.5&7.Diameter=4.01&7.Rotation=0&7.Position=19.2,0,0&7.Velocity=Auto&8.Name=Neptune&8.Type=Planet&8.Material=Neptune&8.Mass=17.1&8.Diameter=3.88&8.Rotation=0&8.Position=30.05,0,0&8.Velocity=Auto",
        "Speed=434700&0.Name=Small+Planet&0.Type=Planet&0.Material=Mercury&0.Mass=2&0.Diameter=2&0.Rotation=0.0005&0.Position=0.004,0,0&0.Velocity=Auto&1.Name=Red+Dwarf&1.Type=Star&1.Material=Star_Red&1.Mass=1&1.Diameter=15&1.Rotation=0.0005&1.Position=0,0,0&1.Velocity=Auto&2.Name=Large+Planet&2.Type=Planet&2.Material=Neptune&2.Mass=5&2.Diameter=10&2.Rotation=0.0005&2.Position=-0.002,0,0&2.Velocity=Auto"
    };

    /// <summary>
    /// Retrives the selected value.
    /// </summary>
    /// <param name="preset"></param>
    /// <returns></returns>
    public static string Get(this Preset preset)
    {
        return VALUES[(int)preset];
    }

}


