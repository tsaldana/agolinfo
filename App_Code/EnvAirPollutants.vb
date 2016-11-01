Imports Microsoft.VisualBasic
Imports System.Data
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.Configuration
Imports System

Public Class EnvAirPollutants


    Public Function GetEmissions(ByVal EUID As String, ByVal strFY As String) As String
        Try
            Dim strHTML As String = String.Empty
            If EUID <> String.Empty Then
                Dim myConnection As New OracleConnection(ConfigurationManager.ConnectionStrings("afmprod_asAFM").ConnectionString)
                myConnection.Open()
                Dim myCommand As New OracleCommand("EU_Pollution_TPY", myConnection)
                'myCommand.CommandType
                myCommand.CommandText = "Select TITLE_V_DESC, STATUS " & _
                                        "FROM ENVAQSITEOBJ " & _
                                        "WHERE EU_ID = :EUID"
                myCommand.Parameters.Add("EUID", OracleDbType.Varchar2, 10)
                myCommand.Parameters("EUID").Value = EUID
                Dim reader As OracleDataReader = myCommand.ExecuteReader()
                Dim strEUDesc As String = String.Empty
                If reader.Read() Then
                    strEUDesc = Convert.ToString(reader("TITLE_V_DESC")) & vbNewLine & Convert.ToString(reader("STATUS"))
                End If
                reader.Close()

                myCommand.CommandText = "SELECT EU_ID, PLUTDESC, PLUTCAS, EMSAMOUNTREPORTED " & _
                                        "FROM ENVAQEMISSIONS " & _
                                        "WHERE (EMSINVENTORYYEAR = :FY) AND (EU_ID = :EUID) " & _
                                        "order by PLUTDESC"

                myCommand.Parameters.Add("FY", OracleDbType.Varchar2, 10)
                myCommand.Parameters("FY").Value = strFY

                reader = myCommand.ExecuteReader()
                If reader.HasRows = True Then

                    strHTML = "<table class='EUTable'>"
                    'strHTML += "<tr><th  colspan='2' >Emission Unit:</th><th class='GISChartRightTD'>" & EUID & "</th></tr>"
                    strHTML += "<tr ><th>Pollutant</th><th>CAS</th><th>Emissions (TPY)</th></tr>"

                    While reader.Read()
                        'Dim strEUID as string = string.empty
                        Dim strCAS As String = String.Empty
                        Dim strName As String = String.Empty
                        Dim strTons As String = String.Empty

                        If Not reader("plutdesc") Is DBNull.Value Then
                            strName = Convert.ToString(reader("plutDesc"))
                        End If
                        If Not reader("plutcas") Is DBNull.Value Then
                            strCAS = Convert.ToString(reader("plutCas"))
                        End If
                        If Not reader("emsamountreported") Is DBNull.Value Then
                            strTons = Convert.ToString(reader("emsamountreported"))
                        End If
                        strHTML += "<tr ><td class='ltd'>" & strName & "</td><td class='mtd'>" & strCAS & "</td><td class='rtd'>" & strTons & "</td></tr>"
                    End While
                    'set the chart heading
                    strHTML += "*?*?*?*" & "Emission Unit " & EUID & "<br/><div class='EUDesc'>" & strEUDesc & "</div>"

                Else 'an EU was not returned from the tool
                    Return "<table class='GISResultsTable'><tr><td>Fiscal year " & strFY & " data not found for Emission Unit " & EUID & "</td></tr></table>" & _
                            "*?*?*?*" & "Emission Unit " & EUID & "<br/><div class='EUDesc'>" & strEUDesc & "</div>"
                End If
            Else
                Return String.Empty
            End If
            Return strHTML

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class
