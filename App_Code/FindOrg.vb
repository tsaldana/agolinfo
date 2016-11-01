Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System


Public Class FindOrg
    Public Function GetCboCtrls(ByVal sVals As String, ByVal blnFiltered As Boolean) As String
        Dim s As String = String.Empty
        Dim sOrgVals As String()
        sOrgVals = sVals.Split("!")
        'get value representing which level of org was specified
        Dim intGroupLevel As Integer = GetOrgLevel(sOrgVals)

        'build disabled ComboBox control markup
        Dim sCboMkUp As String = String.Empty
        Dim sGoBtnEnabled As String = String.Empty

        Select Case intGroupLevel
            Case 0
                sCboMkUp = BuildEnabledCombo("cmdSelect", sOrgVals, "CMD")
                sCboMkUp += BuildDisabledCombos("wingSelect", "WING")
                sCboMkUp += BuildDisabledCombos("grpSelect", "GROUP")
                sCboMkUp += BuildDisabledCombos("squadSelect", "SQUAD")
                sCboMkUp += BuildDisabledCombos("flightSelect", "FLIGHT")
                sCboMkUp += "document.getElementById('dChkFilter').style.display = 'none';console.log('hey0');"
                'sGoBtnEnabled = "document.getElementById('doQueryButton').style.display = 'inline';"
                'sGoBtnEnabled = "dijit.byId('doQueryButton').setAttribute('disabled', true);"
                ' sGoBtnEnabled = "var orgGISBtn = document.getElementById('GISOrgButton');"
                'sGoBtnEnabled += "orgGISBtn.Attributes.Add('disabled', 'disabled');;"
            Case 1
                sCboMkUp = BuildEnabledCombo("wingSelect", sOrgVals, "WING")
                sCboMkUp += BuildDisabledCombos("grpSelect", "GROUP")
                sCboMkUp += BuildDisabledCombos("squadSelect", "SQUAD")
                sCboMkUp += BuildDisabledCombos("flightSelect", "FLIGHT")
                sCboMkUp += "console.log('hey1');"
                sCboMkUp += "document.getElementById('dChkFilter').style.display = 'none';"
                sCboMkUp += "document.getElementById('cmdGoDiv').style.display = 'none';"
                sCboMkUp += "document.getElementById('dChkFilter').checked = 'false';"
                'sGoBtnEnabled = "document.getElementById('dChkFilter').style.display = 'inline';"
                'sGoBtnEnabled = "document.getElementById('doQueryButton').style.display = 'inline';"
            Case 2
                sCboMkUp = BuildEnabledCombo("grpSelect", sOrgVals, "GROUP")
                sCboMkUp += BuildDisabledCombos("squadSelect", "SQUAD")
                sCboMkUp += BuildDisabledCombos("flightSelect", "FLIGHT")
                sCboMkUp += "document.getElementById('cmdGoDiv').style.display = 'inline';"
                sCboMkUp += "document.getElementById('doQueryButton').style.display = 'inline';"
                sGoBtnEnabled = "document.getElementById('dChkFilter').style.display = 'inline';console.log('hey2');"
            Case 3
                sCboMkUp = BuildEnabledCombo("squadSelect", sOrgVals, "SQUAD")
                sCboMkUp += BuildDisabledCombos("flightSelect", "FLIGHT")
                sCboMkUp += "document.getElementById('cmdGoDiv').style.display = 'inline';"
                sCboMkUp += "document.getElementById('doQueryButton').style.display = 'inline';"
                sGoBtnEnabled = "document.getElementById('dChkFilter').style.display = 'inline';console.log('hey3');"
            Case 4
                sCboMkUp = BuildEnabledCombo("flightSelect", sOrgVals, "FLIGHT")
                sCboMkUp += "document.getElementById('cmdGoDiv').style.display = 'inline';"
                sCboMkUp += "document.getElementById('doQueryButton').style.display = 'inline';"
                sGoBtnEnabled = "document.getElementById('dChkFilter').style.display = 'inline';console.log('hey4');"
            Case 5
                sCboMkUp += "document.getElementById('cmdGoDiv').style.display = 'inline';"
                sCboMkUp += "document.getElementById('doQueryButton').style.display = 'inline';"
                sGoBtnEnabled = "document.getElementById('dChkFilter').style.display = 'inline';console.log('hey5');"
                'don't build any controls
            Case Else 'something went wrong

        End Select
        Dim strRoomCountMkUp As String = String.Empty
        strRoomCountMkUp = getRoomCount(intGroupLevel, sVals, blnFiltered)
        sCboMkUp += strRoomCountMkUp
        If Not strRoomCountMkUp.Contains("doQueryButton") Then 'if the room count markup contains the 'doQueryButton' then room count was zero and the 'go' button disable mkup is already in the string
            sCboMkUp += sGoBtnEnabled
        End If

        Return sCboMkUp
    End Function

    Private Function BuildDisabledCombos(ByVal cboName As String, ByVal strJSName As String) As String
        'create a disabled combobox control
        Dim sCbo As String = String.Empty

        'sCbo = "document.getElementById('div" & strJSName & "').innerHTML = " & ControlChars.Quote & "<select id='" & cboName & "' dojotype = 'dijit.form.ComboBox' Style = 'width:95%;font-size:10px;' " & _
        '    " disabled='disabled' value = ''></select>" & ControlChars.Quote & ";"

        sCbo = "clearDisableCbo('" & cboName & "');"
        Select Case cboName 'hide the titles too
            Case "cmdSelect"
                sCbo += "document.getElementById('commandTitle').style.display = 'none';"
            Case "wingSelect"
                sCbo += "document.getElementById('wingTitle').style.display = 'none';"
            Case "grpSelect"
                sCbo += "document.getElementById('grpTitle').style.display = 'none';"
            Case "squadSelect"
                sCbo += "document.getElementById('squadTitle').style.display = 'none';"
            Case "flightSelect"
                sCbo += "document.getElementById('flightTitle').style.display = 'none';"
        End Select

        Return sCbo
    End Function

    Private Function BuildEnabledCombo(ByVal cboName As String, ByVal orgVals() As String, ByVal strJSCommand As String) As String
        Dim reader As OracleDataReader = GetOrgReader(cboName, orgVals) 'query the db
        Dim strHTML As String
        'strHTML = "document.getElementById('div" & strJSCommand & "').innerHTML = " & ControlChars.Quote & "<select id='" & cboName & "' "
        'strHTML += "dojotype = 'dijit.form.ComboBox'"
        'strHTML += "style = 'width:95%;font-size:10px;'"
        'strHTML += "autoComplete = 'true'"
        'strHTML += "class='dijitComboBox'"
        'strHTML += "forceValidOption = 'false'"
        'strHTML += "value = ''"

        'strHTML += "onChange=\" & ControlChars.Quote & "buildOrgString('" & strJSCommand & "');\" & ControlChars.Quote & ">"

        strHTML = "var jsonData = { identifier: 'index', items: [], label: 'org' };"
        strHTML += "var store" & strJSCommand & " = new dojo.data.ItemFileWriteStore( { data: jsonData } );"
        strHTML += "store" & strJSCommand & ".newItem({index:0,org:''});"
        If reader.HasRows Then
            Dim i As Integer = 1
            While reader.Read
                strHTML += "store" & strJSCommand & ".newItem({index:" & i & ",org:'" & reader(0).ToString & "'});"
                i += 1
            End While
        Else
            'no matching records
        End If

        strHTML += "var cboCtl = dijit.byId('" & cboName & "');"
        strHTML += "cboCtl.attr('store', store" & strJSCommand & ");"
        strHTML += "cboCtl.attr('disabled', false);"
        strHTML += "cboCtl.attr('value', '');"
        strHTML += "dijit.byId('" & cboName & "').domNode.style.display = '';"
        Select Case cboName 'unhide the titles too
            Case "cmdSelect"
                strHTML += "document.getElementById('commandTitle').style.display = '';"
            Case "wingSelect"
                strHTML += "document.getElementById('wTitle').style.display = '';"
                strHTML += "document.getElementById('dWING').style.display = '';"
                strHTML += "document.getElementById('wingTitle').style.display = '';"
                'strHTML += "document.getElementById('dChkFilter').style.display = '';"
            Case "grpSelect"
                strHTML += "document.getElementById('gTitle').style.display = '';"
                strHTML += "document.getElementById('dGROUP').style.display = '';"
                strHTML += "document.getElementById('grpTitle').style.display = '';"
                strHTML += "document.getElementById('dChkFilter').style.display = '';"
            Case "squadSelect"
                strHTML += "document.getElementById('sTitle').style.display = '';"
                strHTML += "document.getElementById('dSQUAD').style.display = '';"
                strHTML += "document.getElementById('squadTitle').style.display = '';"
                strHTML += "document.getElementById('dChkFilter').style.display = '';"
            Case "flightSelect"
                strHTML += "document.getElementById('fTitle').style.display = '';"
                strHTML += "document.getElementById('dFLIGHT').style.display = '';"
                strHTML += "document.getElementById('flightTitle').style.display = '';"
                strHTML += "document.getElementById('dChkFilter').style.display = '';"
        End Select

        Return strHTML




    End Function

    Private Function GetOrgReader(ByVal cboName As String, ByVal orgVals() As String) As OracleDataReader
        'query the database for the values that will appear in the combo box

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        myConnection.Open()
        Dim myCommand As New OracleCommand("GetOrgVals", myConnection)

        'Read in the first record
        Dim sSQL As String = String.Empty

        Select Case cboName
            Case "cmdSelect"
                sSQL = "Select CMD_ID from afm.ORG_CMD"
            Case "wingSelect"
                sSQL = "Select WING_ID from afm.ORG_WING where CMD_ID = :CMD"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgVals(0)
            Case "grpSelect"
                sSQL = "Select GRP_ID from afm.ORG_GRP where CMD_ID = :CMD and WING_ID = :WING ORDER BY GRP_ID"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgVals(0)
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = orgVals(1)
            Case "squadSelect"
                sSQL = "Select SQUADRON_ID from afm.ORG_SQUADRON where CMD_ID = :CMD and WING_ID = :WING and GRP_ID = :GRP"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgVals(0)
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = orgVals(1)
                myCommand.Parameters.Add("GRP", OracleDbType.Varchar2, 10)
                myCommand.Parameters("GRP").Value = orgVals(2)
            Case "flightSelect"
                sSQL = "Select FLIGHT_ID from afm.ORG_FLIGHT where CMD_ID = :CMD and WING_ID = :WING and GRP_ID = :GRP and SQUADRON_ID = :SQUAD"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgVals(0)
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = orgVals(1)
                myCommand.Parameters.Add("GRP", OracleDbType.Varchar2, 10)
                myCommand.Parameters("GRP").Value = orgVals(2)
                myCommand.Parameters.Add("SQUAD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("SQUAD").Value = orgVals(3)
        End Select

        myCommand.CommandText = sSQL
        Dim reader As OracleDataReader = myCommand.ExecuteReader()
        Return reader

    End Function

    Private Function GetOrgLevel(ByVal sOrgVals As String()) As Integer
        Dim intGroupLevel As Integer = 0
        'add one to intGroupLevel to determine the lowest level of group chosen
        If sOrgVals(0).Length > 0 Then '"CMD" is good
            intGroupLevel = 1
        Else 'Nothing supplied - this is on load
            'intGroupLevel += 1
        End If
        If sOrgVals(1).Length > 0 Then '"WING" is good
            intGroupLevel += 1
        End If
        If sOrgVals(2).Length > 0 Then '"GROUP" is good
            intGroupLevel += 1
        End If
        If sOrgVals(3).Length > 0 Then '"SQUAD" is good
            intGroupLevel += 1
        End If
        If sOrgVals(4).Length > 0 Then '"FLIGHT" - everything
            intGroupLevel += 1
        End If
        Return intGroupLevel
    End Function

    Public Function GetOrgCost(ByVal sVals As String, ByVal blnFiltered As Boolean) As String
        Dim OrgVals As String()
        OrgVals = sVals.Split("!")
        Dim intGroupLevel As Integer = GetOrgLevel(OrgVals)
        Dim strWhere As String = String.Empty
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        myConnection.Open()
        Dim myCommand As New OracleCommand("view1", myConnection)
        '--------------------------- myCommand.CommandType
        '--------------------------- myCommand.CommandType = CommandType.StoredProcedure
        strWhere = buildWhere(intGroupLevel, myCommand, OrgVals, blnFiltered)

        Dim strSQL As String
        strSQL = "Select BL_ID, sum(ROOM_AREA) as area, sum(ROOMUTILCHARGES) as charges, area_rentable, sum(pctArea) pctArea " & _
                "From afm.VUTILITYCOSTBYROOM " & _
                strWhere & " GROUP BY BL_ID, AREA_RENTABLE order by cast(BL_ID as NUMERIC)"
        myCommand.CommandText = strSQL

        Try
            myCommand.CommandTimeout = 300
            Dim reader As OracleDataReader = myCommand.ExecuteReader()
            'myCommand.ResetCommandTimeout()
            'Return reader
            Dim strHTML As String = String.Empty
            Dim totalSqFt As Double = 0
            Dim totalChgSqFt As Double = 0
            'Dim totalUtilCost As Double = 0

            If reader.HasRows Then
                While reader.Read
                    Dim orgUtilCost As Double = 0
                    orgUtilCost = reader("charges")
                    totalSqFt += reader("area_rentable")
                    totalChgSqFt += reader("area")
                    'totalUtilCost += orgUtilCost
                    strHTML += "<table class='orgCostTable' style='width:100%;'>"
                    strHTML += "<tr class='GISRadioButtonLV'><td class='orgCostTable' colspan='2' style='font-weight: bold; width:100%;' ><a class='GisOrgBldgLinkDiv' onclick='zoomToBldg(" & reader("bl_id").ToString.Trim & ");'>Bldg " & reader("bl_id").ToString.Trim & "</a></td></tr>"
                    strHTML += "<tr class='GISRadioButtonLV'><td class='orgCostTable'>Sq Ft</td><td class='orgCostTable' style='text-align:right;'>" & Microsoft.VisualBasic.Format(CInt(reader("area").ToString), "##,##0") & "</td></tr>"
                    strHTML += "<tr class='GISRadioButtonLV'><td class='orgCostTable'>Chargeable Sq Ft</td><td class='orgCostTable' style='text-align:right;'>" & Microsoft.VisualBasic.Format(CInt(reader("area_rentable").ToString), "##,##0") & "</td></tr>"
                    strHTML += "<tr class='GISRadioButtonLV'><td class='orgCostTable'>Area %</td><td class='orgCostTable' style='text-align:right;'>" & Microsoft.VisualBasic.Format(CDbl(reader("pctArea").ToString), "##,###.###0") & "%</td></tr>"
                    'strHTML += "<tr class='GISRadioButtonLV'><td class='orgCostTable'>Utility Cost</td><td class='orgCostTable' style='text-align:right;'>" & FormatCurrency(Math.Round(orgUtilCost, 2, MidpointRounding.AwayFromZero).ToString, 2) & "</td></tr>"
                    strHTML += "</table><br/>"
                End While
            End If
            Dim strHTML2 As String = String.Empty
            'build summary table
            strHTML2 = "<table style='width:100%;'>"
            strHTML2 += "<tr><td style='font-weight: bold;'>"
            strHTML2 += "<img id='totSqFtImg' class='GISRadioButtonLV' alt='' src='./images/info.png' />"
            strHTML2 += "Total Sq Ft</td><td style='font-weight: bold; text-align:right;'>" & Microsoft.VisualBasic.Format(CInt(totalSqFt.ToString), "##,##0") & "</td></tr>"
            strHTML2 += "<tr><td style='font-weight: bold;'>"
            strHTML2 += "<img id='totChargableSqFtImg' class='GISRadioButtonLV' alt='' src='./images/info.png' />"
            strHTML2 += "Total Chargeable Sq Ft</td><td style='font-weight: bold; text-align:right;'>" & Microsoft.VisualBasic.Format(CInt(totalChgSqFt.ToString), "##,##0") & "</td></tr>"
            strHTML2 += "<tr><td style='font-weight: bold;'>"
            'strHTML2 += "<img id='totUtilCostImg' class='GISRadioButtonLV' alt='' src='./images/info.png' />"
            'strHTML2 += "Total Utility Cost (FY 08)</td><td style='font-weight: bold; text-align:right;'>" & FormatCurrency(Math.Round(totalUtilCost, 2).ToString, 2) & "</td></tr>"
            strHTML2 += "</table><br/>"

            'put the total table before all of the individual bldg tables
            strHTML = strHTML2 & strHTML
            'wrap the html for filling into a div
            strHTML = "dojo.byId(" & ControlChars.Quote & "divOrgCharge" & ControlChars.Quote & ").innerHTML = " & ControlChars.Quote & strHTML & ControlChars.Quote & ";"
            'add the tooltips
            strHTML += "new dijit.Tooltip({connectId: ['totSqFtImg'],label: 'Total Sq Ft = sum of all net room area for an organization. <br/>Net room area is calculated by measuring room dimensions from <br/>the walls interior finish' });"
            strHTML += "new dijit.Tooltip({connectId: ['totChargableSqFtImg'],label: 'Total Chargeable Sq Ft = sum of all net room area + the <br/>pro rata share of common area for an organization (also referred to <br/>as Rentable SF)' });"
            'strHTML += "new dijit.Tooltip({connectId: ['totUtilCostImg'],label: 'Total Utility Costs = the yearly facility utility costs (FY 08) of an <br/>organization as a function of room type and Total Chargeable SF' });"
            Return strHTML
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function buildWhere(ByVal intGroupLevel As Integer, ByRef myCommand As OracleCommand, ByVal orgvals As String(), ByVal blnFiltered As Boolean) As String

        Dim strWhere As String = String.Empty
        Select Case intGroupLevel
            Case 0 'nothing selected... do nothing?

            Case 1
                strWhere = "WHERE (CMD_ID = :CMD)"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgvals(0)
                If blnFiltered Then
                    strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING) AND (GRP_ID = :GRP) AND (SQUADRON_ID = :SQUAD) AND (FLIGHT_ID = :FLIGHT)"
                    myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("WING").Value = "NA"
                    myCommand.Parameters.Add("GRP", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("GRP").Value = "NA"
                    myCommand.Parameters.Add("SQUAD", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("SQUAD").Value = "NA"
                    myCommand.Parameters.Add("FLIGHT", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("FLIGHT").Value = "NA"
                End If
            Case 2 'WING
                strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING)"
                myCommand.Parameters.Add(":CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters(":CMD").Value = orgvals(0)
                myCommand.Parameters.Add(":WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters(":WING").Value = orgvals(1)
                If blnFiltered Then
                    strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING) AND (GRP_ID = :GRP) AND (SQUADRON_ID = :SQUAD) AND (FLIGHT_ID = :FLIGHT)"
                    myCommand.Parameters.Add("GRP", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("GRP").Value = "NA"
                    myCommand.Parameters.Add("SQUAD", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("SQUAD").Value = "NA"
                    myCommand.Parameters.Add("FLIGHT", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("FLIGHT").Value = "NA"
                End If
            Case 3 'GROUP
                strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING) AND (GRP_ID = :GRP)"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgvals(0)
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = orgvals(1)
                myCommand.Parameters.Add("GRP", OracleDbType.Varchar2, 10)
                myCommand.Parameters("GRP").Value = orgvals(2)
                If blnFiltered Then
                    strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING) AND (GRP_ID = :GRP) AND (SQUADRON_ID = :SQUAD) AND (FLIGHT_ID = :FLIGHT)"
                    myCommand.Parameters.Add("SQUAD", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("SQUAD").Value = "NA"
                    myCommand.Parameters.Add("FLIGHT", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("FLIGHT").Value = "NA"
                End If
            Case 4 'SQUAD
                strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING) AND (GRP_ID = :GRP) AND (SQUADRON_ID = :SQUAD)"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgvals(0)
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = orgvals(1)
                myCommand.Parameters.Add("GRP", OracleDbType.Varchar2, 10)
                myCommand.Parameters("GRP").Value = orgvals(2)
                myCommand.Parameters.Add("SQUAD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("SQUAD").Value = orgvals(3)
                If blnFiltered Then
                    strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING) AND (GRP_ID = :GRP) AND (SQUADRON_ID = :SQUAD) AND (FLIGHT_ID = :FLIGHT)"
                    myCommand.Parameters.Add("FLIGHT", OracleDbType.Varchar2, 10)
                    myCommand.Parameters("FLIGHT").Value = "NA"
                End If
            Case 5 'Flight
                strWhere = "WHERE (CMD_ID = :CMD) AND (WING_ID = :WING) AND (GRP_ID = :GRP) AND (SQUADRON_ID = :SQUAD) AND (FLIGHT_ID = :FLIGHT)"
                myCommand.Parameters.Add("CMD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("CMD").Value = orgvals(0)
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = orgvals(1)
                myCommand.Parameters.Add("GRP", OracleDbType.Varchar2, 10)
                myCommand.Parameters("GRP").Value = orgvals(2)
                myCommand.Parameters.Add("SQUAD", OracleDbType.Varchar2, 10)
                myCommand.Parameters("SQUAD").Value = orgvals(3)
                myCommand.Parameters.Add("FLIGHT", OracleDbType.Varchar2, 10)
                myCommand.Parameters("FLIGHT").Value = orgvals(4)
                If blnFiltered Then

                End If
        End Select
        Return strWhere
    End Function

    Private Function getRoomCount(ByVal intGroupLevel As Integer, ByVal sVals As String, ByVal blnFiltered As Boolean) As String
        'get a count of the rooms that will be returned from the select orgs
        Dim sReturn As String = String.Empty
        If Not intGroupLevel = 0 Then
            Dim OrgVals As String()
            OrgVals = sVals.Split("!")
            Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
            myConnection.Open()
            Dim myCommand As New OracleCommand("GetRoomCount", myConnection)
            Dim strWhere As String = buildWhere(intGroupLevel, myCommand, OrgVals, blnFiltered)
            Dim strSQL As String
            strSQL = "select count(qry.Expr2) from (SELECT distinct(RM_ID || '_' ||  BL_ID || '_' ||  FL_ID) Expr2 " & _
                    "FROM RMPCT " & _
                     strWhere & _
                    " GROUP BY RM_ID, BL_ID, FL_ID) qry"
            myCommand.CommandText = strSQL
            Try
                Dim reader As OracleDataReader = myCommand.ExecuteReader()
                If reader.HasRows Then
                    While reader.Read
                        sReturn = "dojo.byId(" & ControlChars.Quote & "divOrgCharge" & ControlChars.Quote & ").innerHTML = "
                        If reader(0) < 1000 Then 'set the threshold for the map zoom to rooms.  If it's over the threshold (false) then tell user that only tabular results will be shown
                            sReturn += ControlChars.Quote & "<table><tr><td>This selection will return " & reader(0).ToString & " rooms</td></tr></table>" & ControlChars.Quote & ";"
                            'set the number of results returned var so client side javascript can either build room graphics or not
                            sReturn += "numRoomsFromOrgSearch = " & reader(0).ToString & ";"
                        Else
                            sReturn += ControlChars.Quote & "<table><tr><td>This selection will return " & reader(0).ToString & " rooms.</td></tr><tr><td>Tabular results only, no map graphics.</td></tr></table>" & ControlChars.Quote & ";"
                            'set the number of results returned var so client side javascript can either build room graphics or not
                            sReturn += "numRoomsFromOrgSearch = " & reader(0).ToString & ";"
                        End If
                        'if the count is zero, disable the 'go' button
                        If reader(0).ToString = "0" Then
                            sReturn += "document.getElementById('cmdGoDiv').style.display = 'none';"
                            sReturn += "document.getElementById('dChkFilter').style.display = 'none';"
                            sReturn += "document.getElementById('doQueryButton').style.display = 'none';"
                            sReturn += "document.getElementById('dChkFilter').checked = 'false';"
                            'sReturn += "dijit.byId('doQueryButton').setAttribute('disabled', true);"
                        End If
                    End While
                End If
            Catch ex As Exception
            End Try
        Else
            sReturn = "dojo.byId(" & ControlChars.Quote & "divOrgCharge" & ControlChars.Quote & ").innerHTML = " & ControlChars.Quote & ControlChars.Quote & ";"
        End If
        sReturn += "cleargraphics();"
        Return sReturn
    End Function

End Class
