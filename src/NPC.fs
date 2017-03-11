module NPC

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let NpcSize = 32

type Item = 
    | Guns of int
    | Butter of int 

type Want =
    {
        item : Item
        desire : int
    }

type NPCType = 
    {
        pos : Vector2
        name : string
        tex : Texture2D
        items : Item[]
        wants : Want []
        money : int 
    }    
        

let mutable private npcTex : Texture2D Option = None

let GetNPCTex graphics = 
    match npcTex with
    | None ->
        let tex = new Texture2D(graphics,NpcSize,NpcSize,false,SurfaceFormat.Color)
        let colors = Array.init (NpcSize*NpcSize) (fun i -> Color.Purple)    
        tex.SetData(colors)
        npcTex <- Some tex
        tex
    | Some tex -> tex



