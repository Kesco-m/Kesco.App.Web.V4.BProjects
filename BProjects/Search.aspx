<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Kesco.App.Web.V4.BProjects.Search" %>
<%@ Register TagPrefix="csg" Namespace="Kesco.Lib.Web.Controls.V4.Grid" Assembly="Controls.V4" %>
<%@ Register TagPrefix="cstv" Namespace="Kesco.Lib.Web.Controls.V4.TreeView" Assembly="Controls.V4" %>
<%@ Register TagPrefix="v4control" Namespace="Kesco.Lib.Web.Controls.V4" Assembly="Controls.V4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%= Resx.GetString("Bp_lblBusinesProjects")%></title>
    <link rel="stylesheet" type="text/css" href="Kesco.BProjects.css" />
    <script src="Kesco.BProjects.js" type="text/javascript"></script>
</head>
<body>

    <form id="mvcDialogResult" method="post">
        <input type="hidden" name="escaped" value="0" />
		<input type="hidden" name="control" value="" />
        <input type="hidden" name="multiReturn" value="" />
		<input type="hidden" name="value" value="" />
    </form>

    <div class="marginD"><%=RenderDocumentHeader()%></div>  
    <div id="divContainer">
        <div id="divMyTreeContainer" class="ui-widget-content">
            <cstv:TreeView runat="server" ID="tvProject"/>
        </div>
    </div>
</body>
</html>