module App

open GlobalState
open Types

open Sutil
open Sutil.DOM
open Sutil.Attr
open Feliz
open type Feliz.length
open System

let viewPage model dispatch page =
    let errorMessage = (model |> Store.map getErrorMessage)
    match page with
    | Home ->
        Home.view()
    | About ->
        About.view()
    | Login ->
        Login.view ("", "") errorMessage (dispatch << LogUserIn) (fun _ -> Home |> SetPage |> dispatch)


let view() =
    // model is an IStore<ModeL>
    // This means we can write to it if we want, but when we're adopting
    // Elmish, we treat it like an IObservable<Model>
    let model, dispatch = () |> Store.makeElmish init update ignore

    // Projections from model. These will be bound to elements below
    let page : IObservable<Page> = model |> Store.map getPage |> Store.distinct
    //let isLoggedIn : IObservable<bool> = model |> Store.map (getUser >> (fun u -> u.IsSome))
    let loggedInUser: IObservable<LoggedInUser option> = model  |> Store.map getUser

    // Local store to connect hamburger to nav menu. We *could* route this through Elmish
    let navMenuActive = Store.make false
    // Local store for spotting media change. Also, could go through Elmish
    let isMobile = Store.make false
    let showAside = Store.zip isMobile navMenuActive |> Store.map (fun (m,a) -> not m || a)

    // Listen to browser-sourced events
    let routerSubscription  = Navigable.listenLocation Router.parseRoute (dispatch << SetPage)
    //let mediaSubscription = MediaQuery.listenMedia "(max-width: 768px)" (Store.set isMobile)

    fragment [
        disposeOnUnmount [ model ]
        unsubscribeOnUnmount [ routerSubscription ]
        Html.div [
            class' "flex flex-row"
            Html.div [
                class' "flex flex-col space-y-5 h-screen mr-10 border-r-2 border-red-700 px-3"
                Html.div [
                    Bind.fragment loggedInUser <| fun liu ->
                        if (liu.IsSome) then
                            Html.div [
                                Html.span [
                                    class' ""
                                    text liu.Value.Name
                                ]
                                Html.span [
                                    class' ""
                                    text "Logout"
                                    onClick (fun _ -> dispatch LogUserOut) [ PreventDefault ]
                                ]
                            ]
                        else
                            Html.div [
                                class' ""
                                text "Login"
                                onClick (fun _ -> dispatch (SetPage Login)) [ PreventDefault ]
                            ]

                    Html.hr []

                    Html.div [
                        Html.a [ class' "item"; Attr.href "#home"; text "Home" ]
                    ]
                    Html.div [
                        Html.a [ class' "item"; Attr.href "#about"; text "About" ]
                    ]
                ]
            ]

            Html.div [
                Html.div [
                    class' ""
                    Bind.fragment page <| viewPage model dispatch
                ]
            ]
        ]
    ] //|> withStyle appStyle

// Start the app
view() |> mountElement "sutil-app"