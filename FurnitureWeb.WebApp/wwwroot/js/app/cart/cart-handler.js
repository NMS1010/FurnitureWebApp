function openCartItemModal(e){
    let id = parseInt(e.getAttribute("data-cartItemId"));
    document.getElementById('link-delete').setAttribute("data-cartItemId", id.toString())
}
function addCartItem(e, context){
    let productId  = parseInt(e.getAttribute("data-productId"));
    let url = context + `/cart/add`
    $.ajax({
        url: url,
        method: "GET",
        data: {
            'productId' : productId,
            'quantity' : 1
        },
        async: false,
        success: function (data){
            console.log(data)
            let str = data.toString()
            if(str.includes('error')){
                document.getElementById("modal-error").classList.add('is-visible')
            }
            else if(str.includes('expired')){
                document.getElementById("modal-expired").classList.add('is-visible')
            }
            else if(str.includes('success')){
                document.querySelectorAll('.cart_item_count').forEach(c => c.innerText = (parseInt(str)).toString())
                document.getElementById("modal-success").classList.add('is-visible')
            }
            else if(str.includes('must-login')){
                window.location.replace(context + '/cart/items')
            }
        },
        error: function (error){

        }
    })
}
function deleteCartItem(e, context){
    let cartItemId  = parseInt(e.getAttribute("data-cartItemId"));
    let url = context + `/cart/remove`
    $.ajax({
        url: url,
        method: "GET",
        data: {
            'cartItemId' : cartItemId
        },
        async: false,
        success: function (data){
            console.log(data)
            let str = data.toString()
            if(str.includes('error')){
                document.getElementById("modal-error").classList.add('is-visible')
            }
            else{
                window.location.replace(context+ '/cart/items')
            }
        },
        error: function (error){

        }
    })
}
function updateCartItemQuantity(e, context, currQuantity){
    let cartItemId  = parseInt(e.getAttribute("data-cartItemId"));
    let quantity = parseInt(document.getElementById("cart-item-quantity-"+cartItemId.toString()).value)
    if(e.type === 'button'){
        if(e.value.includes('Increase')){
            quantity += 1
        }else{
            quantity -= 1
        }
    }
    if(quantity < 0 || quantity === 0 || isNaN(quantity)) {
        document.getElementById("modal-delete-cart").classList.add('is-visible')
        document.getElementById("cart-item-quantity-" + cartItemId.toString()).value = parseInt(currQuantity)
        openCartItemModal(document.getElementById('cart-remove-'+cartItemId))
        return;
    }
    let url = context + `/cart/update`
    $.ajax({
        url: url,
        method: "GET",
        data: {
            'cartItemId' : cartItemId,
            'quantity':quantity
        },
        async: false,
        success: function (data){
            console.log(data)
            let str = data.toString()
            if(str.includes('error') ){
                if(str.length <=10)
                    document.getElementById("modal-error").classList.add('is-visible')
            }
            else{
                let overMessage = document.getElementById("over-quantity-" + cartItemId.toString())
                if(str.includes('over')){
                    overMessage.innerText = "Số sản phẩm không đủ"
                    quantity = parseInt(str)
                    document.getElementById("cart-item-quantity-" + cartItemId.toString()).value = quantity
                }else{
                    overMessage.innerText = ""
                }
                let unitPrice = parseFloat(document.getElementById("cart-item-" + cartItemId.toString() + "-unitPrice").innerText)
                let totalPrice = unitPrice * quantity;
                document.getElementById("cart-item-" + cartItemId.toString() + "-totalPrice").innerText = totalPrice.toFixed(2).toString()

                let total = 0;
                document.querySelectorAll('.total-price').forEach(c => total += parseFloat(c.innerText))
                document.getElementById("total-item-price").innerText = total.toFixed(2).toString() + " VND"
            }
        },
        error: function (error){

        }
    })
}