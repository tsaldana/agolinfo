Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System

Public Class getLists
    Function CreateBldgList(ByVal cbo As String) As String
        Dim strHTML As String = String.Empty

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            myConnection.Open()
            Dim myCommand As New OracleCommand("BldgList", myConnection)
            'myCommand.CommandText = "select convert(integer, BL_ID) as BL_ID from afm.BL order by BL_ID"
            myCommand.CommandText = "SELECT to_number(BL_ID) AS BL_ID FROM BL WHERE bl_id NOT LIKE 'BASE%' AND BL_ID NOT LIKE '%-%' ORDER BY BL_ID"
            Dim reader As OracleDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If reader.HasRows = True Then
                strHTML = "var jsonData = { identifier: 'index', items: [], label: 'bldg' };"
                strHTML += "var storeBldg = new dojo.data.ItemFileWriteStore( { data: jsonData } );"
                strHTML += "storeBldg.newItem({index:0,bldg:''});"
                Dim i As Integer = 1
                While reader.Read()
                    Dim bl_id As String = String.Empty
                    If Not reader("bl_id") Is DBNull.Value Then
                        bl_id = Convert.ToString(reader("bl_id")).Trim
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

    Function CreateWingList(ByVal cbo As String) As String
        Dim strHTML As String = String.Empty

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            myConnection.Open()
            Dim myCommand As New OracleCommand("WingList", myConnection)
            myCommand.CommandText = "select distinct WING_ID from RM where WING_ID is not null and WING_ID <> 'NK' and WING_ID <> 'NA' order by WING_ID"
            Dim reader As OracleDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If reader.HasRows = True Then
                strHTML = "var jsonData = { identifier: 'index', items: [], label: 'wing' };"
                strHTML += "var storeWing = new dojo.data.ItemFileWriteStore( { data: jsonData } );"
                strHTML += "storeWing.newItem({index:0,wing:''});"
                Dim i As Integer = 1
                While reader.Read()
                    Dim wing_id As String = String.Empty
                    If Not reader("wing_id") Is DBNull.Value Then
                        wing_id = Convert.ToString(reader("wing_id")).Trim
                    Else
                        wing_id = i.ToString
                    End If
                    strHTML += "storeWing.newItem({index:" & i & ",wing:'" & wing_id & "'});"
                    'strHTML += "store" & strJSCommand & ".newItem({index:" & i & ",org:'" & reader(0).ToString & "'});"
                    i += 1
                End While
            ElseIf reader.HasRows = False Then 'bl_id's were not returned from the db
                'strHTML = "HeyYouNot"
            End If
            strHTML += "var cboCtl = dijit.byId('" & cbo & "');"
            strHTML += "cboCtl.attr('store', storeWing);"
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

    Function CreateCatCodeList(ByVal cbo As String, ByVal CatCodeWing As String) As String
        Dim strHTML As String = String.Empty

        'strHTML = "hey"

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            myConnection.Open()
            Dim myCommand As New OracleCommand("CatCodeList", myConnection)
            Dim sSQL As String = String.Empty
            sSQL = "SELECT distinct afm.RM.CATCODE_ID, afm.CATCODES.NAME FROM afm.RM INNER JOIN afm.CATCODES ON RM.CATCODE_ID = CATCODES.CATCODE_ID where WING_ID = :WING"
            'sSQL = "Select distinct catcode_id from afm.rm where wing_id = WING"
            myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
            myCommand.Parameters("WING").Value = CatCodeWing

            myCommand.CommandText = sSQL
            'myCommand.CommandText = "select convert(integer, bl_id) as bl_id from afm.bl order by bl_id"
            Dim reader As OracleDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If reader.HasRows = True Then
                'strHTML = "gotRows"
                'strHTML = "var jsonData = { identifier: 'index', items: [], label: 'catcodename', value: 'catcode' };"
                strHTML = "var jsonData = { identifier: 'index', items: [], label: 'catcode'};"
                strHTML += "var storeCatCode = new dojo.data.ItemFileWriteStore( { data: jsonData } );"
                strHTML += "storeCatCode.newItem({index:0,catcode:''});"
                Dim i As Integer = 1
                While reader.Read()
                    Dim catcode_id As String = String.Empty
                    Dim name As String = String.Empty
                    If Not reader("catcode_id") Is DBNull.Value Then
                        catcode_id = Convert.ToString(reader("catcode_id")).Trim
                    End If
                    If Not reader("name") Is DBNull.Value Then
                        name = Convert.ToString(reader("name")).Trim
                    End If
                    strHTML += "storeCatCode.newItem({index:" & i & ",catcode:'" & catcode_id & " - " & name & "'});"
                    'strHTML += "store" & strJSCommand & ".newItem({index:" & i & ",org:'" & reader(0).ToString & "'});"
                    i += 1
                End While
                strHTML += "var cboCtl = dijit.byId('" & cbo & "');"
                strHTML += "cboCtl.attr('store', storeCatCode);"
                strHTML += "cboCtl.attr('disabled', false);"
                strHTML += "cboCtl.attr('value', '');"
                'strHTML += "cboCtl.attr('valueField', catcode);"
                'strHTML += "cboCtl.attr('displayedField', catcodename);"
                strHTML += "dijit.byId('" & cbo & "').domNode.style.display = '';"
                strHTML += "document.getElementById('ccTitle').style.display = '';"
                strHTML += "document.getElementById('ccCbo').style.display = '';"
                strHTML += "document.getElementById('CatCodeTitle').style.display = '';"
            ElseIf reader.HasRows = False Then 'bl_id's were not returned from the db
                '        'strHTML = "HeyYouNot"
            End If

            reader.Close()
            Return strHTML

        Catch ex As Exception
            strHTML += ex.Message
            Return "CreateCatCodeList Error: " & strHTML
        End Try
    End Function

    Function CreateOSCREList(ByVal cbo As String, ByVal oscre As String) As String
        'OSCRE level 1,2,3 data is contained in afm.rm.rm_cat, rm_type, rm_detail

        Dim strHTML As String = String.Empty
        Dim sOSCREVals As String()
        sOSCREVals = oscre.Split("|")
        Dim intGroupLevel As Integer = GetOSCRELevel(sOSCREVals)

        'build disabled ComboBox control markup
        Dim sCboMkUp As String = String.Empty
        Dim sGoBtnEnabled As String = String.Empty

        Select Case intGroupLevel
            Case 0
                sCboMkUp = enableCboOSCRE("cboSelWingOSCREX", sOSCREVals, "wing")
                sCboMkUp += disableCboOSCRE("cboSelOSCRE1", "oscre1")
                sCboMkUp += disableCboOSCRE("cboSelOSCRE2", "oscre2")
                sCboMkUp += disableCboOSCRE("cboSelOSCRE3", "oscre3")
                'sGoBtnEnabled = "dijit.byId('doQueryButton').setAttribute('disabled', true);"
                sGoBtnEnabled = "var orgGISBtn = document.getElementById('doQueryButton');"
                sGoBtnEnabled += "orgGISBtn.Attributes.Add('disabled', 'disabled');"
                sCboMkUp += "document.getElementById('dChkFilter').style.display = 'none';"
                'sGoBtnEnabled = "dijit.byId('doQueryButton').setAttribute('disabled', true);"
            Case 1
                sCboMkUp = enableCboOSCRE("cboSelOSCRE1", sOSCREVals, "oscre1")
                sCboMkUp += disableCboOSCRE("cboSelOSCRE2", "oscre2")
                sCboMkUp += disableCboOSCRE("cboSelOSCRE3", "oscre3")
                sCboMkUp += "document.getElementById('dChkFilter').style.display = 'none';"
                'sGoBtnEnabled = "dijit.byId('doQueryButton').setAttribute('disabled', true);"
            Case 2
                sCboMkUp = enableCboOSCRE("cboSelOSCRE2", sOSCREVals, "oscre2")
                sCboMkUp += disableCboOSCRE("cboSelOSCRE3", "oscre3")
                sCboMkUp += "document.getElementById('dChkFilter').style.display = 'inline';"
                'sGoBtnEnabled = "dijit.byId('doQueryButton').setAttribute('disabled', false);"
            Case 3
                sCboMkUp = enableCboOSCRE("cboSelOSCRE3", sOSCREVals, "oscre3")
                sCboMkUp += "document.getElementById('dChkFilter').style.display = 'inline';"
                'sGoBtnEnabled = "dijit.byId('doQueryButton').setAttribute('disabled', false);"
                'don't build any controls
            Case Else 'something went wrong

        End Select
        Dim strRoomCountMkUp As String = String.Empty
        ''strRoomCountMkUp = getRoomCount(intGroupLevel, sVals, blnFiltered)
        sCboMkUp += strRoomCountMkUp
        If Not strRoomCountMkUp.Contains("doQueryButton") Then 'if the room count markup contains the 'doQueryButton' then room count was zero and the 'go' button disable mkup is already in the string
            sCboMkUp += sGoBtnEnabled
        End If


        Return sCboMkUp

    End Function

    Private Function GetOSCRELevel(ByVal sOSCREVals As String()) As Integer
        Dim intGroupLevel As Integer = 0

        'add one to intGroupLevel to determine the lowest level of group chosen
        If sOSCREVals(0).Length > 0 Then
            intGroupLevel = 1
        Else 'Nothing supplied - this is on load
            'intGroupLevel += 1
        End If
        If sOSCREVals(1).Length > 0 Then
            intGroupLevel += 1
        End If
        If sOSCREVals(2).Length > 0 Then
            intGroupLevel += 1
        End If
        If sOSCREVals(3).Length > 0 Then
            intGroupLevel += 1
        End If
        'If sOSCREVals(4).Length > 0 Then
        '    intGroupLevel += 1
        'End If
        Return intGroupLevel
    End Function

    Private Function disableCboOSCRE(ByVal cboName As String, ByVal strJSName As String) As String
        'disable unused comboboxes
        Dim sCbo As String = String.Empty

        'sCbo = "clearDisableCbo('" & cboName & "');"
        Select Case cboName 'hide the titles too
            Case "cboSelWingOSCREX"
                sCbo += "document.getElementById('wingOSCRETitle').style.display = 'none';"
            Case "cboSelOSCRE1"
                sCbo += "document.getElementById('o1Title').style.display = 'none';"
                sCbo += "document.getElementById('o1Cbo').style.display = 'none';"
                sCbo += "dijit.byId('cboSelOSCRE1').set('value', '0');"
                sCbo += "document.getElementById('OSCRE1Title').style.display = 'none';"
            Case "cboSelOSCRE2"
                sCbo += "document.getElementById('o2Title').style.display = 'none';"
                sCbo += "document.getElementById('o2Cbo').style.display = 'none';"
                sCbo += "dijit.byId('cboSelOSCRE2').set('value', '0');"
                sCbo += "document.getElementById('OSCRE2Title').style.display = 'none';"
                sCbo += "document.getElementById('dChkFilter').style.display = 'none';"
            Case "cboSelOSCRE3"
                sCbo += "document.getElementById('o3Title').style.display = 'none';"
                sCbo += "document.getElementById('o3Cbo').style.display = 'none';"
                sCbo += "dijit.byId('cboSelOSCRE3').set('value', '0');"
                sCbo += "document.getElementById('OSCRE3Title').style.display = 'none';"
                sCbo += "document.getElementById('dChkFilter').style.display = 'none';"
        End Select

        Return sCbo
    End Function

    Private Function enableCboOSCRE(ByVal cboName As String, ByVal orgVals() As String, ByVal strJSCommand As String) As String
        Dim reader As OracleDataReader = GetOSCREReader(cboName, orgVals) 'query the db
        Dim strHTML As String

        strHTML = "var jsonData = { identifier: 'index', items: [], label: 'oscre' };"
        strHTML += "var store" & strJSCommand & " = new dojo.data.ItemFileWriteStore( { data: jsonData } );"
        strHTML += "store" & strJSCommand & ".newItem({index:0,oscre:''});"
        If reader.HasRows Then
            Dim i As Integer = 1
            While reader.Read
                strHTML += "store" & strJSCommand & ".newItem({index:" & i & ",oscre:'" & reader(0).ToString.Trim & "'});"
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
            Case "cboSelWingOSCRE"
                strHTML += "document.getElementById('wingOSCRETitle').style.display = '';"
            Case "cboSelOSCRE1"
                strHTML += "document.getElementById('OSCRE1Title').style.display = '';"
                strHTML += "document.getElementById('o1Title').style.display = '';"
                strHTML += "document.getElementById('o1Cbo').style.display = '';"
            Case "cboSelOSCRE2"
                strHTML += "document.getElementById('OSCRE2Title').style.display = '';"
                strHTML += "document.getElementById('o2Title').style.display = '';"
                strHTML += "document.getElementById('o2Cbo').style.display = '';"
            Case "cboSelOSCRE3"
                strHTML += "document.getElementById('OSCRE3Title').style.display = '';"
                strHTML += "document.getElementById('o3Title').style.display = '';"
                strHTML += "document.getElementById('o3Cbo').style.display = '';"
        End Select

        Return strHTML

    End Function

    Private Function GetOSCREReader(ByVal cboName As String, ByVal OSCREVals() As String) As OracleDataReader
        'query the database for the values that will appear in the combo box

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        myConnection.Open()
        Dim myCommand As New OracleCommand("OSCREVals", myConnection)

        'Read in the first record
        Dim sSQL As String = String.Empty

        Select Case cboName
            Case "cboSelWingOSCREX"
                sSQL = "Select distinct WING_ID from afm.RM"
            Case "cboSelOSCRE1"
                sSQL = "Select distinct RM_CAT from afm.RM where WING_ID = :WING"
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = OSCREVals(0)
            Case "cboSelOSCRE2"
                sSQL = "Select distinct RM_TYPE from afm.RM where WING_ID = :WING and RM_CAT = :RMCAT"
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = OSCREVals(0)
                myCommand.Parameters.Add("RMCAT", OracleDbType.Varchar2, 10)
                myCommand.Parameters("RMCAT").Value = OSCREVals(1)
            Case "cboSelOSCRE3"
                sSQL = "Select distinct RM_DETAIL from afm.RM where WING_ID = :WING and RM_CAT = :RMCAT and RM_TYPE = :RMTYPE"
                myCommand.Parameters.Add("WING", OracleDbType.Varchar2, 10)
                myCommand.Parameters("WING").Value = OSCREVals(0)
                myCommand.Parameters.Add("RMCAT", OracleDbType.Varchar2, 10)
                myCommand.Parameters("RMCAT").Value = OSCREVals(1)
                myCommand.Parameters.Add("RMTYPE", OracleDbType.Varchar2, 10)
                myCommand.Parameters("RMTYPE").Value = OSCREVals(2)
        End Select

        myCommand.CommandText = sSQL
        Dim reader As OracleDataReader = myCommand.ExecuteReader()
        Return reader

    End Function

    Function CreateSelWingBldgList(ByVal wing As String) As String
        Dim strHTML As String = String.Empty  ' Holds Query Params for eval()
        Dim strHTMl2 As String = String.Empty ' Holds Selected BL list for eval()

        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            myConnection.Open()
            Dim myCommand As New OracleCommand("WingList", myConnection)
            myCommand.CommandText = "SELECT DISTINCT BL.BL_ID, BL.BL_STATUS FROM BL JOIN RM ON BL.BL_ID = RM.BL_ID WHERE RM.WING_ID = '" & wing & "' ORDER BY BL_ID"
            'myCommand.CommandText = "SELECT BL.BL_ID, BL.BL_STATUS, RM.BL_ID FROM BL where WING_ID = '" & wing & "' FROM BL JOIN RM ON BL.BL_ID = RM.BL_ID"
            '            SELECT BL.BL_ID, BL.BL_STATUS, RM.BL_ID
            'FROM BL JOIN RM
            'ON BL.BL_ID = RM.BL_ID
            'myCommand.CommandText = "select distinct BL_ID from RM where WING_ID = '" & wing & "' order by BL_ID"
            Dim reader As OracleDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If reader.HasRows = True Then
                strHTMl2 = "<table style=class='GISTable'>"
                strHTMl2 += "<tr><td style='font-weight: bold;'>"
                strHTMl2 += "Buildings Occupied By " & wing & "</td></tr>"

                strHTML += "bldgs_query.returnGeometry = true;"
                Dim i As Integer = 1
                While reader.Read()
                    Dim BL_STATUS As String
                    BL_STATUS = reader(1)

                    If i = 1 Then
                        strHTML += "bldgs_query.where = " & Chr(34) & "FACIL_ID = '" & reader(0) & "'"
                        If BL_STATUS = "A" Then
                            BL_STATUS = "Active"
                            strHTMl2 += "<tr><td><a class='GisOrgBldgLinkDiv' onclick='zoomToBldg(" & reader(0) & ");'> " & " Bldg " & reader(0) & ";" & " Status: " & BL_STATUS & "</a></td></tr>"
                        ElseIf BL_STATUS = "D" Then
                            BL_STATUS = "Demolished"
                            strHTMl2 += "<tr><td><a class='GisOrgBldgDiv' > " & " Bldg " & reader(0) & ";" & " Status: " & BL_STATUS & "</a></td></tr>"
                        ElseIf BL_STATUS = "N" Then
                            BL_STATUS = "Non Tracked"
                            strHTMl2 += "<tr><td><a class='GisOrgBldgDiv' > " & " Bldg " & reader(0) & ";" & " Status: " & BL_STATUS & "</a></td></tr>"
                        End If

                    ElseIf i > 1 Then
                        strHTML += " OR FACIL_ID = '" & reader(0) & "'"
                        If BL_STATUS = "A" Then
                            BL_STATUS = "Active"
                            strHTMl2 += "<tr><td><a class='GisOrgBldgLinkDiv' onclick='zoomToBldg(" & reader(0) & ");'> " & " Bldg " & reader(0) & ";" & " Status: " & BL_STATUS & "</a></td></tr>"
                        ElseIf BL_STATUS = "D" Then
                            BL_STATUS = "Demolished"
                            strHTMl2 += "<tr><td><a class='GisOrgBldgDiv' > " & " Bldg " & reader(0) & ";" & " Status: " & BL_STATUS & "</a></td></tr>"
                        ElseIf BL_STATUS = "N" Then
                            BL_STATUS = "Non Tracked"
                            strHTMl2 += "<tr><td><a class='GisOrgBldgDiv' > " & " Bldg " & reader(0) & ";" & " Status: " & BL_STATUS & "</a></td></tr>"

                        End If
                        'strHTMl2 += "<tr><td><a class='GisOrgBldgLinkDiv' onclick='zoomToBldg(" & reader(0) & ");'> " & i & " Bldg " & reader(0) & " Zip " & reader(1) & "</a></td></tr>"
                    End If
                    i += 1
                End While
                strHTML += Chr(34) & ";"
                strHTML += "queryBuildingsTask.execute(bldgs_query, showFeatures);"
                strHTMl2 += "</table>"
                strHTMl2 = "dojo.byId(" & ControlChars.Quote & "divOrgCharge" & ControlChars.Quote & ").innerHTML = " & ControlChars.Quote & strHTMl2 & ControlChars.Quote & ";"
                strHTML = strHTML & strHTMl2
            ElseIf reader.HasRows = False Then 'bl_id's were not returned from the db
                strHTML = "No buildings found"
            End If
            reader.Close()
            Return strHTML
        Catch ex As Exception
            strHTML += ex.Message
            Return "Error: " & strHTML
        End Try
    End Function

End Class


