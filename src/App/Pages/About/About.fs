module About

open Sutil
open Sutil.DOM
open Sutil.Attr
open Sutil.Bulma
open System


let view () =
    Html.div [
        Html.text "About"
    ]