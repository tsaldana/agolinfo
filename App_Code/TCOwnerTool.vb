Imports Microsoft.VisualBasic
Imports System.Data
'Imports System.Data.SqlClient
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System
Imports System.Collections

Public Class TCOwner

    Public Function GetChartHTML(ByVal BldID As String, ByVal strFY As String) As String
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BldID.Length > 0 Then
                BldID = BldID.TrimStart("0")

                myConnection.Open()
                Dim myCommand As New OracleCommand
                myCommand.Connection = myConnection

                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.CommandText = "pkg_FacStats_params.pr_set_FacStats_ctx"
                myCommand.Parameters.Add(New OracleParameter("p_attrib", OracleDbType.Varchar2)).Value = "FY"
                myCommand.Parameters.Add(New OracleParameter("p_value", OracleDbType.Varchar2)).Value = strFY
                myCommand.ExecuteNonQuery()
                myCommand.Parameters.Clear()
                myCommand.CommandText = "pkg_FacStats_params.pr_set_FacStats_ctx"
                myCommand.Parameters.Add(New OracleParameter("p_attrib", OracleDbType.Varchar2)).Value = "FAC"
                myCommand.Parameters.Add(New OracleParameter("p_value", OracleDbType.Varchar2)).Value = BldID
                myCommand.ExecuteNonQuery()
                'Return "Done"
                myCommand.CommandType = CommandType.Text
                myCommand.CommandText = "select * from vFacStats"


                Dim reader As OracleDataReader = myCommand.ExecuteReader()
                'Return "Got Past the Reader" & reader.FieldCount
                If reader.HasRows = True Then
                    If reader.Read() Then

                        Dim strHTML As String
                        'setup dollar format strings for table
                        Dim strBaseCosts As String = "$0.00"
                        Dim strGenOper As String = "$0.00"
                        Dim strProjCost As String = "$0.00"
                        Dim strEnvCost As String = "$0.00"
                        Dim strOthCost As String = "$0.00"
                        Dim strTCOwner As String = "$0.00"
                        Dim strTcoSf As String = "0"
                        If Not reader("BaseCost") Is DBNull.Value Then
                            strBaseCosts = String.Format("{0:c}", Convert.ToDouble(reader("BaseCost")))
                        End If
                        If Not reader("GeneralOperatingCost") Is DBNull.Value Then
                            strGenOper = String.Format("{0:c}", Convert.ToDouble(reader("GeneralOperatingCost")))
                        End If
                        If Not reader("ProjectCost") Is DBNull.Value Then
                            strProjCost = String.Format("{0:c}", Convert.ToDouble(reader("ProjectCost")))
                        End If
                        'If Not reader("EnvCost") Is DBNull.Value Then
                        strEnvCost = String.Format("{0:c}", 0)
                        'End If
                        'If Not reader("OthCost") Is DBNull.Value Then
                        strOthCost = String.Format("{0:c}", 0)
                        'End If
                        If Not reader("TCOwner") Is DBNull.Value Then
                            strTCOwner = String.Format("{0:c}", Convert.ToDouble(reader("TCOwner")))
                        End If
                        If Not reader("TCOwnerPerGSF") Is DBNull.Value Then
                            strTcoSf = String.Format("{0:c}", Convert.ToDouble(reader("TCOwnerPerGSF")))
                        End If
                        'build html table
                        'strHTML = "<div><div id=" & ControlChars.Quote & "chartDiv" & ControlChars.Quote & " style=" & ControlChars.Quote & "margin-left:auto; margin-right:auto;" & ControlChars.Quote & "></div>"
                        strHTML = "<table class='GISResultsTable' cellpadding=" & ControlChars.Quote & "1" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & ">"
                        'strHTML += "<tr><td colspan='2'  class ='GISTitleTD'>Total Cost of Ownership</td></tr>"
                        'strHTML += "<tr class='GISChartFacilNoTR'><td class='GISChartFacilNoLeftTD'>Facility Number: </td><td class='GISChartFacilNoRightTD'>" & strID & "</td></tr>"
                        strHTML += "<tr><td>Base Cost:</td><td class='GISChartRightTD'>" & strBaseCosts & "</td></tr>"
                        strHTML += "<tr><td>Operating Cost:</td><td class='GISChartRightTD'>" & strGenOper & "</td> </tr>"
                        strHTML += "<tr><td>Project Cost:</td><td class='GISChartRightTD'>" & strProjCost & "</td></tr>"
                        strHTML += "<tr><td>Environmental Cost SRM:</td><td class='GISChartRightTD'>$0.00</td></tr>"
                        'Grayed out Env Cost: strHTML += "<tr style=" & ControlChars.Quote & "background-color:#909090;" & ControlChars.Quote & "><td style=" & ControlChars.Quote & "background-color:#909090;" & ControlChars.Quote & ">Environmental Costs SRM:</td><td style=" & ControlChars.Quote & "text-align:right; background-color:#909090;" & ControlChars.Quote & ">" & strEnvCost & "</td></tr>"
                        strHTML += "<tr><td>Other Cost:</td><td class='GISUnderlineTD'>$0.00</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>Total Cost of Ownership:</td><td class='GISChartRightTDBold'>" & strTCOwner & "</td></tr>"
                        strHTML += "<tr style='font-weight: bold;'><td class='GISChartTDBold'>TCO / GSF:</td><td class='GISChartRightTDBold'>" & strTcoSf & "</td></tr>"
                        strHTML += "</table>"
                        'create individual chart series objects for each category
                        Dim alSeries As ArrayList = New ArrayList

                        Dim myChartSeries As ChartSeries
                        If Not reader("BaseCost") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Base", Convert.ToInt32(reader("BaseCost")), "#008625", "Base Cost: " & strBaseCosts)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("GeneralOperatingCost") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Operating", Convert.ToInt32(reader("GeneralOperatingCost")), "#FF9900", "Operating Cost: " & strGenOper)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("ProjectCost") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Project", Convert.ToInt32(reader("ProjectCost")), "#550099", "Project Cost: " & strProjCost)
                            alSeries.Add(myChartSeries)
                        End If
                        myChartSeries = New ChartSeries("Environmental", 0, "#00BF35", "Environmental Cost: " & strEnvCost)
                        alSeries.Add(myChartSeries)
                        myChartSeries = New ChartSeries("Other", 0, "#B36B00", "Other Cost: " & strOthCost)
                        alSeries.Add(myChartSeries)
                        'generate the dojo format chart series for the pie chart
                        Dim strDojoPieSeries As String = BuildDojoChartSeries(alSeries)
                        'send back the javascript to fill in the TCO tool's floating panel
                        Return strHTML & "*?*?*?*" & strDojoPieSeries
                    Else 'the query didnt' find the building
                        Return String.Empty
                    End If
                Else 'a building was not returned from the tool
                    Return "<table class='GISResultsTable'><tr><td>Information not found for facility " & BldID & "</td></tr></table>"
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Return "Error" & ex.ToString
        Finally
            myConnection.Close()
        End Try
    End Function

    'Private Function GetTCOInfo(ByVal BldID As String, ByVal strFY As String) As SqlDataReader
    '    '  Try
    '    BldID = BldID.TrimStart("0")
    '    Dim myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
    '    myConnection.Open()
    '    Dim myCommand As New SqlCommand("FacilityPerformaceTCOwnerOccupByFacilFY", myConnection)
    '    'myCommand.CommandType
    '    myCommand.CommandType = CommandType.StoredProcedure

    '    myCommand.Parameters.Add("@Facil", SqlDbType.NVarChar, 10)
    '    myCommand.Parameters("@Facil").Value = BldID
    '    myCommand.Parameters.Add("@FY", SqlDbType.NVarChar, 10)
    '    myCommand.Parameters("@FY").Value = strFY
    '    Dim reader As SqlDataReader = myCommand.ExecuteReader()

    '    'Read in the first record
    '    Return reader
    '    ' Catch ex As Exception
    '    '     Return Nothing
    '    'End Try
    'End Function




    Private Function BuildDojoChartSeries(ByVal alSeries As ArrayList) As String
        'create a series (Title, Number, Color) for the dojo chart
        Dim strSeries As String = "pieChart.addSeries('Series A', [ "
        Dim mySeries As ChartSeries
        For i As Integer = 0 To alSeries.Count - 1
            mySeries = alSeries(i)
            If mySeries.Value > 0 Then
                strSeries += "{y: " & mySeries.Value & ", text: '" & mySeries.Title & "', color: '" & mySeries.Color & "', tooltip: '" & mySeries.ToolTip & "' }, "
            End If
        Next
        If strSeries.Contains("{y:") Then
            strSeries = strSeries.Remove(strSeries.Length - 2, 2)
        End If
        strSeries += "]);"
        Return strSeries

    End Function

    Private Class ChartSeries
        'holds the values for creating a dojo chart series string
        Public Title As String
        Public Value As Int32
        Public ToolTip As String
        Public Color As String
        Public Sub New(ByVal NewTitle As String, ByVal NewValue As Int32, ByVal NewColor As String, ByVal NewToolTip As String)
            Title = NewTitle
            Value = NewValue
            Color = NewColor
            ToolTip = NewToolTip
        End Sub
    End Class

End Class
