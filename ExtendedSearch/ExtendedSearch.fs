namespace ExtendedSearch

    open System
    open System.Windows.Forms
    open System.Resources
    open System.Reflection
    open System.IO
    open System.Diagnostics
    
    module UI =
        let extendedSearchResourceManager = new ResourceManager("ExtendedSearch",Assembly.GetExecutingAssembly())
        let initialFormWidth = 500
        let initialFormHeight = 500
        
        let getAllFilesWhichMatchSpec path recurse filespec verspec =
            try
                let recurseOption = 
                    if recurse then
                        System.IO.SearchOption.AllDirectories
                    else
                        System.IO.SearchOption.TopDirectoryOnly
                Directory.GetFiles(path, filespec, recurseOption)
                |> Seq.map FileVersionInfo.GetVersionInfo
                |> Seq.filter (fun fvi -> fvi.ProductVersion = verspec)//fvi = FileVersionInfo
            with
            | :? System.UnauthorizedAccessException -> Seq.empty //If the user doesn't have sufficient permissions 
                                                                 //to read a directory
            
        [<STAThread>]
        [<EntryPoint>]
        
        let main(args:string[]) =
            let esMainForm = new Form(Text=extendedSearchResourceManager.GetString("AppDisplayName"),TopMost=true,Width=initialFormWidth+5, Height=initialFormHeight, FormBorderStyle= FormBorderStyle.FixedSingle;)
            
            let filePathSpecControl =  
                let fpscLabel = new Label(Text=extendedSearchResourceManager.GetString("FilePathSpecLabel"),Dock=DockStyle.Left, Width = 85)
                let fpscTextBox = new TextBox(Dock=DockStyle.Left,Width=250,TabIndex = 1,Text=extendedSearchResourceManager.GetString("FilePathSpecDefault"))
                let fpscRecurseSubDirCheckBox = new CheckBox(Dock=DockStyle.Left ,Text=extendedSearchResourceManager.GetString("RecurseIntoSubdirsLabel"), TabIndex=3)
                let fpscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top) ,Width = initialFormWidth, Height = initialFormHeight/20)
                let fpscSelectDirButton = new Button(Dock=DockStyle.Left, TabIndex = 2,Text=extendedSearchResourceManager.GetString("SelectDirButtonLabel"))
                fpscSelectDirButton.Click.Add(
                    fun _ -> 
                        let fd = new System.Windows.Forms.OpenFileDialog(InitialDirectory = extendedSearchResourceManager.GetString("FilePathSpecDefault"))
                        fd.ShowDialog()|> ignore)
 
                fpscPanel.Controls.Add(fpscRecurseSubDirCheckBox)
                fpscPanel.Controls.Add(fpscSelectDirButton)
                fpscPanel.Controls.Add(fpscTextBox)
                fpscPanel.Controls.Add(fpscLabel)
                fpscPanel   
                
            let fileNameSpecControl =  
                let fnscLabel = new Label(Text=extendedSearchResourceManager.GetString("FileNameSpecLabel"),Dock=DockStyle.Left, Width = 95)
                let fnscTextBox = new TextBox(Dock=DockStyle.Left,Width=250,TabIndex = 1, Text=extendedSearchResourceManager.GetString("FileNameSpecDefault"))
                let fnscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top) ,Width = initialFormWidth, Height = initialFormHeight/20)
                fnscPanel.Controls.Add(fnscTextBox)
                fnscPanel.Controls.Add(fnscLabel)
                fnscPanel   

            let fileVersionSpecControl =
                let defaultTextBoxWidth = 95
                let majorTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("MajorVerDefault"), Dock=DockStyle.Left, Width=defaultTextBoxWidth, TabIndex = 1)
                let minorTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("MinorVerDefault"), Dock=DockStyle.Left, Width=defaultTextBoxWidth, TabIndex = 2)
                let revisionTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("RevisionVerDefault") , Dock=DockStyle.Left, Width=defaultTextBoxWidth, TabIndex = 3)
                let buildTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("BuildVerDefault"), Dock=DockStyle.Left, Width=defaultTextBoxWidth, TabIndex = 4)
                let vscLabel = new Label(Text=extendedSearchResourceManager.GetString("VersionLabel"),Dock=DockStyle.Left)
                let vscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top),Width=initialFormWidth, Height = initialFormHeight/20)
                vscPanel.Controls.Add(buildTextBox)
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
            esMainForm.Controls.Add(filePathSpecControl)

            let verSpec() = 
                      let verPartSeparator = "."
                      fileVersionSpecControl.Controls.[3].Text + verPartSeparator
                      + fileVersionSpecControl.Controls.[2].Text + verPartSeparator
                      + fileVersionSpecControl.Controls.[1].Text + verPartSeparator
                      + fileVersionSpecControl.Controls.[0].Text
                      
            let pathSpec() =
                fileNameSpecControl.Controls.[1].Text
                
            let recurseIntoSubdirs() = 
                let recurseControl = fileNameSpecControl.Controls.[0] :?> CheckBox
                recurseControl.CheckState = CheckState.Checked
               

            let searchButtonControl = 
//                let basePath = @"c:\users\onorio_development\documents\downloads"
                let fileSpec = "*.exe"
                (*6.0.180.79 *)            
                let searchButton = new Button(Text = extendedSearchResourceManager.GetString("SearchButtonCaption"), Width=initialFormWidth/2, Dock=DockStyle.Left, TabIndex = 1)
                searchButton.Click.Add(fun _ -> 
                        resultsList.Items.Clear()
                        let vs = verSpec()
                        let ps = pathSpec()
                        let recurse = recurseIntoSubdirs()
                        getAllFilesWhichMatchSpec ps recurse fileSpec vs
                        |> Seq.iter (fun fs -> resultsList.Items.Add(fs.FileName,fs.FileVersion)|> ignore))
                searchButton
            
            let cancelButtonControl = 
                let cancelButton = new Button(Text = extendedSearchResourceManager.GetString("CancelButtonCaption"), Width=initialFormWidth/2, Dock=DockStyle.Left, TabIndex = 2)
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