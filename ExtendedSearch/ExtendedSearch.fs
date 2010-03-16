namespace ExtendedSearch

    open System.Windows.Forms
    open System.Resources
    open System.Reflection
    open System.Drawing
    
    module UI =  
        let extendedSearchResourceManager = new ResourceManager("ExtendedSearch",Assembly.GetExecutingAssembly())
        let initialFormWidth = 500
        let initialFormHeight = 500
        let versionSpecControl =
            let versionLabel = extendedSearchResourceManager.GetString("VersionLabel")
            let majorTextBox = new TextBox()
            let minorTextBox = new TextBox()
            let revisionTextBox = new TextBox()
            new Label(Text=versionLabel)
  //          majorTextBox
             
        [<EntryPoint>]
        let main(args:string[]) =
            let esMainForm = new Form(Text="ExtendedSearch",TopMost=true,Width=initialFormWidth, Height=initialFormHeight)
            
            let version = versionSpecControl
            
            let searchButtonCaption = extendedSearchResourceManager.GetString("SearchButtonCaption")
            let searchButton = new Button(Text=searchButtonCaption, Width = 250, Location = new Point(0,435))
            let cancelButtonCaption = extendedSearchResourceManager.GetString("CancelButtonCaption")
            let cancelButton = new Button(Text=cancelButtonCaption, Width = 250, Location = new Point(250,435))
            esMainForm.Controls.Add(version)
            esMainForm.Controls.Add(searchButton)
            esMainForm.Controls.Add(cancelButton)
            Application.Run(esMainForm)
            0