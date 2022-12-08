function addWish(e, context){
    let productId  = parseInt(e.getAttribute("data-productId"));
    let url = context + `/add-wish`
    $.ajax({
        url: url,
        method: "GET",
        data: {
            'productId' : productId
        },
        async: false,
        success: function (data){
            console.log(data)
            let str = data.toString()
            if(str.includes('error')){
                document.getElementById("modal-error").classList.add('is-visible')
            }
            else if(str.includes('success')){
                document.querySelectorAll('.wish_count').forEach(c => c.innerText = (parseInt(str)).toString())
                document.getElementById("modal-success").classList.add('is-visible')

            }
            else if(str.includes('must-login')){
                window.location.replace(context + '/wish-list')
            }
        },
        error: function (error){

        }
    })
}
function openWishModal(e){
    let id = parseInt(e.getAttribute("data-wishItemId"));
    document.getElementById('link-delete').setAttribute("data-wishItemId", id.toString())
}
function deleteWishItem(e, context){
    let wishItemId  = parseInt(e.getAttribute("data-wishItemId"));
    let url = context + `/remove-wish`
    $.ajax({
        url: url,
        method: "GET",
        data: {
            'wishItemId' : wishItemId
        },
        async: false,
        success: function (data){
            console.log(data)
            let str = data.toString()
            if(str.includes('error')){
                if(str.length <=10)
                    document.getElementById("modal-error").classList.add('is-visible')
            }
            else{
                window.location.replace(context+ '/wish-list')
            }
        },
        error: function (error){

        }
    })
}