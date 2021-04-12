module App

open Sutil
open Sutil.DOM
open Sutil.Attr
open Feliz
open type Feliz.length

let view() =
    Html.div [

        Html.span [
            text "Herp Dderp"
        ]
        Html.div [
            class' "p-6 max-w-sm mx-auto bg-white rounded-xl shadow-md flex items-center space-x-4"
            Html.div [
                class' "flex-shrink-0"
                Html.span [
                    text "logo"
                ]
            ]
            Html.div [
                class' "text-xl font-medium text-black"
                text "Header tsexffftf"
                Html.p [
                    class' "text-gray-500"
                    text "wreg kwhertgk jhert"
                ]
            ]
        ]
    ]

view() |> mountElement "sutil-app"

// style [
//             Css.fontFamily "Arial, Helvetica, sans-serif"
//             Css.textAlignCenter
//             Css.marginTop (px 40)
//             Css.fontSize (ex 10)
//         ]