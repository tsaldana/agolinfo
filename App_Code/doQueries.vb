Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System

Public Class popQueries
    Function popList(ByVal type As String, ByVal cbo As String) As Integer

        Select Case type
            Case "bldg"
                popBldgList("cboBldgSelect")

            Case "wing"

            Case "oscre1"

            Case "oscre2"

            Case "oscre3"

            Case "catcode"

            Case Else

        End Select

        Return 0
    End Function


    Function popBldgList(ByVal cbo As String) As String
        Dim strHTML As String = String.Empty

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            myConnection.Open()
            Dim myCommand As New OracleCommand("BldgList", myConnection)
            'myCommand.CommandText = "select convert(integer, BL_ID) as bl_id from afm.BL order by BL_ID"
            myCommand.CommandText = "select BL_ID from BL order by BL_ID"
            Dim reader As OracleDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If reader.HasRows = True Then
                strHTML = "var jsonData = { identifier: 'index', items: [], label: 'bldg' };"
                strHTML += "var storeBldg = new dojo.data.ItemFileWriteStore( { data: jsonData } );"
                'strHTML += "storeBldg.newItem({index:0,bldg:''});"
                Dim i As Integer = 1
                While reader.Read()
                    Dim bl_id As String = String.Empty
                    If Not reader("BL_ID") Is DBNull.Value Then
                        bl_id = Convert.ToString(reader("BL_ID")).Trim
                    Else
                        bl_id = i.ToString
                    End If
                    strHTML += "storeBldg.newItem({index:" & i & ",bldg:'" & bl_id & "'});"
                    'strHTML += "store" & strJSCommand & ".newItem({index:" & i & ",org:'" & reader(0).ToString & "'});"
                    i += 1
                End While
            ElseIf reader.HasRows = False Then 'bl_id's were not returned from the db
                'strHTML = "HeyYouNot"
            End If
            strHTML += "var cboCtl = dijit.byId('" & cbo & "');"
            strHTML += "cboCtl.attr('store', storeBldg);"
            strHTML += "cboCtl.attr('disabled', false);"
            strHTML += "cboCtl.attr('value', '');"
            strHTML += "dijit.byId('" & cbo & "').domNode.style.visibility = '';"
            reader.Close()
            Return strHTML
        Catch ex As Exception
            strHTML += ex.Message
            Return "Error: " & strHTML
        End Try
    End Function

End Class
