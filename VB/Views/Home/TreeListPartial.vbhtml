@ModelType System.Collections.IEnumerable
    
@Html.DevExpress().TreeList( _
    Sub(settings)
            settings.Name = "TreeList"
            settings.KeyFieldName = "EmployeeID"
            settings.ParentFieldName = "SupervisorID"
            settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "TreeListPartial"}
            settings.SettingsEditing.AddNewNodeRouteValues = New With {Key .Controller = "Home", Key .Action = "TreeListAddNewPartial"}
            settings.SettingsEditing.UpdateNodeRouteValues = New With {Key .Controller = "Home", Key .Action = "TreeListUpdatePartial"}
            settings.SettingsEditing.NodeDragDropRouteValues = New With {Key .Controller = "Home", Key .Action = "TreeListMovePartial"}
            settings.SettingsEditing.DeleteNodeRouteValues = New With {Key .Controller = "Home", Key .Action = "TreeListDeletePartial"}

            settings.CommandColumn.Visible = True
            settings.CommandColumn.EditButton.Visible = True
            settings.CommandColumn.NewButton.Visible = True
            settings.CommandColumn.DeleteButton.Visible = True

            settings.Columns.Add("FirstName")
            settings.Columns.Add("MiddleName")
            settings.Columns.Add("LastName")
            settings.Columns.Add("Title")

            settings.PreRender = _
                 Sub(s, e)
                         Dim treeList As MVCxTreeList = CType(s, MVCxTreeList)
                         treeList.ExpandAll()
                 End Sub

            settings.CustomJSProperties = _
                         Sub(s, e)
                                 Dim treeList As MVCxTreeList = CType(s, MVCxTreeList)

                                 Dim visibleNodes = treeList.GetVisibleNodes()
                                 Dim dictionary = New System.Collections.Generic.Dictionary(Of Object, String)()

                                 For i As Integer = 0 To visibleNodes.Count - 1
                                     Dim rowValues() As Object = {visibleNodes(i).GetValue(treeList.KeyFieldName), visibleNodes(i).GetValue("RowVersion")}

                                     dictionary(rowValues(0).ToString()) = Convert.ToBase64String(CType(rowValues(1), Byte()))
                                 Next i

                                 e.Properties("cpRowVersions") = New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dictionary)

                                 If ViewData("EditError") IsNot Nothing Then
                                     e.Properties("cpEditError") = ViewData("EditError")
                                 End If
                         End Sub

            settings.ClientSideEvents.BeginCallback = "TreeList_BeginCallback"
            settings.ClientSideEvents.EndCallback = "TreeList_EndCallback"
    
    End Sub).SetEditErrorText(CStr(ViewData("EditError"))).Bind(Model).GetHtml()