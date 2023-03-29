$('.message_click').each(function (ix, e) {
    $(e).click(function (element) {
        $("#message_show").val($(this).data("content"))
    })
})

$(".click_date").click(function () {
    alert("kk")
})

/*$('.table td').each(function (ix, e) {
    $(e).click(function (element) {
        alert("ss");
    })
})*/
