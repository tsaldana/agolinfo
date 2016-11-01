Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Collections
Imports System
Imports System.Configuration

Public Class OrgChargebackTool
    Private strYearFromQuery As String = String.Empty
    Public Function GetChartHTML(ByVal strID As String) As String
        'Test
        Try
            If strID = "400" Then
                Dim reader As OracleDataReader = GetElecCCInfo(strID)
                'Return "Got Past the Reader" & reader.FieldCount
                Dim strHTML As String
                'setup dollar format strings for table
                Dim strSD As String = ""
                Dim strNameCt As String = ""
                Dim strChgSum As String = ""
                Dim strChgAvg As String = ""
                'Dim strCmnAvg As String = ""
                Dim strRoomAvg As String = ""


                Dim dblNameCt As Double = 0
                Dim dblChgSum As Double = 0
                Dim dblChgAvg As Double = 0
                'Dim dblCmnAvg As Double = 0
                Dim dblRoomAvg As Double = 0


                Dim alSeries As ArrayList = New ArrayList
                Dim pieTotal As Double = 0
                Dim strColor As String = String.Empty

                strHTML = "<td></td>"
                strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">Org</td>"
                strHTML += "<td>Empl Count</td>"
                strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">Rent Area (Sum)</td>"
                strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">Rent Area (Avg)</td>"
                strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">Net Area (Avg)</td>"
                strHTML += "</tr>"

                If reader.HasRows = True Then
                    Dim cntr As Integer = 0
                    While reader.Read()
                        cntr += 1
                        If Not reader("SquadronDivision") Is DBNull.Value Then
                            strSD = Convert.ToString(reader("SquadronDivision"))
                        Else
                            strSD = String.Empty
                        End If
                        If Not reader("NameFirstCount") Is DBNull.Value Then
                            strNameCt = Math.Round(Convert.ToDouble(reader("NameFirstCount")), 0).ToString
                            dblNameCt += Math.Round(Convert.ToDouble(reader("NameFirstCount")), 0)

                        Else
                            strNameCt = String.Empty
                        End If
                        If Not reader("ChargeableAreaSum") Is DBNull.Value Then
                            strChgSum = Math.Round(Convert.ToDouble(reader("ChargeableAreaSum")), 0).ToString
                            dblChgSum += Math.Round(Convert.ToDouble(reader("ChargeableAreaSum")), 0)
                        Else
                            strChgSum = String.Empty
                        End If

                        If Not reader("ChargeableAreaAvg") Is DBNull.Value Then
                            strChgAvg = Math.Round(Convert.ToDouble(reader("ChargeableAreaAvg")), 0).ToString
                            dblChgAvg += Math.Round(Convert.ToDouble(reader("ChargeableAreaAvg")), 0)
                        Else
                            strChgAvg = String.Empty
                        End If
                        'If Not reader("TotalCommonAreaAvg") Is DBNull.Value Then
                        '    strCmnAvg = Math.Round(Convert.ToDouble(reader("TotalCommonAreaAvg")), 0).ToString
                        '    dblCmnAvg += Math.Round(Convert.ToDouble(reader("TotalCommonAreaAvg")), 0)
                        'Else
                        '    strCmnAvg = String.Empty
                        'End If
                        If Not reader("AllocatedRoomAreaAvg") Is DBNull.Value Then
                            strRoomAvg = Math.Round(Convert.ToDouble(reader("AllocatedRoomAreaAvg")), 0).ToString
                            dblRoomAvg += Math.Round(Convert.ToDouble(reader("AllocatedRoomAreaAvg")), 0)
                        Else
                            strRoomAvg = String.Empty
                        End If
                        'If Not reader("UYear") Is DBNull.Value Then
                        '    strUYear = Convert.ToString(reader("UYear"))
                        'Else
                        '    strUYear = String.Empty
                        'End If

                        'If strYearFromQuery = String.Empty Then 'only create these tablerows the first time through the read loop
                        '    If Not theYear = "" Then
                        '        strHTML += "<tr><td colspan='4' style='font-size:10pt; FONT-WEIGHT: bold; TEXT-ALIGN: center'>FY 2007</td></tr>"
                        '        strYearFromQuery = strUYear
                        '    Else
                        '        strYearFromQuery = strUYear
                        '        strHTML += "<tr><td colspan='4' style='font-size:10pt; FONT-WEIGHT: bold; TEXT-ALIGN: center'>FY 2007</td></tr>"
                        '    End If
                        '    strHTML += "<tr style=" & ControlChars.Quote & "width:200px; background-color:#e0e0e0; border-left-color: #000000; border-bottom-color: #000000; border-top-style: solid; border-top-color: #000000; border-right-style: solid; border-left-style: solid; border-right-color: #000000; border-bottom-style: solid;" & ControlChars.Quote & "><td colspan='3' style=" & ControlChars.Quote & "text-align:left;  background-color:#e0e0e0; border-right-color:#e0e0e0; border-right-style: solid;" & ControlChars.Quote & ">Facility Number: </td><td style=" & ControlChars.Quote & "text-align:right;  background-color:#e0e0e0; border-left-color: #e0e0e0; border-left-style: solid;" & ControlChars.Quote & ">" & strID & "</td></tr>"
                        'End If
                        'build html table
                        'strHTML = "<div><div id=" & ControlChars.Quote & "chartDiv" & ControlChars.Quote & " style=" & ControlChars.Quote & "margin-left:auto; margin-right:auto;" & ControlChars.Quote & "></div>"
                        strColor = RandomQBColor(cntr)
                        strHTML += "<tr>"
                        strHTML += "<td style=" & ControlChars.Quote & "background-color:" & strColor & "; width:10px;" & ControlChars.Quote & "></td>"
                        strHTML += "<td>" & strSD & "</td>"
                        strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strNameCt & "</td>"
                        strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strChgSum.ToString & "</td>"
                        strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strChgAvg.ToString & "</td>"

                        strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & strRoomAvg.ToString & "</td>"
                        strHTML += "</tr>"
                        'create an entry in the chart series
                        Dim myChartSeries As New ChartSeries("", Convert.ToInt32(reader("ChargeableAreaSum")), strColor, strSD & " - " & strChgSum.ToString & " sqft")
                        alSeries.Add(myChartSeries)

                    End While
                    strHTML += "<tr  style=" & ControlChars.Quote & " border-top:solid; border-top-color:#000000;" & ControlChars.Quote & ">"
                    strHTML += "<td></td>"
                    strHTML += "<td>Total</td>"
                    strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & dblNameCt & "</td>"
                    strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & dblChgSum & "</td>"
                    strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & Math.Round(dblChgAvg / cntr, 0) & "</td>"
                    strHTML += "<td style=" & ControlChars.Quote & "text-align:right;" & ControlChars.Quote & ">" & Math.Round(dblRoomAvg / cntr, 0) & "</td>"
                    strHTML += "</tr>"
                    strHTML += "</table>"


                    'create the top part of the table with totals calculated, it is assembled late because totals needed to be calculated
                    Dim strHTML1 As String
                    strHTML1 = "<table style=" & ControlChars.Quote & "width:200px; font-size:8pt; margin-left:auto; margin-right:auto;" & ControlChars.Quote & " cellpadding=" & ControlChars.Quote & "1" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & ">"
                    'strHTML += "<tr><td colspan='6' style='font-size:10pt; FONT-WEIGHT: bold; TEXT-ALIGN: center'>Organization Chargeback</td></tr>"
                    strHTML1 += "<tr style=" & ControlChars.Quote & "width:200px; background-color:#e0e0e0; border-left-color: #000000; border-bottom-color: #000000; border-top-style: solid; border-top-color: #000000; border-right-style: solid; border-left-style: solid; border-right-color: #000000; border-bottom-style: solid;" & ControlChars.Quote & "><td colspan='5' style=" & ControlChars.Quote & "text-align:left;  background-color:#e0e0e0; border-right-color:#e0e0e0; border-right-style: solid;" & ControlChars.Quote & ">Facility Number: </td><td style=" & ControlChars.Quote & "text-align:right;  background-color:#e0e0e0; border-left-color: #e0e0e0; border-left-style: solid;" & ControlChars.Quote & ">" & strID & "</td></tr>"
                    strHTML1 += "<tr style=" & ControlChars.Quote & "width:200px; background-color:#e0e0e0; border-left-color: #000000; border-bottom-color: #000000; border-top-style: solid; border-top-color: #000000; border-right-style: solid; border-left-style: solid; border-right-color: #000000; border-bottom-style: solid;" & ControlChars.Quote & "><td colspan='5' style=" & ControlChars.Quote & "text-align:left;  background-color:#e0e0e0; border-right-color:#e0e0e0; border-right-style: solid;" & ControlChars.Quote & ">Facility Rentable (sqft): </td><td style=" & ControlChars.Quote & "text-align:right;  background-color:#e0e0e0; border-left-color: #e0e0e0; border-left-style: solid;" & ControlChars.Quote & ">" & dblChgSum & "</td></tr>"
                    strHTML1 += "<tr style=" & ControlChars.Quote & "width:200px; background-color:#e0e0e0; border-left-color: #000000; border-bottom-color: #000000; border-top-style: solid; border-top-color: #000000; border-right-style: solid; border-left-style: solid; border-right-color: #000000; border-bottom-style: solid;" & ControlChars.Quote & "><td colspan='5' style=" & ControlChars.Quote & "text-align:left;  background-color:#e0e0e0; border-right-color:#e0e0e0; border-right-style: solid;" & ControlChars.Quote & ">Rentable per Employee (sqft): </td><td style=" & ControlChars.Quote & "text-align:right;  background-color:#e0e0e0; border-left-color: #e0e0e0; border-left-style: solid;" & ControlChars.Quote & ">" & dblNameCt & "</td></tr>"
                    strHTML1 += "<tr style=" & ControlChars.Quote & " border-bottom:solid; border-bottom-color:#000000;" & ControlChars.Quote & ">"
                    'put the html strings together
                    strHTML = strHTML1 & strHTML

                    'create individual chart series objects for each category
                    'generate the dojo format chart series for the pie chart
                    Dim strDojoPieSeries As String = BuildDojoChartSeries(alSeries, dblChgSum)
                    'send back the javascript to fill in the TCO tool's floating panel
                    Return strHTML & "*?*?*?*" & strDojoPieSeries
                Else 'the query didnt' find the building
                    Return "<table style=" & ControlChars.Quote & "width:100%;  font-size:8pt; margin-left:auto; margin-right:auto;" & ControlChars.Quote & "><tr><td>Information not found for facility " & strID & "</td></tr></table>"
                End If
            Else
                Return "<table style=" & ControlChars.Quote & "width:100%; font-size:8pt; margin-left:auto; margin-right:auto;" & ControlChars.Quote & "><tr><td>Information not found for facility " & strID & "</td></tr></table>"
                'Return String.Empty
            End If
        Catch ex As Exception
            Return ex.ToString
        End Try
    End Function

    Private Function GetElecCCInfo(ByVal BldID As String) As OracleDataReader
        '  Try
        BldID = BldID.TrimStart("0")
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        myConnection.Open()
        Dim myCommand As New OracleCommand("store_GetAverageInventoryPrice", myConnection)
        'myCommand.CommandType
        myCommand.CommandText = "SELECT FACILITY_NBR, SquadronDivision, NameFirstCount, ChargeableAreaSum, ChargeableAreaAvg, TotalCommonAreaAvg, AllocatedRoomAreaAvg  FROM afm.OrgChargeback WHERE (FACILITY_NBR = :FACILITY_NBR) order by ChargeableAreaSum DESC "
        'Else 'get the specified year's data
        '    myCommand.CommandText = "SELECT FACILITY_NBR, CATEGORY_CD, CATEGORY_NAME, UYear, SumElec FROM dbo.symElecByCatYr WHERE (FACILITY_NBR = @FACILITY_NBR) AND (UYear = @TheYear) order by SumElec DESC"
        '    myCommand.Parameters.Add("@TheYear", SqlDbType.NVarChar, 10)
        '    myCommand.Parameters("@TheYear").Value = theYear
        myCommand.Parameters.Add("FACILITY_NBR", OracleDbType.Varchar2, 10)
        myCommand.Parameters("FACILITY_NBR").Value = BldID
        Dim reader As OracleDataReader = myCommand.ExecuteReader()

        'Read in the first record
        Return reader
        ' Catch ex As Exception
        '     Return Nothing
        'End Try
    End Function



    Private Function BuildDojoChartSeries(ByVal alSeries As ArrayList, ByVal dblPieTotal As Integer) As String
        'create a series (Title, Number, Color) for the dojo chart
        Dim strSeries As String = "pieChart.addSeries('Series A', [ "
        Dim mySeries As ChartSeries
        For i As Integer = 0 To alSeries.Count - 1
            mySeries = alSeries(i)
            If mySeries.Value > 0 Then
                If (mySeries.Value / dblPieTotal) > 0.08 Then 'if the percentage of the total pie is < 8% then don't show the label
                    strSeries += "{y: " & mySeries.Value & ", text: '', color: '" & mySeries.Color & "', tooltip: '" & mySeries.ToolTip & "'}, "
                Else
                    strSeries += "{y: " & mySeries.Value & ", text: '' , color: '" & mySeries.Color & "', tooltip: '" & mySeries.ToolTip & "'}, "
                End If
            End If
        Next
        If strSeries.Contains("{y:") Then
            strSeries = strSeries.Remove(strSeries.Length - 2, 2)
        End If
        strSeries += "]);"
        Return strSeries

    End Function


    Public Function RandomQBColor(ByVal loopNo As Integer) As String

        Dim myInt As Integer = (loopNo * 50) + 6500
        Dim rnd As Random = New Random(myInt)
        'Dim color_num As Integer = rRnd.Next(0, 15)
        Dim c As Drawing.Color
        c = Drawing.Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))

        Return System.Drawing.ColorTranslator.ToHtml(c)

    End Function

    Private Class ChartSeries
        'holds the values for creating a dojo chart series string
        Public Title As String
        Public Value As Integer
        Public Color As String
        Public ToolTip As String
        Public Sub New(ByVal NewTitle As String, ByVal NewValue As Integer, ByVal NewColor As String, ByVal NewToolTip As String)
            Title = NewTitle
            Value = NewValue
            Color = NewColor
            ToolTip = NewToolTip
        End Sub
    End Class
End Class
