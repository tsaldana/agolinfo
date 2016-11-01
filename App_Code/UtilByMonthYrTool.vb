Imports Microsoft.VisualBasic
Imports System.Data
'Imports System.Data.SqlClient
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System.Collections
Imports System

Public Class UtilByMonthYr
    Private strYearFromQuery As String = String.Empty
    Public Function GetChartHTML(ByVal BldID As String, ByVal strFY As String) As String
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BldID.Length > 0 Then
                BldID = BldID.TrimStart("0")

                myConnection.Open()
                Dim myCommand As New OracleCommand("TOT_ENER_FAC_CATCODE_FY.TOT_NRG_FAC_MONTH_FY", myConnection)
                'myCommand.CommandType
                'myCommand.CommandType = CommandType.StoredProcedure

                'myCommand.CommandType
                'myCommand.CommandText = "Select * from afm.sp_TCOccupByBldYr"
                myCommand.CommandText = "SELECT Fiscal_Year, Fiscal_Month, FACILITY_NBR, ROUND(totElectric + totNatGas + totCompAir + totSteam + totChillH2O , 2) as TotalEnergyCost FROM UTILITIES.All_Utils_By_Fac_Yr_Mo WHERE Fiscal_Year = :Year and FACILITY_NBR = :bl_id order by Fiscal_Month"


                myCommand.Parameters.Add("Year", OracleDbType.Varchar2, 10)
                myCommand.Parameters("Year").Value = strFY
                myCommand.Parameters.Add("bl_id", OracleDbType.Varchar2, 10)
                myCommand.Parameters("bl_id").Value = BldID
                'myCommand.Parameters.Add(New OracleParameter("io_cursor", OracleType.Cursor)).Direction = ParameterDirection.Output
                Dim reader As OracleDataReader = myCommand.ExecuteReader()
                'Return "Got Past the Reader" & reader.FieldCount
                Dim strHTML As String
                'setup dollar format strings for table
                Dim strUMonth As String = ""
                Dim strUMonthShort As String = ""
                Dim strUYear As String = ""
                Dim strSumElec As String = ""
                Dim alSeries As ArrayList = New ArrayList
                Dim pieTotal As Double = 0
                strHTML = "<table class='GISResultsTable' cellpadding=" & ControlChars.Quote & "1" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & ">"
                ' strHTML += "<tr><td colspan='3' class='GISTitleTD'>Monthly Energy Use</td></tr>"
                If reader.HasRows = True Then
                    Dim intMonth As Integer
                    While reader.Read()

                        If Not reader("Fiscal_Month") Is DBNull.Value Then
                            intMonth = Convert.ToInt32(reader("Fiscal_Month"))
                            If intMonth < 5 Then 'account for fiscal year.  
                                intMonth += 8 'add 8 to get the calendar month 1=september + 8 = 9, etc.
                            Else
                                intMonth -= 4 'subtract 4 to get the calendar month 5=january -4 = 1, etc.
                            End If
                            strUMonth = MonthName(intMonth, False)
                            strUMonthShort = strUMonth.Substring(0, 1) 'only use the first letter of the month for labels
                        Else
                            strUMonth = String.Empty
                            strUMonthShort = String.Empty
                        End If

                        If Not reader("Fiscal_Year") Is DBNull.Value Then
                            strUYear = Convert.ToString(reader("Fiscal_Year"))
                        Else
                            strUYear = String.Empty
                        End If

                        If Not reader("TotalEnergyCost") Is DBNull.Value Then
                            strSumElec = String.Format("{0:c}", Convert.ToDouble(reader("TotalEnergyCost")))
                            pieTotal += Convert.ToDouble(reader("TotalEnergyCost"))
                        Else
                            strSumElec = String.Empty
                        End If

                        'If strYearFromQuery = String.Empty Then 'only create these tablerows the first time through the read loop
                        '    If Not theYear = "" Then
                        '        strHTML += "<tr><td colspan='2' class='GISTitleTD'>FY 2007</td></tr>"
                        '        strYearFromQuery = strUYear
                        '    Else
                        '        strYearFromQuery = strUYear
                        '        strHTML += "<tr><td colspan='2' class='GISTitleTD'>FY 2007</td></tr>"
                        '    End If
                        '    strHTML += "<tr class='GISChartFacilNoTR'><td class='GISChartFacilNoLeftTD'>Facility Number: </td><td class='GISChartFacilNoRightTD'>" & strID & "</td></tr>"
                        'End If
                        'build html table
                        'strHTML = "<div><div id=" & ControlChars.Quote & "chartDiv" & ControlChars.Quote & " style=" & ControlChars.Quote & "margin-left:auto; margin-right:auto;" & ControlChars.Quote & "></div>"

                        strHTML += "<tr>"
                        strHTML += "<td style='width: 65%;'>" & strUMonth & "</td>"
                        strHTML += "<td class='GISChartRightTD'>" & strSumElec & "</td>"
                        strHTML += "</tr>"
                        'create an entry in the chart series
                        Dim myChartSeries As New ChartSeries(strUMonthShort, Convert.ToInt32(reader("TotalEnergyCost")))
                        alSeries.Add(myChartSeries)

                    End While
                    strHTML += "<tr><td class='GISChartTDBold'>Total</td><td class='GISChartTDBoldRightOverline'>" & String.Format("{0:c}", pieTotal) & "</td>"
                    strHTML += "</table>"
                    'create individual chart series objects for each category

                    'generate the dojo format chart series for the pie chart
                    Dim strDojoPieSeries As String = BuildDojoChartSeries(alSeries, pieTotal)
                    'send back the javascript to fill in the TCO tool's floating panel
                    Return strHTML & "*?*?*?*" & strDojoPieSeries
                Else 'the query didnt' find the building
                    Return "<table class='GISResultsTable'><tr><td>Information not found for facility " & BldID & "</td></tr></table>"
                End If
            Else
                Return String.Empty '"alert('Information not found for building " & strID & "');"
            End If

        Catch ex As Exception
            Return ex.ToString
        Finally

            myConnection.Close()
        End Try
    End Function

    'Private Function GetElecMonthlyInfo(ByVal BldID As String, ByVal strFY As String) As SqlDataReader
    '    '  Try
    '    BldID = BldID.TrimStart("0")
    '    Dim myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
    '    myConnection.Open()
    '    Dim myCommand As New SqlCommand("sp_TotalEnergyByFacilFMonthFYear", myConnection)
    '    'myCommand.CommandType
    '    myCommand.CommandType = CommandType.StoredProcedure

    '    'myCommand.CommandType
    '    'myCommand.CommandText = "Select * from afm.sp_TCOccupByBldYr"

    '    myCommand.Parameters.Add("@bl_id", SqlDbType.NVarChar, 10)
    '    myCommand.Parameters("@bl_id").Value = BldID
    '    myCommand.Parameters.Add("@Year", SqlDbType.NVarChar, 10)
    '    myCommand.Parameters("@Year").Value = strFY
    '    Dim reader As SqlDataReader = myCommand.ExecuteReader()

    '    'Read in the first record
    '    Return reader
    '    ' Catch ex As Exception
    '    '     Return Nothing
    '    'End Try
    'End Function



    Private Function BuildDojoChartSeries(ByVal alSeries As ArrayList, ByVal dblPieTotal As Integer) As String
        'create a series (Title, Number, Color) for the dojo chart
        Dim strSeries As String = "barChart.addSeries('Series 1', [ "
        Dim strLabels As String = "barChart.addAxis('x', {majorTickStep: 1, labels: ["
        Dim mySeries As ChartSeries
        Dim minY As Integer = 1000000000
        Dim maxY As Integer = 0
        For i As Integer = 0 To alSeries.Count - 1
            mySeries = alSeries(i)
            If mySeries.Value > 0 Then
                If minY > (mySeries.Value * 0.9) Then minY = Math.Round((mySeries.Value * 0.9), 0) 'y min is ~10% lower than lowest value
                If maxY < (mySeries.Value) Then maxY = Math.Round((mySeries.Value), 0) 'y max is ~10% more than highest value
                strSeries += mySeries.Value & ", "
                strLabels += " {value: " & Convert.ToString(i + 1) & ", text: '" & mySeries.Title.ToString & "'}, "
            End If
        Next

        If strSeries.Length > 30 Then 'remove the ', ' from the end of the string
            strSeries = strSeries.Remove(strSeries.Length - 2, 2)
            strLabels = strLabels.Remove(strLabels.Length - 2, 2) & "]}); "
        End If
        strSeries += "], {fill: '#BD5351'});"
        strSeries = strLabels & strSeries
        'calculate max and min y axis values
        Dim yTickStep As Integer 'find the range so that tick steps  can be set
        Select Case maxY - minY
            Case Is < 1000
                yTickStep = 100
            Case Is < 5000
                yTickStep = 500
            Case Is < 10000
                yTickStep = 1000
            Case Is < 50000
                yTickStep = 5000
            Case Is < 100000
                yTickStep = 10000
            Case Else
                yTickStep = 50000
        End Select


        Select Case maxY
            Case Is < 1000
                maxY = Convert.ToInt32(Math.Round(maxY / 100, 2) * 100) + 10
                minY = 0
                'yTickStep = 100
            Case Is < 5000
                maxY = Convert.ToInt32(Math.Round(maxY / 500, 2) * 500) + 50
                minY = 0
                'yTickStep = 100
            Case Is < 10000
                maxY = Convert.ToInt32(Math.Round(maxY / 1000, 2) * 1000) + 100
                minY = Convert.ToInt32(Math.Floor(minY / 100) * 100)
                'yTickStep = 1000
            Case Is < 50000
                maxY = Convert.ToInt32(Math.Round(maxY / 5000, 2) * 5000) + 500
                minY = Convert.ToInt32(Math.Floor(minY / 500) * 500)
            Case Is < 100000
                maxY = Convert.ToInt32(Math.Round(maxY / 10000, 2) * 10000) + 1000
                minY = Convert.ToInt32(Math.Floor(minY / 1000) * 1000)
                'yTickStep = 10000
            Case Is < 500000
                maxY = Convert.ToInt32(Math.Round(maxY / 50000, 2) * 50000) + 5000
                minY = Convert.ToInt32(Math.Floor(minY / 5000) * 5000)
            Case Else
                maxY = Convert.ToInt32(Math.Round(maxY / 100000, 2) * 100000) + 10000
                minY = Convert.ToInt32(Math.Floor(minY / 10000) * 10000)
                'yTickStep = 50000
        End Select


        strSeries = "barChart.addAxis('y', {vertical: true, majorTickStep: " & yTickStep & ",  includeZero: false, min: " & minY & ", max: " & maxY & "}); " & strSeries
        Return strSeries

    End Function

    Private Class ChartSeries
        'holds the values for creating a dojo chart series string
        Public Title As String
        Public Value As Int32
        Public Sub New(ByVal NewTitle As String, ByVal NewValue As Int32)
            Title = NewTitle
            Value = NewValue
        End Sub
    End Class
    Public Function RandomQBColor(ByVal loopNo As Integer) As String

        Dim myInt As Integer = (loopNo * 50) + 7500
        Dim rnd As Random = New Random(myInt)
        'Dim color_num As Integer = rRnd.Next(0, 15)
        Dim c As Drawing.Color
        c = Drawing.Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))

        Return System.Drawing.ColorTranslator.ToHtml(c)

    End Function
End Class
