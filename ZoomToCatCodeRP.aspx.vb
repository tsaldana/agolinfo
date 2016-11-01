
Partial Class ZoomToCatCodeRP
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "ZoomTo" Then
            Dim server As String = System.Configuration.ConfigurationSettings.AppSettings("server_name")
            Response.Redirect("https://" + server + "/FMLW/viewer.htm?cc=" + e.CommandArgument + "&act=zo")
        End If
    End Sub

End Class
