namespace ManageExpenses

open WebSharper
open WebSharper.UI
open WebSharper.UI.Templating
open WebSharper.UI.Notation

[<JavaScript>]
module Templates =

    type MainTemplate = Templating.Template<"Main.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>
    type HouseholdTemplate = Templating.Template<"Household.html">

[<JavaScript>]
module Client =
    let currenthh = Var.Create ""

    let Main () =
        let rvReversed = Var.Create ""
        Templates.MainTemplate.MainForm()
            .OnSend(fun e ->
                async {
                    let! res = Server.DoSomething e.Vars.TextToReverse.Value
                    rvReversed := res
                }
                |> Async.StartImmediate
            )
            .Reversed(rvReversed.View)
            .Doc()

    let GetCurrentHouseholdAsync =
        async {
            let! res = Server.CurrentHousehold()
            currenthh := res
        }

    do GetCurrentHouseholdAsync |> Async.StartImmediate

    let Household() =
        Templates.HouseholdTemplate.Household()             
            .Currenthh(currenthh.View)
            .Doc()