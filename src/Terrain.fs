module Terrain

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open System.Collections.Generic
open System
open System.Diagnostics
open NPC
open Noise

let TileSize = 128

type TerrainTile =
    | Water 
    | Grass
    | Mountain
    | Snow

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
                  
        | Mountain ->        
            tex.SetData(GenGrid 10 TileSize Color.Gray)                
        | Snow -> 
             tex.SetData(GenGrid 10 TileSize Color.White)                
        | Water ->        
            tex.SetData(GenGrid 10 TileSize Color.Blue)             
        TileTextureDict.Add(tile,tex)
        tex   

let GenWorld graphics w h =
    let r = Random(1)
     //If the precalc stuff isn't set right, set it                    
            
    let tiles = 
        [|
        for y in 0 .. h do
            for x in 0 .. w do
               let f = SimplexNoise 1024 (0.1f*float32(x)) (0.1f*float32(y))                            
               yield 
                   if f < -0.15f then
                        Water
                   else if f < 0.15f then
                        Grass
                   else if f < 0.45f then
                        Mountain
                   else 
                        Snow
                              
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
    
