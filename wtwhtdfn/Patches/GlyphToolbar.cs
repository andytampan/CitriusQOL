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
            var glyph = PartTypeForToolbar.Make(Resource.GlyphOfDebugger, true, false);
            toolbar.Add(glyph);
            glyph = PartTypeForToolbar.Make(Resource.GlyphOfSurrender, true, false);
            toolbar.Add(glyph);

            orig_method_2047("Citrius QOL", toolbar);
        }

    }
}