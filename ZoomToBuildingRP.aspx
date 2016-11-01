<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ZoomToBuildingRP.aspx.vb"
    Inherits="ZoomToBuildingRP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Buildings</title>
    <meta lang="en" />
    <link href="css/GridStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="vertical-align: middle; width: 100%; color: #ffffff; font-family: 'Arial Black', Monospace, Arial;
                height: 20px; background-color: transparent; text-align: left; background-image: url(images/ab-slate-gradient-panelreportheader.png);
                background-repeat: repeat-x;">
                Buildings</div>
            <asp:Panel ID="Panel1" runat="server">
            <asp:DropDownList runat="server" ID="ddlBuildings" DataSourceID="SqlDataSource1" DataTextField="Building Number" DataValueField="Building Number"></asp:DropDownList>
            
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Building Number"
                    DataSourceID="SqlDataSource1" CellPadding="5" GridLines="None" HorizontalAlign="Left"
                    ShowHeader="False" BackColor="White" CssClass="legendTitle" ForeColor="White"
                    Width="167px">
                    <HeaderStyle CssClass="tblHead" BorderStyle="None" Font-Bold="True" />
                    <FooterStyle CssClass="tableViewFooter" />
                    <RowStyle CssClass="tableViewRow" />
                    <EditRowStyle CssClass="tableViewEditRow" />
                    <SelectedRowStyle CssClass="tableViewSelectedRow" />
                    <PagerStyle CssClass="tableViewPager" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" visible="false" ImageUrl="~/images/ab-icon-tree-norm.gif" />
                        <asp:BoundField DataField="Building Number" HeaderText="Building Number" ReadOnly="True"
                            SortExpression="Building Number" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" CommandName="ZoomTo"
                                    CommandArgument='<%# Eval("Building Number") %>' ImageUrl="~/images/Globe_ICON.gif" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:afmprod_asAFM %>"
            SelectCommand="SELECT bl_id  AS 'Building Number' FROM gis.GIS_ZoomBldgs order by bl_id"></asp:SqlDataSource>
    </form>
</body>
</html>
