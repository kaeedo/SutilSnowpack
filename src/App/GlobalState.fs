module GlobalState

open Types
open Sutil
open System

open Browser.Dom

type Message =
    | SetPage of Page
    | SetUser of LoggedInUser option
    | LogUserIn of (string * string)
    | LogUserOut
    | SetException of Exception

let private authorizeUser (username, password) =
    async {
        if username = "" || password <> "password" then
            failwith "User credentials incorrect"
        return "auth-0001"
    }

let init (): Model * Cmd<Message> =
    { User = None; Page = Home; ErrorMessage = "" }, Cmd.ofMsg (SetUser None)

let update (msg: Message) (model: Model): Model * Cmd<Message> =
    //Browser.Dom.console.log($"{msg}")
    match msg with
    | SetPage p ->
        window.location.href <- "#" + (string p).ToLower()

        { model with Page = p}, Cmd.none
    | SetUser u ->
        let greeting =
            match u with
            | Some u -> "Welcome back, " + u.Name
            | None -> "You are logged in as a guest user. Login in to see your account details."

        { model with User = u }, Cmd.none
    | LogUserIn (username, password) ->
        let success authToken =
            { AuthToken = authToken
              Name = username }
            |> Some
            |> SetUser

        model, Cmd.OfAsync.either authorizeUser (username, password) success SetException
    | LogUserOut -> model, Cmd.ofMsg (SetUser None)
