module Terrain

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open System.Collections.Generic
open System
open System.Diagnostics
open NPC

let TileSize = 128

type TerrainTile =
    | Grass 
    | Dirt 
    | Water

type World =
    {
        w : int
        h : int
        tiles : TerrainTile[]
        npcs : NPCType[]
    }

let GenGrid width size color = 
    Array.init (size * size) (fun _ -> color)
           
let TileTextureDict = new Dictionary<TerrainTile,Texture2D>()

let GetTexture graphics tile =    
    match TileTextureDict.TryGetValue(tile) with
    | true,tex -> tex
    | false,_ ->
        let tex = new Texture2D(graphics,TileSize,TileSize,false,SurfaceFormat.Color)
        match tile with
        | Grass ->         
            tex.SetData(GenGrid 10 TileSize Color.Green)          
                  
        | Dirt ->        
            tex.SetData(GenGrid 10 TileSize Color.LightYellow)                
        | Water ->        
            tex.SetData(GenGrid 10 TileSize Color.Blue)             
        TileTextureDict.Add(tile,tex)
        tex   

let GenWorld graphics w h =
    let r = Random(1)
     //If the precalc stuff isn't set right, set it            
    let noise = FastNoise(r.Next(0, 100000))
    noise.SetNoiseType(FastNoise.NoiseType.Simplex)
            
    let tiles = 
        [|
        for y in 0 .. h do
            for x in 0 .. w do
               let f = noise.GetNoise(1000.1f*float32(x)/float32(w),1000.01f*float32(y)/float32(h))               
               yield 
                   if f < -0.25f then
                        Water
                   else if f < 0.25f then
                        Dirt
                   else
                        Grass
                              
        |]
    let npcs = 
        Array.init 1000 (fun i -> {
                                    pos = new Vector2(float32(r.NextDouble()*float(w)),float32(r.NextDouble()*float(h)))
                                    name = "Fred"
                                    tex = GetNPCTex graphics     
                                    items = null
                                    wants = null   
                                    money = 100
                                   } )        
    {
        w = w
        h = h
        tiles = tiles
        npcs = npcs
    }
            
let GetTile world x y = 
    world.tiles.[y*world.w + x]
    
