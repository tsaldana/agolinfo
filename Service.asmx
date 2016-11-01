<%@ WebService Language="vb"  Class="Service" %>

Imports System
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
 
<WebService([Namespace]:="http://tempuri.org/")> _
<WebServiceBinding([ConformsTo]:=WsiProfiles.BasicProfile1_1)> _
<ScriptService()> _
Public Class Service
    Inherits System.Web.Services.WebService
 
    <WebMethod()> _
    <ScriptMethod([ResponseFormat]:=ResponseFormat.Json, [UseHttpGet]:=True)> _
    Public Function getFMLWData() As String
        Dim BldID As String = String.Empty
        If Me.Context.Request.QueryString("bldg") IsNot Nothing Then
            BldID = Me.Context.Request.QueryString("bldg")
        End If
        Dim FYear As String = String.Empty
        If Me.Context.Request.QueryString("FY") IsNot Nothing Then
            FYear = Me.Context.Request.QueryString("FY")
        End If
        Dim UserID As String = String.Empty
        If Me.Context.Request.QueryString("usr") IsNot Nothing Then
            UserID = Me.Context.Request.QueryString("usr")
        End If
        Dim sEUID As String = String.Empty
        If Me.Context.Request.QueryString("EUID") IsNot Nothing Then
            sEUID = Me.Context.Request.QueryString("EUID")
        End If

        'processing for Room Occupant query
        'Dim BLID As String = String.Empty
        'If Me.Context.Request.QueryString("BLID") IsNot Nothing Then
        '    BLID = Me.Context.Request.QueryString("BLID")
        'End If
        Dim FLID As String = String.Empty
        If Me.Context.Request.QueryString("FLID") IsNot Nothing Then
            FLID = Me.Context.Request.QueryString("FLID")
        End If
        Dim RMID As String = String.Empty
        If Me.Context.Request.QueryString("RMID") IsNot Nothing Then
            RMID = Me.Context.Request.QueryString("RMID")
        End If

        
        Dim s As String = String.Empty
        Dim b As String
        Me.Context.Request.ContentType = "application/json"
        Me.Context.Response.ContentType = "application/json"
        
        ' me.Context.Response.ContentEncoding = 
        b = Me.Context.Response.HeaderEncoding.ToString
        
        If BldID = "null" Then
            s = "There is no selected facility."
            Return s
        End If
        
        Select Case Me.Context.Request.QueryString("Action")
            Case "EUs"
                Dim AP As EnvAirPollutants = New EnvAirPollutants()
                s = AP.GetEmissions(sEUID, FYear)
            Case "Perf"
                Dim BP As BldgPerf = New BldgPerf()
                s = BP.Performance(BldID, FYear)
            Case "ROs"
                Dim RO As RoomData_Occupants = New RoomData_Occupants()
                s = RO.GetRoomOrganizations(BldID, FLID, RMID)
            Case "RStds"
                Dim RO As RoomData_Occupants = New RoomData_Occupants()
                s = RO.GetRoomEmStandards(BldID, FLID, RMID)
            Case "RReport"
                Dim RO As RoomData_Occupants = New RoomData_Occupants()
                s = RO.GetRoomReports(BldID, FLID, RMID)
            Case "Own"
                Dim Own As TCOwner = New TCOwner
                s = Own.GetChartHTML(BldID, FYear)
            Case "Operate"
                Dim Operate As TCOper = New TCOper
                s = Operate.GetChartHTML(BldID, FYear)
            Case "EnergyCC"
                Dim EC As UtilByCatCode = New UtilByCatCode
                s = EC.GetChartHTML(BldID, FYear)
            Case "EnergyMo"
                Dim EM As UtilByMonthYr = New UtilByMonthYr
                s = EM.GetChartHTML(BldID, FYear)
            Case "OrgChgBk"
                Dim OCB As OrgChargebackTool = New OrgChargebackTool
                s = OCB.GetChartHTML(BldID)
            Case "OrgCbo"
                Dim FO As New FindOrg
                s = FO.GetCboCtrls(Me.Context.Request.QueryString("orgs"), CBool(Me.Context.Request.QueryString("orgsFiltered")))
            Case "OrgCost"
                Dim FO As New FindOrg
                s = FO.GetOrgCost(Me.Context.Request.QueryString("orgs"), CBool(Me.Context.Request.QueryString("orgsFiltered")))
            Case "OSCREList"
                Dim GL As getLists = New getLists
                s = GL.CreateOSCREList(Me.Context.Request.QueryString("cbo"), Me.Context.Request.QueryString("oscre"))
            Case "CatCodeList"
                Dim GL As getLists = New getLists
                s = GL.CreateCatCodeList(Me.Context.Request.QueryString("cbo"), Me.Context.Request.QueryString("catcodewing"))
            Case "WingList"
                Dim GL As getLists = New getLists
                s = GL.CreateWingList(Me.Context.Request.QueryString("cbo"))
            Case "BldgList"
                Dim GL As getLists = New getLists
                s = GL.CreateBldgList(Me.Context.Request.QueryString("cbo")) 
            Case "selWingBldgList"
                Dim GL As getLists = New getLists
                s = GL.CreateSelWingBldgList(Me.Context.Request.QueryString("wing"))                            
            Case Else
                s = "Bad Request!"
        End Select
        Return s
       
    End Function
End Class

