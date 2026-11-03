using MonoMod;
using System;
using System.Collections.Generic;
using System.Text;

public class patch_PartDraggingInputMode : PartDraggingInputMode
{
    public patch_PartDraggingInputMode() : base()
    {
    }

    [MonoModPublic]
    public List<PartDraggingInputMode.DraggedPart> draggedParts;
    public HexIndex selectedHex;

    [MonoModIgnore]
    public extern Maybe<IInputMode> orig_Process(SolutionEditorScreen solEditScrn);
    public Maybe<IInputMode> Process(SolutionEditorScreen solEditScrn)
    {
        if (wtwhtdfn.wtwhtdfn.MQSettings.Instance.mergeTrack.Pressed())
        {
            if (draggedParts.Count == 2)
            {
                Part trackA = draggedParts[0].part;
                Part trackB = draggedParts[1].part;
                if (trackA.GetType() == trackB.GetType() && trackB.GetType() == PartTypes.track)
                {
                    HexIndex trackAOriginCoordinate = trackA.GetHexPos();
                    HexIndex trackBOriginCoordinate = trackB.GetHexPos();
                    HexIndex trackStartA = trackA.InFrontBy(trackA.GetTrack()[0]);
                    HexIndex trackStartB = trackB.InFrontBy(trackB.GetTrack()[0]);
                    HexIndex trackEndA = trackA.InFrontBy(trackA.GetTrack()[trackA.GetTrack().Count - 1]);
                    HexIndex trackEndB = trackB.InFrontBy(trackB.GetTrack()[trackB.GetTrack().Count - 1]);

                    if (wtwhtdfn.Resource.HexSide(trackEndA, trackStartB))
                    {
                        Part mergedTrack = new Part(PartTypes.track, false);
                        mergedTrack.RemoveTrackFront();
                        foreach (HexIndex hex in trackA.GetTrack())
                        {
                            mergedTrack.AddTrackBack(trackA.InFrontBy(hex));
                        }
                        foreach (HexIndex hex in trackB.GetTrack())
                        {
                            mergedTrack.AddTrackBack(trackB.InFrontBy(hex));
                        }
                        DraggedPart draggedPart = new DraggedPart();
                        draggedParts.Clear();
                        draggedPart.part = mergedTrack;
                        draggedParts.Add(draggedPart);
                        Volume.PlaySound(Assets.sounds.glyph_unbonding, 1);
                    }
                    else if (wtwhtdfn.Resource.HexSide(trackEndB, trackStartA))
                    {
                        Part mergedTrack = new Part(PartTypes.track, false);
                        mergedTrack.RemoveTrackFront();
                        foreach (HexIndex hex in trackB.GetTrack())
                        {
                            mergedTrack.AddTrackBack(trackB.InFrontBy(hex));
                        }
                        foreach (HexIndex hex in trackA.GetTrack())
                        {
                            mergedTrack.AddTrackBack(trackA.InFrontBy(hex));
                        }
                        DraggedPart draggedPart = new DraggedPart();
                        draggedParts.Clear();
                        draggedPart.part = mergedTrack;
                        draggedParts.Add(draggedPart);
                        Volume.PlaySound(Assets.sounds.glyph_unbonding, 1);
                    }


                }
            }
        }
        if (wtwhtdfn.wtwhtdfn.MQSettings.Instance.mirrorTrack.Pressed())
        {
            if (draggedParts.Count == 1)
            {
                Part track = draggedParts[0].part;
                if (track.GetType() == PartTypes.track)
                {
                    patch_Part oldpart = draggedParts[0].part as patch_Part;
                    oldpart.trackHexes.Reverse();
                    Volume.PlaySound(Assets.sounds.click_button, 1);
                }
            }
        }
        if (wtwhtdfn.wtwhtdfn.MQSettings.Instance.regularSplit.Pressed())
        {
            if (draggedParts.Count == 1)
            {
                Part track = draggedParts[0].part;
                if (track.GetType() == PartTypes.track)
                {
                    patch_Part oldPart = track as patch_Part;

                    patch_Part newPartA = new Part(PartTypes.track, false) as patch_Part;
                    patch_Part newPartB = new Part(PartTypes.track, false) as patch_Part;

                    var coordinate = oldPart.GetHexPos();
                    newPartA.SetHexPosAndUpdate(coordinate);
                    if (oldPart.trackHexes.Count < 2) return orig_Process(solEditScrn);

                    HexIndex? handleHex = null;

                    foreach (HexIndex hex in oldPart.trackHexes)
                    {

                        if (hex == selectedHex - coordinate)
                        {

                            newPartB.SetHexPosAndUpdate(hex);

                            handleHex = hex - coordinate;
                        }
                        ;

                        if (handleHex.HasValue)
                        {
                            newPartB.AddTrackFront(hex - handleHex.Value);
                        }
                        else
                        {
                            newPartA.AddTrackFront(hex);

                        }


                    }
                    if (newPartA.trackHexes.Count > 1)
                    {
                        newPartA.RemoveTrackBack();
                    }

                    if (newPartB.trackHexes.Count > 1)
                    {
                        newPartB.RemoveTrackBack();
                    }

                    newPartA.trackHexes.Reverse();
                    newPartB.trackHexes.Reverse();
                    draggedParts.Clear();

                    DraggedPart draggedPartA = new DraggedPart();
                    draggedPartA.part = newPartA;
                    draggedParts.Add(draggedPartA);
                    DraggedPart draggedPartB = new DraggedPart();
                    draggedPartB.part = newPartB;
                    draggedParts.Add(draggedPartB);

                    Volume.PlaySound(Assets.sounds.glyph_unbonding, 1);
                }
            }
        }
        if (wtwhtdfn.wtwhtdfn.MQSettings.Instance.disjointSplit.Pressed())
        {
            if (draggedParts.Count == 1)
            {
                Part track = draggedParts[0].part;
                if (track.GetType() == PartTypes.track)
                {
                    patch_Part oldPart = track as patch_Part;

                    patch_Part newPart = new Part(PartTypes.track, false) as patch_Part;

                    var coordinate = oldPart.GetHexPos();
                    newPart.SetHexPosAndUpdate(coordinate);
                    if (oldPart.trackHexes.Count < 2) return orig_Process(solEditScrn);


                    foreach (HexIndex hex in oldPart.trackHexes)
                    {

                        if (hex != selectedHex - coordinate) newPart.AddTrackFront(hex);

                    }
                    if (newPart.trackHexes.Count > 1)
                    {
                        newPart.RemoveTrackBack();
                    }

                    newPart.trackHexes.Reverse();
                    draggedParts.Clear();

                    DraggedPart draggedPart = new DraggedPart();
                    draggedPart.part = newPart;
                    draggedParts.Add(draggedPart);

                    Volume.PlaySound(Assets.sounds.glyph_unbonding, 1);
                }
            }
        }
        return orig_Process(solEditScrn);
    }
}
public class patch_Part : Part
{
    [MonoModPublic]
    public List<HexIndex> trackHexes;

    public patch_Part(PartType type, bool isFixed) : base(type, isFixed)
    {
    }
}

