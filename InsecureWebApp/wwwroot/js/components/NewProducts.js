$.fn.NewProducts = function (options) {
    return this.each(function (index, el) {

        var settings = $.extend({
            limit: 3,
            currencySymbol: "&#164;"
        });

        var $this = $(this), $data = $this.find('#product-data');
        _getProducts(settings.limit).then(response => {
            $data.empty();
            if (response.length > 0) {
                $.each(response, function (i, row) {
                    product = _productDiv(row);
                    $data.append(product);
                    if (i == (settings.limit-1)) {
                        return false;
                    }
                });
            } else {
                $data.append("<div class='col-12 text-center'>No products found</div>");
            }
        });
    });

    function _productDiv(product) {
        return (
            "<div class='col-sm-6 col-lg-4 text-center item mb-4'>" +
                (product.onSale ? "<span class='tag'>Sale</span>" : "") +
                "<a href='/products/details/" + product.id + "'>" +
                    (product.image ? "<img src='/img/products/" + product.image + "' alt='Image' class='img-fluid'" : "<img src='/img/products/awaiting-image-sm.png' alt='Image' class='img-fluid'>") +
                "</a>" +
                "<h3 class='text - dark'><a href='/products/details/" + product.id + "'>" + product.name + "</a></h3>" +
            (product.onSale ? "<p class='price'><del>" + options.currencySymbol + product.price + "</del> &mdash;" + options.currencySymbol + product.salePrice + "</p>" : "<p class='price'>" + options.currencySymbol + product.price + "</p>") +
            "</div>"
        );
        return productDiv;
    }
    async function _getProducts(limit) {
        return await $.get(`/api/v1/products/getproducts?limit=${limit}`).then();
    }

};