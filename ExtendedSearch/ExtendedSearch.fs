namespace ExtendedSearch

    open System.Windows.Forms
    open System.Resources
    open System.Reflection
    
    module UI =  
        [<EntryPoint>]
        let main(args:string[]) =
            let esMainForm = new Form(Text="ExtendedSearch",TopMost=true, Height=500, Width=500)
            let nameLabel = new Label(Text="Name",Width=100,Left=10,Top=10)
            let esRM = new ResourceManager("ExtendedSearch",Assembly.GetExecutingAssembly())
            
            let searchButtonCaption = esRM.GetString("SearchButtonCaption")
            let searchButton = new Button(Text=searchButtonCaption)
            let cancelButtonCaption = esRM.GetString("CancelButtonCaption")
            let cancelButton = new Button(Text=cancelButtonCaption)
//            let buttons = [|searchButton;cancelButton|]
//            esMainForm.Controls.Add(nameLabel)
            esMainForm.Controls.Add(searchButton)
            esMainForm.Controls.Add(cancelButton)
            Application.Run(esMainForm)
            0