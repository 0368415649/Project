$('#add_cart').click(function (e) {
    CartAraay = [];
    let storage = localStorage.getItem('cart');
    let Product_More = {
        id: $(this).data("id"),
        name: $(this).data("name"),
        price: $(this).data("price"),
        img: $(this).data("img"),
        discount: $(this).data("discount"),
    }
    if (storage) {
        CartAraay = JSON.parse(storage);
    }
    let item = CartAraay.find(c => c.product.id == $(this).data("id"));
    if (item) {
        item.quantity = Number(item.quantity) + 1;
    } else {
        CartAraay.push({ product: Product_More, quantity: 1 });
    }
    localStorage.setItem('cart', JSON.stringify(CartAraay));
    show_cart_number();
})
function addToCart(id, name, img, price, discount) {
    CartAraay = [];
    let storage = localStorage.getItem('cart');
    let Product_More = {
        id: id,
        name: name,
        price: price,
        img: img,
        discount: discount,
    }
    if (storage) {
        CartAraay = JSON.parse(storage);
    }
    let item = CartAraay.find(c => c.product.id == id);
    if (item) {
        item.quantity = Number(item.quantity) + 1;
    } else {
        CartAraay.push({ product: Product_More, quantity: 1 });
    }
    localStorage.setItem('cart', JSON.stringify(CartAraay));
    show_cart_number();
}
show_cart_number();
function show_cart_number() {
    CartAraay = [];
    let storage = localStorage.getItem('cart');
    if (storage) {
        CartAraay = JSON.parse(storage);
    }
    view_cart = "";
    $(".grap_icon_cart_number").html(CartAraay.length);
    if (CartAraay.length.length == 0) {
        alert("gaha")
    } else {
        countInt = 0;
        CartAraay.map((item) => {
            if (item.product.discount != 0) {
                countInt += ((100 - item.product.discount) / 100 * item.product.price) * item.quantity;
            } else {
                countInt += item.product.price * item.quantity;
            }
            view_cart += `
                 <div class="row min_heigthth">
                            <div class="col-2 view_cart_img">
                                <img src="/Resources/img/product/${item.product.img}.png" width="100%" alt="">
                            </div>
                            <div class="col-10">
                                <div class="view_cart_grap_title mb-2">${item.product.name}</div>
                                <span class="pricr me-3">${(item.product.discount != 0) ? ((100 - item.product.discount) / 100 * item.product.price) + "$" : ""}</span>
                                <span class="pricr ${(item.product.discount != 0) ? "text_ngang" : ""}">${item.product.price}$</span>
                                <div class="content-more-quantity-number">
                                    <div onclick=subtractCart("${item.product.id}") class="content-more-op content-more-op_minus2 subtract_cart">-</div>
                                    <input type="number" id="quantity" value="${item.quantity}" class="buy_input2">
                                    <div onclick=plusCart("${item.product.id}") class="content-more-op content-more-op_plus2 plus_cart">+</div>
                                </div>
                            </div>
                            <div class="sen2"></div>
                    </div>

                    `;
        })
    }

    $(".list_cart_view_grap_all").html(view_cart);
    $(".total_total").html(countInt + "$");

}

function plusCart(id) {
    let storage = localStorage.getItem('cart');
    if (storage) {
        CartAraay = JSON.parse(storage);
    }
    let item = CartAraay.find(c => c.product.id == id);
    item.quantity = Number(item.quantity) + 1;
    localStorage.setItem('cart', JSON.stringify(CartAraay));
    show_cart_number();
}
function subtractCart(id) {
    let storage = localStorage.getItem('cart');
    if (storage) {
        CartAraay = JSON.parse(storage);
    }
    let item = CartAraay.find(c => c.product.id == id);
    if (item.quantity <= 1) {
        CartAraay = CartAraay.filter(function (item) {
            return item.product.id != id;
        })
    } else {
        item.quantity = Number(item.quantity) - 1;
    }
    localStorage.setItem('cart', JSON.stringify(CartAraay));
    show_cart_number();
}