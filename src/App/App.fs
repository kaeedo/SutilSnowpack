module App

open Sutil
open Sutil.DOM
open Sutil.Attr
open Feliz
open type Feliz.length

let view() =
    Html.div [
        style [
            Css.fontFamily "Arial, Helvetica, sans-serif"
            Css.textAlignCenter
            Css.marginTop (px 40)
            Css.fontSize (ex 10)
        ]
        text "Herp Derp"
    ]

view() |> mountElement "sutil-app"