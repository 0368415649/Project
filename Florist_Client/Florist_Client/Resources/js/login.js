
/*$(".price_cart").click(function () {
    alert("??")
})
*/
$("#register_btn").click(function () {
    $(".modal__body-registration").show();
    $(".modall").show();
})
$("#login_btn").click(function () {
    $(".modal__body-login").show();
    $(".modall").show();
})
$(".modall").click(function () {
    $(".modal__body-registration").hide();
    $(".modal__body-login").hide();
    $(".modall").hide();
})
$(".modal__inner-heard-login").click(function () {
    $(".modal__body-registration").hide();
    $(".modal__body-login").show();
    $(".modall").show();
})
$(".modal__inner-heard-registration").click(function () {
    $(".modal__body-registration").show();
    $(".modal__body-login").hide();
    $(".modall").show();
})
$(".btn--hover").click(function () {
    $(".modal__body-registration").hide();
    $(".modal__body-login").hide();
    $(".modall").hide();
})


$(".add_new_address").click(function () {
    $(".modal__body-address").show();
    $(".modall_ad").show();
})

$(".modall_ad").click(function () {
    $(".modal__body-address").hide();
    $(".modall_ad").hide();
})

$(".btn_back_address").click(function () {
    $(".modal__body-address").hide();
    $(".modall_ad").hide();
})
