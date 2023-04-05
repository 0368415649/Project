
/*$(".price_cart").click(function () {
    alert("??")
})
*/

$(".cart_go_login").click(function () {
    $(".modal__body-login").show();
    $(".modall").show();
    validate('#login_form');
    $(".alert_check_login").hide();
})
$("#register_btn").click(function () {
    $(".modal__body-registration").show();
    $(".modall").show();
    validate('#form_registration');
})
$("#login_btn").click(function () {
    $(".modal__body-login").show();
    $(".modall").show();
    validate('#login_form');
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
    validate('#login_form');
})
$(".modal__inner-heard-registration").click(function () {
    $(".modal__body-registration").show();
    $(".modal__body-login").hide();
    $(".modall").show();
    validate('#form_registration');
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

function showAlerRegistration() {
    $(".alert_registration").show()
    $(".alert_registration").animate({ opacity: 0 }, 3000);
    setTimeout(() => {
        $(".alert_registration").animate({ opacity: 1 });
        $(".alert_registration").hide()
    }, "4000");
}

function showAlerLogin() {
    $(".alert_login").show()
    $(".alert_login").animate({ opacity: 0 }, 3000);
    setTimeout(() => {
        $(".alert_login").animate({ opacity: 1 });
        $(".alert_login").hide()
    }, "4000");
}
