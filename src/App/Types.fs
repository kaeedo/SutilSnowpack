module Types

open System

type Page =
    | Home
    | About
    | Login
    static member All =
        [ "Home", Home
          "About", About
          "Login", Login ]

    static member Find(name: string) =
        Page.All
        |> List.tryFind (fun (n, _) -> String.Equals(n, name, StringComparison.InvariantCultureIgnoreCase))
        |> Option.map snd


type LoggedInUser = { Name: string; AuthToken: string }

type Model =
    { User: LoggedInUser option
      Page: Page
      ErrorMessage : string } // Greeting for home page

// Model helpers
let getUser m = m.User
let getPage m = m.Page
let getErrorMessage m = m.ErrorMessage
