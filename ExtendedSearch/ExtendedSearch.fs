// Learn more about F# at http://fsharp.net

namespace ExtendedSearch

    open System.Windows.Forms
    module UI =  
        [<EntryPoint>]
        let main(args:string[]) =
            let esMainForm = new Form(Text="ExtendedSearch",TopMost=true, Height=500, Width=500)
            let nameLabel = new Label(Text="Name",Width=100,Left=10,Top=10)
            let nameButton = new Button()
            esMainForm.Controls.Add(nameLabel)
            esMainForm.Controls.Add(nameButton)
            Application.Run(esMainForm)
            0