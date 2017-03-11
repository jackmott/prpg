module Player

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

let PlayerSize = 32

type PlayerType =
    {
        mutable pos : Vector2
        mutable tex : Texture2D
    }
    static member Empty = 
        { pos = Vector2.Zero; tex = null}

let InitPlayer player graphics pos =
    player.pos <- pos
    let tex = new Texture2D(graphics,PlayerSize,PlayerSize,false,SurfaceFormat.Color)
    let colors = Array.init (PlayerSize * PlayerSize) (fun i -> Color.Red)    
    tex.SetData(colors)
    player.tex <- tex


