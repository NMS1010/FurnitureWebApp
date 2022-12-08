function quickView(e, context) {
    let index = parseInt(e.getAttribute("data-id"));
    document.querySelector(".quickview__inner .current__price").innerText = $("#product-price-" + index).val()
    document.querySelector(".quickview__inner .product__details--info__title").innerText = $("#product-name-" + index).val()
    let html = ``
    let totalStar = parseInt($("#product-rating-" + index).val())
    for (let i = 1; i <= 5; i++) {
        html += `<li class="rating__list">
                    <span class="rating__list--icon">
                        <svg class="rating__list--icon__svg" xmlns="http://www.w3.org/2000/svg" width="14.105" height="14.732" viewBox="0 0 10.105 9.732">
                            <path data-name="star - Copy" d="M9.837,3.5,6.73,3.039,5.338.179a.335.335,0,0,0-.571,0L3.375,3.039.268,3.5a.3.3,0,0,0-.178.514L2.347,6.242,1.813,9.4a.314.314,0,0,0,.464.316L5.052,8.232,7.827,9.712A.314.314,0,0,0,8.292,9.4L7.758,6.242l2.257-2.231A.3.3,0,0,0,9.837,3.5Z" transform="translate(0 -0.018)"  fill=${i <= totalStar ? "currentColor" : ""}></path>
                        </svg>
                    </span>
                 </li>`
    }

    document.querySelector(".quickview__inner .rating").innerHTML = html
    document.querySelector(".quickview__inner .quickview__info--review__text").innerText = $("#product-totalReview-" + index).val() + ` reviews`
    document.querySelector(".quickview__inner .product__details--info__desc").innerText = $("#product-desc-" + index).val()

    let htmlBigImg = ``
    let htmlSmallImg = ``
    $('input.product-image-' + index).each(function () {
        htmlBigImg += `<div class="swiper-slide">
                            <div class="product__media--preview__items">
                                <a class="product__media--preview__items--link glightbox" data-gallery="product-media-preview" href=""><img class="product__media--preview__items--img" src="${context}${this.value}" alt="product-media-img"></a>
                                <div class="product__media--view__icon">
                                    <a class="product__media--view__icon--link glightbox" href="" data-gallery="product-media-preview">
                                        <svg class="product__media--view__icon--svg" xmlns="http://www.w3.org/2000/svg" width="22.51" height="22.443" viewBox="0 0 512 512"><path d="M221.09 64a157.09 157.09 0 10157.09 157.09A157.1 157.1 0 00221.09 64z" fill="none" stroke="currentColor" stroke-miterlimit="10" stroke-width="32"></path><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-miterlimit="10" stroke-width="32" d="M338.29 338.29L448 448"></path></svg>
                                        <span class="visually-hidden">Media Gallery</span>
                                    </a>
                                </div>
                            </div>
                        </div>`
        htmlSmallImg += `<div class="swiper-slide">
                            <div class="product__media--nav__items">
                                <img class="product__media--nav__items--img" src="${context}${this.value}" alt="product-nav-img">
                            </div>
                        </div>`
    });
    document.querySelector(".quickview__inner .product__media--preview .swiper-wrapper").innerHTML = htmlBigImg
    document.querySelector(".quickview__inner .product__media--nav .swiper-wrapper").innerHTML = htmlSmallImg
    document.querySelector(".quickview__inner #add-wishlist").setAttribute("data-productId", $("#product-id-" + index).val())
    document.querySelector(".quickview__inner #add-cartitem").setAttribute("data-productId", $("#product-id-" + index).val())
}