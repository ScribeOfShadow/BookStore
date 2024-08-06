<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRCodeGenerater.aspx.cs" Inherits="BookStore.QRCodeGenerater" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate and Read QR Code in Asp.net</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
            <asp:Button ID="btnGenerate" runat="server" Text="Generate QR Code" OnClick="btnGenerate_Click" />
            <hr />
            <asp:Image ID="imgQRCode"  Width="100px" Height="100px" runat="server" Visible="false" />
            <br /><br />
            <asp:Button ID="btnRead" runat="server" Text="Read QR Image" OnClick="btnRead_Click" />
            <br /><br />
            <asp:Label ID="lblQRCode" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
