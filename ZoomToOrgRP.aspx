<%@ Page Language="VB" AutoEventWireup="true" CodeFile="ZoomToOrgRP.aspx.vb" Inherits="ZoomToOrgRP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Org</title>
    <link href="css/GridStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="vertical-align: middle; width: 100%; color: #ffffff; font-family: 'Arial Black', Monospace, Arial;
                height: 20px; background-color: transparent; text-align: left; background-image: url(images/ab-slate-gradient-panelreportheader.png);
                background-repeat: repeat-x;">
                Organizations</div>
            <asp:Panel ID="Panel1" runat="server">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                    CellPadding="5" GridLines="None" HorizontalAlign="Left" ShowHeader="False" BackColor="White"
                    CssClass="legendTitle" ForeColor="black" Width="357px">
                    <HeaderStyle CssClass="tblHead" BorderStyle="None" Font-Bold="True" />
                    <FooterStyle CssClass="tableViewFooter" />
                    <RowStyle CssClass="tableViewRow" />
                    <EditRowStyle CssClass="tableViewEditRow" />
                    <SelectedRowStyle CssClass="tableViewSelectedRow" />
                    <PagerStyle CssClass="tableViewPager" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/images/ab-icon-tree-norm.gif" CommandName="Ignore" />
                        <asp:BoundField DataField="Org_Account_Code" HeaderText="Org Account Code" SortExpression="Org_Account_Code" />
                        <asp:BoundField DataField="NAME" HeaderText="Org Name" SortExpression="NAME" />
                        <asp:BoundField DataField="OFFICE_SYMBOL" HeaderText="Org Symbol" SortExpression="OFFICE_SYMBOL" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" CommandName="ZoomTo"
                                    CommandArgument='<%# Eval("OFFICE_SYMBOL") %>' ImageUrl="~/images/Globe_ICON.gif" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:afmprod_asAFM %>"
            SelectCommand="SELECT [Org_Account_Code], [NAME], [OFFICE_SYMBOL] FROM gis.GIS_ZoomOrgs">
        </asp:SqlDataSource>
    </form>
</body>
</html>
