Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System
Imports System.Collections


Public Class TCOper

    Public Function GetChartHTML(ByVal BldID As String, ByVal strFY As String) As String

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BldID.Trim.Length > 0 Then
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
                myCommand.CommandType = CommandType.Text
                myCommand.CommandText = "select * from vFacStats"

                Dim reader As OracleDataReader = myCommand.ExecuteReader()
                If reader.HasRows = True Then
                    If reader.Read() Then

                        Dim strHTML As String
                        'setup dollar format strings for table
                        Dim strElec As String = "$0.00"
                        Dim strGas As String = "$0.00"
                        Dim strWater As String = "$0.00"
                        Dim strSewg As String = "$0.00"
                        Dim strJant As String = "$0.00"
                        Dim strLab As String = "$0.00"
                        Dim strMat As String = "$0.00"

                        Dim strStm As String = "$0.00"
                        Dim strCompAir As String = "$0.00"
                        Dim strChillWtr As String = "$0.00"

                        Dim strCons As String = "$0.00"
                        Dim strOth As String = "$0.00"
                        Dim strF_Area As String = String.Empty
                        Dim strEnerCost As String = "$0.00"
                        Dim strWatSew As String = "$0.00"
                        Dim strWO As String = "$0.00"
                        Dim strBaseCosts As String = "$0.00"
                        Dim strGenOper As String = "$0.00"
                        Dim strCostSF As String = "$0.00"
                        'Dim strTCo As String = string.empty
                        'Dim strTCoSF As String = string.empty

                        If Not reader("totElectric") Is DBNull.Value Then
                            strElec = String.Format("{0:c}", Convert.ToDouble(reader("totElectric")))
                        End If
                        If Not reader("totNatGas") Is DBNull.Value Then
                            strGas = String.Format("{0:c}", Convert.ToDouble(reader("totNatGas")))
                        End If
                        If Not reader("totWater") Is DBNull.Value Then
                            strWater = String.Format("{0:c}", Convert.ToDouble(reader("totWater")))
                        End If
                        If Not reader("totSewer") Is DBNull.Value Then
                            strSewg = String.Format("{0:c}", Convert.ToDouble(reader("totSewer")))
                        End If

                        If Not reader("totSteam") Is DBNull.Value Then
                            strStm = String.Format("{0:c}", Convert.ToDouble(reader("totSteam")))
                        End If
                        If Not reader("totCompAir") Is DBNull.Value Then
                            strCompAir = String.Format("{0:c}", Convert.ToDouble(reader("totCompAir")))
                        End If
                        If Not reader("totChillH2O") Is DBNull.Value Then
                            strChillWtr = String.Format("{0:c}", Convert.ToDouble(reader("totChillH2O")))
                        End If

                        If Not reader("Facility_Janitorial_Cost") Is DBNull.Value Then
                            strJant = String.Format("{0:c}", Convert.ToDouble(reader("Facility_Janitorial_Cost")))
                        End If
                        If Not reader("sumLABCOST") Is DBNull.Value Then
                            strLab = String.Format("{0:c}", Convert.ToDouble(reader("sumLABCOST")))
                        End If
                        If Not reader("sumMATCOST") Is DBNull.Value Then
                            strMat = String.Format("{0:c}", Convert.ToDouble(reader("sumMATCOST")))
                        End If
                        If Not reader("sumCONTCOST") Is DBNull.Value Then
                            strCons = String.Format("{0:c}", Convert.ToDouble(reader("sumCONTCOST")))
                        End If
                        If Not reader("sumOTHCOST") Is DBNull.Value Then
                            strOth = String.Format("{0:c}", Convert.ToDouble(reader("sumOTHCOST")))
                        End If
                        If Not reader("area_gross_ext") Is DBNull.Value Then
                            strF_Area = String.Format("{0:c}", Convert.ToDouble(reader("area_gross_ext")))
                        End If
                        If Not reader("TotalEnergyCost") Is DBNull.Value Then
                            strEnerCost = String.Format("{0:c}", Convert.ToDouble(reader("TotalEnergyCost")))
                        End If
                        If Not reader("TotalWaterSewageCost") Is DBNull.Value Then
                            strWatSew = String.Format("{0:c}", Convert.ToDouble(reader("TotalWaterSewageCost")))
                        End If
                        If Not reader("sumTotalWO") Is DBNull.Value Then
                            strWO = String.Format("{0:c}", Convert.ToDouble(reader("sumTotalWO")))
                        End If
                        If Not reader("BaseCost") Is DBNull.Value Then
                            strBaseCosts = String.Format("{0:c}", Convert.ToDouble(reader("BaseCost")))
                        End If
                        If Not reader("GeneralOperatingCost") Is DBNull.Value Then
                            strGenOper = String.Format("{0:c}", Convert.ToDouble(reader("GeneralOperatingCost")))
                        End If
                        If Not reader("generaloperatingcostGSF") Is DBNull.Value Then
                            strCostSF = String.Format("{0:c}", Convert.ToDouble(reader("generaloperatingcostGSF")))
                        End If
                        'If Not reader("TCo") Is DBNull.Value Then
                        '    strTCo = String.Format("{0:c}", Convert.ToDouble(reader("TCo")))
                        'End If
                        'If Not reader("TcoSF") Is DBNull.Value Then
                        '    strTCoSF = String.Format("{0:c}", Convert.ToDouble(reader("TcoSF")))
                        'End If



                        'build html table
                        'strHTML = "<table cellpadding=" & ControlChars.Quote & "5" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & "><tr style=" & ControlChars.Quote & "width:258px; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & "><td style=" & ControlChars.Quote & "text-align:left; font-size:10pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & ">Facility Number: </td><td style=" & ControlChars.Quote & "text-align:right; font-size:11pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-bottom-color: #e0e0e0; border-top-style: solid; border-top-color: #e0e0e0; border-right-style: solid; border-left-style: solid; border-right-color: #e0e0e0; border-bottom-style: solid;" & ControlChars.Quote & ">" & strBldgNos & "</td></tr>"
                        'strHTML = "<div id=" & ControlChars.Quote & "chartDiv" & ControlChars.Quote & " style=" & ControlChars.Quote & "margin-left:auto; margin-right:auto;" & ControlChars.Quote & "></div>"
                        strHTML = "<table class='GISResultsTable' cellpadding=" & ControlChars.Quote & "1" & ControlChars.Quote & " cellspacing=" & ControlChars.Quote & "0" & ControlChars.Quote & ">"
                        'strHTML += "<tr><td colspan='2' class ='GISTitleTD'>Total Cost of Operations</td></tr>"
                        ''strHTML += "<tr style=" & ControlChars.Quote & "width:200px; background-color:#e0e0e0; border-left-color: #000000; border-bottom-color: #000000; border-top-style: solid; border-top-color: #000000; border-right-style: solid; border-left-style: solid; border-right-color: #000000; border-bottom-style: solid;" & ControlChars.Quote & "><td style=" & ControlChars.Quote & "text-align:left;  background-color:#e0e0e0; border-right-color:#e0e0e0; border-right-style: solid;" & ControlChars.Quote & ">Facility Number: </td><td style=" & ControlChars.Quote & "text-align:right; font-size:8pt; background-color:#e0e0e0; border-left-color: #e0e0e0; border-left-style: solid;" & ControlChars.Quote & ">" & strID & "</td></tr>"
                        'strHTML += "<tr class='GISChartFacilNoTR'><td class='GISChartFacilNoLeftTD'>Facility Number: </td><td class='GISChartFacilNoRightTD' >" & strID & "</td></tr>"

                        strHTML += "<tr><td>Electricity:</td><td class='GISChartRightTD'>" & strElec & "</td></tr>"
                        strHTML += "<tr><td>Natural Gas:</td><td class='GISChartRightTD'>" & strGas & "</td> </tr>"
                        strHTML += "<tr><td>Compressed Air:</td><td class='GISChartRightTD'>" & strCompAir & "</td> </tr>"
                        strHTML += "<tr><td>Steam:</td><td class='GISChartRightTD'>" & strStm & "</td> </tr>"
                        strHTML += "<tr><td>Chilled Water:</td><td class='GISChartRightTD'>" & strChillWtr & "</td> </tr>"
                        strHTML += "<tr><td>Water:</td><td class='GISChartRightTD'>" & strWater & "</td></tr>"
                        strHTML += "<tr><td>Sewage:</td><td class='GISChartRightTD'>" & strSewg & "</td></tr>"
                        strHTML += "<tr><td>Janitorial:</td><td class='GISChartRightTD'>" & strJant & "</td></tr>"
                        strHTML += "<tr><td>Labor:</td><td class='GISChartRightTD'>" & strLab & "</td></tr>"
                        strHTML += "<tr><td>Material:</td><td class='GISChartRightTD'>" & strMat & "</td></tr>"
                        strHTML += "<tr><td>Construction:</td><td class='GISChartRightTD'>" & strCons & "</td></tr>"
                        strHTML += "<tr><td>Other:</td><td class='GISUnderlineTD'>" & strOth & "</td></tr>"



                        strHTML += "<tr><td class='GISChartTDBold'>General Operating Cost:</td><td class='GISChartRightTDBold'>" & strGenOper & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>Operating Cost per GSF:</td><td class='GISChartRightTDBold'>" & strCostSF & "</td></tr>"
                        strHTML += "</table>"
                        'create individual chart series objects for each category
                        Dim alSeries As ArrayList = New ArrayList
                        Dim myChartSeries As ChartSeries
                        If Not reader("totElectric") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Elec", Convert.ToInt32(reader("totElectric")), "#008625", "Electricity: " & strElec)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("totNatGas") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Gas", Convert.ToInt32(reader("totNatGas")), "#FF9900", "Natural Gas: " & strGas)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("totSteam") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Steam", Convert.ToInt32(reader("totSteam")), "#260085", "Steam: " & strStm)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("totChillH2O") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Chill H2O", Convert.ToInt32(reader("totChillH2O")), "#B31919", "Chilled Water: " & strChillWtr)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("totCompAir") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Comp Air", Convert.ToInt32(reader("totCompAir")), "#00E6FF", "Compressed Air: " & strCompAir)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("totWater") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Water", Convert.ToInt32(reader("totWater")), "#550099", "Water: " & strWater)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("totSewer") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Sewer", Convert.ToInt32(reader("totSewer")), "#00BF35", "Sewage: " & strSewg)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("Facility_Janitorial_Cost") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Janit", Convert.ToInt32(reader("Facility_Janitorial_Cost")), "#B36B00", "Janitorial: " & strJant)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("sumLABCOST") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Labor", Convert.ToInt32(reader("sumLABCOST")), "#1919B3", "Labor: " & strLab)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("sumMATCOST") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Matl", Convert.ToInt32(reader("sumMATCOST")), "#9191FF", "Material: " & strMat)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("sumCONTCOST") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Const", Convert.ToInt32(reader("sumCONTCOST")), "#B34700", "Construction: " & strCons)
                            alSeries.Add(myChartSeries)
                        End If
                        If Not reader("sumOTHCOST") Is DBNull.Value Then
                            myChartSeries = New ChartSeries("Other", Convert.ToInt32(reader("sumOTHCOST")), "#C8C8FF", "Other: " & strOth)
                            alSeries.Add(myChartSeries)
                        End If
                        'generate the dojo format chart series for the pie chart
                        Dim strDojoPieSeries As String = BuildDojoChartSeries(alSeries, Convert.ToDouble(reader("GeneralOperatingCost")))
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
    '    Try
    '        BldID = BldID.TrimStart("0")
    '        Dim myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
    '        myConnection.Open()
    '        Dim myCommand As New SqlCommand("FacilityPerformaceTCOwnerOccupByFacilFY", myConnection)
    '        'myCommand.CommandType
    '        myCommand.CommandType = CommandType.StoredProcedure

    '        myCommand.Parameters.Add("@Facil", SqlDbType.NVarChar, 10)
    '        myCommand.Parameters("@Facil").Value = BldID
    '        myCommand.Parameters.Add("@FY", SqlDbType.NVarChar, 10)
    '        myCommand.Parameters("@FY").Value = strFY
    '        Dim reader As SqlDataReader = myCommand.ExecuteReader()

    '        'Read in the first record
    '        Return reader
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function



    Private Function BuildDojoChartSeries(ByVal alSeries As ArrayList, ByVal dblPieTotal As Integer) As String
        'create a series (Title, Number, Color) for the dojo chart
        Dim strSeries As String = "pieChart.addSeries('Series A', [ "
        Dim mySeries As ChartSeries
        For i As Integer = 0 To alSeries.Count - 1
            mySeries = alSeries(i)
            If mySeries.Value > 0 Then
                If (mySeries.Value / dblPieTotal) > 0.015 Then 'if the percentage of the total pie is < 1.5% then don't show the label
                    strSeries += "{y: " & mySeries.Value & ", text: '" & mySeries.Title & "', color: '" & mySeries.Color & "', tooltip: '" & mySeries.ToolTip & "'}, "
                Else
                    strSeries += "{y: " & mySeries.Value & ", text: '', color: '" & mySeries.Color & "', tooltip: '" & mySeries.ToolTip & "'}, "
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
        Public ToolTip As String
        Public Sub New(ByVal NewTitle As String, ByVal NewValue As Int32, ByVal NewColor As String, ByVal NewToolTip As String)
            Title = NewTitle
            Value = NewValue
            Color = NewColor
            ToolTip = NewToolTip
        End Sub
    End Class

End Class
