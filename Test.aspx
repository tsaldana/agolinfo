<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Test.aspx.vb" Inherits="Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="BL_ID" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:BoundField DataField="BL_ID" HeaderText="BL_ID" ReadOnly="True" 
                    SortExpression="BL_ID" />
            </Columns>
        </asp:GridView>    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:afmprod_asAFM %>" 
            ProviderName="<%$ ConnectionStrings:afmprod_asAFM.ProviderName %>" 
            SelectCommand="SELECT BL_ID FROM BL ORDER BY BL_ID"></asp:SqlDataSource>
    </div>
    <asp:TextBox ID="txtFacil" runat="server"></asp:TextBox>
    <asp:DropDownList ID="DropDownList1" runat="server" 
        DataSourceID="SqlDataSource1" DataTextField="BL_ID" DataValueField="BL_ID">
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnGetRecs" runat="server" Height="22px" Text="Fetch" 
        Width="56px" />
    </form>
</body>
</html>
