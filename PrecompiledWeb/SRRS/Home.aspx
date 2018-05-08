<%@ page language="C#" autoeventwireup="true" inherits="Home, App_Web_3wng2riv" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3>
        Welcome <asp:LoginName ID="LoginName1" runat="server" />
    </h3>
        <asp:LoginStatus ID="LoginStatus1" runat="server" />
    </div>
    </form>
</body>
</html>
