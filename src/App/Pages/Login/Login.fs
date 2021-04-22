module Login

open Sutil
open Sutil.DOM
open Sutil.Attr
open Sutil.Bulma
open System

type LoginDetails = {
    Username: string
    Password: string
    RememberMe: bool
    }
with
    static member Default = { Username = ""; Password = ""; RememberMe = false }

type Model = LoginDetails

let private username = fst
let private password = snd

type private Message =
    | SetUsername of string
    | SetPassword of string
    | AttemptLogin
    | CancelLogin

let private init details =
    details, Cmd.none

module EventHelpers =
    open Browser.Types

    let inputElement (target:EventTarget) = target |> asElement<HTMLInputElement>

    let validity (e : Event) =
        inputElement(e.target).validity

// View function. Responsibilities:
// - Arrange for cleanup of model on dispose
// - Present model to user
// - Handle input according to Message API

let private defaultView (message : IObservable<string>) model dispatch =
    bulma.hero [
        disposeOnUnmount [ model ]

        //hero.isInfo

        bulma.heroBody [
            bulma.container [
                bulma.columns [
                    columns.isCentered
                    bulma.column [
                        column.is10Tablet; column.is8Desktop; column.is6Widescreen
                        bulma.box [
                            on "submit" (fun _ -> AttemptLogin |> dispatch) [PreventDefault]
                            Attr.action ""

                            bulma.field.div [
                                class' "has-text-danger"
                                Bind.fragment message Html.text
                            ] |> Transition.showIf (message |> Store.map (fun m -> m <> ""))

                            bulma.field.div [
                                bulma.label "Username"
                                bulma.control.div [
                                    control.hasIconsLeft
                                    bulma.input.text [

                                        bindEvent "input" (fun e -> EventHelpers.validity(e).valid |> not) (fun s -> bindClass s "is-danger")

                                        Attr.placeholder "Hint: your-name"
                                        Bind.attr ("value", model .> username , SetUsername >> dispatch)
                                        Attr.required true
                                    ]
                                    bulma.icon [
                                        icon.isSmall
                                        icon.isLeft
                                        fa "user"
                                    ]
                                ]
                            ]
                            bulma.field.div [
                                bulma.label "Password"
                                bulma.control.div [
                                    control.hasIconsLeft
                                    bulma.input.password [
                                        Attr.placeholder "Hint: sutilx9"
                                        Bind.attr("value", model .> password, SetPassword >> dispatch)
                                        Attr.required true]
                                    bulma.icon [
                                        icon.isSmall
                                        icon.isLeft
                                        fa "lock"
                                    ]
                                ]
                            ]
                            bulma.field.div [
                                field.isGrouped
                                bulma.control.div [
                                    bulma.button.button [
                                        color.isSuccess
                                        Html.text "Login"
                                        onClick (fun _ -> dispatch AttemptLogin) [PreventDefault]
                                    ]
                                ]
                                bulma.control.div [
                                    bulma.button.button [
                                        Html.text "Cancel"
                                        onClick (fun _ -> dispatch CancelLogin) [PreventDefault]
                                    ]
                                ]
                            ]
                        ] ] ] ] ] ]

let private createWithView initDetails login cancel view =

    let update msg (model : (string * string)) : (string * string) * Cmd<Message> =
        match msg with
            |SetUsername name -> (name, snd model), Cmd.none
            |SetPassword pwd -> (fst model, pwd), Cmd.none
            |AttemptLogin -> model, [ fun _ -> login model ]
            |CancelLogin -> model, [ fun _ -> cancel () ]

    let model, dispatch = initDetails |> Store.makeElmish init update ignore

    view model dispatch

let view initDetails (message : IObservable<string>) loginFn cancelFn =
    createWithView initDetails loginFn cancelFn (defaultView message)

