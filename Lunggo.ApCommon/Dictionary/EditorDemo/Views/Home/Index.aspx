<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EditorDemo.Models.Gift>>" %>
<%@ Import Namespace="EditorDemo.Helpers"%>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>Gift List</h2>
    What do you want for your birthday?

    <% using(Html.BeginForm()) { %>
        <div id="editorRows">
            <% foreach (var item in Model)
                Html.RenderPartial("GiftEditorRow", item);
            %>
        </div>
        <%= Html.ActionLink("Add another...", "Add", null, new { id = "addItem" }) %>
        
        <input type="submit" value="Finished" />
    <% } %>       
</asp:Content>
