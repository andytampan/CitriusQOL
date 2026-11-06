using MonoMod;
using System;
using System.Collections.Generic;
using System.Text;
using wtwhtdfn;

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
        if (sectionName == Translations.Translate("Glyphs"))
        {
            List<PartTypeForToolbar> toolbar = new List<PartTypeForToolbar>();
            PartType[] glyphList =
            {
                Resource.GlyphOfDebugger,
                Resource.GlyphOfSurrender,
                Resource.PocketDisposal,
            };
            foreach (PartType part in glyphList)
            {
                var glyph = PartTypeForToolbar.Make(part, true, false);
                toolbar.Add(glyph);
            }
           
            orig_method_2047("Citrius QOL", toolbar);
        }

    }
}