﻿module Noise

let private F2 = 1.0f / 2.0f
let private G2 = 1.0f / 4.0f
let private X_PRIME = 1619
let private Y_PRIME = 31337

let inline private FastFloor (f:float32) = 
    if f >= 0.0f then
        int(f)
    else
        int(f - 1.0f)

let inline private GradCoord2D (seed:int) (x:int) (y:int) (xd:float32) (yd:float32) =
        let mutable hash = seed        
        hash <- hash + X_PRIME * x
        hash <- hash + Y_PRIME * y
        hash <- hash * hash * hash * 60493
        hash <- (hash >>> 13) ^^^ hash

        if ((hash &&& 4) = 0) then
            if hash &&& 1 <> 0 then xd                
            else -xd 
            +
            if hash &&& 2 <> 0 then yd                
            else -yd                            
        else
            if hash &&& 1 <> 0 then xd
            else yd
            *
            if hash &&& 2 <> 0 then 1.0f
            else -1.0f
            

let SimplexNoise (seed: int) (x:float32) (y:float32) =
    let mutable t = (x+y) * F2    
    let i = FastFloor (x + t)
    let j = FastFloor (y + t)
    
    t <- float32(i + j) * G2;
    let X0 = float32(i) - t
    let Y0 = float32(j) - t

    let x0 = x - X0
    let y0 = y - Y0

    let mutable i1 = 0
    let mutable j1 = 1    
    if x0 > y0 then
        i1 <- 1
        j1 <- 0
        
    let x1 = x0 - float32(i1) + G2
    let y1 = y0 - float32(j1) + G2
    let x2 = x0 - 1.0f + 2.0f * G2
    let y2 = y0 - 1.0f + 2.0f * G2

    t <- 0.5f - x0*x0 - y0*y0
    let n0 = 
        if t < 0.0f then 0.0f
        else
            t <- t*t
            t*t*GradCoord2D seed i j x0 y0
    t <- 0.5f - x1 * x1 - y1 * y1;
    let n1 =
        if t < 0.0f then 0.0f
        else
            t <- t*t
            t*t*GradCoord2D seed (i+i1) (j+j1) x1 y1
        
    t <- 0.5f - x2 * x2 - y2 * y2;
    let n2 = 
        if t < 0.0f then 0.0f
        else 
            t <- t*t
            t*t*GradCoord2D seed (i+1) (j+1) x2 y2
        

    50.0f * (n0 + n1 + n2)
    