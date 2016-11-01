Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System
Imports System.Collections
Imports System.Configuration

Public Class UtilTool
    Public Function GetChartHTML(ByVal strID As String) As String



        If strID.Trim.Length > 0 Then
            Dim reader As OracleDataReader = GetTCOInfo(strID)
            If reader.HasRows = True Then
                If reader.Read() Then

                    Dim strHTML As String
                    'setup dollar format strings for table
                    Dim strElec As String = ""
                    Dim strGas As String = ""
                    Dim strWater As String = ""
                    Dim strSewg As String = ""


                    If Not reader("Elec") Is DBNull.Value Then
                        strElec = String.Format("{0:c}", Convert.ToDouble(reader("Elec")))
                    End If
                    If Not reader("Gas") Is DBNull.Value Then
                        strGas = String.Format("{0:c}", Convert.ToDouble(reader("Gas")))
                    End If
                    If Not reader("Water") Is DBNull.Value Then
                        strWater = String.Format("{0:c}", Convert.ToDouble(reader("Water")))
                    End If
                    If Not reader("sewg") Is DBNull.Value Then
                        strSewg = String.Format("{0:c}", Convert.ToDouble(reader("sewg")))
                    End If




                    'build html table
                    'strHTML = "<table cellpadding=" & ControlChars.Quote & "5" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & "><tr style=" & ControlChars.Quote & "width:258px; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & "><td style=" & ControlChars.Quote & "text-align:left; font-size:10pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & ">Facility Number: </td><td style=" & ControlChars.Quote & "text-align:right; font-size:11pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & ">" & strBldgNos & "</td></tr>"
                    'strHTML = "<div id=" & ControlChars.Quote & "chartDiv" & ControlChars.Quote & " style=" & ControlChars.Quote & "margin-left:auto; margin-right:auto;" & ControlChars.Quote & "></div>"
                    strHTML = "<table style=" & ControlChars.Quote & "width:200px; font-size:8pt; margin-left:auto; margin-right:auto;" & ControlChars.Quote & " cellpadding=" & ControlChars.Quote & "1" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & ">"
                    strHTML += "<tr><td colspan='2' style='font-size:10pt; FONT-WEIGHT: bold; TEXT-ALIGN: center'>Total Utility Cost</td></tr>"
                    strHTML += "<tr style=" & ControlChars.Quote & "width:200px; background-color:#e0e0e0; border-left-color: #000000; border-bottom-color: #000000; border-top-style: solid; border-top-color: #000000; border-right-style: solid; border-left-style: solid; border-right-color: #000000; border-bottom-style: solid;" & ControlChars.Quote & "><td style=" & ControlChars.Quote & "text-align:left;  background-color:#e0e0e0; border-right-color:#e0e0e0; border-right-style: solid;" & ControlChars.Quote & ">Facility Number: </td><td style=" & ControlChars.Quote & "text-align:right; font-size:8pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-left-style: solid;" & ControlChars.Quote & ">" & strID & "</td></tr>"
                    strHTML += "<tr><td>Electricity:</td><td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strElec & "</td></tr>"
                    strHTML += "<tr><td>Gas:</td><td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strGas & "</td> </tr>"
                    strHTML += "<tr><td>Water:</td><td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strWater & "</td></tr>"
                    strHTML += "<tr><td>Sewage:</td><td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strSewg & "</td></tr>"
                    strHTML += "</table>"
                    'create individual chart series objects for each category
                    Dim alSeries As ArrayList = New ArrayList
                    Dim myChartSeries As New ChartSeries("Elec", Convert.ToInt32(reader("Elec")), "#008625")
                    alSeries.Add(myChartSeries)
                    myChartSeries = New ChartSeries("Gas", Convert.ToInt32(reader("Gas")), "#FF9900")
                    alSeries.Add(myChartSeries)
                    myChartSeries = New ChartSeries("Water", Convert.ToInt32(reader("Water")), "#550099")
                    alSeries.Add(myChartSeries)
                    myChartSeries = New ChartSeries("Sewer", Convert.ToInt32(reader("sewg")), "#00BF35")
                    alSeries.Add(myChartSeries)

                    alSeries.Add(myChartSeries)
                    'generate the dojo format chart series for the pie chart
                    Dim strDojoPieSeries As String = BuildDojoChartSeries(alSeries, Convert.ToDouble(reader("GenOper")))
                    'send back the javascript to fill in the TCO tool's floating panel
                    Return strHTML & "*?*?*?*" & strDojoPieSeries
                Else 'the query didnt' find the building
                    Return "<table style=" & ControlChars.Quote & "width:100%; FONT-WEIGHT: bold; font-size:8pt; margin-left:auto; margin-right:auto;" & ControlChars.Quote & "><tr><td>Information not found for facility " & strID & "</td></tr></table>"
                End If
            Else 'a building was not returned from the tool
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function

    Private Function GetTCOInfo(ByVal BldID As String) As OracleDataReader
        Try
            BldID = BldID.TrimStart("0")
            Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
            myConnection.Open()
            Dim myCommand As New OracleCommand("store_GetAverageInventoryPrice", myConnection)
            'myCommand.CommandType
            myCommand.CommandText = "Select * from tcoccup WHERE (FACILITY_NBR = :FACILITY_NBR)"

            myCommand.Parameters.Add("FACILITY_NBR", OracleDbType.Varchar2, 10)
            myCommand.Parameters("FACILITY_NBR").Value = BldID
            Dim reader As OracleDataReader = myCommand.ExecuteReader()

            'Read in the first record
            Return reader
        Catch ex As Exception
            Return Nothing
        End Try
    End Function



    Private Function BuildDojoChartSeries(ByVal alSeries As ArrayList, ByVal dblPieTotal As Integer) As String
        'create a series (Title, Number, Color) for the dojo chart
        Dim strSeries As String = "pieChart.addSeries('Series A', [ "
        Dim mySeries As ChartSeries
        For i As Integer = 0 To alSeries.Count - 1
            mySeries = alSeries(i)
            If mySeries.Value > 0 Then
                If (mySeries.Value / dblPieTotal) > 0.015 Then 'if the percentage of the total pie is < 1.5% then don't show the label
                    strSeries += "{y: " & mySeries.Value & ", text: '" & mySeries.Title & "', color: '" & mySeries.Color & "'}, "
                Else
                    strSeries += "{y: " & mySeries.Value & ", text: '', color: '" & mySeries.Color & "'}, "
                End If
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
        Public Color As String
        Public Sub New(ByVal NewTitle As String, ByVal NewValue As Int32, ByVal NewColor As String)
            Title = NewTitle
            Value = NewValue
            Color = NewColor
        End Sub
    End Class
End Class
