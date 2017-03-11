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
        mutable pos : Vector2
        name : string
        tex : Texture2D
        items : Item[]
        wants : Want []
        money : int         
    }
    static member Empty =
        { pos = Vector2.Zero; name = "";tex = null; items = null; wants = null; money = 0}
    member this.Draw (spriteBatch :SpriteBatch) (scale:float32) (offset : Vector2) =
        spriteBatch.Draw(this.tex,this.pos*scale - offset,Color.White)              

        

let mutable private npcTex : Texture2D Option = None
let GetNPCTex graphics = 
    match npcTex with
    | None ->
        let tex = new Texture2D(graphics,NpcSize,NpcSize,false,SurfaceFormat.Color)
        let colors = Array.init (NpcSize*NpcSize) (fun i -> Color.Yellow)    
        tex.SetData(colors)
        npcTex <- Some tex
        tex
    | Some tex -> tex




