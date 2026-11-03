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




