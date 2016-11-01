Imports Microsoft.VisualBasic
Imports System.Data
'Imports System.Data.SqlClient
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System.Collections
Imports System

Public Class UtilByCatCode
    Private strYearFromQuery As String = String.Empty
    Public Function GetChartHTML(ByVal BldID As String, ByVal strFY As String) As String
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BldID.Length > 0 Then
                BldID = BldID.TrimStart("0")

                myConnection.Open()
                Dim myCommand As New OracleCommand("TOT_ENER_FAC_CATCODE_FY.TOT_NRG_FAC_CC_FY", myConnection)
                'myCommand.CommandText = "TOT_ENER_FAC_CATCODE_FY.TOT_NRG_FAC_CC_FY"
                'myCommand.CommandType = CommandType.StoredProcedure

                myCommand.CommandText = "SELECT All_Utils_By_Fac_Cat_Yr.Fiscal_Year, All_Utils_By_Fac_Cat_Yr.FACILITY_NBR, ROUND(All_Utils_By_Fac_Cat_Yr.totElectric + All_Utils_By_Fac_Cat_Yr.totNatGas + All_Utils_By_Fac_Cat_Yr.totCompAir + All_Utils_By_Fac_Cat_Yr.totSteam + All_Utils_By_Fac_Cat_Yr.totChillH2O, 2) AS totalEnergyCost, GIS_CCLIST.CCList FROM utilities.All_Utils_By_Fac_Cat_Yr, GIS_CCLIST WHERE (All_Utils_By_Fac_Cat_Yr.Fiscal_Year = :Year) AND (All_Utils_By_Fac_Cat_Yr.FACILITY_NBR = :bl_id) AND (All_Utils_By_Fac_Cat_Yr.Cat_Code = GIS_CCLIST.CCNUM)"

                'myCommand.CommandType
                'myCommand.CommandText = "Select * from afm.sp_TCOccupByBldYr"
                'Dim sSQL As String = String.Empty

                myCommand.Parameters.Add("Year", OracleDbType.Varchar2, 10)
                myCommand.Parameters("Year").Value = strFY
                myCommand.Parameters.Add("bl_id", OracleDbType.Varchar2, 10)
                myCommand.Parameters("bl_id").Value = BldID
                'myCommand.CommandText = sSQL

                Dim reader As OracleDataReader = myCommand.ExecuteReader()
                'Return "Got Past the Reader" & reader.FieldCount
                Dim strHTML As String
                'setup dollar format strings for table
                Dim strCATEGORY_CD As String = ""
                Dim strCATEGORY_NAME As String = ""
                'Dim strUYear As String = ""
                Dim strSumElec As String = ""
                Dim alSeries As ArrayList = New ArrayList
                Dim pieTotal As Double = 0
                Dim strColor As String = String.Empty
                strHTML = "<table class='GISResultsTable' cellpadding=" & ControlChars.Quote & "1" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & ">"
                'strHTML += "<tr><td colspan='3' class ='GISTitleTD'>Energy Use by Cat Code</td></tr>"
                'strHTML += "<tr><td colspan='3' class ='GISTitleTD'>FY 2007</td></tr>"
                'strHTML += "<tr class='GISChartFacilNoTR'><td class='GISChartFacilNoLeftTD' colspan='2'>Facility Number: </td><td class='GISChartFacilNoRightTD'>" & strID & "</td></tr>"
                If reader.HasRows = True Then
                    Dim cntr As Integer = 0
                    While reader.Read()
                        If Not reader("CCLIST") Is DBNull.Value Then
                            strCATEGORY_CD = Convert.ToString(reader("CCLIST"))
                        Else
                            strCATEGORY_CD = String.Empty
                        End If

                        'If Not reader("CATEGORY_NAME") Is DBNull.Value Then
                        '    strCATEGORY_NAME = Convert.ToString(reader("CATEGORY_NAME"))
                        'Else
                        '    strCATEGORY_NAME = String.Empty
                        'End If

                        'If Not reader("UYear") Is DBNull.Value Then
                        '    strUYear = Convert.ToString(reader("UYear"))
                        'Else
                        '    strUYear = String.Empty
                        'End If

                        If Not reader("totalEnergyCost") Is DBNull.Value Then
                            strSumElec = String.Format("{0:c}", Convert.ToDouble(reader("TOTALENERGYCOST")))
                            pieTotal += Convert.ToDouble(reader("TOTALENERGYCOST"))
                        Else
                            strSumElec = String.Empty
                        End If

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
                        strHTML += "<td style=" & ControlChars.Quote & "background-color:" & strColor & "; width:8px;" & ControlChars.Quote & "></td>"
                        strHTML += "<td style='font-size: 6.5pt;'>" & strCATEGORY_CD & "</td>"
                        'strHTML += "<td >" & strCATEGORY_NAME & "</td>"
                        strHTML += "<td style='font-size: 6.5pt;' class='GISChartRightTD'>" & strSumElec & "</td>"
                        strHTML += "</tr>"
                        'create an entry in the chart series
                        Dim myChartSeries As New ChartSeries(strCATEGORY_CD, Convert.ToInt32(reader("totalEnergyCost")), strColor, strCATEGORY_CD & ": " & String.Format("{0:c}", Convert.ToDouble(reader("totalEnergyCost"))))
                        alSeries.Add(myChartSeries)
                        cntr += 1
                    End While
                    strHTML += "<tr><td class='GISChartTDBold' colspan='2'>Total</td><td class='GISChartTDBoldRightOverline'>" & String.Format("{0:c}", pieTotal) & "</td>"
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
                Return String.Empty
            End If
        Catch ex As Exception
            Return ex.ToString
        Finally

            myConnection.Close()
        End Try
    End Function

    'Private Function GetElecCCInfo(ByVal BldID As String, ByVal strFY As String) As SqlDataReader
    '    '  Try
    '    BldID = BldID.TrimStart("0")
    '    Dim myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
    '    myConnection.Open()
    '    Dim myCommand As New SqlCommand("sp_TotalEnergyByFacilCatCodeFYear", myConnection)
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
        Public Value As Int32
        Public Color As String
        Public ToolTip As String
        Public Sub New(ByVal NewTitle As String, ByVal NewValue As Int32, ByVal NewColor As String, ByVal NewToolTip As String)
            Title = NewTitle
            Value = NewValue
            Color = NewColor
            ToolTip = NewToolTip
        End Sub
    End Class
End Class
