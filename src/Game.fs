module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Terrain
open Player
open NPC
open System

type GameState =
    | Roam
    | Dialogue
    | Trade

type PRPGame () as G =
    inherit Game()
        
    let dialogueDist = 1.0f
    do G.Content.RootDirectory <- "Content"
    let graphicsDeviceManager = new GraphicsDeviceManager(G)    
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let player : PlayerType = { pos = Vector2(50.0f,50.0f);tex=null}
    let mutable worldPos = Vector2(50.0f,50.0f)
    let mutable world = { w = 0; h = 0; tiles=null; npcs = null}
    let GameState = Roam
    let mutable font = null

    override G.Initialize() =
        do spriteBatch <- new SpriteBatch(G.GraphicsDevice)
        InitPlayer player G.GraphicsDevice
        world <- GenWorld G.GraphicsDevice 100 100
        graphicsDeviceManager.PreferredBackBufferHeight <- 1080
        graphicsDeviceManager.PreferredBackBufferWidth <- 1920
        graphicsDeviceManager.ApplyChanges()
        do base.Initialize()
         
         // TODO: Add your initialization logic here

        ()

    override G.LoadContent() =
        font <- G.Content.Load<SpriteFont>("ConsoleFont")
         // TODO: use this.Content to load your game content here   
        
        ()
 
    override G.Update (gameTime) =

        let gamePadCap = GamePad.GetCapabilities(PlayerIndex.One)
        let dist = 0.1f
        let threshold = 1.5f


        if gamePadCap.IsConnected then
            let padState = GamePad.GetState(PlayerIndex.One)
            if  gamePadCap.HasLeftXThumbStick then
                player.pos.X <- player.pos.X +  padState.ThumbSticks.Left.X * dist
            if gamePadCap.HasLeftYThumbStick then
                player.pos.Y <- player.pos.Y -  padState.ThumbSticks.Left.Y * dist
         
        let state = Keyboard.GetState()

        
        if state.IsKeyDown(Keys.Right) then
            player.pos.X <- player.pos.X + dist

        if state.IsKeyDown(Keys.Left) then
            player.pos.X <- player.pos.X - dist

        if state.IsKeyDown(Keys.Up) then
            player.pos.Y <- player.pos.Y - dist

        if state.IsKeyDown(Keys.Down) then
            player.pos.Y <- player.pos.Y + dist
         

        let xdiff = worldPos.X - player.pos.X
        if xdiff > threshold then            
            worldPos.X <- worldPos.X - dist
        else if xdiff < -threshold then
            worldPos.X <- worldPos.X + dist
        let ydiff = worldPos.Y - player.pos.Y
        if ydiff > threshold then
            worldPos.Y <- worldPos.Y - dist
        else if ydiff < -threshold then
            worldPos.Y <- worldPos.Y + dist
                 
        ()
 
    override G.Draw (gameTime) =
        do G.GraphicsDevice.Clear Color.CornflowerBlue
        
        let bounds = G.GraphicsDevice.Viewport.Bounds
        let width = bounds.Width
        let height = bounds.Height
        
        let screenCenter = new Vector2(float32(width)/2.0f,float32(height)/2.0f)


        
        let Offset = (worldPos*float32(TileSize) - screenCenter) 
        
      
        let numTilesX = float(width)/float(TileSize)
        let numTilesY = float(height)/float(TileSize)
        let startX = int(System.Math.Floor(float(worldPos.X) - numTilesX/2.0))
        let endX = int(System.Math.Ceiling(float(worldPos.X) + numTilesX/2.0)) 
        let startY = int(System.Math.Floor(float(worldPos.Y) - numTilesY/2.0))
        let endY = int(System.Math.Ceiling(float(worldPos.Y) + numTilesY/2.0)) 

        spriteBatch.Begin()        
        
        for y in  startY .. endY+1 do
            for x in startX .. endX+1 do
                let tile = GetTile world x y
                let tex = GetTexture G.GraphicsDevice tile                
                let screenPos = (new Vector2(float32(x),float32(y))*float32(TileSize) - Offset) 
                spriteBatch.Draw(tex,screenPos,Color.White)              
        
        let maxDist =float32( Math.Sqrt(numTilesX*numTilesX+numTilesY*numTilesY))
        for n in world.npcs do            
            if Vector2.Distance(n.pos,worldPos) <= maxDist then
                spriteBatch.Draw(n.tex,n.pos*float32(TileSize) - Offset,Color.White)              
            if Vector2.Distance(player.pos,n.pos) <= dialogueDist then
                spriteBatch.DrawString(font,"Hello!",n.pos*float32(TileSize) - Offset,Color.White)
                
        
        spriteBatch.Draw(player.tex,player.pos*float32(TileSize) - Offset,Color.White)

        spriteBatch.End()
        // TODO: Add your drawing code here

        ()


