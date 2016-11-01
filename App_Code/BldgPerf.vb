Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System


Public Class BldgPerf
    Public Sub New()

    End Sub

    Public Function Performance(ByVal BldID As String, ByVal strFY As String) As String

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BldID.Trim.Length > 0 Then
                If BldID.StartsWith("0") Then
                    BldID = BldID.TrimStart("0"c)
                End If

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

                'Dim myCommand As New OracleCommand("BLMETRICS.BLPERF", myConnection)
                'myCommand.CommandType
                'myCommand.CommandType = CommandType.StoredProcedure

                'myCommand.Parameters.Add("Facil", OracleDbType.VarChar2, 10)
                'myCommand.Parameters("Facil").Value = BldID
                'myCommand.Parameters.Add("FY", OracleDbType.VarChar2, 10)
                'myCommand.Parameters("FY").Value = strFY


                Dim reader As OracleDataReader = myCommand.ExecuteReader()
                If reader.HasRows = True Then
                    If reader.Read() Then

                        Dim strHTML As String
                        'setup table
                        Dim strName As String = Convert.ToString(reader("LOCAL_DESIGNATION"))
                        Dim strAddress As String = Convert.ToString(reader("Address1"))
                        Dim strSiteCode As String = Convert.ToString(reader("INSTL_CODE"))
                        Dim strDtBuilt As String = ""
                        Dim strUse As String = ""
                        Dim strOccup As String = ""
                        Dim strIntArea As String = ""
                        Dim strExtArea As String = ""
                        Dim strRentArea As String = ""
                        Dim strUsableArea As String = ""
                        Dim strPctUsable As String = ""
                        Dim strRURatio As String = ""
                        Dim strEffRate As String = ""
                        Dim strAreaPerEmp As String = ""
                        Dim strReplacement As String = ""
                        Dim strEnergy As String = ""
                        Dim strProject As String = ""
                        Dim strADA As String = ""
                        Dim strCondition As String = ""
                        Dim strUtiliz As String = "100"
                        Dim strOper As String = ""
                        'Dim strRank As String = ""
                        Dim strTCOccup As String = ""
                        Dim strTCOwn As String = ""

                        strName = Convert.ToString(reader("LOCAL_DESIGNATION"))
                        strAddress = Convert.ToString(reader("Address1"))
                        strSiteCode = Convert.ToString(reader("INSTL_CODE"))
                        strDtBuilt = ""
                        If Not reader("FACILITY_BUILT_DT") Is DBNull.Value Then
                            strDtBuilt = Convert.ToString(FormatDateTime(reader("FACILITY_BUILT_DT"), DateFormat.ShortDate))
                        End If
                        If Not reader("PrimaryCatCode") Is DBNull.Value Then
                            strUse = Convert.ToString(reader("PrimaryCatCode"))
                        End If
                        If Not reader("count_occup") Is DBNull.Value Then
                            strOccup = Convert.ToString(reader("count_occup"))
                        End If
                        If Not reader("area_gross_int") Is DBNull.Value Then
                            strIntArea = Convert.ToString(reader("area_gross_int"))
                        End If
                        If Not reader("area_gross_ext") Is DBNull.Value Then
                            strExtArea = Convert.ToString(reader("area_gross_ext"))
                        End If
                        If Not reader("area_rentable") Is DBNull.Value Then
                            strRentArea = Convert.ToString(reader("area_rentable"))
                        End If
                        If Not reader("area_usable") Is DBNull.Value Then
                            strUsableArea = Convert.ToString(reader("area_usable"))
                        End If
                        If Not reader("area_percent_usable") Is DBNull.Value Then
                            strPctUsable = Convert.ToString(reader("area_percent_usable"))
                        End If
                        If Not reader("ratio_ru") Is DBNull.Value Then
                            strRURatio = Convert.ToString(reader("ratio_ru"))
                        End If
                        If Not reader("ratio_ur") Is DBNull.Value Then
                            strEffRate = Convert.ToString(reader("ratio_ur"))
                        End If
                        If Not reader("area_avg_em") Is DBNull.Value Then
                            strAreaPerEmp = Convert.ToString(reader("area_avg_em"))
                        End If
                        If Not reader("PRV") Is DBNull.Value Then
                            strReplacement = String.Format("{0:c}", Convert.ToDouble(reader("PRV")))
                        End If
                        If Not reader("TotalEnergyCost") Is DBNull.Value Then
                            strEnergy = String.Format("{0:c}", Convert.ToDouble(reader("TotalEnergyCost")))
                        End If
                        If Not reader("ProjectCost") Is DBNull.Value Then
                            strProject = String.Format("{0:c}", Convert.ToDouble(reader("ProjectCost")))
                        End If
                        If Not reader("ADA_COMPLIANCE") Is DBNull.Value Then
                            strADA = Convert.ToString(reader("ADA_COMPLIANCE"))
                        End If
                        If Not reader("CONDITION_CD") Is DBNull.Value Then
                            strCondition = Convert.ToString(reader("CONDITION_CD"))
                        End If
                        ' strUtiliz = "100"
                        If Not reader("GeneralOperatingCost") Is DBNull.Value Then
                            strOper = String.Format("{0:c}", Convert.ToDouble(reader("GeneralOperatingCost")))
                        End If
                        If Not reader("TCOccup") Is DBNull.Value Then
                            strTCOccup = String.Format("{0:c}", Convert.ToDouble(reader("TCOccup")))
                        End If
                        If Not reader("TCOwner") Is DBNull.Value Then
                            strTCOwn = String.Format("{0:c}", Convert.ToDouble(reader("TCOwner")))
                        End If




                        'build html table
                        'strHTML = "<table cellpadding=" & ControlChars.Quote & "5" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & "><tr style=" & ControlChars.Quote & "width:258px; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & "><td style=" & ControlChars.Quote & "text-align:left; font-size:10pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & ">Facility Number: </td><td style=" & ControlChars.Quote & "text-align:right; font-size:11pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & ">" & strBldgNos & "</td></tr>"
                        'strHTML = "<div id=" & ControlChars.Quote & "chartDiv" & ControlChars.Quote & " style=" & ControlChars.Quote & "margin-left:auto; margin-right:auto;" & ControlChars.Quote & "></div>"
                        strHTML = "<table class='GISResultsTable' cellpadding=" & ControlChars.Quote & "1" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & ">"
                        'strHTML += "<tr><td colspan='2'  class ='GISTitleTD'>Building Performance</td></tr>"
                        'strHTML += "<tr class='GISChartFacilNoTR'><td class='GISChartFacilNoLeftTD'>Facility Number: </td><td class='GISChartFacilNoRightTD'>" & strID & "</td></tr>"
                        strHTML += "<tr><td>Building Name:</td><td class='GISChartRightTD'>" & strName & "</td></tr>"
                        strHTML += "<tr><td>Address:</td><td class='GISChartRightTD'>" & strAddress & "</td> </tr>"
                        strHTML += "<tr><td>Site Code:</td><td class='GISChartRightTD'>" & strSiteCode & "</td></tr>"
                        strHTML += "<tr><td>Date Built:</td><td class='GISChartRightTD'>" & strDtBuilt & "</td></tr>"
                        strHTML += "<tr><td>Use(Primary Cat Code):</td><td class='GISChartRightTD'>" & strUse & "</td></tr>"
                        strHTML += "<tr><td>Occupancy:</td><td class='GISChartRightTD'>" & strOccup & "</td></tr>"
                        strHTML += "<tr><td>Interior Area(sqft):</td><td class='GISChartRightTD'>" & strIntArea & "</td></tr>"
                        strHTML += "<tr><td>Exterior Area(sqft):</td><td class='GISChartRightTD'>" & strExtArea & "</td></tr>"
                        strHTML += "<tr><td>Rentable Area(sqft):</td><td class='GISChartRightTD'>" & strRentArea & "</td></tr>"
                        strHTML += "<tr><td>Usable Area(sqft):</td><td class='GISChartRightTD'>" & strUsableArea & "</td></tr>"
                        strHTML += "<tr><td>% Usable:</td><td class='GISChartRightTD'>" & strPctUsable & "</td></tr>"
                        strHTML += "<tr><td>R/U Ratio:</td><td class='GISChartRightTD'>" & strRURatio & "</td></tr>"
                        strHTML += "<tr><td>Efficiency Rate:</td><td class='GISChartRightTD'>" & strEffRate & "</td></tr>"
                        strHTML += "<tr><td>Avg Area per Employee:</td><td class='GISChartRightTD'>" & strAreaPerEmp & "</td></tr>"
                        strHTML += "<tr><td>Replacement Value:</td><td class='GISChartRightTD'>" & strReplacement & "</td></tr>"
                        strHTML += "<tr><td>Annual Energy Cost:</td><td class='GISChartRightTD'>" & strEnergy & "</td></tr>"
                        strHTML += "<tr><td>Projects Cost:</td><td class='GISChartRightTD'>" & strProject & "</td></tr>"
                        strHTML += "<tr><td>ADA Compliant:</td><td class='GISChartRightTD'>" & strADA & "</td></tr>"
                        strHTML += "<tr><td>Condition Index:</td><td class='GISChartRightTD'>" & strCondition & "</td></tr>"
                        strHTML += "<tr><td>Utilization Index:</td><td class='GISChartRightTD'>" & strUtiliz & "</td></tr>"
                        strHTML += "<tr><td>Annual Operating Cost:</td><td class='GISChartRightTD'>" & strOper & "</td></tr>"
                        'strHTML += "<tr><td>Demobilizing Rank:</td><td class='GISChartRightTD'>" & strSewg & "</td></tr>"
                        strHTML += "<tr><td>Cost of Occupancy:</td><td class='GISChartRightTD'>" & strTCOccup & "</td></tr>"
                        strHTML += "<tr><td>Cost of Ownership:</td><td class='GISChartRightTD'>" & strTCOwn & "</td></tr>"
                        'strHTML += "<tr><td>Other:</td><td style=" & ControlChars.Quote & "text-align:right; border-bottom:solid; border-bottom-color:#000000; border-bottom-width:2px;" & ControlChars.Quote & ">" & strOth & "</td></tr>"
                        'strHTML += "<tr><td>General Operating Cost:</td><td class='GISChartRightTD'>" & strGenOper & "</td></tr>"
                        ' strHTML += "<tr><td>Operating Cost per GSF:</td><td class='GISChartRightTD'>" & strCostSF & "</td></tr>"
                        strHTML += "</table>"
                        'create individual chart series objects for each category
                        'Dim alSeries As ArrayList = New ArrayList
                        'Dim myChartSeries As New ChartSeries("Elec", Convert.ToInt32(reader("Elec")), "#008625")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Gas", Convert.ToInt32(reader("Gas")), "#FF9900")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Water", Convert.ToInt32(reader("Water")), "#550099")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Sewer", Convert.ToInt32(reader("sewg")), "#00BF35")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Janit", Convert.ToInt32(reader("jant")), "#B36B00")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Labor", Convert.ToInt32(reader("Lab")), "#1919B3")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Matl", Convert.ToInt32(reader("Mat")), "#9191FF")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Const", Convert.ToInt32(reader("Cons")), "#B34700")
                        'alSeries.Add(myChartSeries)
                        'myChartSeries = New ChartSeries("Other", Convert.ToInt32(reader("Oth")), "#C8C8FF")


                        'alSeries.Add(myChartSeries)
                        'generate the dojo format chart series for the pie chart
                        'Dim strDojoPieSeries As String = BuildDojoChartSeries(alSeries, Convert.ToDouble(reader("GenOper")))
                        'send back the javascript to fill in the TCO tool's floating panel
                        Return strHTML '& "*?*?*?*"
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
            Return ex.ToString
        Finally
            myConnection.Close()
        End Try
    End Function

    'Private Function GetTCOInfo(ByVal BldID As String, ByVal strFY As String) As SqlDataReader
    '    Try
    '        'Get rid of the leading zero for the building id, this view doesnt' use leading zeros
    '        'If BldID.StartsWith("0") Then
    '        '    BldID = BldID.TrimStart("0"c)
    '        'End If
    '        'Dim myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
    '        'myConnection.Open()
    '        'Dim myCommand As New SqlCommand("FacilityPerformaceTCOwnerOccupByFacilFY", myConnection)
    '        ''myCommand.CommandType
    '        'myCommand.CommandType = CommandType.StoredProcedure

    '        'myCommand.Parameters.Add("@Facil", SqlDbType.NVarChar, 10)
    '        'myCommand.Parameters("@Facil").Value = BldID
    '        'myCommand.Parameters.Add("@FY", SqlDbType.NVarChar, 10)
    '        'myCommand.Parameters("@FY").Value = strFY
    '        'Dim reader As SqlDataReader = myCommand.ExecuteReader()

    '        'Read in the first record
    '        Return reader
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function


    'Private Function BuildDojoChartSeries(ByVal alSeries As ArrayList, ByVal dblPieTotal As Integer) As String
    '    'create a series (Title, Number, Color) for the dojo chart
    '    Dim strSeries As String = "pieChart.addSeries('Series A', [ "
    '    Dim mySeries As ChartSeries
    '    For i As Integer = 0 To alSeries.Count - 1
    '        mySeries = alSeries(i)
    '        If mySeries.Value > 0 Then
    '            If (mySeries.Value / dblPieTotal) > 0.015 Then 'if the percentage of the total pie is < 1.5% then don't show the label
    '                strSeries += "{y: " & mySeries.Value & ", text: '" & mySeries.Title & "', color: '" & mySeries.Color & "'}, "
    '            Else
    '                strSeries += "{y: " & mySeries.Value & ", text: '', color: '" & mySeries.Color & "'}, "
    '            End If
    '        End If
    '    Next
    '    If strSeries.Contains("{y:") Then
    '        strSeries = strSeries.Remove(strSeries.Length - 2, 2)
    '    End If
    '    strSeries += "]);"
    '    Return strSeries

    'End Function
    'Private Class ChartSeries
    '    'holds the values for creating a dojo chart series string
    '    Public Title As String
    '    Public Value As Int32
    '    Public Color As String
    '    Public Sub New(ByVal NewTitle As String, ByVal NewValue As Int32, ByVal NewColor As String)
    '        Title = NewTitle
    '        Value = NewValue
    '        Color = NewColor
    '    End Sub
    'End Class

End Class
