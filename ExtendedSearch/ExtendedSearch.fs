namespace ExtendedSearch

    open System.Windows.Forms
    open System.Resources
    open System.Reflection
    open System.Drawing
    
    module UI =  
        let extendedSearchResourceManager = new ResourceManager("ExtendedSearch",Assembly.GetExecutingAssembly())
        let initialFormWidth = 500
        let initialFormHeight = 500
        
        let searchButtonControl = 
            ///@Todo Add the default clickhandler 
            new Button(Text = extendedSearchResourceManager.GetString("SearchButtonCaption"))
        
        let cancelButtonControl = 
            ///@Todo Add the default clickhandler
            new Button(Text = extendedSearchResourceManager.GetString("CancelButtonCaption"))
                
        let buttonPanel =
            let panel = new Panel()
            panel.Controls.Add(searchButtonControl)
            panel.Controls.Add(cancelButtonControl)
            panel
            
        let versionSpecControl =
            let majorTextBox = new TextBox()
            let minorTextBox = new TextBox()
            let revisionTextBox = new TextBox()
            new Panel(Text=extendedSearchResourceManager.GetString("VersionLabel"))
             
        [<EntryPoint>]
        let main(args:string[]) =
            let esMainForm = new Form(Text=extendedSearchResourceManager.GetString("AppDisplayName"),TopMost=true,Width=initialFormWidth, Height=initialFormHeight)
            
//            let version = versionSpecControl
//            esMainForm.Controls.Add(versionSpecControl)
            esMainForm.Controls.Add(buttonPanel)
            
            Application.Run(esMainForm)
            0