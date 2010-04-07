﻿namespace ExtendedSearch

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
        
        let initialButtonWidth = initialFormWidth/2 - 1
        let defaultDockLocation = DockStyle.Left
        
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
                let fpscLabel = new Label(Text=extendedSearchResourceManager.GetString("FilePathSpecLabel"),Dock=defaultDockLocation, Width = 85)
                let fpscTextBox = new TextBox(Dock=defaultDockLocation,Width=250,TabIndex = 1,Text=extendedSearchResourceManager.GetString("FilePathSpecDefault"))
                let fpscRecurseSubDirCheckBox = new CheckBox(Dock=defaultDockLocation ,Text=extendedSearchResourceManager.GetString("RecurseIntoSubdirsLabel"), TabIndex=3)
                let fpscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top) ,Width = initialFormWidth, Height = initialFormHeight/20)
                let fpscSelectDirButton = new Button(Dock=defaultDockLocation, TabIndex = 2,Text=extendedSearchResourceManager.GetString("SelectDirButtonLabel"))
                let SelectDirButtonHandler _ = 
                    let fd = new System.Windows.Forms.FolderBrowserDialog(ShowNewFolderButton = false)
                    let res = fd.ShowDialog()
                    if res = DialogResult.OK then
                        fpscTextBox.Text <- fd.SelectedPath
                fpscSelectDirButton.Click.Add(SelectDirButtonHandler)
                fpscPanel.Controls.Add(fpscRecurseSubDirCheckBox)
                fpscPanel.Controls.Add(fpscSelectDirButton)
                fpscPanel.Controls.Add(fpscTextBox)
                fpscPanel.Controls.Add(fpscLabel)
                fpscPanel   
                
            let fileNameSpecControl =  
                let fnscLabel = new Label(Text=extendedSearchResourceManager.GetString("FileNameSpecLabel"),Dock=defaultDockLocation, Width = 95)
                let fnscTextBox = new TextBox(Dock=defaultDockLocation,Width=235,TabIndex = 1, Text=extendedSearchResourceManager.GetString("FileNameSpecDefault"))
                let fnscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top) ,Width = initialFormWidth, Height = initialFormHeight/20)
                fnscPanel.Controls.Add(fnscTextBox)
                fnscPanel.Controls.Add(fnscLabel)
                fnscPanel   

            let fileVersionSpecControl =
                let defaultTextBoxWidth = 95
                let majorTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("MajorVerDefault"), Dock=defaultDockLocation, Width=defaultTextBoxWidth, TabIndex = 1)
                let minorTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("MinorVerDefault"), Dock=defaultDockLocation, Width=defaultTextBoxWidth, TabIndex = 2)
                let revisionTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("RevisionVerDefault") , Dock=defaultDockLocation, Width=defaultTextBoxWidth, TabIndex = 3)
                let buildTextBox = new TextBox(Text = extendedSearchResourceManager.GetString("BuildVerDefault"), Dock=defaultDockLocation, Width=defaultTextBoxWidth, TabIndex = 4)
                let vscLabel = new Label(Text=extendedSearchResourceManager.GetString("VersionLabel"),Dock=defaultDockLocation)
                let vscPanel = new Panel(Dock=(DockStyle.Fill &&& DockStyle.Top),Width=initialFormWidth, Height = initialFormHeight/20)
                vscPanel.Controls.Add(buildTextBox)
                vscPanel.Controls.Add(revisionTextBox)
                vscPanel.Controls.Add(minorTextBox)
                vscPanel.Controls.Add(majorTextBox)
                vscPanel.Controls.Add(vscLabel)
                vscPanel

            let resultsList = 
                new ListBox(Dock=DockStyle.Top, Width=initialFormWidth, Height = 375)
                
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
                      
            let fileSpec() =
                    fileNameSpecControl.Controls.[0].Text         
                      
            let pathSpec() =
                filePathSpecControl.Controls.[2].Text
                
            let recurseIntoSubdirs() = 
                let recurseControl = filePathSpecControl.Controls.[0] :?> CheckBox
                recurseControl.CheckState = CheckState.Checked
               

            let searchButtonControl = 
                let searchButtonClickHandler _ = 
                    resultsList.Items.Clear()
                    let vs = verSpec()
                    let ps = pathSpec()
                    let fs = fileSpec()
                    let recurse = recurseIntoSubdirs()
                    getAllFilesWhichMatchSpec ps recurse fs vs
                    |> Seq.iter (fun fs -> resultsList.Items.Add(fs.FileName,fs.FileVersion) |> ignore )
                    
                let searchButton = new Button(Text = extendedSearchResourceManager.GetString("SearchButtonCaption"), Width=initialButtonWidth, Dock=DockStyle.Left, TabIndex = 1)
                searchButton.Click.Add(searchButtonClickHandler)        
                searchButton
            
            let cancelButtonControl = 
                let cancelButton = new Button(Text = extendedSearchResourceManager.GetString("CancelButtonCaption"), Width=initialButtonWidth, Dock=DockStyle.Left, TabIndex = 2)
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