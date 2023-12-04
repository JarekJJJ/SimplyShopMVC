$("input").click(function () {
    var selectedElement = this.id;
    $("#selectedTag").val(selectedElement);
    $("#headForm").submit();
});

