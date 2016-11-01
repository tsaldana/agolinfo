<%@ Page Language="VB" AutoEventWireup="false" CodeFile="connect.aspx.vb" Inherits="connect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:afmprod_asAFM %>" ProviderName="<%$ ConnectionStrings:afmprod_asAFM.ProviderName %>" SelectCommand="SELECT &quot;ADDRESS1&quot;, &quot;AC_ID&quot;, &quot;ADDRESS2&quot; FROM &quot;BL&quot;"></asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="ADDRESS1" HeaderText="ADDRESS1" SortExpression="ADDRESS1" />
                <asp:BoundField DataField="AC_ID" HeaderText="AC_ID" SortExpression="AC_ID" />
                <asp:BoundField DataField="ADDRESS2" HeaderText="ADDRESS2" SortExpression="ADDRESS2" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
