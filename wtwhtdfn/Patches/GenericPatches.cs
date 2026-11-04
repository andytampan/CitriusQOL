using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod;
using MonoMod.Cil;
using MonoMod.InlineRT;
using Quintessential;
using Quintessential.Settings;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using wtwhtdfn;


[MonoModPatch("Sim")]
public class patch_Sim_DebuggerGlyph : Sim
{

    [MonoModPublic]
    [MonoModIgnore]
    public extern void SetInCycle(bool inCycle);

}

[MonoModPatch("SolutionEditorScreen")]
public class patch_SolutionEditorScreen_DebuggerGlyph : SolutionEditorScreen
{
    public patch_SolutionEditorScreen_DebuggerGlyph(Solution solution, MusicTrack musicTrack, Sound fanfare) : base(solution, musicTrack, fanfare)
    {
    }

    [MonoModPublic]
    public SimPlayState simulationState;

    [MonoModPublic]
    [MonoModIgnore]
    public extern void CompleteAndPauseSim();

}

[MonoModPatch("SolutionEditorPartsPanel/ToolbarManager")]
public class patch_SolutionEditorPartsPanel 
{
    [MonoModIgnore]

    public List<SolutionEditorPartsPanel.PartsSection> sections;

    [MonoModIgnore]
    public extern void orig_method_2047(string sectionName, List<PartTypeForToolbar> partToolbar);

    [MonoModPatch("method_2047")]
    public void method_2047(string sectionName, List<PartTypeForToolbar> partToolbar)
    {
        orig_method_2047(sectionName, partToolbar);
        if (sectionName == Translations.Translate("Glyphs")) {
            List<PartTypeForToolbar> toolbar = new List<PartTypeForToolbar>();
            var glyph = PartTypeForToolbar.Make(wtwhtdfn.Resource.GlyphOfDebugger, true, false);
            toolbar.Add(glyph);
            orig_method_2047("Citrius QOL", toolbar);
        }

    }
}

public class patch_Solution
    {

        [MonoModPatch("Solution")]
        public extern bool orig_IsAllowedPlacement(Part part, HexIndex otherInputOutputIndex, HexIndex offset, HexRotation rotationOffset, out string errorMessage);

        
        public bool IsAllowedPlacement(Part part, HexIndex otherInputOutputIndex, HexIndex offset, HexRotation rotationOffset, out string errorMessage)
        {
            
            bool yea = orig_IsAllowedPlacement(part, otherInputOutputIndex, offset, rotationOffset, out errorMessage);
            if (errorMessage == Translations.Translate("There is already another part here.") && wtwhtdfn.wtwhtdfn.MQSettings.Instance.enableOverlap) return true ;
            return yea;
        }
}

public class patch_PartTypeForToolbar
{
    [MonoModIgnore]
    public extern static PartTypeForToolbar orig_Make(PartType type, int indexOfType, bool isAvailable, bool hasPerms);


    public static PartTypeForToolbar Make(PartType type, int indexOfType, bool isAvailable, bool hasPerms)
    {

        return orig_Make(type, indexOfType, wtwhtdfn.wtwhtdfn.MQSettings.Instance.disableUnique || isAvailable, hasPerms);

    }
}




