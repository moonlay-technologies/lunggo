<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EditorDemo.Models.Gift>>" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <h2>Completed</h2>
    You asked for:
    <ul>
        <% foreach (var gift in Model) { %>
            <li><b><%= Html.Encode(gift.Name) %></b>, valued at <%= gift.Price.ToString("c") %></li>     
        <% } %>
    </ul>
    
</asp:Content>