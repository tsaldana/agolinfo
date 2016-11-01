Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System

Public Class RoomData_Occupants


    Public Function GetRoomOrganizations(ByVal BLID As String, ByVal FLID As String, ByVal RMID As String) As String
        Dim strHTML As String = String.Empty
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BLID <> String.Empty Then
                myConnection.Open()
                Dim myCommand As New OracleCommand("RoomOccs", myConnection)
                myCommand.CommandText = "Select CMD_ID,WING_ID,GRP_ID,SQUADRON_ID,FLIGHT_ID,COUNT_EM,OFFICE_SYM,AREA_RM,NAME_LAST,NAME_FIRST " & _
                                        "FROM GIS_ENHROOMDATA WHERE " & "(BL_ID = :BLID) AND (FL_ID = :FLID) AND (RM_ID = :RMID)"
                myCommand.Parameters.Add("BLID", OracleDbType.Varchar2, 10)
                myCommand.Parameters("BLID").Value = BLID.ToString
                myCommand.Parameters.Add("FLID", OracleDbType.Varchar2, 10)
                If FLID.Length = 1 And Not FLID.StartsWith("M") Then
                    myCommand.Parameters("FLID").Value = "0" + FLID.ToString
                Else
                    myCommand.Parameters("FLID").Value = FLID.ToString
                End If
                myCommand.Parameters.Add("RMID", OracleDbType.Varchar2, 10)
                myCommand.Parameters("RMID").Value = RMID.ToString
                Dim reader As OracleDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
                If reader.HasRows = True Then
                    strHTML = "<table class='GISResultsTable'>"
                    'strHTML += "<tr style='padding-bottom:0px;'><td class='GISChartTDBold'></td><td class='GISChartTDBold'></td><td class='GISChartTDBold'></td><td class='GISChartTDBold'></td><td class='GISChartTDBold'></td><td class='GISChartTDBold'></td><td class='GISChartTDBold'>Area</td><td class='GISChartTDBold'>Facility Mgr</td></tr>"
                    While reader.Read()
                        Dim cmd_id As String = String.Empty
                        Dim wing_id As String = String.Empty
                        Dim grp_id As String = String.Empty
                        Dim squadron_id As String = String.Empty
                        Dim flight_id As String = String.Empty
                        Dim count_em As String = String.Empty
                        Dim cap_em As String = String.Empty
                        Dim office_sym As String = String.Empty
                        Dim area_rm As String = String.Empty
                        Dim name_full As String = String.Empty
                        If Not reader("cmd_id") Is DBNull.Value Then
                            cmd_id = Convert.ToString(reader("cmd_id"))
                        End If
                        If Not reader("wing_id") Is DBNull.Value Then
                            wing_id = Convert.ToString(reader("wing_id"))
                        End If
                        If Not reader("grp_id") Is DBNull.Value Then
                            grp_id = Convert.ToString(reader("grp_id"))
                        End If
                        If Not reader("squadron_id") Is DBNull.Value Then
                            squadron_id = Convert.ToString(reader("squadron_id"))
                        End If
                        If Not reader("flight_id") Is DBNull.Value Then
                            flight_id = Convert.ToString(reader("flight_id"))
                        End If
                        If Not reader("count_em") Is DBNull.Value Then
                            count_em = Convert.ToString(reader("count_em"))
                        End If
                        If Not reader("office_sym") Is DBNull.Value Then
                            office_sym = Convert.ToString(reader("office_sym"))
                        End If
                        If Not reader("area_rm") Is DBNull.Value Then
                            area_rm = Convert.ToString(reader("area_rm")) & " Sq feet"
                        End If
                        If Not reader("name_last") Is DBNull.Value And Not reader("name_first") Is DBNull.Value Then
                            name_full = Convert.ToString(reader("name_last")) & ", " & Convert.ToString(reader("name_first"))
                        End If
                        'strHTML += "<tr><td colspan=8 ><hr style='color:#a9a9a9;padding-bottom:0px;height:1px;'/></td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Command: " & "</td><td>" & cmd_id & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Wing: " & "</td><td>" & wing_id & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Group: " & "</td><td>" & grp_id & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Squadron: " & "</td><td>" & squadron_id & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Flight: " & "</td><td>" & flight_id & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Office: " & "</td><td>" & office_sym & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Area: " & "</td><td>" & area_rm & "</td></tr>"
                        strHTML += "<tr><td class='GISChartTDBold'>" & "Facility Mgr: " & "</td><td>" & name_full & "</td></tr>"
                    End While
                    strHTML += "</table>"
                ElseIf reader.HasRows = False Then 'room orgs were not returned from the db
                    strHTML = "<table class='GISResultsTable'><tr><td>No Organizations</td></tr></table>"
                End If
                reader.Close()
            End If
            Return strHTML
        Catch ex As Exception
            strHTML = ex.Message
            Return "Error: " & strHTML
        End Try
    End Function

    Public Function GetRoomEmStandards(ByVal BLID As String, ByVal FLID As String, ByVal RMID As String) As String
        Dim strHTML As String = String.Empty
        Dim bl_id As String = String.Empty
        Dim fl_id As String = String.Empty
        Dim rm_id As String = String.Empty
        Dim description As String = String.Empty
        Dim count As String = String.Empty
        Dim reader As OracleDataReader
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BLID <> String.Empty Then
                myConnection.Open()
                Dim myCommand As New OracleCommand("RoomStds", myConnection)
                myCommand.CommandText = "select bl_id, fl_id, rm_id, description, count " & _
                                        "FROM GIS_ROOMEMSTANDARDS WHERE " & "(BL_ID = :BLID) AND (FL_ID = :FLID) AND (RM_ID = :RMID)"
                myCommand.Parameters.Add("BLID", OracleDbType.Varchar2, 10)
                myCommand.Parameters("BLID").Value = BLID.ToString
                myCommand.Parameters.Add("FLID", OracleDbType.Varchar2, 10)
                If FLID.Length = 1 And Not FLID.StartsWith("M") Then
                    myCommand.Parameters("FLID").Value = "0" + FLID.ToString
                Else
                    myCommand.Parameters("FLID").Value = FLID.ToString
                End If
                myCommand.Parameters.Add("RMID", OracleDbType.Varchar2, 10)
                myCommand.Parameters("RMID").Value = RMID.ToString
                reader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
                If reader.HasRows = True Then
                    strHTML = "<table class='GISResultsTable'>"
                    strHTML += "<tr><th>Description</th><th>Count</th>"
                    While reader.Read()
                        If Not reader("bl_id") Is DBNull.Value Then
                            bl_id = Convert.ToString(reader("bl_id")).Trim
                        End If
                        If Not reader("fl_id") Is DBNull.Value Then
                            fl_id = Convert.ToString(reader("fl_id")).Trim
                        End If
                        If Not reader("rm_id") Is DBNull.Value Then
                            rm_id = Convert.ToString(reader("rm_id")).Trim
                        End If
                        If Not reader("description") Is DBNull.Value Then
                            description = Convert.ToString(reader("description")).Trim
                        End If
                        If Not reader("count") Is DBNull.Value Then
                            count = Convert.ToString(reader("count")).Trim
                        End If
                        strHTML += "<tr><td>" & description & "</td><td>" & count & "</td></tr>"
                    End While
                    strHTML += "<tr><td><a class=" & ControlChars.Quote & "GisOrgBldgLinkDiv" & ControlChars.Quote & " onclick=" & ControlChars.Quote & "openWeb('" & "AbSpacePersonnelInventory/GIS_Employees.axvw" & "','" & "em" & "','" & bl_id & "','" & fl_id & "','" & rm_id & "');" & ControlChars.Quote & ">View Employee List</a></td></tr>"
                    strHTML += "</table>"
                ElseIf reader.HasRows = False Then 'employees were not returned from the db
                    strHTML = "<table class='GISResultsTable'><tr><td>No Employees found</td></tr></table>"
                    'strHTML += BLID + " " + FLID + " " + RMID
                End If
            End If
            Return strHTML
        Catch ex As Exception
            strHTML = ex.Message
            Return "Error: " & strHTML
        End Try
    End Function


    Public Function GetRoomReports(ByVal BLID As String, ByVal FLID As String, ByVal RMID As String) As String
        Dim strHTML As String = String.Empty
        Dim bl_id As String = BLID
        Dim fl_id As String = FLID
        Dim rm_id As String = RMID
        Dim description As String = String.Empty
        Dim reportfile As String = String.Empty
        Dim tablename As String = String.Empty
        Dim reader As OracleDataReader
        Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
        Try
            If BLID <> String.Empty Then
                myConnection.Open()
                Dim myCommand As New OracleCommand("RoomReports", myConnection)
                myCommand.CommandText = "select DESCRIPTION, REPORTFILE, TABLENAME " & _
                                        "FROM GIS_RoomReports"
                reader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
                If reader.HasRows = True Then
                    strHTML = "<table class='GISResultsTable'>"
                    strHTML += "<tr><th>Description</th>"
                    While reader.Read()
                        If Not reader("description") Is DBNull.Value Then
                            description = Convert.ToString(reader("description")).Trim
                        End If
                        If Not reader("reportfile") Is DBNull.Value Then
                            reportfile = Convert.ToString(reader("reportfile")).Trim
                        End If
                        If Not reader("tablename") Is DBNull.Value Then
                            tablename = Convert.ToString(reader("tablename")).Trim
                        End If
                        If fl_id.Length = 1 And Not fl_id.StartsWith("M") Then
                            fl_id = "0" + fl_id.ToString
                        End If
                        strHTML += "<tr><td><a class=" & ControlChars.Quote & "GisOrgBldgLinkDiv" & ControlChars.Quote & " onclick=" & ControlChars.Quote & "openWeb('" & reportfile & "','" & tablename & "','" & bl_id & "','" & fl_id & "','" & rm_id & "');" & ControlChars.Quote & ">" & description & "</a></td></tr>"
                    End While
                    strHTML += "</table>"
                ElseIf reader.HasRows = False Then 'employees were not returned from the db
                    strHTML = "<table class='GISResultsTable'><tr><td>No Employees found</td></tr></table>"
                    strHTML += BLID + " " + FLID + " " + RMID
                End If
            End If
            Return strHTML
        Catch ex As Exception
            strHTML = ex.Message
            Return "Error: " & strHTML
        End Try
    End Function
End Class
