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



show_cart_comfirm();
function show_cart_comfirm() {
    CartAraay = [];
    let storage = localStorage.getItem('cart');

    if (storage) {
        CartAraay = JSON.parse(storage);
    }
    view_cart = "";
    countInt = 0;
    CartAraay.map((item) => {
        if (item.product.discount != 0) {
            countInt += ((100 - item.product.discount) / 100 * item.product.price) * item.quantity;
        } else {
            countInt += item.product.price * item.quantity;
        }
        view_cart += `
                <div class="row min_heigthth mt-2">
                        <div class="col-2 view_cart_img">
                            <img src="/Resources/img/product/${item.product.img}.png" width="100%" alt="">
                        </div>
                        <div class="col-10">
                            <div class="view_cart_grap_title mb-2">${item.product.name}</div>
                            <span class="pricr me-3">${(item.product.discount != 0) ? ((100 - item.product.discount) / 100 * item.product.price) + "$" : ""}</span>
                            <span class="pricr ${(item.product.discount != 0) ? "text_ngang" : ""}">${item.product.price}$</span>
                            <span class="pricr mt-2"> -  Quantily :   ${item.quantity}</span>

                            
                        </div>


                        <div class="sen2"></div>
                </div>

                `;
    })
    $(".cart_submit").val(storage);
    $(".cart_total").val(countInt);
    $(".grap_cart_comfirm_image").html(view_cart);
    $(".total_cart").html(countInt + "$");

}
