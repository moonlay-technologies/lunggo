<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditorDemo.Models.Gift>" %>
<%@ Import Namespace="EditorDemo.Helpers"%>

<div class="editorRow">
    <% using(Html.BeginCollectionItem("gifts")) { %>
        Item: <%= Html.TextBoxFor(x => x.Name) %> 
        Value: $<%= Html.TextBoxFor(x => x.Price, new { size = 4 }) %> 
        <a href="#" class="deleteRow">delete</a>
    <% } %>
</div>        
