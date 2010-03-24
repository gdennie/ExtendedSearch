namespace ExtendedSearch

    open System.Windows.Forms
    open System.Resources
    open System.Reflection
    open System.Drawing
    open System.IO
    open System.Diagnostics
    
    module UI =
        let extendedSearchResourceManager = new ResourceManager("ExtendedSearch",Assembly.GetExecutingAssembly())
        let initialFormWidth = 500
        let initialFormHeight = 500
        
        let getAllFilesWhichMatchSpec path recurse filespec verspec =
            let recurseOption = 
                if recurse then
                    System.IO.SearchOption.AllDirectories
                else
                    System.IO.SearchOption.TopDirectoryOnly
            Directory.GetFiles(path, filespec, recurseOption)
            |> Seq.map FileVersionInfo.GetVersionInfo
            |> Seq.filter (fun fvi -> fvi.ProductVersion = verspec) //fvi = FileVersionInfo
            (*Unauthorized Access Exception *)
            
            
            
        [<EntryPoint>]
        let main(args:string[]) =
            let esMainForm = new Form(Text=extendedSearchResourceManager.GetString("AppDisplayName"),TopMost=true,Width=initialFormWidth+5, Height=initialFormHeight, FormBorderStyle= FormBorderStyle.FixedDialog;)
            
            (* FileDialog *)
            
                
            let fileNameSpecControl =  
                let fnscLabel = new Label(Text=extendedSearchResourceManager.GetString("FileNameSpecLabel"),Dock=DockStyle.Left, Width = 150)
                let fnscTextBox = new TextBox(Dock=DockStyle.Left,Width=250)
                let fnscRecurseSubDirCheckBox = new CheckBox(Dock=DockStyle.Left,Text=extendedSearchResourceManager.GetString("RecurseIntoSubdirsLabel"))
                let fnscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top) ,Width = initialFormWidth, Height = initialFormHeight/20)
                fnscPanel.Controls.Add(fnscRecurseSubDirCheckBox)
                fnscPanel.Controls.Add(fnscTextBox)
                fnscPanel.Controls.Add(fnscLabel)
                fnscPanel   
                
            let fileVersionSpecControl =
                let defaultTextBoxWidth = 120
                let majorTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("MajorVerDefault"),Dock=DockStyle.Left, Width=defaultTextBoxWidth)
                let minorTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("MinorVerDefault"),Dock=DockStyle.Left, Width=defaultTextBoxWidth)
                let revisionTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("RevisionVerDefault"),Dock=DockStyle.Left, Width=defaultTextBoxWidth)
                let vscLabel = new Label(Text=extendedSearchResourceManager.GetString("VersionLabel"),Dock=DockStyle.Left)
                let vscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top),Width=initialFormWidth, Height = initialFormHeight/20)
                vscPanel.Controls.Add(revisionTextBox)
                vscPanel.Controls.Add(minorTextBox)
                vscPanel.Controls.Add(majorTextBox)
                vscPanel.Controls.Add(vscLabel)
                vscPanel

            let resultsList = 
                new ListBox(Dock=DockStyle.Top, Width=initialFormWidth, Height = 300)
                
            esMainForm.Controls.Add(resultsList)
            esMainForm.Controls.Add(fileVersionSpecControl)
            esMainForm.Controls.Add(fileNameSpecControl)

            let searchButtonControl = 
                ///@Todo Add the default clickhandler 
                let searchButton = new Button(Text = extendedSearchResourceManager.GetString("SearchButtonCaption"), Width=initialFormWidth/2, Dock=DockStyle.Left)
                searchButton.Click.Add(fun _ -> 
                        getAllFilesWhichMatchSpec @"c:\users\onorio_development\documents\downloads" true "*.exe" "6.0.180.79"
                        |> Seq.iter (fun fs -> resultsList.Items.Add(fs) |> ignore))
                searchButton
            
            let cancelButtonControl = 
                let cancelButton = new Button(Text = extendedSearchResourceManager.GetString("CancelButtonCaption"), Width=initialFormWidth/2, Dock=DockStyle.Left)
                cancelButton.Click.Add(fun _ -> Application.Exit())
                cancelButton
                    
            let buttonPanel =
                let panel = new Panel(Dock=DockStyle.Bottom, Height=initialFormHeight/20, Width = initialFormWidth)
                panel.Controls.Add(cancelButtonControl)
                panel.Controls.Add(searchButtonControl)
                panel

            esMainForm.Controls.Add(buttonPanel)
            Application.EnableVisualStyles()
            Application.Run(esMainForm)
            0