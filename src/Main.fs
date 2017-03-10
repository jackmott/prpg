// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Game

[<EntryPoint>]
let main argv = 
    use g = new PRPGame()
    g.Run()
    0