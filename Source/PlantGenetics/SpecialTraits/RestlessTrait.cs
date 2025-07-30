using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace PlantGenetics.Gens;

public static class RestlessTrait
{
    public static bool hasRestlessTrait(this Plant plant)
    {
        var traitExt = plant.def.GetModExtension<TraitExtension>();
        return traitExt?.SpecialTrait?.Equals(InternalDefOf.Restless) == true;
    }

    /// <summary>
    /// Plants with RestlessTrait will always try to grow at night.
    /// This does means if you can't satisfy its light requirements, they will take damage.
    /// Default nutrifungus will always benefit from this, others require a unique setup or extra genes.
    /// </summary>
    [HarmonyPatch(typeof(Plant), nameof(Plant.Resting), MethodType.Getter)]
    [HarmonyPrefix]
    private static bool Get_Resting_Prefix(ref bool __result, Plant __instance)
    {
        if (!__instance.hasRestlessTrait())
            return true; // vanilla behaviour

        // never rest, restless plant...
        __result = false;
        return false;
    }
}
